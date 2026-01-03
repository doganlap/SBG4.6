#!/bin/bash
#==============================================================================
# TLS Certificate Setup Script
# Obtains Let's Encrypt certificates for all Dogan Consult domains
#==============================================================================

set -e

echo "Installing certbot..."
apt-get update
apt-get install -y certbot

echo "Obtaining wildcard certificate for doganconsult.com..."
echo "This requires DNS-01 challenge (manual DNS TXT record)"

certbot certonly --manual --preferred-challenges dns \
  -d doganconsult.com \
  -d "*.doganconsult.com" \
  -d "*.os.doganconsult.com" \
  --agree-tos \
  --email support@doganconsult.com \
  --no-eff-email

echo ""
echo "Certificate obtained successfully!"
echo "Certificates stored in: /etc/letsencrypt/live/doganconsult.com/"
echo ""
echo "Next steps:"
echo "1. Ensure Nginx container has access to /etc/letsencrypt"
echo "2. Mount certificates in docker-compose.yml:"
echo "   volumes:"
echo "     - /etc/letsencrypt:/etc/letsencrypt:ro"
echo "3. Restart Nginx to load SSL config"
echo ""
echo "Certificate auto-renewal (add to crontab):"
echo "0 3 * * * certbot renew --quiet && docker exec nginx nginx -s reload"
