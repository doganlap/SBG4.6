# Email Recovery & Customer Subscription Access

**Purpose**: Enable customers to recover their account access and manage subscriptions via email-based password reset.

---

## Overview

The email recovery system allows:
- ✅ Customers to reset forgotten passwords
- ✅ Email-based account recovery
- ✅ Access to subscription details, billing, and usage
- ✅ Secure token-based verification (30-minute expiry)
- ✅ Automatic email delivery via configured SMTP

---

## Configuration

### Step 1: Configure Email Provider

Edit `configs/common.env`:

```bash
# Gmail Example
MAIL_SERVER=smtp.gmail.com
MAIL_PORT=587
MAIL_USE_TLS=true
MAIL_USERNAME=your-email@gmail.com
MAIL_PASSWORD=your-app-specific-password  # Use app password, not Gmail password
MAIL_DEFAULT_SENDER=noreply@yourdomain.com
MAIL_FROM_NAME=ERPNext SaaS Platform

# Alternative: SendGrid
MAIL_SERVER=smtp.sendgrid.net
MAIL_PORT=587
MAIL_USERNAME=apikey
MAIL_PASSWORD=SG.xxxxxxxxxxxxx  # SendGrid API key

# Alternative: Office 365
MAIL_SERVER=smtp.office365.com
MAIL_PORT=587
MAIL_USE_TLS=true
MAIL_USERNAME=your-email@company.com
MAIL_PASSWORD=your-password
```

### Step 2: Recovery Settings

```bash
# Password Reset Token Expiry (seconds)
PASSWORD_RESET_TOKEN_EXPIRY=1800  # 30 minutes

# Session Timeout
SESSION_TIMEOUT=3600  # 1 hour

# Password Requirements
PASSWORD_MIN_LENGTH=8
PASSWORD_REQUIRE_UPPERCASE=true
PASSWORD_REQUIRE_NUMBERS=true
PASSWORD_REQUIRE_SPECIAL=true
```

### Step 3: Email Templates

**Password Recovery Email**:
```
Subject: Reset Your ERPNext SaaS Password

Dear [Customer Name],

You requested to reset your password for your ERPNext SaaS account.

Click the link below to reset your password:
https://portal.yourdomain.com/reset-password?token=[RESET_TOKEN]

This link expires in 30 minutes.

If you didn't request this, please ignore this email.

Best regards,
ERPNext SaaS Team
```

---

## Customer Workflow

### 1. Forgot Password

```
Customer Portal
    ↓
Click "Forgot Password"
    ↓
Enter email address
    ↓
System sends reset email
    ↓
Customer receives email (2-3 minutes)
```

### 2. Password Reset

```
Customer opens email
    ↓
Clicks "Reset Password" link
    ↓
Portal loads reset form
    ↓
Enter new password
    ↓
System validates password strength
    ↓
Password updated successfully
    ↓
Redirect to login
```

### 3. Access Subscriptions

```
Login with email & new password
    ↓
Portal Dashboard loads
    ↓
View Subscriptions tab
    ↓
See:
   - Active subscriptions
   - Billing status
   - Usage metrics
   - Invoice history
   - Payment methods
```

---

## Implementation Details

### Database Schema

Password recovery uses the `tenant_admin` schema:

```sql
-- User credentials table
CREATE TABLE users (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Password reset tokens table
CREATE TABLE password_reset_tokens (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    user_id BIGINT NOT NULL,
    token VARCHAR(255) UNIQUE NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    used_at TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    INDEX idx_token (token),
    INDEX idx_expires (expires_at)
);

-- Email verification table
CREATE TABLE email_verifications (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    user_id BIGINT NOT NULL,
    token VARCHAR(255) UNIQUE NOT NULL,
    email VARCHAR(255) NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    verified_at TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- Email delivery log
CREATE TABLE email_logs (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    recipient_email VARCHAR(255) NOT NULL,
    subject VARCHAR(255) NOT NULL,
    email_type VARCHAR(50) NOT NULL,  -- 'password_reset', 'subscription_access'
    sent_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(50) NOT NULL DEFAULT 'sent',  -- 'sent', 'failed', 'bounced'
    error_message TEXT NULL,
    user_id BIGINT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL,
    INDEX idx_recipient (recipient_email),
    INDEX idx_type (email_type),
    INDEX idx_sent_at (sent_at)
);
```

### API Endpoints

**POST /api/auth/forgot-password**
```json
{
  "email": "customer@example.com"
}

Response (200):
{
  "success": true,
  "message": "Password reset email sent"
}
```

**POST /api/auth/reset-password**
```json
{
  "token": "abc123def456...",
  "new_password": "SecurePassword123!"
}

Response (200):
{
  "success": true,
  "message": "Password reset successful"
}
```

**GET /api/subscriptions/me**
```
Headers: Authorization: Bearer [JWT_TOKEN]

Response (200):
{
  "subscriptions": [
    {
      "id": "sub_123",
      "plan": "Professional",
      "status": "active",
      "billing_email": "customer@example.com",
      "billing_period_start": "2026-01-01",
      "billing_period_end": "2026-02-01",
      "amount_due": 99.99,
      "next_billing_date": "2026-02-01"
    }
  ]
}
```

---

## Testing Email Recovery

### Local Development

```bash
# 1. Start the platform
docker compose -f docker/docker-compose.yml up -d

# 2. Check mail logs (development uses console output)
docker compose logs backend | grep -i email

# 3. Test password reset
curl -X POST http://localhost:3002/api/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '{"email":"customer@example.com"}'

# 4. Check email logs in database
docker compose exec -T mariadb mysql -u root -p$DB_ROOT_PASSWORD \
  -e "SELECT * FROM tenant_admin_db.email_logs ORDER BY sent_at DESC LIMIT 5;"
```

### Production Testing

```bash
# 1. SSH to production
ssh deployer@prod-server

# 2. Test email configuration
cd /opt/erpnext-saas
docker compose exec -T backend python3 << 'EOF'
import smtplib
from email.mime.text import MIMEText

smtp = smtplib.SMTP("smtp.gmail.com", 587)
smtp.starttls()
smtp.login(os.getenv('MAIL_USERNAME'), os.getenv('MAIL_PASSWORD'))
msg = MIMEText("Test email from ERPNext SaaS")
msg['Subject'] = 'Test Email'
msg['From'] = os.getenv('MAIL_DEFAULT_SENDER')
smtp.send_message(msg, to_addrs=["test@example.com"])
smtp.quit()
print("✓ Email sent successfully")
EOF

# 3. Check email delivery logs
docker compose exec -T mariadb mysql \
  -e "SELECT recipient_email, status, sent_at FROM email_logs \
      ORDER BY sent_at DESC LIMIT 10;"
```

---

## Troubleshooting

### Email Not Sending

**Problem**: Password reset emails not delivered

**Solutions**:

1. **Check SMTP configuration**
```bash
docker compose logs backend | grep -i smtp
```

2. **Verify credentials**
   - Gmail: Use app-specific password (not Gmail password)
   - SendGrid: Use API key as password, 'apikey' as username
   - Office 365: Use full email address as username

3. **Check firewall**
```bash
# Test SMTP port
nc -zv smtp.gmail.com 587
```

4. **Review email logs**
```sql
SELECT * FROM email_logs 
WHERE status != 'sent' 
ORDER BY sent_at DESC 
LIMIT 10;
```

### Token Expired

**Problem**: Reset link shows "token expired"

**Solutions**:

1. **Check token expiry setting**
```bash
# Should be at least 1800 seconds (30 minutes)
grep PASSWORD_RESET_TOKEN_EXPIRY configs/common.env
```

2. **Check server time sync**
```bash
# Ensure server time is synchronized
timedatectl status

# Sync if needed
sudo timedatectl set-ntp true
```

### Email Bouncing

**Problem**: Emails returned as undeliverable

**Solutions**:

1. **SPF/DKIM/DMARC configuration**
   - Configure SPF record in DNS for your domain
   - Enable DKIM in email provider
   - Set up DMARC policy

2. **Example SPF record**
```
v=spf1 include:sendgrid.net ~all
```

3. **Check sender email**
   - Should match your domain: `noreply@yourdomain.com`
   - Update `MAIL_DEFAULT_SENDER` if needed

---

## Security Considerations

### Password Reset Tokens

- ✅ Tokens are cryptographically random (128+ bits)
- ✅ Tokens expire after 30 minutes
- ✅ Tokens are single-use only
- ✅ Tokens are stored as hashes in database

### Email Verification

- ✅ All email addresses verified before account access
- ✅ Verification links expire after 1 hour
- ✅ Rate limiting on password reset requests (5 per hour)

### Data Protection

- ✅ Passwords hashed with bcrypt (12 rounds)
- ✅ Email delivery logged for audit trail
- ✅ HTTPS enforced for all authentication endpoints
- ✅ Secure session cookies (HttpOnly, Secure, SameSite)

---

## Monitoring & Alerts

### Email Delivery Metrics

```sql
-- Daily email delivery rate
SELECT 
    DATE(sent_at) as date,
    COUNT(*) as total_sent,
    SUM(CASE WHEN status = 'sent' THEN 1 ELSE 0 END) as successful,
    SUM(CASE WHEN status = 'failed' THEN 1 ELSE 0 END) as failed,
    ROUND(100 * SUM(CASE WHEN status = 'sent' THEN 1 ELSE 0 END) / COUNT(*), 2) as success_rate
FROM email_logs
GROUP BY DATE(sent_at)
ORDER BY date DESC
LIMIT 30;
```

### Configure Alerts

Set up alerts for:
- Email delivery failure rate > 5%
- Password reset token generation spike
- Multiple failed reset attempts from same IP

---

## Production Deployment Checklist

Before deploying to production:

- [ ] Email provider account created (Gmail, SendGrid, etc.)
- [ ] SMTP credentials tested in development
- [ ] SPF/DKIM/DMARC DNS records configured
- [ ] `MAIL_USERNAME` and `MAIL_PASSWORD` set in production secrets
- [ ] `MAIL_DEFAULT_SENDER` matches domain
- [ ] Database tables created (password_reset_tokens, email_verifications)
- [ ] Rate limiting configured
- [ ] Email logging enabled
- [ ] Health check monitoring configured
- [ ] Rollback procedure tested

---

## Next Steps

1. **Configure email provider**: Choose and setup Gmail, SendGrid, or Office 365
2. **Update secrets**: Add SMTP credentials to production environment
3. **Test recovery flow**: Send test password reset emails
4. **Monitor delivery**: Check email logs for failures
5. **Configure alerts**: Set up monitoring for delivery issues

---

**Documentation Version**: 1.0  
**Last Updated**: January 2, 2026
