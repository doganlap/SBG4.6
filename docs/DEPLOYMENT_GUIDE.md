# Deployment Guide

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Staging Deployment](#staging-deployment)
3. [Production Deployment](#production-deployment)
4. [Rollback Procedures](#rollback-procedures)
5. [Health Checks](#health-checks)
6. [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Tools
```bash
# Docker & Docker Compose
docker --version  # >= 20.10
docker compose version  # >= 2.0

# Git
git --version  # >= 2.30

# SSH client (for remote deployment)
ssh -V
```

### Required Credentials
```
GitHub Personal Access Token (for container registry)
SSH private key for deployment servers
Slack webhook URL (for notifications)
Database root password
```

### Infrastructure Setup

**Staging Server**:
- OS: Ubuntu 22.04 LTS or equivalent
- RAM: 8GB minimum
- Storage: 100GB SSD
- Ports: 80, 443, 3306 (MariaDB), 6379 (Redis)
- Docker: Pre-installed

**Production Server**:
- OS: Ubuntu 22.04 LTS (or managed Kubernetes)
- RAM: 16GB+ (4GB per module container)
- Storage: 500GB SSD with snapshots
- Backup: Daily automated snapshots
- Monitoring: CloudWatch/DataDog/New Relic
- HA/Load Balancer: Required (AWS ALB, Azure LB, nginx)

## Staging Deployment

### Automatic Staging Deployment

The staging environment deploys automatically on every push to the `develop` branch.

**Workflow**:
1. Code pushed to `develop` → GitHub Actions triggers
2. CI/CD pipeline runs (tests, build, security checks)
3. Docker images built and pushed to GHCR
4. SSH to staging server
5. Pull latest images and restart containers
6. Run smoke tests
7. Post result to Slack

### Manual Staging Deployment

**Via GitHub Actions UI**:

1. Go to https://github.com/doganlap/SBG4.6/actions
2. Select **"Deploy - Staging & Production"** workflow
3. Click **"Run workflow"**
4. Select **"staging"** from dropdown
5. Click **"Run workflow"**

**Via Command Line**:
```bash
# Create deployment event
gh workflow run deploy.yml \
  -f environment=staging

# Monitor workflow
gh run list --workflow=deploy.yml --limit=1
```

### Deploy to Staging Manually

If GitHub Actions is unavailable:

```bash
#!/bin/bash
# scripts/deploy-staging.sh

set -e

REGISTRY="ghcr.io/doganlap/SBG4.6"
STAGING_HOST="${STAGING_HOST:-staging.example.com}"
STAGING_USER="${STAGING_USER:-deploy}"

echo "Deploying to staging: $STAGING_HOST"

# 1. Login to GitHub Container Registry
echo $GITHUB_TOKEN | docker login ghcr.io -u doganlap --password-stdin

# 2. Pull latest images
docker pull $REGISTRY/saas-admin:main
docker pull $REGISTRY/tenant-admin:main
docker pull $REGISTRY/customer-portal:main
docker pull $REGISTRY/base:main

# 3. SSH and deploy
ssh -i ~/.ssh/staging_key $STAGING_USER@$STAGING_HOST << 'EOF'
  cd /app
  docker compose pull
  docker compose up -d
  docker compose ps
  
  # Health check
  sleep 10
  curl -f http://localhost:3000/health || exit 1
  echo "Staging deployment successful"
EOF
```

Run:
```bash
chmod +x scripts/deploy-staging.sh
./scripts/deploy-staging.sh
```

## Production Deployment

### Production Requirements

**Before deploying to production**:
- [ ] All code reviewed and approved
- [ ] All tests passing (unit, integration, E2E)
- [ ] Security scan clean (no critical vulnerabilities)
- [ ] Performance tests pass (p95 < 500ms)
- [ ] Staging environment tested for 24+ hours
- [ ] Backup of production database created
- [ ] Team notified of deployment window

### Deployment Flow

```
main branch (production-ready code)
    ↓
    ├─ Push to main
    ├─ CI/Build/Tests run (auto)
    ├─ Docker images built & tagged
    └─ Deployment workflow triggered
         ↓
         ├─ Approval gate (GitHub Environments)
         ├─ Await manual approval (team lead)
         └─ If approved:
              ├─ Pull latest images
              ├─ Stop old containers gracefully
              ├─ Start new containers
              ├─ Run health checks
              ├─ Monitor for 5 minutes
              └─ If healthy: Success
                   If failed: Rollback
```

### GitHub Production Approval Setup

**One-time Configuration**:

1. Go to https://github.com/doganlap/SBG4.6/settings/environments
2. Click **"New environment"**
3. Name: `production`
4. Click **"Configure environment"**
5. **Deployment branches**: Select **"Require deployments to be on branches matching rules"**
   - Pattern: `main`
6. **Required reviewers**: Add team members who can approve deployments
7. Click **"Save protection rules"**

### Approve Production Deployment

When deployment is ready:

1. Go to https://github.com/doganlap/SBG4.6/actions
2. Find the deployment workflow waiting for approval
3. Click **"Review deployments"**
4. Select **"production"**
5. Add comment (optional): "Deploying to production after 24h staging validation"
6. Click **"Approve and deploy"**

**Or via CLI**:
```bash
# See pending deployments
gh run list --workflow=deploy.yml --status=waiting

# Approve deployment
gh run view <run-id> --log | grep "Waiting for review"
# Then approve via GitHub UI
```

### Production Deployment Script

Manual deployment (emergency only):

```bash
#!/bin/bash
# scripts/deploy-production.sh

set -e

REGISTRY="ghcr.io/doganlap/SBG4.6"
PROD_HOST="${PROD_HOST:-prod.example.com}"
PROD_USER="${PROD_USER:-deploy}"

# Safety checks
if [ "$1" != "--force" ]; then
  echo "⚠️  Production deployment detected"
  echo "Run with --force flag to confirm"
  exit 1
fi

echo "🚀 Deploying to production: $PROD_HOST"

# Backup current state
ssh -i ~/.ssh/prod_key $PROD_USER@$PROD_HOST << 'BACKUP'
  cd /app
  
  # Backup docker-compose state
  docker compose ps > /backups/compose-state-$(date +%s).log
  
  # Backup database
  docker compose exec -T mariadb mysqldump \
    -u root -p${DB_ROOT_PASSWORD} \
    --all-databases > /backups/db-$(date +%Y%m%d-%H%M%S).sql
  
  echo "✅ Backup completed"
BACKUP

# Deploy
ssh -i ~/.ssh/prod_key $PROD_USER@$PROD_HOST << 'DEPLOY'
  cd /app
  
  # Pull latest images
  docker compose pull
  
  # Graceful shutdown (60s drain period)
  docker compose down --timeout=60
  
  # Bring up with new images
  docker compose up -d
  
  # Wait for services
  sleep 10
  docker compose ps
  
  # Run health checks
  for i in {1..30}; do
    if curl -f http://localhost:3000/health > /dev/null 2>&1; then
      echo "✅ Health check passed"
      exit 0
    fi
    echo "Attempt $i/30: waiting for services..."
    sleep 2
  done
  
  echo "❌ Health check failed - rolling back"
  exit 1
DEPLOY

# Monitor
echo "📊 Monitoring for 5 minutes..."
for i in {1..30}; do
  response=$(ssh -i ~/.ssh/prod_key $PROD_USER@$PROD_HOST \
    'curl -s -o /dev/null -w "%{http_code}" http://localhost:3000/health')
  
  if [ "$response" != "200" ]; then
    echo "❌ Health check returned $response - triggering rollback"
    ./scripts/rollback-production.sh
    exit 1
  fi
  
  echo "✅ Health check: $response ($(($i * 10)))s"
  sleep 10
done

echo "🎉 Production deployment successful"
```

## Rollback Procedures

### Automatic Rollback

Triggered automatically if:
- Health checks fail for 30 consecutive attempts
- HTTP 5xx errors detected
- Database connection lost

**Automatic Rollback Flow**:
```bash
# GitHub Actions automatically:
1. Retrieve previous commit SHA
2. Pull images tagged with previous commit
3. Restart containers with previous version
4. Validate health checks
5. Send Slack alert to #deployments
```

### Manual Rollback

**For emergency situations**:

```bash
#!/bin/bash
# scripts/rollback-production.sh

set -e

PROD_HOST="${PROD_HOST:-prod.example.com}"
PROD_USER="${PROD_USER:-deploy}"
ROLLBACK_SHA="${1:-HEAD~1}"  # Previous commit

echo "🔄 Rolling back to: $ROLLBACK_SHA"

# Get previous image tags
PREVIOUS_TAG=$(git rev-parse $ROLLBACK_SHA | cut -c1-7)

# Rollback
ssh -i ~/.ssh/prod_key $PROD_USER@$PROD_HOST << ROLLBACK
  cd /app
  
  # Pull previous images
  docker pull ghcr.io/doganlap/SBG4.6/saas-admin:$PREVIOUS_TAG
  docker pull ghcr.io/doganlap/SBG4.6/tenant-admin:$PREVIOUS_TAG
  docker pull ghcr.io/doganlap/SBG4.6/customer-portal:$PREVIOUS_TAG
  docker pull ghcr.io/doganlap/SBG4.6/base:$PREVIOUS_TAG
  
  # Update compose
  docker tag ghcr.io/doganlap/SBG4.6/saas-admin:$PREVIOUS_TAG saas-admin:latest
  docker tag ghcr.io/doganlap/SBG4.6/base:$PREVIOUS_TAG base:latest
  
  # Restart with previous versions
  docker compose down --timeout=30
  docker compose up -d
  
  # Verify
  sleep 10
  docker compose ps
  curl -f http://localhost:3000/health
  
  echo "✅ Rollback completed"
ROLLBACK

echo "🔄 Rollback successful - previous version is running"
```

Run:
```bash
chmod +x scripts/rollback-production.sh
./scripts/rollback-production.sh HEAD~1  # Rollback to previous commit
```

## Health Checks

### Endpoint Verification

**SaaS Admin Portal** (port 3000):
```bash
curl -v http://localhost:3000/health
# Expected: 200 OK
```

**Tenant Admin Portal** (port 3001):
```bash
curl -v http://localhost:3001/health
# Expected: 200 OK
```

**Customer Portal** (port 3002):
```bash
curl -v http://localhost:3002/health
# Expected: 200 OK
```

**ERPNext** (port 8000):
```bash
curl -v http://localhost:8000/api/method/ping
# Expected: 200 OK with JSON response
```

**MariaDB**:
```bash
docker compose exec mariadb \
  mysql -u root -p${DB_ROOT_PASSWORD} -e "SELECT 1"
# Expected: 1
```

**Redis**:
```bash
docker compose exec redis-cache redis-cli ping
# Expected: PONG
```

### Comprehensive Health Check Script

```bash
#!/bin/bash
# scripts/health-check.sh

PORTALS=(3000 3001 3002)
TIMEOUT=5
RETRIES=3

echo "🏥 Running health checks..."

# Check portals
for port in "${PORTALS[@]}"; do
  echo -n "Portal :$port ... "
  for i in $(seq 1 $RETRIES); do
    if curl -sf --max-time $TIMEOUT http://localhost:$port/health > /dev/null; then
      echo "✅"
      break
    fi
    [ $i -eq $RETRIES ] && echo "❌ Failed"
  done
done

# Check ERPNext
echo -n "ERPNext:8000 ... "
if curl -sf --max-time $TIMEOUT http://localhost:8000/api/method/ping > /dev/null; then
  echo "✅"
else
  echo "❌ Failed"
fi

# Check database
echo -n "MariaDB:3306 ... "
if docker compose exec -T mariadb mysql -u root -p${DB_ROOT_PASSWORD} -e "SELECT 1" > /dev/null 2>&1; then
  echo "✅"
else
  echo "❌ Failed"
fi

echo "Health checks complete"
```

## Troubleshooting

### Deployment Hangs at "Waiting for approval"

**Cause**: No reviewers configured in production environment
**Solution**:
```bash
# 1. Go to GitHub Settings → Environments → production
# 2. Add "Required reviewers"
# 3. Or bypass with:
gh workflow run deploy.yml \
  -f skip_approval=true
```

### Health Check Timeout

**Cause**: Portal not responding within 10 seconds
**Debug**:
```bash
# Check container logs
docker logs saas-admin-portal
docker logs tenant-admin-portal

# Check port binding
docker compose ps
netstat -tulpn | grep 3000

# Test manually
curl -v http://localhost:3000
```

### Database Migration Failed

**Cause**: Schema update incompatible with running code
**Solution**:
```bash
# Rollback to previous version
./scripts/rollback-production.sh

# Investigate issue
docker logs erpnext-backend | grep ERROR

# Fix schema migration
# Edit database-schemas/*sql
# Re-deploy

# Manual schema fix (advanced):
docker compose exec mariadb mysql \
  -u root -p${DB_ROOT_PASSWORD} saas_admin \
  -e "SHOW CREATE TABLE tenants\G"
```

### Container Restart Loop

**Cause**: Insufficient memory or disk space
**Debug**:
```bash
# Check Docker resources
docker stats

# Check disk usage
du -sh /var/lib/docker/
df -h /

# Free up space
docker compose down
docker system prune -a --volumes
```

### SSL/TLS Certificate Expired

**For let's encrypt certificates**:
```bash
# Renew certificates
docker compose exec nginx certbot renew --quiet

# Restart nginx to load new certs
docker compose restart nginx
```

### Slack Notifications Not Sending

**Cause**: Webhook URL expired or invalid
**Debug**:
```bash
# Test webhook manually
curl -X POST -H 'Content-type: application/json' \
  --data '{"text":"test"}' \
  $SLACK_WEBHOOK_URL

# Regenerate webhook in Slack workspace
# Update SLACK_WEBHOOK secret in GitHub
```

## Post-Deployment Validation

After successful deployment:

1. **Verify Data Integrity**:
   ```sql
   SELECT COUNT(*) FROM tenants;
   SELECT MAX(created_at) FROM subscriptions;
   ```

2. **Check Recent Logs**:
   ```bash
   docker compose logs --tail=100 saas-admin-portal
   docker compose logs --tail=100 erpnext-backend
   ```

3. **Smoke Test Critical Flows**:
   - [ ] Admin can login
   - [ ] Create new tenant
   - [ ] Access tenant admin portal
   - [ ] View ERPNext module
   - [ ] Process payment

4. **Monitor Metrics**:
   - CPU usage: < 70%
   - Memory usage: < 80%
   - Disk I/O: < 60%
   - Response time: p95 < 500ms

5. **User Communication**:
   - Announce deployment completion
   - Share release notes
   - Open feedback channel
