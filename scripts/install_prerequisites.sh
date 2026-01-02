#!/bin/bash
#==============================================================================
# ERPNext Multi-Module SaaS Platform - Prerequisites Installation Script
# Version: 1.0.0
# Target: Ubuntu 22.04 LTS / Debian 12
#==============================================================================

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

log_info() { echo -e "${BLUE}[INFO]${NC} $1"; }
log_success() { echo -e "${GREEN}[SUCCESS]${NC} $1"; }
log_warn() { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

#==============================================================================
# Configuration
#==============================================================================
PYTHON_VERSION="3.11"
NODE_VERSION="18"
MARIADB_VERSION="10.6"
REDIS_VERSION="latest"
BENCH_BRANCH="version-16"
ERPNEXT_BRANCH="version-16"

#==============================================================================
# System Update
#==============================================================================
install_system_packages() {
    log_info "Updating system packages..."
    sudo apt-get update -y
    sudo apt-get upgrade -y

    log_info "Installing essential packages..."
    sudo apt-get install -y \
        git \
        curl \
        wget \
        build-essential \
        python3-dev \
        python3-pip \
        python3-venv \
        python3-setuptools \
        libffi-dev \
        libssl-dev \
        libjpeg-dev \
        libpng-dev \
        libfreetype6-dev \
        zlib1g-dev \
        libmysqlclient-dev \
        libxslt1-dev \
        libxml2-dev \
        libcups2-dev \
        libtiff5-dev \
        libopenjp2-7-dev \
        libwebp-dev \
        tcl8.6-dev \
        tk8.6-dev \
        software-properties-common \
        apt-transport-https \
        ca-certificates \
        gnupg \
        lsb-release \
        xvfb \
        libfontconfig \
        wkhtmltopdf

    log_success "System packages installed successfully"
}

#==============================================================================
# Python 3.11 Installation
#==============================================================================
install_python() {
    log_info "Installing Python ${PYTHON_VERSION}..."
    
    sudo add-apt-repository ppa:deadsnakes/ppa -y
    sudo apt-get update
    sudo apt-get install -y \
        python${PYTHON_VERSION} \
        python${PYTHON_VERSION}-dev \
        python${PYTHON_VERSION}-venv \
        python${PYTHON_VERSION}-distutils

    # Set Python 3.11 as default
    sudo update-alternatives --install /usr/bin/python3 python3 /usr/bin/python${PYTHON_VERSION} 1
    sudo update-alternatives --set python3 /usr/bin/python${PYTHON_VERSION}

    # Install pip for Python 3.11
    curl -sS https://bootstrap.pypa.io/get-pip.py | sudo python${PYTHON_VERSION}

    log_success "Python ${PYTHON_VERSION} installed successfully"
    python3 --version
}

#==============================================================================
# Node.js 18 Installation
#==============================================================================
install_nodejs() {
    log_info "Installing Node.js ${NODE_VERSION}..."

    # Remove existing Node.js
    sudo apt-get remove -y nodejs npm 2>/dev/null || true

    # Install Node.js from NodeSource
    curl -fsSL https://deb.nodesource.com/setup_${NODE_VERSION}.x | sudo -E bash -
    sudo apt-get install -y nodejs

    # Install yarn globally
    sudo npm install -g yarn

    log_success "Node.js ${NODE_VERSION} installed successfully"
    node --version
    npm --version
    yarn --version
}

#==============================================================================
# MariaDB Installation
#==============================================================================
install_mariadb() {
    log_info "Installing MariaDB ${MARIADB_VERSION}..."

    # Add MariaDB repository
    curl -LsS https://downloads.mariadb.com/MariaDB/mariadb_repo_setup | \
        sudo bash -s -- --mariadb-server-version=${MARIADB_VERSION}

    sudo apt-get update
    sudo apt-get install -y mariadb-server mariadb-client

    # Start and enable MariaDB
    sudo systemctl start mariadb
    sudo systemctl enable mariadb

    log_success "MariaDB ${MARIADB_VERSION} installed successfully"

    # Create MariaDB configuration for ERPNext
    sudo tee /etc/mysql/mariadb.conf.d/99-erpnext.cnf > /dev/null <<EOF
[mysqld]
# ERPNext Recommended Settings
innodb_file_format=Barracuda
innodb_file_per_table=1
innodb_large_prefix=1
character-set-client-handshake=FALSE
character-set-server=utf8mb4
collation-server=utf8mb4_unicode_ci

# Performance Settings
innodb_buffer_pool_size=2G
innodb_log_file_size=256M
innodb_log_buffer_size=64M
max_allowed_packet=256M
max_connections=500
thread_cache_size=50

# Query Cache
query_cache_type=1
query_cache_size=64M
query_cache_limit=2M

# Slow Query Log
slow_query_log=1
slow_query_log_file=/var/log/mysql/slow-query.log
long_query_time=2

[mysql]
default-character-set=utf8mb4
EOF

    sudo systemctl restart mariadb
    log_success "MariaDB configured for ERPNext"
}

#==============================================================================
# Redis Installation
#==============================================================================
install_redis() {
    log_info "Installing Redis..."

    sudo apt-get install -y redis-server

    # Configure Redis for production
    sudo tee /etc/redis/redis.conf > /dev/null <<EOF
# Redis Configuration for ERPNext

# Network
bind 127.0.0.1
port 6379
protected-mode yes

# General
daemonize yes
supervised systemd
pidfile /var/run/redis/redis-server.pid
loglevel notice
logfile /var/log/redis/redis-server.log

# Persistence
save 900 1
save 300 10
save 60 10000
stop-writes-on-bgsave-error yes
rdbcompression yes
rdbchecksum yes
dbfilename dump.rdb
dir /var/lib/redis

# Memory Management
maxmemory 512mb
maxmemory-policy allkeys-lru

# Append Only Mode
appendonly yes
appendfilename "appendonly.aof"
appendfsync everysec
EOF

    sudo systemctl restart redis-server
    sudo systemctl enable redis-server

    log_success "Redis installed and configured"
}

#==============================================================================
# Nginx Installation
#==============================================================================
install_nginx() {
    log_info "Installing Nginx..."

    sudo apt-get install -y nginx

    sudo systemctl start nginx
    sudo systemctl enable nginx

    log_success "Nginx installed successfully"
}

#==============================================================================
# Docker Installation
#==============================================================================
install_docker() {
    log_info "Installing Docker..."

    # Remove old versions
    sudo apt-get remove -y docker docker-engine docker.io containerd runc 2>/dev/null || true

    # Add Docker's official GPG key
    sudo install -m 0755 -d /etc/apt/keyrings
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
    sudo chmod a+r /etc/apt/keyrings/docker.gpg

    # Set up the repository
    echo \
        "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
        $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
        sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

    sudo apt-get update
    sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

    # Add current user to docker group
    sudo usermod -aG docker $USER

    sudo systemctl start docker
    sudo systemctl enable docker

    log_success "Docker installed successfully"
    docker --version
    docker compose version
}

#==============================================================================
# Frappe Bench Installation
#==============================================================================
install_bench() {
    log_info "Installing Frappe Bench CLI..."

    sudo pip3 install frappe-bench

    log_success "Frappe Bench installed successfully"
    bench --version
}

#==============================================================================
# Create frappe User
#==============================================================================
create_frappe_user() {
    log_info "Creating frappe user..."

    if id "frappe" &>/dev/null; then
        log_warn "User 'frappe' already exists"
    else
        sudo useradd -m -s /bin/bash frappe
        sudo usermod -aG sudo frappe
        sudo usermod -aG docker frappe
        echo "frappe ALL=(ALL) NOPASSWD:ALL" | sudo tee /etc/sudoers.d/frappe
        log_success "User 'frappe' created successfully"
    fi
}

#==============================================================================
# Main Execution
#==============================================================================
main() {
    log_info "=============================================="
    log_info "ERPNext SaaS Platform - Prerequisites Setup"
    log_info "=============================================="

    install_system_packages
    install_python
    install_nodejs
    install_mariadb
    install_redis
    install_nginx
    install_docker
    install_bench
    create_frappe_user

    log_info "=============================================="
    log_success "All prerequisites installed successfully!"
    log_info "=============================================="
    log_info ""
    log_info "Next steps:"
    log_info "1. Log out and log back in (for docker group)"
    log_info "2. Run: ./scripts/setup_bench.sh"
    log_info ""
}

# Run main function
main "$@"
