-- ============================================================================
-- ERPNext Multi-Module SaaS Platform - Tenant Admin Database Schema
-- Version: 1.0.0
-- Description: Per-tenant configuration and user management
-- Note: This schema is created per tenant with database name: tenant_{tenant_id}
-- ============================================================================

SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;

-- This is a template - actual database name will be tenant_<tenant_id>
-- CREATE DATABASE IF NOT EXISTS tenant_template;
-- USE tenant_template;

-- ============================================================================
-- TABLE: tenant_settings
-- Purpose: Tenant-specific configuration
-- ============================================================================
CREATE TABLE tenant_settings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    setting_key VARCHAR(255) NOT NULL UNIQUE,
    setting_value TEXT,
    setting_type ENUM('string', 'number', 'boolean', 'json') DEFAULT 'string',
    category VARCHAR(100),
    description TEXT,
    is_user_editable BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_category (category),
    INDEX idx_key (setting_key)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_users
-- Purpose: Users within the tenant organization
-- ============================================================================
CREATE TABLE tenant_users (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    user_id VARCHAR(50) NOT NULL UNIQUE COMMENT 'Public-facing ID',
    
    -- Authentication
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255),
    
    -- Profile
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    full_name VARCHAR(255) GENERATED ALWAYS AS (CONCAT(first_name, ' ', last_name)) STORED,
    phone VARCHAR(50),
    mobile VARCHAR(50),
    avatar_url VARCHAR(500),
    
    -- Employment
    employee_id VARCHAR(50),
    department VARCHAR(100),
    designation VARCHAR(100),
    reports_to BIGINT,
    
    -- Role & Permissions
    role_id INT,
    is_admin BOOLEAN DEFAULT FALSE,
    permissions JSON,
    
    -- Access
    allowed_modules JSON COMMENT 'Array of module codes user can access',
    
    -- Security
    is_active BOOLEAN DEFAULT TRUE,
    is_verified BOOLEAN DEFAULT FALSE,
    two_factor_enabled BOOLEAN DEFAULT FALSE,
    two_factor_secret VARCHAR(255),
    
    -- Session
    last_login_at TIMESTAMP NULL,
    last_login_ip VARCHAR(45),
    last_activity_at TIMESTAMP NULL,
    failed_login_attempts INT DEFAULT 0,
    locked_until TIMESTAMP NULL,
    
    -- ERPNext Integration
    erpnext_user_id VARCHAR(100),
    erpnext_user_email VARCHAR(255),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP NULL,
    
    -- Foreign Keys
    FOREIGN KEY (reports_to) REFERENCES tenant_users(id) ON DELETE SET NULL,
    
    -- Indexes
    INDEX idx_email (email),
    INDEX idx_department (department),
    INDEX idx_active (is_active),
    INDEX idx_employee_id (employee_id)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_roles
-- Purpose: Role definitions for the tenant
-- ============================================================================
CREATE TABLE tenant_roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    role_code VARCHAR(50) NOT NULL UNIQUE,
    role_name VARCHAR(100) NOT NULL,
    description TEXT,
    
    -- Permissions
    permissions JSON COMMENT 'Detailed permission matrix',
    allowed_modules JSON COMMENT 'Modules this role can access',
    
    -- Hierarchy
    level INT DEFAULT 0 COMMENT 'Higher = more access',
    parent_role_id INT,
    
    -- Status
    is_system_role BOOLEAN DEFAULT FALSE COMMENT 'Cannot be deleted',
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (parent_role_id) REFERENCES tenant_roles(id) ON DELETE SET NULL,
    
    INDEX idx_code (role_code),
    INDEX idx_level (level)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_departments
-- Purpose: Organizational departments
-- ============================================================================
CREATE TABLE tenant_departments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    department_code VARCHAR(50) NOT NULL UNIQUE,
    department_name VARCHAR(100) NOT NULL,
    description TEXT,
    
    -- Hierarchy
    parent_department_id INT,
    
    -- Head
    head_user_id BIGINT,
    
    -- Cost Center
    cost_center VARCHAR(100),
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (parent_department_id) REFERENCES tenant_departments(id) ON DELETE SET NULL,
    FOREIGN KEY (head_user_id) REFERENCES tenant_users(id) ON DELETE SET NULL,
    
    INDEX idx_code (department_code)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_module_settings
-- Purpose: Per-module configuration for the tenant
-- ============================================================================
CREATE TABLE tenant_module_settings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    module_code VARCHAR(50) NOT NULL,
    
    -- Configuration
    settings JSON,
    custom_fields JSON,
    workflow_config JSON,
    
    -- Features
    enabled_features JSON,
    disabled_features JSON,
    
    -- UI Customization
    dashboard_config JSON,
    list_view_config JSON,
    form_layout_config JSON,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    UNIQUE KEY uk_module (module_code),
    INDEX idx_module (module_code)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_api_keys
-- Purpose: API keys for external integrations
-- ============================================================================
CREATE TABLE tenant_api_keys (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    key_id VARCHAR(50) NOT NULL UNIQUE,
    
    -- Key
    api_key_prefix VARCHAR(10) NOT NULL,
    api_key_hash VARCHAR(255) NOT NULL,
    
    -- Metadata
    name VARCHAR(100) NOT NULL,
    description TEXT,
    
    -- Permissions
    scopes JSON COMMENT 'API scopes this key has access to',
    allowed_ips JSON COMMENT 'IP whitelist',
    rate_limit_per_minute INT DEFAULT 100,
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Usage
    last_used_at TIMESTAMP NULL,
    usage_count BIGINT DEFAULT 0,
    
    -- Expiry
    expires_at TIMESTAMP NULL,
    
    -- Creator
    created_by BIGINT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (created_by) REFERENCES tenant_users(id) ON DELETE SET NULL,
    
    INDEX idx_active (is_active),
    INDEX idx_prefix (api_key_prefix)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_webhooks
-- Purpose: Webhook configurations
-- ============================================================================
CREATE TABLE tenant_webhooks (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    webhook_id VARCHAR(50) NOT NULL UNIQUE,
    
    -- Configuration
    name VARCHAR(100) NOT NULL,
    description TEXT,
    url VARCHAR(500) NOT NULL,
    
    -- Events
    events JSON NOT NULL COMMENT 'Array of event types to trigger on',
    
    -- Security
    secret_hash VARCHAR(255),
    headers JSON COMMENT 'Custom headers to send',
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    
    -- Retry
    max_retries INT DEFAULT 3,
    retry_delay_seconds INT DEFAULT 60,
    
    -- Stats
    last_triggered_at TIMESTAMP NULL,
    last_success_at TIMESTAMP NULL,
    last_failure_at TIMESTAMP NULL,
    success_count BIGINT DEFAULT 0,
    failure_count BIGINT DEFAULT 0,
    
    -- Creator
    created_by BIGINT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (created_by) REFERENCES tenant_users(id) ON DELETE SET NULL,
    
    INDEX idx_active (is_active)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_audit_log
-- Purpose: Tenant-level audit trail
-- ============================================================================
CREATE TABLE tenant_audit_log (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    
    -- Actor
    user_id BIGINT,
    user_email VARCHAR(255),
    
    -- Action
    action VARCHAR(100) NOT NULL,
    resource_type VARCHAR(100) NOT NULL,
    resource_id VARCHAR(100),
    
    -- Details
    description TEXT,
    old_values JSON,
    new_values JSON,
    
    -- Context
    ip_address VARCHAR(45),
    user_agent TEXT,
    session_id VARCHAR(100),
    
    -- Timestamp
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (user_id) REFERENCES tenant_users(id) ON DELETE SET NULL,
    
    -- Indexes
    INDEX idx_user (user_id),
    INDEX idx_action (action),
    INDEX idx_resource (resource_type, resource_id),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_activity_log
-- Purpose: User activity tracking
-- ============================================================================
CREATE TABLE tenant_activity_log (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    
    -- User
    user_id BIGINT NOT NULL,
    
    -- Activity
    activity_type VARCHAR(50) NOT NULL,
    description TEXT,
    
    -- Context
    module VARCHAR(50),
    doctype VARCHAR(100),
    document_name VARCHAR(255),
    
    -- Metadata
    metadata JSON,
    
    -- Timestamp
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (user_id) REFERENCES tenant_users(id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX idx_user (user_id),
    INDEX idx_type (activity_type),
    INDEX idx_module (module),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_notifications
-- Purpose: User notifications within tenant
-- ============================================================================
CREATE TABLE tenant_notifications (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    
    -- Target
    user_id BIGINT,
    role_id INT,
    target_all BOOLEAN DEFAULT FALSE,
    
    -- Content
    title VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    type ENUM('info', 'warning', 'error', 'success') DEFAULT 'info',
    category VARCHAR(50),
    
    -- Action
    action_url VARCHAR(500),
    action_text VARCHAR(100),
    
    -- Status per user (for individual notifications)
    is_read BOOLEAN DEFAULT FALSE,
    read_at TIMESTAMP NULL,
    
    -- Metadata
    metadata JSON,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP NULL,
    
    -- Foreign Keys
    FOREIGN KEY (user_id) REFERENCES tenant_users(id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES tenant_roles(id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX idx_user (user_id),
    INDEX idx_read (is_read),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_files
-- Purpose: File uploads and attachments
-- ============================================================================
CREATE TABLE tenant_files (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    file_id VARCHAR(50) NOT NULL UNIQUE,
    
    -- File Info
    original_name VARCHAR(255) NOT NULL,
    stored_name VARCHAR(255) NOT NULL,
    mime_type VARCHAR(100),
    file_size BIGINT COMMENT 'Size in bytes',
    
    -- Storage
    storage_type ENUM('local', 's3', 'azure', 'gcs') DEFAULT 'local',
    storage_path VARCHAR(500) NOT NULL,
    public_url VARCHAR(500),
    
    -- Attachment
    attached_to_doctype VARCHAR(100),
    attached_to_name VARCHAR(255),
    
    -- Security
    is_private BOOLEAN DEFAULT FALSE,
    
    -- Uploaded By
    uploaded_by BIGINT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Keys
    FOREIGN KEY (uploaded_by) REFERENCES tenant_users(id) ON DELETE SET NULL,
    
    -- Indexes
    INDEX idx_attached (attached_to_doctype, attached_to_name),
    INDEX idx_uploader (uploaded_by)
) ENGINE=InnoDB;

-- ============================================================================
-- SEED DATA: Default Roles
-- ============================================================================
INSERT INTO tenant_roles (role_code, role_name, description, level, is_system_role, permissions) VALUES
('admin', 'Administrator', 'Full access to all features and settings', 100, TRUE, '{"all": true}'),
('manager', 'Manager', 'Can manage users and most settings', 80, TRUE, '{"users": ["read", "create", "update"], "settings": ["read", "update"]}'),
('user', 'Standard User', 'Regular user with basic access', 50, TRUE, '{"own_data": true}'),
('readonly', 'Read Only', 'Can only view data, no modifications', 10, TRUE, '{"read": true}');

-- ============================================================================
-- SEED DATA: Default Settings
-- ============================================================================
INSERT INTO tenant_settings (setting_key, setting_value, setting_type, category, description) VALUES
('company_name', NULL, 'string', 'general', 'Company display name'),
('timezone', 'UTC', 'string', 'general', 'Default timezone'),
('date_format', 'YYYY-MM-DD', 'string', 'general', 'Date display format'),
('time_format', 'HH:mm:ss', 'string', 'general', 'Time display format'),
('currency', 'USD', 'string', 'general', 'Default currency'),
('language', 'en', 'string', 'general', 'Default language'),
('enable_two_factor', 'false', 'boolean', 'security', 'Require 2FA for all users'),
('session_timeout_minutes', '60', 'number', 'security', 'Session timeout in minutes'),
('password_min_length', '8', 'number', 'security', 'Minimum password length'),
('email_notifications', 'true', 'boolean', 'notifications', 'Enable email notifications'),
('slack_notifications', 'false', 'boolean', 'notifications', 'Enable Slack notifications');
