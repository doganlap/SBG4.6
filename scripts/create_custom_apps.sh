#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Create Custom Apps for Each Module
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
COMPANY_PREFIX="saas"

#==============================================================================
# Custom Apps List
#==============================================================================
CUSTOM_APPS=(
    "accounting_custom:Accounting Customizations:Customizations for Accounting module"
    "crm_custom:CRM Customizations:Customizations for CRM module"
    "selling_custom:Selling Customizations:Customizations for Sales module"
    "buying_custom:Buying Customizations:Customizations for Purchasing module"
    "stock_custom:Stock Customizations:Customizations for Inventory module"
    "manufacturing_custom:Manufacturing Customizations:Customizations for Manufacturing module"
    "projects_custom:Projects Customizations:Customizations for Projects module"
    "support_custom:Support Customizations:Customizations for Support module"
    "website_custom:Website Customizations:Customizations for Website module"
    "ecommerce_custom:E-commerce Customizations:Customizations for E-commerce module"
    "pos_custom:POS Customizations:Customizations for POS module"
    "assets_custom:Assets Customizations:Customizations for Asset Management module"
    "hr_custom:HR Customizations:Customizations for HRMS module"
    "education_custom:Education Customizations:Customizations for Education module"
    "healthcare_custom:Healthcare Customizations:Customizations for Healthcare module"
    "payments_custom:Payments Customizations:Customizations for Payments module"
    "lms_custom:LMS Customizations:Customizations for Learning Management"
    "helpdesk_custom:Helpdesk Customizations:Customizations for Helpdesk module"
    "wiki_custom:Wiki Customizations:Customizations for Wiki module"
    "insights_custom:Insights Customizations:Customizations for Analytics module"
    "builder_custom:Builder Customizations:Customizations for Builder module"
    "crm_app_custom:CRM App Customizations:Customizations for CRM App"
)

#==============================================================================
# Create Custom App
#==============================================================================
create_custom_app() {
    local app_name=$1
    local app_title=$2
    local app_description=$3
    local full_app_name="${COMPANY_PREFIX}_${app_name}"

    log_module "Creating custom app: ${full_app_name}"

    cd $BENCH_DIR

    if [ -d "${APPS_DIR}/${full_app_name}" ]; then
        log_warn "App ${full_app_name} already exists. Skipping..."
        return 0
    fi

    # Create app using bench
    bench new-app ${full_app_name} --no-git <<EOF
${app_title}
${app_description}
SaaS Platform
https://your-company.com
MIT
EOF

    # Create additional directories
    mkdir -p ${APPS_DIR}/${full_app_name}/${full_app_name}/{config,custom,fixtures,patches,templates,www}
    mkdir -p ${APPS_DIR}/${full_app_name}/${full_app_name}/custom/{doctype,page,report,web_form,dashboard}
    mkdir -p ${APPS_DIR}/${full_app_name}/${full_app_name}/public/{js,css,images}

    # Create hooks.py with customization structure
    cat > ${APPS_DIR}/${full_app_name}/${full_app_name}/hooks.py <<HOOKS
from . import __version__ as app_version

app_name = "${full_app_name}"
app_title = "${app_title}"
app_publisher = "SaaS Platform"
app_description = "${app_description}"
app_email = "support@your-company.com"
app_license = "MIT"

# Includes in <head>
# ------------------
app_include_css = "/assets/${full_app_name}/css/${app_name}.css"
app_include_js = "/assets/${full_app_name}/js/${app_name}.js"

# Include js in doctype views
# doctype_js = {"doctype" : "public/js/doctype.js"}
# doctype_list_js = {"doctype" : "public/js/doctype_list.js"}
# doctype_tree_js = {"doctype" : "public/js/doctype_tree.js"}
# doctype_calendar_js = {"doctype" : "public/js/doctype_calendar.js"}

# Home Pages
# ----------
# application_home_page = "login"
# website_route_rules = [{"from_route": "/", "to_route": "home"}]

# Generators
# ----------
# automatically create page for each record of this doctype
# website_generators = ["Web Page"]

# Jinja
# ----------
# add methods and filters to jinja environment
# jinja = {
#     "methods": "${full_app_name}.utils.jinja_methods",
#     "filters": "${full_app_name}.utils.jinja_filters"
# }

# Installation
# ------------
# before_install = "${full_app_name}.install.before_install"
# after_install = "${full_app_name}.install.after_install"

# Uninstallation
# ------------
# before_uninstall = "${full_app_name}.uninstall.before_uninstall"
# after_uninstall = "${full_app_name}.uninstall.after_uninstall"

# Desk Notifications
# ------------------
# notification_config = "${full_app_name}.notifications.get_notification_config"

# Permissions
# -----------
# permission_query_conditions = {"Event": "frappe.desk.doctype.event.event.get_permission_query_conditions"}
# has_permission = {"Event": "frappe.desk.doctype.event.event.has_permission"}

# DocType Class
# ---------------
# Override standard doctype classes
# override_doctype_class = {"ToDo": "custom_app.overrides.CustomToDo"}

# Document Events
# ---------------
# Hook on document methods and events
# doc_events = {
#     "*": {
#         "on_update": "method",
#         "on_cancel": "method",
#         "on_trash": "method"
#     }
# }

# Scheduled Tasks
# ---------------
# scheduler_events = {
#     "all": ["${full_app_name}.tasks.all"],
#     "daily": ["${full_app_name}.tasks.daily"],
#     "hourly": ["${full_app_name}.tasks.hourly"],
#     "weekly": ["${full_app_name}.tasks.weekly"],
#     "monthly": ["${full_app_name}.tasks.monthly"],
# }

# Testing
# -------
# before_tests = "${full_app_name}.install.before_tests"

# Overriding Methods
# ----------------
# override_whitelisted_methods = {"frappe.desk.doctype.event.event.get_events": "${full_app_name}.event.get_events"}

# Fixtures
# --------
# fixtures = ["Custom Field", "Property Setter", "Client Script", "Server Script"]

# User Data Protection
# --------------------
# user_data_fields = [
#     {"doctype": "{doctype_1}", "filter_by": "{filter_by}", "redact_fields": ["{field_1}", "{field_2}"], "partial": 1},
# ]

# Authentication and authorization
# --------------------------------
# auth_hooks = ["${full_app_name}.auth.validate"]
HOOKS

    # Create base CSS file
    cat > ${APPS_DIR}/${full_app_name}/${full_app_name}/public/css/${app_name}.css <<CSS
/*
 * ${app_title} - Custom Styles
 * Version: 1.0.0
 */

/* Custom styles for ${app_name} module */
.${app_name}-container {
    padding: 20px;
}

.${app_name}-header {
    margin-bottom: 20px;
}

.${app_name}-card {
    background: var(--card-bg);
    border-radius: 8px;
    padding: 20px;
    box-shadow: var(--card-shadow);
}
CSS

    # Create base JS file
    cat > ${APPS_DIR}/${full_app_name}/${full_app_name}/public/js/${app_name}.js <<JS
/*
 * ${app_title} - Custom Scripts
 * Version: 1.0.0
 */

frappe.provide("${full_app_name}");

${full_app_name} = {
    init: function() {
        console.log("${app_title} initialized");
    },

    // Add custom methods here
    custom_method: function() {
        // Custom logic
    }
};

\$(document).ready(function() {
    ${full_app_name}.init();
});
JS

    # Create install.py
    cat > ${APPS_DIR}/${full_app_name}/${full_app_name}/install.py <<INSTALL
import frappe

def before_install():
    """Pre-installation setup"""
    pass

def after_install():
    """Post-installation setup"""
    print("${app_title} installed successfully!")
    # Add any post-installation setup here

def after_migrate():
    """Run after migrations"""
    pass
INSTALL

    # Create api.py for REST endpoints
    cat > ${APPS_DIR}/${full_app_name}/${full_app_name}/api.py <<API
import frappe
from frappe import _

@frappe.whitelist()
def get_module_info():
    """Return module information"""
    return {
        "app": "${full_app_name}",
        "title": "${app_title}",
        "version": frappe.get_attr("${full_app_name}.__version__")
    }

@frappe.whitelist()
def get_dashboard_data():
    """Return dashboard data for ${app_name}"""
    # Add custom dashboard data logic
    return {
        "stats": {},
        "charts": [],
        "tables": []
    }
API

    log_success "Created app: ${full_app_name}"
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Create Custom Apps"
    log_info "=============================================="
    echo ""

    # Check if bench directory exists
    if [ ! -d "$BENCH_DIR" ]; then
        log_error "Bench directory not found: ${BENCH_DIR}"
        log_error "Please run setup_bench.sh first"
        exit 1
    fi

    cd $BENCH_DIR

    # Create each custom app
    for app_def in "${CUSTOM_APPS[@]}"; do
        IFS=':' read -r app_name app_title app_description <<< "$app_def"
        create_custom_app "$app_name" "$app_title" "$app_description"
    done

    log_info "=============================================="
    log_success "All custom apps created successfully!"
    log_info "=============================================="
    echo ""
    log_info "Created apps:"
    ls -la ${APPS_DIR} | grep "${COMPANY_PREFIX}_"
    echo ""
    log_info "Next steps:"
    log_info "1. Customize each app as needed"
    log_info "2. Run: ./scripts/containerize_modules.sh"
    echo ""
}

# Run main function
main "$@"
