# CI/CD Pipeline Documentation

## Overview

The ERPNext SaaS platform uses comprehensive GitHub Actions workflows to ensure code quality, security, and reliable deployments across development, staging, and production environments.

## Pipeline Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                     Code Push / PR                              │
├─────────────────────────────────────────────────────────────────┤
│  ├── Code Quality (ESLint, TypeScript)                          │
│  ├── Security Scanning (Trivy, Snyk, SonarQube)                 │
│  └── Build Docker Images (Frontend & Module)                    │
├─────────────────────────────────────────────────────────────────┤
│  ├── Unit Tests (Jest/Vitest)                                   │
│  ├── Integration Tests (MariaDB)                                │
│  ├── E2E Tests (Playwright)                                     │
│  └── Performance Tests (K6 Load Testing)                        │
├─────────────────────────────────────────────────────────────────┤
│  ├── Deploy to Staging (with approvals)                         │
│  ├── Smoke Tests                                                │
│  └── Health Checks                                              │
├─────────────────────────────────────────────────────────────────┤
│  ├── Deploy to Production (main branch only)                    │
│  ├── Health Checks & Validation                                 │
│  └── Automatic Rollback on Failure                              │
└─────────────────────────────────────────────────────────────────┘
```

## Workflows

### 1. Continuous Integration (CI)

**File**: `.github/workflows/ci.yml`

**Triggers**: 
- Push to `main` or `develop`
- Pull requests to `main` or `develop`

**Jobs**:
- **Lint Portals**: ESLint + TypeScript checks for all 4 frontend portals
- **Validate Schemas**: Check SQL database schemas for syntax
- **Validate Scripts**: Shell script linting with shellcheck
- **Docker Build Test**: Verify base module Dockerfile builds successfully

**Configuration**:
```yaml
on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]
```

### 2. Build Pipeline (Phase 5.2)

**File**: `.github/workflows/build.yml`

**Triggers**:
- Push to `main` or `develop` with changes in Docker or portal files
- Pull requests affecting Docker configuration

**Jobs**:

#### Frontend Images
Builds and pushes Next.js portal Docker images:
- `saas-admin-portal` → `saas-admin:latest`
- `tenant-admin-portal` → `tenant-admin:latest`
- `customer-portal` → `customer-portal:latest`
- `showcase-pages` → `showcase:latest`

Tags applied:
- `branch-name` (for branch builds)
- `sha-hash` (commit-specific)
- `version` (semantic versioning)

#### Module Images
Builds ERPNext module images:
- Base image (builds from `frappe/erpnext:v16`)
- 9 module images (accounting, crm, selling, buying, stock, hr, education, healthcare, manufacturing)

#### Code Quality
- **SonarQube**: Static code analysis (requires `SONAR_TOKEN` secret)
- Detects code smells, bugs, vulnerabilities
- Enforces quality gates

#### Security Scanning
- **Trivy**: Scans for filesystem vulnerabilities
- **Snyk**: Dependency vulnerability detection (requires `SNYK_TOKEN` secret)
- **License Checker**: Ensures compliant OSS licenses
  - Allowed: MIT, Apache-2.0, BSD, ISC
  - Rejected: GPL, AGPL, proprietary

**Registry Setup**:
Images pushed to GitHub Container Registry (GHCR):
```
ghcr.io/doganlap/SBG4.6/saas-admin:latest
ghcr.io/doganlap/SBG4.6/tenant-admin:latest
```

**Required Secrets**:
```
SONAR_TOKEN      # SonarCloud token
SNYK_TOKEN       # Snyk.io token (optional)
GITHUB_TOKEN     # Auto-provided by GitHub
```

### 3. Test Pipeline (Phase 5.3)

**File**: `.github/workflows/test.yml`

**Triggers**:
- Push to `main` or `develop`
- Pull requests
- Daily schedule (2 AM UTC)

**Jobs**:

#### Unit Tests
Runs for all 4 frontend portals with Node.js 18:
```bash
npm test -- --coverage --watchAll=false
```
- Coverage reports uploaded to Codecov
- Threshold: 80% coverage (configurable)

#### Integration Tests
Uses Docker service: MariaDB 10.6
```sql
-- Database schemas are imported
-- Validates:
-- 1. saas_admin schema
-- 2. tenant_admin schema
-- 3. subscriptions_billing schema
-- 4. container_management schema
```

**Health Check**:
```sql
SELECT 1 FROM information_schema.tables 
WHERE table_schema='saas_admin' LIMIT 1
```

#### E2E Tests
Playwright automation:
- Tests portal navigation
- Form submissions
- API integration flows
- Multi-tenant isolation scenarios

**Example Test**:
```typescript
test('SaaS Admin Portal loads', async ({ page }) => {
  await page.goto('http://localhost:3000');
  await expect(page).toHaveTitle(/Dashboard/i);
});
```

#### Performance Tests
K6 load testing:
- Simulates 10 concurrent users
- 30-second test duration
- SLA: p95 response time < 500ms
- Failure rate < 0.1%

**Sample Test**:
```javascript
export const options = {
  vus: 10,
  duration: '30s',
  thresholds: {
    'http_req_duration': ['p(95)<500'],
    'http_req_failed': ['<0.1'],
  },
};
```

#### Test Report
Aggregates results from all test suites and posts to PR.

### 4. Deployment Pipeline (Phase 5.4)

**File**: `.github/workflows/deploy.yml`

**Trigger Rules**:
- `main` branch → Production (requires approval)
- `develop` branch → Staging (automatic)
- Manual dispatch with environment selection

#### Staging Deployment

**Condition**: 
```yaml
if: github.ref == 'refs/heads/develop' || 
    (github.event_name == 'workflow_dispatch' && 
     github.event.inputs.environment == 'staging')
```

**Steps**:
1. Pull latest Docker images from GHCR
2. SSH to staging server
3. Run `docker compose pull && docker compose up -d`
4. Execute smoke tests
5. Slack notification

**Smoke Tests**:
- Verify portal availability (HTTP 200)
- Database connectivity
- ERPNext health check

#### Production Deployment

**Condition**:
```yaml
if: github.ref == 'refs/heads/main' && 
    github.event_name == 'push'
```

**Environment Gates**:
- Requires approval in GitHub "production" environment
- No concurrent deployments (concurrency locks)
- Creates deployment record in GitHub API

**Steps**:
1. Wait for approval (manual step in GitHub UI)
2. Record deployment in GitHub
3. SSH to production server
4. Deploy containers
5. Run health checks (up to 30 retries, 10s intervals)
6. Update deployment status
7. Slack notification to team

**Health Check**:
```bash
curl -f https://sbg4.6.example.com/health
```
Success: HTTP 200 response

#### Automatic Rollback

**Triggered On**: Production deployment failure

**Steps**:
1. Checkout previous commit
2. SSH to production server
3. Pull previous Docker images
4. Restart containers with previous version
5. Verify rollback with health checks
6. Slack alert to team

**Rollback Decision Tree**:
```
Deployment Failed
  ├─ Health Check Timeout (30s)
  └─ HTTP 5xx / Connection Refused
       └─ Trigger Rollback
            ├─ Restore Previous Image Tags
            ├─ Verify Health Check (200 OK)
            └─ Notify #deployments channel
```

## Required GitHub Secrets

**For All Workflows**:
```
GITHUB_TOKEN          # Auto-provided, scope: contents, packages
```

**For Build Workflow**:
```
SONAR_TOKEN          # SonarCloud.io token (optional)
SNYK_TOKEN           # Snyk.io token (optional)
```

**For Deployment Workflows**:
```
STAGING_SSH_KEY      # Private SSH key for staging server
STAGING_HOST         # Staging server hostname
PROD_SSH_KEY         # Private SSH key for production server
PROD_HOST            # Production server hostname
SLACK_WEBHOOK        # Slack channel webhook URL (optional)
```

**Setup Instructions**:
```bash
# Generate SSH keys (no passphrase for automation)
ssh-keygen -t ed25519 -f deployment-key -N ""

# Add to GitHub Secrets
# Settings → Secrets and variables → Actions → New repository secret
# Name: PROD_SSH_KEY
# Value: (contents of private key file)

# Add public key to server
ssh-copy-id -i deployment-key.pub user@prod-server
```

## Environment Variables Configuration

**File**: `configs/common.env`

Template:
```env
# Database
DB_ROOT_PASSWORD=your_secure_password
DB_HOST=mariadb
DB_PORT=3306

# Redis
REDIS_CACHE_HOST=redis-cache
REDIS_QUEUE_HOST=redis-queue
REDIS_SOCKETIO_HOST=redis-socketio

# ERPNext
FRAPPE_BRANCH=version-16
ERPNEXT_BRANCH=version-16

# Application
APP_ENV=production
DEBUG=false
```

**Production Override** (via Docker Compose):
```yaml
environment:
  DB_ROOT_PASSWORD: ${DB_ROOT_PASSWORD}  # From CI/CD secrets
  ADMIN_PASSWORD: ${ADMIN_PASSWORD}
  JWT_SECRET: ${JWT_SECRET}
```

## Deployment Checklist

Before merging to `main`:
- [ ] All CI checks pass (lint, type, build, quality)
- [ ] Unit tests ≥ 80% coverage
- [ ] E2E tests pass
- [ ] Code review approved
- [ ] No security vulnerabilities (Trivy, Snyk)
- [ ] Performance tests pass (p95 < 500ms)
- [ ] Changelog updated
- [ ] Documentation updated

## Local Testing

**Run CI locally**:
```bash
# Install act (GitHub Actions runner)
brew install act

# Run specific workflow
act push -j lint-portals

# Run all workflows
act
```

**Run tests locally**:
```bash
# Unit tests
cd saas-admin-portal && npm test

# E2E tests
npx playwright test

# Load tests
k6 run tests/load/tenant-api.js
```

## Monitoring & Observability

**GitHub Actions Dashboard**:
- https://github.com/doganlap/SBG4.6/actions
- View workflow runs, logs, artifacts

**Workflow Badges**:
Add to README.md:
```markdown
[![CI](https://github.com/doganlap/SBG4.6/workflows/CI/badge.svg)](https://github.com/doganlap/SBG4.6/actions)
[![Build](https://github.com/doganlap/SBG4.6/workflows/Build/badge.svg)](https://github.com/doganlap/SBG4.6/actions)
[![Tests](https://github.com/doganlap/SBG4.6/workflows/Test/badge.svg)](https://github.com/doganlap/SBG4.6/actions)
```

## Troubleshooting

### Build Failure: "Docker layer caching failed"
**Solution**: Push to repository to clear Docker buildx cache
```bash
git commit --allow-empty -m "Clear Docker cache"
git push
```

### Test Timeout: "Health check failed"
**Cause**: MariaDB service slow to start
**Solution**: Increase health check retries in workflow

### Deployment Approval Never Completes
**Cause**: Environment not configured for approvals
**Solution**: 
```bash
# GitHub UI → Settings → Environments → production
# → Add "Required reviewers"
# → Add user/team names
```

### Slack Notifications Not Received
**Cause**: Webhook URL invalid or expired
**Solution**: Regenerate in Slack workspace
```
Slack → Your App → Incoming Webhooks → Add New
```

## Cost Optimization

**GitHub Actions Pricing** (as of 2024):
- Free tier: 2,000 minutes/month
- Storage: 500 MB free, $0.25/GB after
- Recommendation: Cache Docker layers, run tests in parallel

**Optimize Workflows**:
```yaml
# Run independent jobs in parallel
needs: []  # No dependencies

# Cache dependencies
- uses: actions/cache@v3
  with:
    path: node_modules
    key: ${{ runner.os }}-npm-${{ hashFiles('**/package-lock.json') }}

# Use conditional steps
if: github.event_name == 'pull_request'
```

## Future Enhancements

- [ ] Database migration testing in CI
- [ ] Contract testing between ABP and ERPNext APIs
- [ ] Multi-region deployment strategy
- [ ] Canary deployments to production (10% traffic)
- [ ] Blue-green deployment strategy
- [ ] Chaos engineering (failure injection testing)
- [ ] Cost analysis and budgeting
- [ ] Automated security patching with Dependabot
