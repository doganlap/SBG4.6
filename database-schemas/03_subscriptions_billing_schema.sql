-- ============================================================================
-- ERPNext Multi-Module SaaS Platform - Subscriptions & Billing Schema
-- Version: 1.0.0
-- Description: Extended billing, metering, and subscription management
-- ============================================================================

SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;

USE saas_admin;

-- ============================================================================
-- TABLE: subscription_addons
-- Purpose: Additional add-ons that can be purchased
-- ============================================================================
CREATE TABLE subscription_addons (
    id INT AUTO_INCREMENT PRIMARY KEY,
    addon_code VARCHAR(50) NOT NULL UNIQUE,
    addon_name VARCHAR(100) NOT NULL,
    description TEXT,
    
    -- Type
    addon_type ENUM('module', 'users', 'storage', 'api_calls', 'feature', 'support') NOT NULL,
    
    -- Pricing
    price_monthly DECIMAL(10, 2) NOT NULL,
    price_yearly DECIMAL(10, 2),
    currency VARCHAR(3) DEFAULT 'USD',
    
    -- Quantity
    is_quantity_based BOOLEAN DEFAULT FALSE,
    quantity_unit VARCHAR(50) COMMENT 'e.g., users, GB, calls',
    min_quantity INT DEFAULT 1,
    max_quantity INT,
    
    -- Module specific
    module_id INT,
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (module_id) REFERENCES modules(id),
    INDEX idx_type (addon_type),
    INDEX idx_active (is_active)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_addons
-- Purpose: Add-ons purchased by tenants
-- ============================================================================
CREATE TABLE tenant_addons (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id BIGINT NOT NULL,
    addon_id INT NOT NULL,
    subscription_id BIGINT NOT NULL,
    
    -- Quantity
    quantity INT DEFAULT 1,
    
    -- Status
    status ENUM('active', 'cancelled', 'expired') DEFAULT 'active',
    
    -- Dates
    start_date DATE NOT NULL,
    end_date DATE,
    cancelled_at TIMESTAMP NULL,
    
    -- Pricing at time of purchase
    unit_price DECIMAL(10, 2) NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (addon_id) REFERENCES subscription_addons(id),
    FOREIGN KEY (subscription_id) REFERENCES subscriptions(id) ON DELETE CASCADE,
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_addon (addon_id),
    INDEX idx_status (status)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: usage_records
-- Purpose: Track resource usage for metered billing
-- ============================================================================
CREATE TABLE usage_records (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id BIGINT NOT NULL,
    subscription_id BIGINT NOT NULL,
    
    -- Period
    usage_period_start DATE NOT NULL,
    usage_period_end DATE NOT NULL,
    
    -- Usage Metrics
    user_count INT DEFAULT 0,
    storage_used_bytes BIGINT DEFAULT 0,
    api_calls INT DEFAULT 0,
    bandwidth_bytes BIGINT DEFAULT 0,
    
    -- Module-specific usage
    module_usage JSON COMMENT 'Usage per module',
    
    -- Calculated costs (for metered billing)
    overage_user_cost DECIMAL(10, 2) DEFAULT 0,
    overage_storage_cost DECIMAL(10, 2) DEFAULT 0,
    overage_api_cost DECIMAL(10, 2) DEFAULT 0,
    total_overage_cost DECIMAL(10, 2) DEFAULT 0,
    
    -- Status
    is_billed BOOLEAN DEFAULT FALSE,
    billed_invoice_id BIGINT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (subscription_id) REFERENCES subscriptions(id) ON DELETE CASCADE,
    FOREIGN KEY (billed_invoice_id) REFERENCES invoices(id),
    
    -- Unique constraint per period
    UNIQUE KEY uk_tenant_period (tenant_id, usage_period_start, usage_period_end),
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_period (usage_period_start, usage_period_end),
    INDEX idx_billed (is_billed)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: daily_usage_snapshots
-- Purpose: Daily snapshots of usage metrics
-- ============================================================================
CREATE TABLE daily_usage_snapshots (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id BIGINT NOT NULL,
    snapshot_date DATE NOT NULL,
    
    -- User metrics
    total_users INT DEFAULT 0,
    active_users INT DEFAULT 0,
    new_users INT DEFAULT 0,
    
    -- Storage metrics
    total_storage_bytes BIGINT DEFAULT 0,
    storage_delta_bytes BIGINT DEFAULT 0,
    
    -- API metrics
    api_calls INT DEFAULT 0,
    api_errors INT DEFAULT 0,
    
    -- Activity metrics
    documents_created INT DEFAULT 0,
    documents_updated INT DEFAULT 0,
    login_count INT DEFAULT 0,
    
    -- Performance
    avg_response_time_ms INT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    
    -- Unique constraint
    UNIQUE KEY uk_tenant_date (tenant_id, snapshot_date),
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_date (snapshot_date)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: invoice_line_items
-- Purpose: Detailed line items for invoices
-- ============================================================================
CREATE TABLE invoice_line_items (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    invoice_id BIGINT NOT NULL,
    
    -- Item details
    item_type ENUM('subscription', 'addon', 'overage', 'credit', 'discount', 'tax') NOT NULL,
    description VARCHAR(500) NOT NULL,
    
    -- Quantity and pricing
    quantity DECIMAL(10, 2) DEFAULT 1,
    unit_price DECIMAL(10, 2) NOT NULL,
    discount_percent DECIMAL(5, 2) DEFAULT 0,
    discount_amount DECIMAL(10, 2) DEFAULT 0,
    tax_percent DECIMAL(5, 2) DEFAULT 0,
    tax_amount DECIMAL(10, 2) DEFAULT 0,
    total_amount DECIMAL(10, 2) NOT NULL,
    
    -- Reference
    reference_type VARCHAR(50),
    reference_id VARCHAR(100),
    
    -- Period
    period_start DATE,
    period_end DATE,
    
    -- Display order
    display_order INT DEFAULT 0,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (invoice_id) REFERENCES invoices(id) ON DELETE CASCADE,
    
    INDEX idx_invoice (invoice_id),
    INDEX idx_type (item_type)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: payment_methods
-- Purpose: Stored payment methods for tenants
-- ============================================================================
CREATE TABLE payment_methods (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id BIGINT NOT NULL,
    payment_method_id VARCHAR(50) NOT NULL UNIQUE,
    
    -- Type
    type ENUM('card', 'bank_account', 'paypal') NOT NULL,
    
    -- Card details (masked)
    card_brand VARCHAR(20),
    card_last_four VARCHAR(4),
    card_exp_month INT,
    card_exp_year INT,
    
    -- Bank details (masked)
    bank_name VARCHAR(100),
    bank_last_four VARCHAR(4),
    
    -- Status
    is_default BOOLEAN DEFAULT FALSE,
    is_active BOOLEAN DEFAULT TRUE,
    
    -- External reference
    stripe_payment_method_id VARCHAR(255),
    
    -- Billing address
    billing_name VARCHAR(255),
    billing_email VARCHAR(255),
    billing_address_line_1 VARCHAR(255),
    billing_address_line_2 VARCHAR(255),
    billing_city VARCHAR(100),
    billing_state VARCHAR(100),
    billing_country VARCHAR(100),
    billing_postal_code VARCHAR(20),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_default (is_default),
    INDEX idx_active (is_active)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: credits
-- Purpose: Account credits and promotional balances
-- ============================================================================
CREATE TABLE credits (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    credit_id VARCHAR(50) NOT NULL UNIQUE,
    tenant_id BIGINT NOT NULL,
    
    -- Amount
    original_amount DECIMAL(10, 2) NOT NULL,
    remaining_amount DECIMAL(10, 2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'USD',
    
    -- Type
    credit_type ENUM('promotional', 'refund', 'referral', 'adjustment', 'other') NOT NULL,
    
    -- Description
    description TEXT,
    
    -- Validity
    valid_from DATE NOT NULL,
    valid_until DATE,
    
    -- Status
    status ENUM('active', 'exhausted', 'expired', 'cancelled') DEFAULT 'active',
    
    -- Reference
    reference_type VARCHAR(50),
    reference_id VARCHAR(100),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status),
    INDEX idx_valid (valid_until)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: credit_transactions
-- Purpose: Track credit usage
-- ============================================================================
CREATE TABLE credit_transactions (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    credit_id BIGINT NOT NULL,
    
    -- Transaction
    transaction_type ENUM('apply', 'expire', 'refund', 'cancel') NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    
    -- Applied to
    invoice_id BIGINT,
    
    -- Balance after transaction
    balance_after DECIMAL(10, 2) NOT NULL,
    
    -- Description
    description TEXT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (credit_id) REFERENCES credits(id) ON DELETE CASCADE,
    FOREIGN KEY (invoice_id) REFERENCES invoices(id),
    
    INDEX idx_credit (credit_id),
    INDEX idx_invoice (invoice_id)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: coupons
-- Purpose: Discount coupons
-- ============================================================================
CREATE TABLE coupons (
    id INT AUTO_INCREMENT PRIMARY KEY,
    coupon_code VARCHAR(50) NOT NULL UNIQUE,
    
    -- Description
    name VARCHAR(100) NOT NULL,
    description TEXT,
    
    -- Discount
    discount_type ENUM('percent', 'fixed') NOT NULL,
    discount_value DECIMAL(10, 2) NOT NULL,
    max_discount_amount DECIMAL(10, 2) COMMENT 'Cap for percentage discounts',
    
    -- Applicability
    applies_to ENUM('all', 'plan', 'addon', 'new_customers') DEFAULT 'all',
    applicable_plan_ids JSON,
    applicable_addon_ids JSON,
    
    -- Limits
    max_redemptions INT,
    max_redemptions_per_tenant INT DEFAULT 1,
    current_redemptions INT DEFAULT 0,
    
    -- Duration
    duration_months INT COMMENT 'How many months discount applies',
    
    -- Validity
    valid_from TIMESTAMP NOT NULL,
    valid_until TIMESTAMP,
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_code (coupon_code),
    INDEX idx_active (is_active),
    INDEX idx_valid (valid_from, valid_until)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: coupon_redemptions
-- Purpose: Track coupon usage
-- ============================================================================
CREATE TABLE coupon_redemptions (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    coupon_id INT NOT NULL,
    tenant_id BIGINT NOT NULL,
    subscription_id BIGINT,
    
    -- Discount applied
    discount_amount DECIMAL(10, 2) NOT NULL,
    
    -- Duration
    starts_at TIMESTAMP NOT NULL,
    ends_at TIMESTAMP,
    months_remaining INT,
    
    -- Status
    status ENUM('active', 'completed', 'cancelled') DEFAULT 'active',
    
    -- Timestamps
    redeemed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (coupon_id) REFERENCES coupons(id),
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (subscription_id) REFERENCES subscriptions(id),
    
    INDEX idx_coupon (coupon_id),
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tax_rates
-- Purpose: Tax rate configurations
-- ============================================================================
CREATE TABLE tax_rates (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tax_code VARCHAR(50) NOT NULL UNIQUE,
    
    -- Location
    country VARCHAR(100) NOT NULL,
    state VARCHAR(100),
    city VARCHAR(100),
    postal_code VARCHAR(20),
    
    -- Rate
    tax_type ENUM('vat', 'gst', 'sales_tax', 'other') DEFAULT 'sales_tax',
    tax_rate DECIMAL(5, 2) NOT NULL,
    
    -- Description
    description TEXT,
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_country (country),
    INDEX idx_active (is_active)
) ENGINE=InnoDB;

-- ============================================================================
-- SEED DATA: Add-ons
-- ============================================================================
INSERT INTO subscription_addons (addon_code, addon_name, description, addon_type, price_monthly, price_yearly, is_quantity_based, quantity_unit) VALUES
('extra_users_10', 'Additional 10 Users', 'Add 10 more users to your plan', 'users', 99.00, 949.00, FALSE, NULL),
('extra_users_25', 'Additional 25 Users', 'Add 25 more users to your plan', 'users', 199.00, 1909.00, FALSE, NULL),
('extra_storage_50', 'Additional 50 GB Storage', 'Add 50 GB of storage', 'storage', 49.00, 469.00, FALSE, NULL),
('extra_storage_100', 'Additional 100 GB Storage', 'Add 100 GB of storage', 'storage', 89.00, 859.00, FALSE, NULL),
('api_unlimited', 'Unlimited API Calls', 'Remove API call limits', 'api_calls', 199.00, 1909.00, FALSE, NULL),
('priority_support', 'Priority Support', '24/7 priority support with dedicated agent', 'support', 299.00, 2869.00, FALSE, NULL),
('custom_domain', 'Custom Domain', 'Use your own domain for ERPNext', 'feature', 29.00, 279.00, FALSE, NULL),
('white_label', 'White Label', 'Remove ERPNext branding', 'feature', 99.00, 949.00, FALSE, NULL);

-- ============================================================================
-- SEED DATA: Tax Rates
-- ============================================================================
INSERT INTO tax_rates (tax_code, country, state, tax_type, tax_rate, description) VALUES
('US_CA', 'United States', 'California', 'sales_tax', 7.25, 'California State Sales Tax'),
('US_NY', 'United States', 'New York', 'sales_tax', 8.00, 'New York State Sales Tax'),
('US_TX', 'United States', 'Texas', 'sales_tax', 6.25, 'Texas State Sales Tax'),
('UK_VAT', 'United Kingdom', NULL, 'vat', 20.00, 'UK Value Added Tax'),
('EU_VAT', 'European Union', NULL, 'vat', 21.00, 'EU Standard VAT Rate'),
('IN_GST', 'India', NULL, 'gst', 18.00, 'India GST Rate');
