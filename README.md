# ERPNext Multi-Module SaaS Platform

A comprehensive multi-tenant SaaS platform built on ERPNext v16, featuring all 22 modules containerized and customizable per tenant.

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────────────────┐
│                        SaaS Super Admin Portal                          │
│                    (Platform Management & Billing)                       │
├─────────────────────────────────────────────────────────────────────────┤
│                        Tenant Admin Portal                               │
│                   (Per-tenant Configuration)                             │
├─────────────────────────────────────────────────────────────────────────┤
│                         Load Balancer (Nginx)                            │
├─────────────────────────────────────────────────────────────────────────┤
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐      │
│  │Accounting│ │   CRM    │ │  Selling │ │  Stock   │ │   HR     │ ...  │
│  │ Module   │ │  Module  │ │  Module  │ │  Module  │ │  Module  │      │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘      │
├─────────────────────────────────────────────────────────────────────────┤
│                     Container Orchestration (Docker)                     │
├─────────────────────────────────────────────────────────────────────────┤
│  ┌──────────┐ ┌──────────┐ ┌──────────┐                                 │
│  │ MariaDB  │ │  Redis   │ │ Storage  │                                 │
│  │ Cluster  │ │  Cache   │ │  (S3)    │                                 │
│  └──────────┘ └──────────┘ └──────────┘                                 │
└─────────────────────────────────────────────────────────────────────────┘
```

## 📦 All 22 ERPNext Modules

### Core ERPNext Modules (1-12)
| # | Module | Description |
|---|--------|-------------|
| 1 | **Accounting** | Financial management, ledgers, reports |
| 2 | **CRM** | Customer relationship management |
| 3 | **Selling** | Sales orders, quotations, invoices |
| 4 | **Buying** | Purchase orders, supplier management |
| 5 | **Stock** | Inventory management, warehouses |
| 6 | **Manufacturing** | Production planning, BOM, work orders |
| 7 | **Projects** | Project management, tasks, timesheets |
| 8 | **Support** | Helpdesk, tickets, SLA |
| 9 | **Website** | Web pages, blog, portal |
| 10 | **E-commerce** | Online store, shopping cart |
| 11 | **POS** | Point of sale system |
| 12 | **Asset Management** | Fixed assets, depreciation |

### Standalone Frappe Apps (13-22)
| # | Module | Description |
|---|--------|-------------|
| 13 | **HRMS** | Employee management, payroll, attendance |
| 14 | **Education** | Schools, courses, students |
| 15 | **Healthcare** | Patients, appointments, lab tests |
| 16 | **Payments** | Payment gateway integrations |
| 17 | **LMS** | Learning Management System |
| 18 | **Helpdesk** | Customer support portal |
| 19 | **Wiki** | Knowledge base and documentation |
| 20 | **Insights** | Business intelligence and analytics |
| 21 | **Builder** | Visual website/app builder |
| 22 | **CRM App** | Standalone CRM application |

## 🚀 Quick Start

### Prerequisites
- Ubuntu 22.04 LTS / Debian 12
- 8GB+ RAM
- 50GB+ Storage
- Docker & Docker Compose

### Installation

```bash
# Clone the repository
git clone https://github.com/your-org/erpnext-saas-platform.git
cd erpnext-saas-platform

# Install prerequisites
chmod +x scripts/*.sh
./scripts/install_prerequisites.sh

# Setup Frappe Bench
./scripts/setup_bench.sh

# Download all 22 modules
./scripts/download_all_modules.sh

# Create custom apps for each module
./scripts/create_custom_apps.sh

# Build and start containers
docker compose up -d
```

### Access Points

| Portal | URL | Description |
|--------|-----|-------------|
| SaaS Admin | http://localhost:3000 | Super admin portal |
| Tenant Admin | http://localhost:3001 | Tenant management |
| Customer Portal | http://localhost:3002 | Customer self-service |
| Showcase | http://localhost:3003 | Module showcase |
| ERPNext | http://localhost:8000 | Main ERPNext instance |

## 📁 Project Structure

```
erpnext-saas-platform/
├── apps/                          # Custom Frappe apps
│   ├── saas_accounting_custom/
│   ├── saas_crm_custom/
│   └── ... (22 custom apps)
├── configs/                       # Configuration files
│   ├── common.env
│   └── nginx/
├── database-schemas/              # Database schema definitions
│   ├── saas_admin.sql
│   ├── tenant_admin.sql
│   └── subscriptions.sql
├── docker/                        # Docker configurations
│   ├── modules/                   # Per-module Dockerfiles
│   ├── databases/
│   └── docker-compose.yml
├── docs/                          # Documentation
├── saas-admin-portal/             # Super Admin Portal (React)
├── tenant-admin-portal/           # Tenant Admin Portal (React)
├── customer-portal/               # Customer Portal (React)
├── showcase-pages/                # Module showcase pages
└── scripts/                       # Setup and utility scripts
```

## 🔧 Configuration

### Environment Variables
Copy `configs/common.env.example` to `configs/common.env` and update:

```bash
# Required settings
DB_ROOT_PASSWORD=your_secure_password
ADMIN_PASSWORD=your_admin_password
JWT_SECRET=your_jwt_secret
```

### Multi-tenancy Setup
Each tenant gets:
- Isolated database
- Customizable modules
- Private container image
- Dedicated subdomain

## 📊 Database Schema

See `database-schemas/` for complete schema definitions including:
- SaaS Admin tables
- Tenant management tables
- Subscription & billing tables
- Module configuration tables

## 🐳 Containerization

Each module is containerized with:
- Base ERPNext image
- Custom app overlay
- Module-specific configuration
- Health checks

Build a specific module:
```bash
docker build -t erpnext-accounting:v16 -f docker/modules/Dockerfile.accounting .
```

## 📝 License

MIT License - See LICENSE file for details.

## 🤝 Support

- Documentation: [docs/](./docs/)
- Issues: GitHub Issues
- Email: support@your-company.com
