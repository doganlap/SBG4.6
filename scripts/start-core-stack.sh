#!/bin/bash

# Minimal ERPNext Stack - without portal builds (core services only)
# This version starts the essential services: MariaDB, Redis, ERPNext, and Nginx

cd /home/dogan/erpnext-saas-platform

# Stop any running containers
echo "Stopping any running containers..."
docker compose -f docker/docker-compose.yml down --remove-orphans 2>/dev/null

# Create a temporary docker-compose file with only core services
cat > docker/docker-compose-core.yml << 'EOF'
version: "3.9"

x-erpnext-common: &erpnext-common
  image: frappe/erpnext:v16
  environment: &erpnext-env
    - FRAPPE_SITE_NAME_HEADER=os.doganconsult.com
    - DB_HOST=mariadb
    - REDIS_CACHE=redis-cache:6379
    - REDIS_QUEUE=redis-queue:6379
    - REDIS_SOCKETIO=redis-socketio:6379
    - SOCKETIO_PORT=9000
  volumes:
    - sites-vol:/home/frappe/frappe-bench/sites
    - logs-vol:/home/frappe/frappe-bench/logs
  depends_on:
    mariadb:
      condition: service_healthy
    redis-cache:
      condition: service_started
    redis-queue:
      condition: service_started
    redis-socketio:
      condition: service_started
  networks:
    - erpnext-network

services:
  mariadb:
    image: mariadb:10.6
    container_name: erpnext-mariadb
    restart: unless-stopped
    command: --max_allowed_packet=256M --character-set-server=utf8mb4
    environment:
      MARIADB_ROOT_PASSWORD: ${DB_PASSWORD:-changeme}
      MARIADB_USER: ${DB_USER:-frappe}
      MARIADB_PASSWORD: ${DB_PASSWORD:-changeme}
    ports:
      - "3306:3306"
    volumes:
      - mariadb-data:/var/lib/mysql
    healthcheck:
      test: ["CMD", "mysqladmin", "ping", "-h", "127.0.0.1"]
      interval: 5s
      timeout: 5s
      retries: 10
    networks:
      - erpnext-network

  redis-cache:
    image: redis:7-alpine
    container_name: erpnext-redis-cache
    restart: unless-stopped
    volumes:
      - redis-cache-data:/data
    networks:
      - erpnext-network

  redis-queue:
    image: redis:7-alpine
    container_name: erpnext-redis-queue
    restart: unless-stopped
    volumes:
      - redis-queue-data:/data
    networks:
      - erpnext-network

  redis-socketio:
    image: redis:7-alpine
    container_name: erpnext-redis-socketio
    restart: unless-stopped
    volumes:
      - redis-socketio-data:/data
    networks:
      - erpnext-network

  erpnext-backend:
    <<: *erpnext-common
    container_name: erpnext-backend
    command: bench serve --no-reload
    ports:
      - "8000:8000"

  erpnext-socketio:
    <<: *erpnext-common
    container_name: erpnext-socketio
    command: node /home/frappe/frappe-bench/apps/frappe/socketio.js
    ports:
      - "9000:9000"

  erpnext-scheduler:
    <<: *erpnext-common
    container_name: erpnext-scheduler
    command: bench scheduler

  erpnext-worker-default:
    <<: *erpnext-common
    container_name: erpnext-worker-default
    command: bench worker --queue default

  erpnext-worker-long:
    <<: *erpnext-common
    container_name: erpnext-worker-long
    command: bench worker --queue long

  erpnext-worker-short:
    <<: *erpnext-common
    container_name: erpnext-worker-short
    command: bench worker --queue short

  nginx:
    image: nginx:1.25-alpine
    container_name: erpnext-nginx
    restart: unless-stopped
    volumes:
      - ./docker/nginx/nginx.conf:/etc/nginx/nginx.conf:ro
      - ./docker/nginx/conf.d:/etc/nginx/conf.d:ro
      - /etc/letsencrypt:/etc/letsencrypt:ro
      - sites-vol:/home/frappe/frappe-bench/sites:ro
      - logs-vol:/home/frappe/frappe-bench/logs:ro
    ports:
      - "80:80"
      - "443:443"
    depends_on:
      - erpnext-backend
      - erpnext-socketio
    networks:
      - erpnext-network

volumes:
  sites-vol:
  logs-vol:
  mariadb-data:
  redis-cache-data:
  redis-queue-data:
  redis-socketio-data:

networks:
  erpnext-network:
    driver: bridge
EOF

echo "Starting core ERPNext services..."
docker compose -f docker/docker-compose-core.yml up -d

echo ""
echo "✓ Core ERPNext stack is starting!"
echo ""
echo "Services running:"
docker compose -f docker/docker-compose-core.yml ps
echo ""
echo "Monitor logs with: docker compose -f docker/docker-compose-core.yml logs -f"
