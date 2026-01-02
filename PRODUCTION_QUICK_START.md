# Production Deployment & Email Recovery - Quick Start Guide

**Status**: ✅ **ALL IMPLEMENTATION COMPLETE**  
**Date**: January 2, 2026

---

## What Was Implemented

### ✅ Production Deployment Pipeline
- **deploy.yml workflow** - Staging auto-deploy + production approval + automatic rollback
- **deploy-production.sh script** - 10-phase deployment with health checks and email setup
- **Backup & recovery** - Automatic database backups before every deployment
- **Health monitoring** - 30 retries with 10-second intervals between checks

### ✅ Email Recovery System  
- **docs/EMAIL_RECOVERY.md** - Complete configuration and usage guide
- **Password reset tokens** - Secure tokens with 30-minute expiry
- **Email providers** - Support for Gmail, SendGrid, Office 365
- **Database schema** - Password tokens, email logs, verification tables

### ✅ Configuration Files
- **configs/common.env** - Email settings, production URLs, security configuration
- **GitHub Actions workflow** - Full staging/production deployment automation
- **Deployment script** - Production-ready bash script with logging

---

## Quick Start: Deploy to Production

### STEP 1: Setup Infrastructure (First Time Only)

```bash
# 1. Create production server (Ubuntu 22.04)
#    - 8+ CPU cores
#    - 32+ GB RAM
#    - 200+ GB SSD storage
#    - SSH access enabled

# 2. Install Docker & Docker Compose
ssh user@prod-server
curl -fsSL https://get.docker.com | sh
docker compose version

# 3. Create deployment directory
mkdir -p /opt/erpnext-saas
mkdir -p /opt/erpnext-saas/backups
mkdir -p /opt/erpnext-saas/deployments
```

### STEP 2: Configure GitHub Secrets

Go to: **GitHub Settings → Secrets and variables → New secret**

```
PROD_SSH_KEY
├─ Value: [Paste your private SSH key]
├─ Private key in PEM format

PROD_HOST  
├─ Value: prod.example.com
├─ Your production server hostname/IP

PROD_DOMAIN
├─ Value: yourdomain.com
├─ Used for email and URLs

MAIL_USERNAME
├─ Value: your-email@gmail.com
├─ Gmail or SendGrid account

MAIL_PASSWORD
├─ Value: [App-specific password]
├─ NOT your regular Gmail password

SLACK_WEBHOOK (optional)
├─ Value: https://hooks.slack.com/services/...
├─ For deployment notifications
```

### STEP 3: Update Configuration

Edit `configs/common.env`:

```bash
# Email Provider (Gmail example)
MAIL_SERVER=smtp.gmail.com
MAIL_PORT=587
MAIL_USE_TLS=true

# Production URLs
PROD_DOMAIN=yourdomain.com
SAAS_ADMIN_URL=https://admin.yourdomain.com
CUSTOMER_PORTAL_URL=https://portal.yourdomain.com

# Security
SSL_ENABLED=true
PASSWORD_MIN_LENGTH=8
PASSWORD_REQUIRE_UPPERCASE=true
```

### STEP 4: Deploy to Staging (Test)

```bash
# Push to develop branch
git checkout develop
git merge main
git push origin develop

# Monitor in GitHub Actions
# Actions → Deploy workflow → deploy-staging job

# Verify staging at:
# http://your-staging-server:3000 (SaaS Admin)
# http://your-staging-server:3002 (Customer Portal)
```

### STEP 5: Deploy to Production (Approval Required)

```bash
# Push to main branch
git checkout main
git merge develop
git push origin main

# GitHub Actions will:
# 1. Run CI checks (lint, type-check)
# 2. Build and push Docker images
# 3. Run tests
# 4. PAUSE and wait for approval

# Go to GitHub Actions tab:
# - Click "Deploy to Production" job
# - Click "Review deployments"
# - Select "production" environment
# - Click "Approve and deploy"

# Once approved:
# - SSH to production server
# - Pull latest images
# - Start services
# - Run health checks
# - Setup email recovery
# - Create backup
```

---

## Customer Email Recovery Workflow

### For End Users:

**1. Forgot Password**
```
Go to Customer Portal
  ↓
Click "Forgot Password"
  ↓
Enter email address
  ↓
Check email for reset link
```

**2. Reset Password**
```
Open reset email (expires in 30 min)
  ↓
Click "Reset Password" link
  ↓
Enter new password
  ↓
Login with new password
```

**3. Access Subscriptions**
```
Dashboard
  ↓
Click "Subscriptions"
  ↓
View active plans, billing, usage
```

### For Administrators:

**Check Email Delivery**
```bash
# SSH to production
ssh deployer@prod-server

# View email logs
docker compose exec -T mariadb mysql \
  -e "SELECT recipient_email, email_type, status, sent_at \
      FROM email_logs \
      ORDER BY sent_at DESC \
      LIMIT 20;"
```

---

## Deployment Verification

### Check All Services Running

```bash
ssh deployer@prod-server
cd /opt/erpnext-saas

# Verify containers
docker compose ps

# Expected output:
# STATUS: Up (running)
# Services: mariadb, redis-cache, redis-queue, redis-socketio
#           saas-admin-portal, tenant-admin-portal, customer-portal
#           erpnext modules
```

### Test Email Configuration

```bash
# Test SMTP connection
docker compose exec -T backend python3 << 'EOF'
import smtplib, os
smtp = smtplib.SMTP(os.getenv('MAIL_SERVER'), 587)
smtp.starttls()
smtp.login(os.getenv('MAIL_USERNAME'), os.getenv('MAIL_PASSWORD'))
print("✓ Email configured correctly")
smtp.quit()
EOF
```

### Test API Endpoints

```bash
# Customer Portal API
curl -s http://localhost:3002/api/subscriptions

# SaaS Admin API
curl -s http://localhost:3000/api/health

# ERPNext API
curl -s http://localhost:8000/api/method/frappe.auth.get_logged_user
```

---

## Monitoring & Maintenance

### View Logs

```bash
# Application logs
ssh deployer@prod-server
cd /opt/erpnext-saas
docker compose logs -f saas-admin-portal

# Database logs
docker compose logs -f mariadb | grep -i error

# Email logs (database)
docker compose exec -T mariadb mysql \
  -e "SELECT * FROM email_logs WHERE status != 'sent';"
```

### Backup Management

```bash
# Backups are created before every deployment
# Located in: /opt/erpnext-saas/backups/

ssh deployer@prod-server
cd /opt/erpnext-saas/backups

# List all backups
ls -lh backup-*.sql

# Manual backup
docker compose exec -T mariadb mysqldump \
  --all-databases --single-transaction \
  > backup-manual-$(date +%Y%m%d-%H%M%S).sql
```

### Automatic Rollback

If deployment fails:
1. ✅ GitHub Actions automatically triggers rollback
2. ✅ Previous database backup is restored
3. ✅ Previous container versions started
4. ✅ Health checks verify rollback success
5. ✅ Slack notification sent
6. ✅ Deployment record created

---

## Troubleshooting

### Email Not Sending

**Problem**: "Password reset emails not received"

**Solution**:
```bash
# 1. Check SMTP credentials
grep MAIL_ configs/common.env

# 2. Verify email service is running
docker compose logs backend | grep -i mail

# 3. Check for bounced emails
docker compose exec -T mariadb mysql \
  -e "SELECT * FROM email_logs WHERE status = 'failed';"

# 4. For Gmail: Use app-specific password (not Gmail password)
# 5. For SendGrid: API key must be in MAIL_PASSWORD
```

### Services Won't Start

**Problem**: "Services failing to start after deployment"

**Solution**:
```bash
# 1. Check service status
docker compose ps

# 2. View logs
docker compose logs mariadb  # Check database started
docker compose logs --tail=50  # Last 50 lines all services

# 3. Check disk space
df -h /opt/erpnext-saas

# 4. Trigger automatic rollback
# Push rollback-production.sh script to execute
# Or manually run:
docker compose down && docker compose up -d
```

### Health Check Timeouts

**Problem**: "Health checks timing out"

**Solution**:
```bash
# Services need time to start
# Health checks wait 30 × 10 = 300 seconds (5 minutes)

# Monitor startup
watch docker compose ps

# Increase timeout if needed (edit deploy.yml):
HEALTH_CHECK_RETRIES=60  # Instead of 30
```

---

## File Reference

| File | Purpose | Location |
|------|---------|----------|
| `deploy.yml` | GitHub Actions workflow | `.github/workflows/` |
| `deploy-production.sh` | Production deployment script | `scripts/` |
| `EMAIL_RECOVERY.md` | Email recovery guide | `docs/` |
| `common.env` | Configuration | `configs/` |

---

## Next Steps Checklist

- [ ] Setup production server (Ubuntu 22.04, Docker installed)
- [ ] Add GitHub secrets (SSH key, host, email credentials)
- [ ] Update `configs/common.env` with your domains
- [ ] Configure email provider (Gmail, SendGrid, etc.)
- [ ] Push to develop branch → test staging deployment
- [ ] Approve production deployment in GitHub
- [ ] Verify services running (`docker compose ps`)
- [ ] Test customer password recovery
- [ ] Monitor logs for errors
- [ ] Setup Slack notifications (optional)

---

## Production Checklist

Before going live:

```
INFRASTRUCTURE
  ☐ Production server provisioned
  ☐ 8+ CPU, 32+ GB RAM, 200+ GB SSD
  ☐ SSH access working
  ☐ Docker & Docker Compose installed
  ☐ Firewall rules allow 80, 443

GITHUB CONFIGURATION
  ☐ All secrets added (PROD_SSH_KEY, PROD_HOST, etc.)
  ☐ Production environment created
  ☐ Required reviewers configured
  ☐ Branch protection enabled

EMAIL SETUP
  ☐ Email provider account active (Gmail/SendGrid)
  ☐ SMTP credentials tested locally
  ☐ SPF/DKIM/DMARC DNS records added
  ☐ MAIL_USERNAME and MAIL_PASSWORD set in secrets
  ☐ MAIL_DEFAULT_SENDER matches domain

DOMAINS & CERTIFICATES
  ☐ Domain registered
  ☐ SSL certificates issued (Let's Encrypt)
  ☐ CNAME records configured for subdomains
  ☐ PROD_DOMAIN set in environment

TESTING
  ☐ Staging deployment successful
  ☐ Smoke tests passed
  ☐ Email recovery tested end-to-end
  ☐ Rollback procedure tested

MONITORING
  ☐ Slack webhook configured (optional)
  ☐ Email logs being collected
  ☐ Health checks verified
  ☐ Backup location verified
```

---

## Support Resources

| Resource | Path |
|----------|------|
| Email Recovery Guide | `docs/EMAIL_RECOVERY.md` |
| CI/CD Documentation | `docs/CI_CD_PIPELINE.md` |
| Deployment Guide | `docs/DEPLOYMENT_GUIDE.md` |
| Pre-Flight Checklist | `docs/PRE_FLIGHT_CHECKLIST.md` |
| Architecture Guide | `.github/copilot-instructions.md` |

---

## Key Features Summary

✅ **Automated Deployment**
- Push to main → Auto-deploy with approval gate
- All services health-checked before completion
- Automatic rollback if any check fails

✅ **Email Recovery**
- Customers can reset passwords via email
- 30-minute secure token expiry
- Support for Gmail, SendGrid, Office 365

✅ **Data Protection**
- Automatic backups before every deployment
- Encrypted password storage (bcrypt)
- Audit logging for all actions

✅ **Production Ready**
- Health monitoring with retries
- Slack notifications
- Comprehensive logging
- Deployment records

---

**Implementation Date**: January 2, 2026  
**Status**: ✅ Complete & Ready for Launch  
**Repository**: https://github.com/doganlap/SBG4.6
