#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Bench Setup Script
# Version: 1.0.0
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
BENCH_DIR="/home/frappe/frappe-bench"
FRAPPE_BRANCH="version-16"
ERPNEXT_BRANCH="version-16"
SITE_NAME="erpnext.local"
DB_ROOT_PASSWORD="your_secure_password_here"
ADMIN_PASSWORD="admin_password_here"

#==============================================================================
# Initialize Bench
#==============================================================================
initialize_bench() {
    log_info "Initializing Frappe Bench..."

    cd /home/frappe

    if [ -d "$BENCH_DIR" ]; then
        log_warn "Bench directory already exists. Skipping initialization."
    else
        bench init frappe-bench \
            --frappe-branch ${FRAPPE_BRANCH} \
            --python python3.11 \
            --verbose

        log_success "Bench initialized successfully"
    fi

    cd $BENCH_DIR
}

#==============================================================================
# Get ERPNext App
#==============================================================================
get_erpnext() {
    log_info "Getting ERPNext app..."

    cd $BENCH_DIR

    bench get-app erpnext --branch ${ERPNEXT_BRANCH}

    log_success "ERPNext app downloaded"
}

#==============================================================================
# Get Additional Apps
#==============================================================================
get_additional_apps() {
    log_info "Getting additional Frappe apps..."

    cd $BENCH_DIR

    # Education App
    bench get-app education --branch ${ERPNEXT_BRANCH} 2>/dev/null || log_warn "Education app not available"

    # Healthcare App
    bench get-app healthcare --branch ${ERPNEXT_BRANCH} 2>/dev/null || log_warn "Healthcare app not available"

    # HRMS App (Human Resource Management System)
    bench get-app hrms --branch ${ERPNEXT_BRANCH} 2>/dev/null || log_warn "HRMS app not available"

    # Payments App
    bench get-app payments --branch ${ERPNEXT_BRANCH} 2>/dev/null || log_warn "Payments app not available"

    # LMS (Learning Management System)
    bench get-app lms --branch ${ERPNEXT_BRANCH} 2>/dev/null || log_warn "LMS app not available"

    log_success "Additional apps downloaded"
}

#==============================================================================
# Create Site
#==============================================================================
create_site() {
    log_info "Creating ERPNext site..."

    cd $BENCH_DIR

    bench new-site ${SITE_NAME} \
        --db-root-password ${DB_ROOT_PASSWORD} \
        --admin-password ${ADMIN_PASSWORD} \
        --db-name erpnext_db \
        --db-type mariadb \
        --verbose

    log_success "Site '${SITE_NAME}' created"
}

#==============================================================================
# Install Apps on Site
#==============================================================================
install_apps() {
    log_info "Installing apps on site..."

    cd $BENCH_DIR

    bench --site ${SITE_NAME} install-app erpnext
    bench --site ${SITE_NAME} install-app hrms 2>/dev/null || log_warn "HRMS install skipped"
    bench --site ${SITE_NAME} install-app education 2>/dev/null || log_warn "Education install skipped"
    bench --site ${SITE_NAME} install-app healthcare 2>/dev/null || log_warn "Healthcare install skipped"
    bench --site ${SITE_NAME} install-app payments 2>/dev/null || log_warn "Payments install skipped"

    log_success "Apps installed on site"
}

#==============================================================================
# Configure Site
#==============================================================================
configure_site() {
    log_info "Configuring site..."

    cd $BENCH_DIR

    # Set as default site
    bench use ${SITE_NAME}

    # Enable developer mode for customization
    bench --site ${SITE_NAME} set-config developer_mode 1

    # Set maintenance mode off
    bench --site ${SITE_NAME} set-maintenance-mode off

    log_success "Site configured"
}

#==============================================================================
# Setup Production
#==============================================================================
setup_production() {
    log_info "Setting up production environment..."

    cd $BENCH_DIR

    # Install supervisor and configure
    sudo bench setup supervisor --user frappe --yes

    # Install nginx configuration
    sudo bench setup nginx --yes

    # Enable and start supervisor
    sudo systemctl enable supervisor
    sudo systemctl start supervisor

    # Reload nginx
    sudo systemctl reload nginx

    log_success "Production setup complete"
}

#==============================================================================
# Build Assets
#==============================================================================
build_assets() {
    log_info "Building assets..."

    cd $BENCH_DIR

    bench build

    log_success "Assets built successfully"
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Bench Setup"
    log_info "=============================================="

    initialize_bench
    get_erpnext
    get_additional_apps
    create_site
    install_apps
    configure_site
    build_assets

    log_info "=============================================="
    log_success "Bench setup complete!"
    log_info "=============================================="
    log_info ""
    log_info "Access ERPNext at: http://${SITE_NAME}:8000"
    log_info "Admin credentials:"
    log_info "  Username: Administrator"
    log_info "  Password: ${ADMIN_PASSWORD}"
    log_info ""
    log_info "To start development server:"
    log_info "  cd ${BENCH_DIR} && bench start"
    log_info ""
    log_info "For production setup, run:"
    log_info "  sudo bench setup production frappe"
    log_info ""
}

# Run main function
main "$@"
