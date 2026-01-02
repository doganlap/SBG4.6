-- ============================================================================
-- ERPNext Multi-Module SaaS Platform - Container Management Schema
-- Version: 1.0.0
-- Description: Container orchestration and tenant deployment tracking
-- ============================================================================

SET NAMES utf8mb4;
SET CHARACTER SET utf8mb4;

USE saas_admin;

-- ============================================================================
-- TABLE: container_images
-- Purpose: Docker images registry
-- ============================================================================
CREATE TABLE container_images (
    id INT AUTO_INCREMENT PRIMARY KEY,
    image_id VARCHAR(100) NOT NULL UNIQUE,
    
    -- Image info
    image_name VARCHAR(255) NOT NULL,
    image_tag VARCHAR(100) NOT NULL,
    full_image_url VARCHAR(500) NOT NULL,
    
    -- Registry
    registry_url VARCHAR(255),
    registry_type ENUM('dockerhub', 'ecr', 'gcr', 'acr', 'private') DEFAULT 'dockerhub',
    
    -- Type
    image_type ENUM('base', 'module', 'custom', 'tenant') NOT NULL,
    
    -- Module (if module image)
    module_id INT,
    
    -- Version info
    erpnext_version VARCHAR(20),
    frappe_version VARCHAR(20),
    
    -- Size
    image_size_mb INT,
    
    -- Status
    is_active BOOLEAN DEFAULT TRUE,
    is_latest BOOLEAN DEFAULT FALSE,
    
    -- Build info
    build_date TIMESTAMP,
    commit_hash VARCHAR(40),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (module_id) REFERENCES modules(id),
    
    INDEX idx_name_tag (image_name, image_tag),
    INDEX idx_type (image_type),
    INDEX idx_module (module_id)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: container_instances
-- Purpose: Running container instances
-- ============================================================================
CREATE TABLE container_instances (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    instance_id VARCHAR(100) NOT NULL UNIQUE,
    
    -- Tenant
    tenant_id BIGINT NOT NULL,
    
    -- Image
    image_id INT NOT NULL,
    
    -- Container info
    container_id VARCHAR(100) COMMENT 'Docker container ID',
    container_name VARCHAR(255),
    
    -- Type
    instance_type ENUM('backend', 'worker_short', 'worker_long', 'worker_default', 'scheduler', 'socketio') NOT NULL,
    
    -- Host
    host_id INT,
    host_ip VARCHAR(45),
    
    -- Ports
    internal_port INT,
    external_port INT,
    
    -- Resources
    cpu_limit DECIMAL(5, 2) COMMENT 'CPU cores limit',
    memory_limit_mb INT COMMENT 'Memory limit in MB',
    
    -- Status
    status ENUM('creating', 'running', 'stopped', 'failed', 'terminated') DEFAULT 'creating',
    health_status ENUM('healthy', 'unhealthy', 'unknown') DEFAULT 'unknown',
    
    -- Metrics
    cpu_usage_percent DECIMAL(5, 2),
    memory_usage_mb INT,
    restart_count INT DEFAULT 0,
    
    -- Timestamps
    started_at TIMESTAMP NULL,
    stopped_at TIMESTAMP NULL,
    last_health_check TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    FOREIGN KEY (image_id) REFERENCES container_images(id),
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status),
    INDEX idx_type (instance_type),
    INDEX idx_health (health_status)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: container_hosts
-- Purpose: Physical/virtual hosts running containers
-- ============================================================================
CREATE TABLE container_hosts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    host_id VARCHAR(50) NOT NULL UNIQUE,
    
    -- Host info
    hostname VARCHAR(255) NOT NULL,
    ip_address VARCHAR(45) NOT NULL,
    
    -- Type
    host_type ENUM('docker', 'kubernetes', 'swarm', 'ecs', 'other') DEFAULT 'docker',
    
    -- Region/Zone
    region VARCHAR(50),
    availability_zone VARCHAR(50),
    
    -- Capacity
    total_cpu_cores INT,
    total_memory_gb INT,
    total_storage_gb INT,
    
    -- Current usage
    used_cpu_cores DECIMAL(5, 2),
    used_memory_gb DECIMAL(10, 2),
    used_storage_gb DECIMAL(10, 2),
    
    -- Container count
    max_containers INT DEFAULT 100,
    current_containers INT DEFAULT 0,
    
    -- Status
    status ENUM('active', 'draining', 'maintenance', 'offline') DEFAULT 'active',
    
    -- Metadata
    labels JSON,
    
    -- Timestamps
    last_heartbeat TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_status (status),
    INDEX idx_region (region)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_deployments
-- Purpose: Track tenant deployment history
-- ============================================================================
CREATE TABLE tenant_deployments (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    deployment_id VARCHAR(50) NOT NULL UNIQUE,
    tenant_id BIGINT NOT NULL,
    
    -- Deployment type
    deployment_type ENUM('initial', 'upgrade', 'rollback', 'scale', 'migrate') NOT NULL,
    
    -- Version info
    from_version VARCHAR(50),
    to_version VARCHAR(50),
    
    -- Status
    status ENUM('pending', 'in_progress', 'completed', 'failed', 'rolled_back') DEFAULT 'pending',
    
    -- Details
    description TEXT,
    error_message TEXT,
    
    -- Timing
    started_at TIMESTAMP NULL,
    completed_at TIMESTAMP NULL,
    duration_seconds INT,
    
    -- Initiated by
    initiated_by VARCHAR(100),
    
    -- Metadata
    metadata JSON,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status),
    INDEX idx_type (deployment_type),
    INDEX idx_created (created_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: tenant_databases
-- Purpose: Track tenant database instances
-- ============================================================================
CREATE TABLE tenant_databases (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    tenant_id BIGINT NOT NULL,
    
    -- Database info
    database_name VARCHAR(100) NOT NULL UNIQUE,
    database_type ENUM('mariadb', 'mysql', 'postgres') DEFAULT 'mariadb',
    
    -- Connection
    host VARCHAR(255) NOT NULL,
    port INT DEFAULT 3306,
    
    -- Credentials (encrypted)
    username VARCHAR(100) NOT NULL,
    password_encrypted TEXT NOT NULL,
    
    -- Size
    size_mb BIGINT DEFAULT 0,
    max_size_mb BIGINT,
    
    -- Status
    status ENUM('creating', 'active', 'suspended', 'migrating', 'deleted') DEFAULT 'creating',
    
    -- Backup
    last_backup_at TIMESTAMP NULL,
    backup_schedule VARCHAR(100),
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_status (status)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: database_backups
-- Purpose: Track database backups
-- ============================================================================
CREATE TABLE database_backups (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    backup_id VARCHAR(50) NOT NULL UNIQUE,
    tenant_database_id BIGINT NOT NULL,
    tenant_id BIGINT NOT NULL,
    
    -- Backup type
    backup_type ENUM('full', 'incremental', 'point_in_time') DEFAULT 'full',
    
    -- Storage
    storage_type ENUM('local', 's3', 'azure', 'gcs') NOT NULL,
    storage_path VARCHAR(500) NOT NULL,
    
    -- Size
    size_mb BIGINT NOT NULL,
    compressed BOOLEAN DEFAULT TRUE,
    
    -- Status
    status ENUM('in_progress', 'completed', 'failed', 'deleted') DEFAULT 'in_progress',
    
    -- Timing
    started_at TIMESTAMP NOT NULL,
    completed_at TIMESTAMP NULL,
    duration_seconds INT,
    
    -- Retention
    retention_days INT DEFAULT 30,
    expires_at TIMESTAMP,
    
    -- Error info
    error_message TEXT,
    
    -- Timestamps
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_database_id) REFERENCES tenant_databases(id),
    FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
    
    INDEX idx_tenant (tenant_id),
    INDEX idx_database (tenant_database_id),
    INDEX idx_status (status),
    INDEX idx_expires (expires_at)
) ENGINE=InnoDB;

-- ============================================================================
-- TABLE: deployment_logs
-- Purpose: Deployment activity logs
-- ============================================================================
CREATE TABLE deployment_logs (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    deployment_id BIGINT NOT NULL,
    
    -- Log entry
    log_level ENUM('debug', 'info', 'warning', 'error') DEFAULT 'info',
    message TEXT NOT NULL,
    
    -- Step info
    step_name VARCHAR(100),
    step_number INT,
    
    -- Timestamp
    logged_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (deployment_id) REFERENCES tenant_deployments(id) ON DELETE CASCADE,
    
    INDEX idx_deployment (deployment_id),
    INDEX idx_level (log_level),
    INDEX idx_logged (logged_at)
) ENGINE=InnoDB;

-- ============================================================================
-- SEED DATA: Base Images
-- ============================================================================
INSERT INTO container_images (image_id, image_name, image_tag, full_image_url, image_type, erpnext_version, is_active, is_latest) VALUES
('img_base_v16', 'erpnext-saas/base', 'v16', 'erpnext-saas/base:v16', 'base', 'v16', TRUE, TRUE),
('img_accounting_v16', 'erpnext-saas/accounting', 'v16', 'erpnext-saas/accounting:v16', 'module', 'v16', TRUE, TRUE),
('img_crm_v16', 'erpnext-saas/crm', 'v16', 'erpnext-saas/crm:v16', 'module', 'v16', TRUE, TRUE),
('img_hr_v16', 'erpnext-saas/hr', 'v16', 'erpnext-saas/hr:v16', 'module', 'v16', TRUE, TRUE),
('img_healthcare_v16', 'erpnext-saas/healthcare', 'v16', 'erpnext-saas/healthcare:v16', 'module', 'v16', TRUE, TRUE),
('img_education_v16', 'erpnext-saas/education', 'v16', 'erpnext-saas/education:v16', 'module', 'v16', TRUE, TRUE);
