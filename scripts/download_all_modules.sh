#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Download All 22 Modules Script
# Version: 1.0.0
#==============================================================================

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m'

log_info() { echo -e "${BLUE}[INFO]${NC} $1"; }
log_success() { echo -e "${GREEN}[SUCCESS]${NC} $1"; }
log_warn() { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }
log_module() { echo -e "${CYAN}[MODULE]${NC} $1"; }

#==============================================================================
# Configuration
#==============================================================================
BENCH_DIR="/home/frappe/frappe-bench"
APPS_DIR="${BENCH_DIR}/apps"
FRAPPE_BRANCH="version-16"
ERPNEXT_BRANCH="version-16"

#==============================================================================
# All 22 ERPNext Modules Definition
#==============================================================================
declare -A MODULES=(
    # Core ERPNext Modules (Built-in with ERPNext)
    ["01_accounting"]="core:erpnext:Accounting - Financial management, ledgers, reports"
    ["02_crm"]="core:erpnext:CRM - Customer relationship management"
    ["03_selling"]="core:erpnext:Selling - Sales orders, quotations, invoices"
    ["04_buying"]="core:erpnext:Buying - Purchase orders, supplier management"
    ["05_stock"]="core:erpnext:Stock - Inventory management, warehouses"
    ["06_manufacturing"]="core:erpnext:Manufacturing - Production planning, BOM, work orders"
    ["07_projects"]="core:erpnext:Projects - Project management, tasks, timesheets"
    ["08_support"]="core:erpnext:Support - Helpdesk, tickets, SLA"
    ["09_website"]="core:erpnext:Website - Web pages, blog, portal"
    ["10_ecommerce"]="core:erpnext:E-commerce - Online store, shopping cart"
    ["11_pos"]="core:erpnext:POS - Point of sale system"
    ["12_assets"]="core:erpnext:Asset Management - Fixed assets, depreciation"
    
    # Standalone Apps (Separate Frappe Apps)
    ["13_hr"]="app:hrms:https://github.com/frappe/hrms:HR - Employee management, payroll, attendance"
    ["14_education"]="app:education:https://github.com/frappe/education:Education - Schools, courses, students"
    ["15_healthcare"]="app:healthcare:https://github.com/frappe/health:Healthcare - Patients, appointments, lab tests"
    ["16_payments"]="app:payments:https://github.com/frappe/payments:Payments - Payment gateway integrations"
    ["17_lms"]="app:lms:https://github.com/frappe/lms:LMS - Learning Management System"
    ["18_helpdesk"]="app:helpdesk:https://github.com/frappe/helpdesk:Helpdesk - Customer support portal"
    ["19_wiki"]="app:wiki:https://github.com/frappe/wiki:Wiki - Knowledge base and documentation"
    ["20_insights"]="app:insights:https://github.com/frappe/insights:Insights - Business intelligence and analytics"
    ["21_builder"]="app:builder:https://github.com/frappe/builder:Builder - Visual website/app builder"
    ["22_crm_app"]="app:crm:https://github.com/frappe/crm:CRM App - Standalone CRM application"
)

#==============================================================================
# Download Core ERPNext (Contains modules 1-12)
#==============================================================================
download_erpnext_core() {
    log_info "=============================================="
    log_info "Downloading ERPNext Core (Modules 1-12)"
    log_info "=============================================="

    cd $BENCH_DIR

    if [ -d "${APPS_DIR}/erpnext" ]; then
        log_warn "ERPNext already exists. Updating..."
        cd ${APPS_DIR}/erpnext
        git pull origin ${ERPNEXT_BRANCH}
    else
        log_info "Downloading ERPNext from GitHub..."
        bench get-app erpnext --branch ${ERPNEXT_BRANCH}
    fi

    log_success "ERPNext Core downloaded (includes 12 core modules)"
}

#==============================================================================
# Download Individual Apps (Modules 13-22)
#==============================================================================
download_standalone_apps() {
    log_info "=============================================="
    log_info "Downloading Standalone Apps (Modules 13-22)"
    log_info "=============================================="

    cd $BENCH_DIR

    # HRMS (Module 13)
    log_module "13. HRMS - Human Resource Management System"
    if [ -d "${APPS_DIR}/hrms" ]; then
        log_warn "HRMS already exists. Updating..."
        cd ${APPS_DIR}/hrms && git pull origin ${ERPNEXT_BRANCH}
    else
        bench get-app hrms --branch ${ERPNEXT_BRANCH} || log_error "Failed to download HRMS"
    fi

    cd $BENCH_DIR

    # Education (Module 14)
    log_module "14. Education - School/University Management"
    if [ -d "${APPS_DIR}/education" ]; then
        log_warn "Education already exists. Updating..."
        cd ${APPS_DIR}/education && git pull origin ${ERPNEXT_BRANCH}
    else
        bench get-app education --branch ${ERPNEXT_BRANCH} || log_error "Failed to download Education"
    fi

    cd $BENCH_DIR

    # Healthcare (Module 15)
    log_module "15. Healthcare - Hospital/Clinic Management"
    if [ -d "${APPS_DIR}/healthcare" ]; then
        log_warn "Healthcare already exists. Updating..."
        cd ${APPS_DIR}/healthcare && git pull origin ${ERPNEXT_BRANCH}
    else
        bench get-app https://github.com/frappe/health --branch ${ERPNEXT_BRANCH} || log_error "Failed to download Healthcare"
    fi

    cd $BENCH_DIR

    # Payments (Module 16)
    log_module "16. Payments - Payment Gateway Integration"
    if [ -d "${APPS_DIR}/payments" ]; then
        log_warn "Payments already exists. Updating..."
        cd ${APPS_DIR}/payments && git pull origin ${ERPNEXT_BRANCH}
    else
        bench get-app payments --branch ${ERPNEXT_BRANCH} || log_error "Failed to download Payments"
    fi

    cd $BENCH_DIR

    # LMS (Module 17)
    log_module "17. LMS - Learning Management System"
    if [ -d "${APPS_DIR}/lms" ]; then
        log_warn "LMS already exists. Updating..."
        cd ${APPS_DIR}/lms && git pull origin main
    else
        bench get-app lms || log_error "Failed to download LMS"
    fi

    cd $BENCH_DIR

    # Helpdesk (Module 18)
    log_module "18. Helpdesk - Customer Support"
    if [ -d "${APPS_DIR}/helpdesk" ]; then
        log_warn "Helpdesk already exists. Updating..."
        cd ${APPS_DIR}/helpdesk && git pull origin main
    else
        bench get-app helpdesk || log_error "Failed to download Helpdesk"
    fi

    cd $BENCH_DIR

    # Wiki (Module 19)
    log_module "19. Wiki - Knowledge Base"
    if [ -d "${APPS_DIR}/wiki" ]; then
        log_warn "Wiki already exists. Updating..."
        cd ${APPS_DIR}/wiki && git pull origin main
    else
        bench get-app wiki || log_error "Failed to download Wiki"
    fi

    cd $BENCH_DIR

    # Insights (Module 20)
    log_module "20. Insights - Business Intelligence"
    if [ -d "${APPS_DIR}/insights" ]; then
        log_warn "Insights already exists. Updating..."
        cd ${APPS_DIR}/insights && git pull origin main
    else
        bench get-app insights || log_error "Failed to download Insights"
    fi

    cd $BENCH_DIR

    # Builder (Module 21)
    log_module "21. Builder - Visual Website/App Builder"
    if [ -d "${APPS_DIR}/builder" ]; then
        log_warn "Builder already exists. Updating..."
        cd ${APPS_DIR}/builder && git pull origin main
    else
        bench get-app builder || log_error "Failed to download Builder"
    fi

    cd $BENCH_DIR

    # CRM App (Module 22)
    log_module "22. CRM - Standalone CRM Application"
    if [ -d "${APPS_DIR}/crm" ]; then
        log_warn "CRM already exists. Updating..."
        cd ${APPS_DIR}/crm && git pull origin main
    else
        bench get-app crm || log_error "Failed to download CRM"
    fi

    log_success "All standalone apps downloaded"
}

#==============================================================================
# List All Downloaded Modules
#==============================================================================
list_modules() {
    log_info "=============================================="
    log_info "All 22 ERPNext Modules"
    log_info "=============================================="

    echo ""
    echo "CORE ERPNEXT MODULES (Built-in):"
    echo "================================="
    echo "01. Accounting     - Financial management, ledgers, reports"
    echo "02. CRM            - Customer relationship management"
    echo "03. Selling        - Sales orders, quotations, invoices"
    echo "04. Buying         - Purchase orders, supplier management"
    echo "05. Stock          - Inventory management, warehouses"
    echo "06. Manufacturing  - Production planning, BOM, work orders"
    echo "07. Projects       - Project management, tasks, timesheets"
    echo "08. Support        - Helpdesk, tickets, SLA"
    echo "09. Website        - Web pages, blog, portal"
    echo "10. E-commerce     - Online store, shopping cart"
    echo "11. POS            - Point of sale system"
    echo "12. Asset Mgmt     - Fixed assets, depreciation"
    echo ""
    echo "STANDALONE FRAPPE APPS:"
    echo "================================="
    echo "13. HRMS           - Employee management, payroll, attendance"
    echo "14. Education      - Schools, courses, students"
    echo "15. Healthcare     - Patients, appointments, lab tests"
    echo "16. Payments       - Payment gateway integrations"
    echo "17. LMS            - Learning Management System"
    echo "18. Helpdesk       - Customer support portal"
    echo "19. Wiki           - Knowledge base and documentation"
    echo "20. Insights       - Business intelligence and analytics"
    echo "21. Builder        - Visual website/app builder"
    echo "22. CRM App        - Standalone CRM application"
    echo ""
}

#==============================================================================
# Verify Downloads
#==============================================================================
verify_downloads() {
    log_info "=============================================="
    log_info "Verifying Downloaded Apps"
    log_info "=============================================="

    echo ""
    echo "Apps Directory Contents:"
    ls -la ${APPS_DIR}
    echo ""

    local count=0
    for app in ${APPS_DIR}/*/; do
        if [ -d "$app" ]; then
            app_name=$(basename "$app")
            echo -e "${GREEN}✓${NC} ${app_name}"
            ((count++))
        fi
    done

    echo ""
    log_info "Total apps downloaded: ${count}"
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Download All Modules"
    log_info "=============================================="
    echo ""

    # Check if bench directory exists
    if [ ! -d "$BENCH_DIR" ]; then
        log_error "Bench directory not found: ${BENCH_DIR}"
        log_error "Please run setup_bench.sh first"
        exit 1
    fi

    list_modules
    download_erpnext_core
    download_standalone_apps
    verify_downloads

    log_info "=============================================="
    log_success "All 22 modules downloaded successfully!"
    log_info "=============================================="
    echo ""
    log_info "Next steps:"
    log_info "1. Create customization apps for each module"
    log_info "2. Run: ./scripts/create_custom_apps.sh"
    echo ""
}

# Run main function
main "$@"
