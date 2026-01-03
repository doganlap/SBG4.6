#!/bin/bash
#==============================================================================
# Setup Let's Encrypt SSL Certificates
#==============================================================================

# Install certbot if not installed
if ! command -v certbot &> /dev/null; then
    echo "Installing certbot..."
    sudo apt update
    sudo apt install -y certbot python3-certbot-nginx
fi

# Stop nginx temporarily
echo "Stopping nginx..."
docker stop nginx-ssl 2>/dev/null || true

# Get certificates for all domains
echo "Obtaining SSL certificates..."
sudo certbot certonly --standalone \
    -d doganconsult.com \
    -d www.doganconsult.com \
    -d admin.doganconsult.com \
    -d os.doganconsult.com \
    -d tenant.doganconsult.com \
    -d portal.doganconsult.com \
    -d demo.doganconsult.com \
    --non-interactive \
    --agree-tos \
    --email admin@doganconsult.com \
    --no-eff-email

# Create symlinks for nginx
echo "Creating certificate links..."
sudo mkdir -p /home/dogan/erpnext-saas-platform/docker/nginx/letsencrypt
sudo ln -sf /etc/letsencrypt/live/doganconsult.com/fullchain.pem /home/dogan/erpnext-saas-platform/docker/nginx/letsencrypt/fullchain.pem
sudo ln -sf /etc/letsencrypt/live/doganconsult.com/privkey.pem /home/dogan/erpnext-saas-platform/docker/nginx/letsencrypt/privkey.pem

echo "Certificates installed successfully!"
echo "Now update nginx config to use Let's Encrypt certificates"