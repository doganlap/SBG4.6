#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Build All Script
# Version: 1.0.0
# Description: Build all Docker images and frontend applications
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
DOCKER_REGISTRY="${DOCKER_REGISTRY:-erpnext-saas}"
VERSION="${VERSION:-v16}"

#==============================================================================
# Build Base Docker Image
#==============================================================================
build_base_image() {
    log_info "Building base Docker image..."
    
    docker build \
        -t ${DOCKER_REGISTRY}/base:${VERSION} \
        -f ${PROJECT_DIR}/docker/modules/Dockerfile.base \
        ${PROJECT_DIR}
    
    log_success "Base image built successfully"
}

#==============================================================================
# Build Module Docker Images
#==============================================================================
build_module_images() {
    log_info "Building module Docker images..."
    
    MODULES=(
        "accounting"
        "crm"
        "selling"
        "buying"
        "stock"
        "manufacturing"
        "hr"
        "education"
        "healthcare"
    )
    
    for module in "${MODULES[@]}"; do
        if [ -f "${PROJECT_DIR}/docker/modules/Dockerfile.${module}" ]; then
            log_info "Building ${module} module image..."
            docker build \
                -t ${DOCKER_REGISTRY}/${module}:${VERSION} \
                -f ${PROJECT_DIR}/docker/modules/Dockerfile.${module} \
                ${PROJECT_DIR}
            log_success "${module} module image built"
        fi
    done
    
    log_success "All module images built successfully"
}

#==============================================================================
# Build Frontend Applications
#==============================================================================
build_frontend_apps() {
    log_info "Building frontend applications..."
    
    # SaaS Admin Portal
    log_info "Building SaaS Admin Portal..."
    cd ${PROJECT_DIR}/saas-admin-portal
    npm ci
    npm run build
    docker build -t ${DOCKER_REGISTRY}/saas-admin:${VERSION} .
    log_success "SaaS Admin Portal built"
    
    # Tenant Admin Portal
    log_info "Building Tenant Admin Portal..."
    cd ${PROJECT_DIR}/tenant-admin-portal
    npm ci
    npm run build
    docker build -t ${DOCKER_REGISTRY}/tenant-admin:${VERSION} .
    log_success "Tenant Admin Portal built"
    
    # Customer Portal
    log_info "Building Customer Portal..."
    cd ${PROJECT_DIR}/customer-portal
    npm ci
    npm run build
    docker build -t ${DOCKER_REGISTRY}/customer-portal:${VERSION} .
    log_success "Customer Portal built"
    
    # Showcase Pages
    log_info "Building Showcase Pages..."
    cd ${PROJECT_DIR}/showcase-pages
    npm ci
    npm run build
    docker build -t ${DOCKER_REGISTRY}/showcase:${VERSION} .
    log_success "Showcase Pages built"
    
    cd ${PROJECT_DIR}
    log_success "All frontend applications built successfully"
}

#==============================================================================
# Push Images to Registry
#==============================================================================
push_images() {
    log_info "Pushing images to registry..."
    
    IMAGES=(
        "base"
        "accounting"
        "crm"
        "selling"
        "buying"
        "stock"
        "manufacturing"
        "hr"
        "education"
        "healthcare"
        "saas-admin"
        "tenant-admin"
        "customer-portal"
        "showcase"
    )
    
    for image in "${IMAGES[@]}"; do
        log_info "Pushing ${image}..."
        docker push ${DOCKER_REGISTRY}/${image}:${VERSION} || log_warn "Failed to push ${image}"
    done
    
    log_success "All images pushed to registry"
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Build All"
    log_info "=============================================="
    log_info "Project Directory: ${PROJECT_DIR}"
    log_info "Docker Registry: ${DOCKER_REGISTRY}"
    log_info "Version: ${VERSION}"
    log_info "=============================================="
    
    case "${1:-all}" in
        base)
            build_base_image
            ;;
        modules)
            build_module_images
            ;;
        frontend)
            build_frontend_apps
            ;;
        push)
            push_images
            ;;
        all)
            build_base_image
            build_module_images
            build_frontend_apps
            ;;
        *)
            echo "Usage: $0 {base|modules|frontend|push|all}"
            exit 1
            ;;
    esac
    
    log_info "=============================================="
    log_success "Build complete!"
    log_info "=============================================="
}

main "$@"
