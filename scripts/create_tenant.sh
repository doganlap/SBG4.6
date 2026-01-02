#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Create Tenant Script
# Version: 1.0.0
# Description: Create a new tenant with isolated database and container
#==============================================================================

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

log_info() { echo -e "${BLUE}[INFO]${NC} $1"; }
log_success() { echo -e "${GREEN}[SUCCESS]${NC} $1"; }
log_warn() { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

#==============================================================================
# Configuration
#==============================================================================
PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DB_ROOT_PASSWORD="${DB_ROOT_PASSWORD:-rootpassword}"
DOCKER_REGISTRY="${DOCKER_REGISTRY:-erpnext-saas}"
VERSION="${VERSION:-v16}"

#==============================================================================
# Usage
#==============================================================================
usage() {
    echo "Usage: $0 <tenant_id> <organization_name> <admin_email> [plan]"
    echo ""
    echo "Arguments:"
    echo "  tenant_id          Unique tenant identifier (e.g., acme)"
    echo "  organization_name  Organization display name"
    echo "  admin_email        Admin user email"
    echo "  plan               Subscription plan (starter|professional|enterprise)"
    echo ""
    echo "Example:"
    echo "  $0 acme 'Acme Corporation' admin@acme.com professional"
    exit 1
}

#==============================================================================
# Validate Arguments
#==============================================================================
validate_args() {
    if [ -z "$1" ] || [ -z "$2" ] || [ -z "$3" ]; then
        usage
    fi
    
    TENANT_ID="$1"
    ORG_NAME="$2"
    ADMIN_EMAIL="$3"
    PLAN="${4:-starter}"
    
    # Validate tenant_id format
    if [[ ! "$TENANT_ID" =~ ^[a-z][a-z0-9_-]{2,29}$ ]]; then
        log_error "Invalid tenant_id. Must be 3-30 lowercase alphanumeric characters."
        exit 1
    fi
    
    # Validate email
    if [[ ! "$ADMIN_EMAIL" =~ ^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$ ]]; then
        log_error "Invalid email format."
        exit 1
    fi
    
    # Validate plan
    if [[ ! "$PLAN" =~ ^(starter|professional|enterprise)$ ]]; then
        log_error "Invalid plan. Must be: starter, professional, or enterprise"
        exit 1
    fi
}

#==============================================================================
# Generate Credentials
#==============================================================================
generate_credentials() {
    log_info "Generating credentials..."
    
    DB_NAME="tenant_${TENANT_ID}"
    DB_USER="user_${TENANT_ID}"
    DB_PASSWORD=$(openssl rand -base64 24 | tr -dc 'a-zA-Z0-9' | head -c 24)
    ADMIN_PASSWORD=$(openssl rand -base64 16 | tr -dc 'a-zA-Z0-9' | head -c 16)
    SITE_NAME="${TENANT_ID}.erp.local"
    
    log_success "Credentials generated"
}

#==============================================================================
# Create Database
#==============================================================================
create_database() {
    log_info "Creating database: ${DB_NAME}..."
    
    docker exec erpnext-mariadb mysql -u root -p${DB_ROOT_PASSWORD} <<EOF
CREATE DATABASE IF NOT EXISTS \`${DB_NAME}\`
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

CREATE USER IF NOT EXISTS '${DB_USER}'@'%' IDENTIFIED BY '${DB_PASSWORD}';

GRANT ALL PRIVILEGES ON \`${DB_NAME}\`.* TO '${DB_USER}'@'%';

FLUSH PRIVILEGES;
EOF

    log_success "Database created"
}

#==============================================================================
# Apply Tenant Schema
#==============================================================================
apply_schema() {
    log_info "Applying tenant schema..."
    
    docker exec -i erpnext-mariadb mysql -u root -p${DB_ROOT_PASSWORD} ${DB_NAME} \
        < ${PROJECT_DIR}/database-schemas/02_tenant_admin_schema.sql
    
    log_success "Schema applied"
}

#==============================================================================
# Create ERPNext Site
#==============================================================================
create_erpnext_site() {
    log_info "Creating ERPNext site: ${SITE_NAME}..."
    
    docker exec erpnext-backend bench new-site ${SITE_NAME} \
        --db-name ${DB_NAME} \
        --db-root-password ${DB_ROOT_PASSWORD} \
        --admin-password ${ADMIN_PASSWORD} \
        --db-host mariadb \
        --install-app erpnext
    
    log_success "ERPNext site created"
}

#==============================================================================
# Register Tenant in SaaS Admin
#==============================================================================
register_tenant() {
    log_info "Registering tenant in SaaS Admin database..."
    
    TENANT_UUID=$(cat /proc/sys/kernel/random/uuid | tr -d '-' | head -c 8)
    
    docker exec erpnext-mariadb mysql -u root -p${DB_ROOT_PASSWORD} saas_admin <<EOF
INSERT INTO tenants (
    tenant_id,
    organization_name,
    slug,
    subdomain,
    primary_email,
    status,
    database_name,
    erpnext_site_name,
    activation_date
) VALUES (
    'TNT${TENANT_UUID}',
    '${ORG_NAME}',
    '${TENANT_ID}',
    '${TENANT_ID}',
    '${ADMIN_EMAIL}',
    'active',
    '${DB_NAME}',
    '${SITE_NAME}',
    NOW()
);

SET @tenant_id = LAST_INSERT_ID();

INSERT INTO subscriptions (
    subscription_id,
    tenant_id,
    plan_id,
    status,
    start_date,
    billing_cycle,
    base_price,
    final_price,
    next_billing_date
) SELECT
    CONCAT('SUB', LPAD(@tenant_id, 6, '0')),
    @tenant_id,
    id,
    'active',
    CURDATE(),
    'monthly',
    price_monthly,
    price_monthly,
    DATE_ADD(CURDATE(), INTERVAL 1 MONTH)
FROM subscription_plans
WHERE plan_code = '${PLAN}';
EOF

    log_success "Tenant registered"
}

#==============================================================================
# Output Summary
#==============================================================================
output_summary() {
    log_info "=============================================="
    log_success "Tenant Created Successfully!"
    log_info "=============================================="
    echo ""
    echo "Tenant Details:"
    echo "  Tenant ID:        ${TENANT_ID}"
    echo "  Organization:     ${ORG_NAME}"
    echo "  Plan:             ${PLAN}"
    echo ""
    echo "ERPNext Access:"
    echo "  URL:              http://${SITE_NAME}:8000"
    echo "  Admin Email:      ${ADMIN_EMAIL}"
    echo "  Admin Password:   ${ADMIN_PASSWORD}"
    echo ""
    echo "Database:"
    echo "  Database Name:    ${DB_NAME}"
    echo "  Database User:    ${DB_USER}"
    echo "  Database Pass:    ${DB_PASSWORD}"
    echo ""
    log_warn "Save these credentials securely!"
    log_info "=============================================="
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Create Tenant"
    log_info "=============================================="
    
    validate_args "$@"
    generate_credentials
    create_database
    apply_schema
    create_erpnext_site
    register_tenant
    output_summary
}

main "$@"
