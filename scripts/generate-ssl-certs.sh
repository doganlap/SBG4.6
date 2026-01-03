#!/bin/bash
#==============================================================================
# Generate Self-Signed SSL Certificates for Development/Cloudflare
#==============================================================================

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${GREEN}Generating self-signed SSL certificates for doganconsult.com...${NC}"

# Create directories
mkdir -p /home/dogan/erpnext-saas-platform/docker/nginx/certs

# Generate self-signed certificate for all domains
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout /home/dogan/erpnext-saas-platform/docker/nginx/certs/doganconsult.key \
  -out /home/dogan/erpnext-saas-platform/docker/nginx/certs/doganconsult.crt \
  -subj "/C=SA/ST=Eastern/L=Dammam/O=Dogan Consult/CN=*.doganconsult.com" \
  -addext "subjectAltName=DNS:doganconsult.com,DNS:*.doganconsult.com,DNS:os.doganconsult.com,DNS:*.os.doganconsult.com,DNS:admin.doganconsult.com,DNS:tenant.doganconsult.com,DNS:portal.doganconsult.com,DNS:demo.doganconsult.com"

echo -e "${GREEN}Certificates generated successfully!${NC}"
echo ""
echo "Certificate location:"
echo "  - Certificate: /home/dogan/erpnext-saas-platform/docker/nginx/certs/doganconsult.crt"
echo "  - Private Key: /home/dogan/erpnext-saas-platform/docker/nginx/certs/doganconsult.key"
echo ""
echo -e "${YELLOW}Note: These are self-signed certificates.${NC}"
echo -e "${YELLOW}For Cloudflare, use 'Full' SSL mode (not 'Full (strict)').${NC}"