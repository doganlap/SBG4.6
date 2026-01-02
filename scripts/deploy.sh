#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Deployment Script
# Version: 1.0.0
# Description: Deploy the complete SaaS platform
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

PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DOCKER_DIR="${PROJECT_DIR}/docker"
ENV_FILE="${PROJECT_DIR}/configs/common.env"

#==============================================================================
# Check Prerequisites
#==============================================================================
check_prerequisites() {
    log_info "Checking prerequisites..."
    
    # Check Docker
    if ! command -v docker &> /dev/null; then
        log_error "Docker is not installed"
        exit 1
    fi
    
    # Check Docker Compose
    if ! docker compose version &> /dev/null; then
        log_error "Docker Compose is not installed"
        exit 1
    fi
    
    log_success "All prerequisites met"
}

#==============================================================================
# Create Docker Network
#==============================================================================
create_network() {
    log_info "Creating Docker network..."
    
    docker network create erpnext-network 2>/dev/null || log_warn "Network already exists"
    
    log_success "Docker network ready"
}

#==============================================================================
# Initialize Databases
#==============================================================================
init_databases() {
    log_info "Initializing databases..."
    
    # Start MariaDB
    docker compose -f ${DOCKER_DIR}/docker-compose.yml up -d mariadb
    
    # Wait for MariaDB to be ready
    log_info "Waiting for MariaDB to be ready..."
    sleep 30
    
    # Run schema files
    for schema in ${PROJECT_DIR}/database-schemas/*.sql; do
        log_info "Applying schema: $(basename $schema)"
        docker compose -f ${DOCKER_DIR}/docker-compose.yml exec -T mariadb \
            mysql -u root -p${DB_ROOT_PASSWORD:-rootpassword} < $schema || log_warn "Schema may already exist"
    done
    
    log_success "Databases initialized"
}

#==============================================================================
# Start Core Services
#==============================================================================
start_core_services() {
    log_info "Starting core services..."
    
    docker compose -f ${DOCKER_DIR}/docker-compose.yml up -d \
        mariadb \
        redis-cache \
        redis-queue \
        redis-socketio
    
    log_info "Waiting for services to be ready..."
    sleep 20
    
    log_success "Core services started"
}

#==============================================================================
# Start ERPNext Services
#==============================================================================
start_erpnext_services() {
    log_info "Starting ERPNext services..."
    
    docker compose -f ${DOCKER_DIR}/docker-compose.yml up -d \
        erpnext-backend \
        erpnext-socketio \
        erpnext-scheduler \
        erpnext-worker-short \
        erpnext-worker-long \
        erpnext-worker-default
    
    log_info "Waiting for ERPNext to be ready..."
    sleep 60
    
    log_success "ERPNext services started"
}

#==============================================================================
# Start Portal Services
#==============================================================================
start_portal_services() {
    log_info "Starting portal services..."
    
    docker compose -f ${DOCKER_DIR}/docker-compose.yml up -d \
        saas-admin-portal \
        tenant-admin-portal \
        customer-portal \
        showcase-pages
    
    log_success "Portal services started"
}

#==============================================================================
# Start Nginx
#==============================================================================
start_nginx() {
    log_info "Starting Nginx..."
    
    docker compose -f ${DOCKER_DIR}/docker-compose.yml up -d nginx
    
    log_success "Nginx started"
}

#==============================================================================
# Full Deployment
#==============================================================================
full_deploy() {
    check_prerequisites
    create_network
    start_core_services
    init_databases
    start_erpnext_services
    start_portal_services
    start_nginx
}

#==============================================================================
# Show Status
#==============================================================================
show_status() {
    log_info "Service Status:"
    docker compose -f ${DOCKER_DIR}/docker-compose.yml ps
}

#==============================================================================
# Show Access URLs
#==============================================================================
show_urls() {
    log_info "=============================================="
    log_info "Access URLs:"
    log_info "=============================================="
    echo ""
    echo "ERPNext:           http://localhost:8000"
    echo "SaaS Admin Portal: http://localhost:3000"
    echo "Tenant Admin:      http://localhost:3001"
    echo "Customer Portal:   http://localhost:3002"
    echo "Showcase Pages:    http://localhost:3003"
    echo ""
    log_info "=============================================="
}

#==============================================================================
# Stop All Services
#==============================================================================
stop_all() {
    log_info "Stopping all services..."
    docker compose -f ${DOCKER_DIR}/docker-compose.yml down
    log_success "All services stopped"
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Deployment"
    log_info "=============================================="
    
    case "${1:-deploy}" in
        deploy)
            full_deploy
            show_urls
            ;;
        start)
            docker compose -f ${DOCKER_DIR}/docker-compose.yml up -d
            show_urls
            ;;
        stop)
            stop_all
            ;;
        restart)
            stop_all
            full_deploy
            show_urls
            ;;
        status)
            show_status
            ;;
        urls)
            show_urls
            ;;
        logs)
            docker compose -f ${DOCKER_DIR}/docker-compose.yml logs -f ${2:-}
            ;;
        *)
            echo "Usage: $0 {deploy|start|stop|restart|status|urls|logs [service]}"
            exit 1
            ;;
    esac
}

main "$@"
