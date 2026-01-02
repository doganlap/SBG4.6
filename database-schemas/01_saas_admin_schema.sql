-- ============================================================================
-- ERPNext Multi-Module SaaS Platform - SaaS Admin Database Schema
-- Version: 1.0.0
-- Description: Complete schema for platform administration
-- ============================================================================

SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;

-- ============================================================================
-- DATABASE CREATION
-- ============================================================================
CREATE DATABASE IF NOT EXISTS saas_admin 
    CHARACTER SET utf8mb4 
    COLLATE utf8mb4_unicode_ci;

USE saas_admin;

-- ============================================================================
-- TABLE: platform_settings
-- Purpose: Global platform configuration
-- ============================================================================
CREATE TABLE platform_settings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    setting_key VARCHAR(255) NOT NULL UNIQUE,
    setting_value TEXT,
    setting_type ENUM('string', 'number', 'boolean', 'json') DEFAULT 'string',
    category VARCHAR(100),
    description TEXT,
    is_sensitive BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_category (category),
    INDEX idx_key (setting_key)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenants
-- Purpose: All tenant organizations registered on the platform
-- ============================================================================
CREATE TABLE tenants (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id VARCHAR(50) NOT NULL UNIQUE COMMENT 'Public-facing ID like TNT001',
    organization_name VARCHAR(255) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE COMMENT 'URL-safe identifier',
    domain VARCHAR(255) UNIQUE COMMENT 'Custom domain if any',
    subdomain VARCHAR(100) NOT NULL UNIQUE COMMENT 'e.g., acme.erp.localhost',
    
    -- Contact Information
    primary_email VARCHAR(255) NOT NULL,
    primary_phone VARCHAR(50),
    billing_email VARCHAR(255),
    
    -- Address
    address_line_1 VARCHAR(255),
    address_line_2 VARCHAR(255),
    city VARCHAR(100),
    state VARCHAR(100),
    country VARCHAR(100),
    postal_code VARCHAR(20),
    
    -- Business Information
    company_type ENUM('company', 'individual', 'non_profit', 'government', 'education') DEFAULT 'company',
    industry VARCHAR(100),
    tax_id VARCHAR(100),
    registration_number VARCHAR(100),
    
    -- Status
    status ENUM('pending', 'active', 'suspended', 'cancelled', 'trial', 'trial_expired') DEFAULT 'pending',
    activation_date TIMESTAMP NULL,
    suspension_date TIMESTAMP NULL,
    suspension_reason TEXT,
    
    -- Metadata
    logo_url VARCHAR(500),
    timezone VARCHAR(100) DEFAULT 'UTC',
    default_language VARCHAR(10) DEFAULT 'en',
    default_currency VARCHAR(3) DEFAULT 'USD',
    
    -- Technical
    database_name VARCHAR(100) UNIQUE,
    database_host VARCHAR(255),
    container_id VARCHAR(100),
    erpnext_site_name VARCHAR(255),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP NULL,
    
    -- Indexes
    INDEX idx_status (status),
    INDEX idx_subdomain (subdomain),
    INDEX idx_organization (organization_name),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: subscription_plans
-- Purpose: Available subscription plans/tiers
-- ============================================================================
CREATE TABLE subscription_plans (
    id INT AUTO_INCREMENT PRIMARY KEY,
    plan_code VARCHAR(50) NOT NULL UNIQUE COMMENT 'e.g., starter, professional, enterprise',
    plan_name VARCHAR(100) NOT NULL,
    description TEXT,
    
    -- Pricing
    price_monthly DECIMAL(10, 2) NOT NULL,
    price_yearly DECIMAL(10, 2),
    currency VARCHAR(3) DEFAULT 'USD',
    
    -- Limits
    max_users INT DEFAULT 5,
    max_storage_gb INT DEFAULT 10,
    max_api_calls_per_month INT DEFAULT 10000,
    
    -- Features
    included_modules JSON COMMENT 'Array of module IDs included',
    features JSON COMMENT 'Key-value pairs of features',
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    is_public BOOLEAN DEFAULT TRUE COMMENT 'Visible on pricing page',
    is_trial_available BOOLEAN DEFAULT TRUE,
    trial_days INT DEFAULT 14,
    
    -- Display
    display_order INT DEFAULT 0,
    badge_text VARCHAR(50) COMMENT 'e.g., Popular, Best Value',
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_active (is_active),
    INDEX idx_public (is_public)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: subscriptions
-- Purpose: Tenant subscription records
-- ============================================================================
CREATE TABLE subscriptions (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    subscription_id VARCHAR(50) NOT NULL UNIQUE COMMENT 'Public-facing ID like SUB001',
    tenant_id BIGINT NOT NULL,
    plan_id INT NOT NULL,
    
    -- Status
    status ENUM('trial', 'active', 'past_due', 'cancelled', 'paused', 'expired') DEFAULT 'trial',
    
    -- Dates
    start_date DATE NOT NULL,
    end_date DATE,
    trial_start_date DATE,
    trial_end_date DATE,
    cancelled_at TIMESTAMP NULL,
    cancellation_reason TEXT,
    
    -- Billing
    billing_cycle ENUM('monthly', 'yearly', 'custom') DEFAULT 'monthly',
    billing_anchor_day INT DEFAULT 1 COMMENT 'Day of month for billing',
    next_billing_date DATE,
    
    -- Pricing at time of subscription
    base_price DECIMAL(10, 2) NOT NULL,
    discount_percent DECIMAL(5, 2) DEFAULT 0,
    discount_amount DECIMAL(10, 2) DEFAULT 0,
    final_price DECIMAL(10, 2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'USD',
    
    -- Usage
    current_users INT DEFAULT 0,
    current_storage_gb DECIMAL(10, 2) DEFAULT 0,
    current_api_calls INT DEFAULT 0,
    
    -- External References
    stripe_subscription_id VARCHAR(255),
    stripe_customer_id VARCHAR(255),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (plan_id) REFERENCES subscription_plans(id),
    
    -- Indexes
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status),
    INDEX idx_billing_date (next_billing_date)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: modules
-- Purpose: All available ERPNext modules
-- ============================================================================
CREATE TABLE modules (
    id INT AUTO_INCREMENT PRIMARY KEY,
    module_code VARCHAR(50) NOT NULL UNIQUE,
    module_name VARCHAR(100) NOT NULL,
    description TEXT,
    category ENUM('core', 'domain', 'standalone', 'addon') DEFAULT 'core',
    
    -- Technical
    frappe_app_name VARCHAR(100),
    github_url VARCHAR(500),
    version VARCHAR(20),
    
    -- Features
    features JSON,
    doctypes JSON COMMENT 'List of ERPNext doctypes in this module',
    dependencies JSON COMMENT 'Other modules this depends on',
    
    -- Pricing
    is_free BOOLEAN DEFAULT FALSE,
    addon_price_monthly DECIMAL(10, 2) DEFAULT 0,
    addon_price_yearly DECIMAL(10, 2) DEFAULT 0,
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    is_beta BOOLEAN DEFAULT FALSE,
    
    -- Display
    icon VARCHAR(50),
    color VARCHAR(20),
    display_order INT DEFAULT 0,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_category (category),
    INDEX idx_active (is_active)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_modules
-- Purpose: Modules enabled for each tenant
-- ============================================================================
CREATE TABLE tenant_modules (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id BIGINT NOT NULL,
    module_id INT NOT NULL,
    
    -- Status
    is_enabled BOOLEAN DEFAULT TRUE,
    enabled_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    disabled_at TIMESTAMP NULL,
    
    -- Configuration
    config JSON COMMENT 'Module-specific configuration',
    
    -- Billing
    is_addon BOOLEAN DEFAULT FALSE,
    addon_subscription_id BIGINT NULL,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (module_id) REFERENCES modules(id),
    
    -- Unique constraint
    UNIQUE KEY uk_tenant_module (tenant_id, module_id),
    
    -- Indexes
    INDEX idx_tenant (tenant_id),
    INDEX idx_enabled (is_enabled)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: invoices
-- Purpose: All invoices generated for tenants
-- ============================================================================
CREATE TABLE invoices (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    invoice_id VARCHAR(50) NOT NULL UNIQUE COMMENT 'e.g., INV-2024-001',
    tenant_id BIGINT NOT NULL,
    subscription_id BIGINT,
    
    -- Type
    invoice_type ENUM('subscription', 'addon', 'one_time', 'credit', 'refund') DEFAULT 'subscription',
    
    -- Status
    status ENUM('draft', 'pending', 'paid', 'overdue', 'cancelled', 'refunded') DEFAULT 'draft',
    
    -- Dates
    invoice_date DATE NOT NULL,
    due_date DATE NOT NULL,
    paid_at TIMESTAMP NULL,
    
    -- Amounts
    subtotal DECIMAL(10, 2) NOT NULL,
    tax_amount DECIMAL(10, 2) DEFAULT 0,
    tax_percent DECIMAL(5, 2) DEFAULT 0,
    discount_amount DECIMAL(10, 2) DEFAULT 0,
    total_amount DECIMAL(10, 2) NOT NULL,
    amount_paid DECIMAL(10, 2) DEFAULT 0,
    amount_due DECIMAL(10, 2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'USD',
    
    -- Billing Period
    period_start DATE,
    period_end DATE,
    
    -- Line Items (stored as JSON for simplicity)
    line_items JSON COMMENT 'Array of invoice line items',
    
    -- Payment
    payment_method VARCHAR(50),
    stripe_invoice_id VARCHAR(255),
    stripe_payment_intent_id VARCHAR(255),
    
    -- PDF
    pdf_url VARCHAR(500),
    
    -- Notes
    notes TEXT,
    internal_notes TEXT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (subscription_id) REFERENCES subscriptions(id),
    
    -- Indexes
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status),
    INDEX idx_invoice_date (invoice_date),
    INDEX idx_due_date (due_date)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: payments
-- Purpose: All payment transactions
-- ============================================================================
CREATE TABLE payments (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    payment_id VARCHAR(50) NOT NULL UNIQUE COMMENT 'e.g., PAY-2024-001',
    tenant_id BIGINT NOT NULL,
    invoice_id BIGINT,
    
    -- Amount
    amount DECIMAL(10, 2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'USD',
    
    -- Status
    status ENUM('pending', 'processing', 'succeeded', 'failed', 'refunded', 'cancelled') DEFAULT 'pending',
    
    -- Payment Method
    payment_method ENUM('card', 'bank_transfer', 'paypal', 'manual', 'other') DEFAULT 'card',
    card_brand VARCHAR(20),
    card_last_four VARCHAR(4),
    
    -- External References
    stripe_payment_id VARCHAR(255),
    stripe_charge_id VARCHAR(255),
    paypal_transaction_id VARCHAR(255),
    
    -- Metadata
    description TEXT,
    failure_reason TEXT,
    metadata JSON,
    
    -- Timestamps
    payment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (invoice_id) REFERENCES invoices(id),
    
    -- Indexes
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status),
    INDEX idx_payment_date (payment_date)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: admin_users
-- Purpose: Platform super admin users
-- ============================================================================
CREATE TABLE admin_users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    
    -- Profile
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    phone VARCHAR(50),
    avatar_url VARCHAR(500),
    
    -- Role & Permissions
    role ENUM('super_admin', 'admin', 'support', 'billing', 'readonly') DEFAULT 'admin',
    permissions JSON,
    
    -- Security
    is_active BOOLEAN DEFAULT TRUE,
    is_verified BOOLEAN DEFAULT FALSE,
    two_factor_enabled BOOLEAN DEFAULT FALSE,
    two_factor_secret VARCHAR(255),
    
    -- Session
    last_login_at TIMESTAMP NULL,
    last_login_ip VARCHAR(45),
    failed_login_attempts INT DEFAULT 0,
    locked_until TIMESTAMP NULL,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP NULL,
    
    INDEX idx_email (email),
    INDEX idx_role (role),
    INDEX idx_active (is_active)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: audit_logs
-- Purpose: Platform-wide audit trail
-- ============================================================================
CREATE TABLE audit_logs (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    
    -- Actor
    actor_type ENUM('admin', 'tenant_admin', 'system', 'api') NOT NULL,
    actor_id VARCHAR(50),
    actor_email VARCHAR(255),
    
    -- Action
    action VARCHAR(100) NOT NULL,
    resource_type VARCHAR(100) NOT NULL,
    resource_id VARCHAR(100),
    
    -- Details
    description TEXT,
    old_values JSON,
    new_values JSON,
    metadata JSON,
    
    -- Context
    tenant_id BIGINT NULL,
    ip_address VARCHAR(45),
    user_agent TEXT,
    request_id VARCHAR(100),
    
    -- Timestamp
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Indexes
    INDEX idx_actor (actor_type, actor_id),
    INDEX idx_action (action),
    INDEX idx_resource (resource_type, resource_id),
    INDEX idx_tenant (tenant_id),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: system_health
-- Purpose: Track system health and metrics
-- ============================================================================
CREATE TABLE system_health (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    
    -- Service
    service_name VARCHAR(100) NOT NULL,
    instance_id VARCHAR(100),
    
    -- Status
    status ENUM('healthy', 'degraded', 'unhealthy', 'unknown') DEFAULT 'unknown',
    
    -- Metrics
    cpu_usage_percent DECIMAL(5, 2),
    memory_usage_percent DECIMAL(5, 2),
    disk_usage_percent DECIMAL(5, 2),
    response_time_ms INT,
    
    -- Details
    health_check_url VARCHAR(255),
    last_error TEXT,
    details JSON,
    
    -- Timestamp
    checked_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_service (service_name),
    INDEX idx_status (status),
    INDEX idx_checked (checked_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: notifications
-- Purpose: Platform notifications for admins
-- ============================================================================
CREATE TABLE notifications (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    
    -- Target
    target_type ENUM('all_admins', 'admin', 'tenant') NOT NULL,
    target_id VARCHAR(50),
    
    -- Content
    title VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    type ENUM('info', 'warning', 'error', 'success') DEFAULT 'info',
    category VARCHAR(50),
    
    -- Action
    action_url VARCHAR(500),
    action_text VARCHAR(100),
    
    -- Status
    is_read BOOLEAN DEFAULT FALSE,
    read_at TIMESTAMP NULL,
    
    -- Metadata
    metadata JSON,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP NULL,
    
    INDEX idx_target (target_type, target_id),
    INDEX idx_read (is_read),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- SEED DATA: Subscription Plans
-- ============================================================================
INSERT INTO subscription_plans (plan_code, plan_name, description, price_monthly, price_yearly, max_users, max_storage_gb, max_api_calls_per_month, included_modules, display_order, badge_text) VALUES
('starter', 'Starter', 'Perfect for small businesses getting started', 499.00, 4789.00, 5, 10, 10000, '["accounting", "crm", "selling", "buying"]', 1, NULL),
('professional', 'Professional', 'For growing organizations with more needs', 1500.00, 14400.00, 50, 50, 100000, '["accounting", "crm", "selling", "buying", "stock", "manufacturing", "hr", "support"]', 2, 'Popular'),
('enterprise', 'Enterprise', 'For large organizations with custom needs', 4500.00, 43200.00, -1, 500, -1, '["all"]', 3, 'Best Value');

-- ============================================================================
-- SEED DATA: Modules
-- ============================================================================
INSERT INTO modules (module_code, module_name, description, category, icon, display_order) VALUES
('accounting', 'Accounting', 'Financial management, ledgers, reports', 'core', 'Calculator', 1),
('crm', 'CRM', 'Customer relationship management', 'core', 'Users', 2),
('selling', 'Selling', 'Sales orders, quotations, invoices', 'core', 'ShoppingCart', 3),
('buying', 'Buying', 'Purchase orders, supplier management', 'core', 'Truck', 4),
('stock', 'Stock', 'Inventory management, warehouses', 'core', 'Package', 5),
('manufacturing', 'Manufacturing', 'Production planning, BOM, work orders', 'core', 'Factory', 6),
('projects', 'Projects', 'Project management, tasks, timesheets', 'core', 'ClipboardList', 7),
('support', 'Support', 'Helpdesk, tickets, SLA', 'core', 'Headphones', 8),
('website', 'Website', 'Web pages, blog, portal', 'core', 'Globe', 9),
('ecommerce', 'E-commerce', 'Online store, shopping cart', 'core', 'CreditCard', 10),
('pos', 'POS', 'Point of sale system', 'core', 'CreditCard', 11),
('assets', 'Assets', 'Fixed assets, depreciation', 'core', 'Building', 12),
('hr', 'HRMS', 'Employee management, payroll, attendance', 'standalone', 'Users', 13),
('education', 'Education', 'Schools, courses, students', 'domain', 'GraduationCap', 14),
('healthcare', 'Healthcare', 'Patients, appointments, lab tests', 'domain', 'Stethoscope', 15),
('payments', 'Payments', 'Payment gateway integrations', 'standalone', 'CreditCard', 16),
('lms', 'LMS', 'Learning Management System', 'standalone', 'GraduationCap', 17),
('helpdesk', 'Helpdesk', 'Customer support portal', 'standalone', 'Headphones', 18),
('wiki', 'Wiki', 'Knowledge base and documentation', 'standalone', 'FileText', 19),
('insights', 'Insights', 'Business intelligence and analytics', 'standalone', 'BarChart', 20),
('builder', 'Builder', 'Visual website/app builder', 'standalone', 'Puzzle', 21),
('crm_app', 'CRM App', 'Standalone CRM application', 'standalone', 'Users', 22);
