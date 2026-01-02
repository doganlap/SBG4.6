# ERPNext Multi-Module SaaS Platform - AI Coding Agent Instructions

## Architecture Overview

This is a **multi-tenant SaaS platform** built on ERPNext v16, featuring all 22 modules as containerized, tenant-isolated services. The platform adds a **.NET ABP backend** for SaaS management while ERPNext remains the ERP engine.

**Layers**
- SaaS Admin Portal (Next.js 14 @3000) – platform management
- Tenant Admin Portal (Next.js 14 @3001) – per-tenant config
- Customer Portal (Next.js 14 @3002) – customer self-service
- .NET ABP Backend (ASP.NET Core @5000) – auth, billing, tenant CRUD, module entitlements
- ERPNext/Frappe (Python @8000) – ERP logic + 22 module containers

### Multi-Tenant Data Isolation

- Each tenant gets its own MariaDB database: `tenant_{tenant_id}`
- Tenant-specific containers reference the platform database schemas in `database-schemas/`:
  - `01_saas_admin_schema.sql` - Platform settings, tenants table, billing
  - `02_tenant_admin_schema.sql` - Tenant configuration, module assignments
  - `03_subscriptions_billing_schema.sql` - Subscription plans, invoices
  - `04_container_management_schema.sql` - Container orchestration metadata

### The 22 Modules Strategy

ERPNext modules (1-12 are core, 13-22 are Frappe apps) each have:
- Individual `Dockerfile.{module}` in `docker/modules/`
- Custom app directory in `apps/saas_{module}_custom/`
- All extend `Dockerfile.base` which uses `frappe/erpnext:v16` as foundation

## Critical Developer Workflows

### Creating a New Tenant

**Always use the script**, never manually:
```bash
./scripts/create_tenant.sh <tenant_id> "Organization Name" admin@example.com [starter|professional|enterprise]
```

This script:
1. Creates isolated database `tenant_{tenant_id}`
2. Generates unique credentials (DB password, admin password)
3. Spawns tenant-specific containers
4. Initializes site at `{tenant_id}.erp.local`
5. Records metadata in `saas_admin.tenants` table

### Building & Deployment

**Development setup** (full platform):
```bash
./scripts/install_prerequisites.sh  # Only once - installs Docker, Node, etc.
./scripts/setup_bench.sh            # Sets up Frappe Bench
./scripts/download_all_modules.sh   # Clones all 22 ERPNext module repos
./scripts/create_custom_apps.sh     # Creates custom wrappers for each module
docker compose -f docker/docker-compose.yml up -d
```

**Production build** (all images):
```bash
./scripts/build_all.sh              # Builds base + 9 module images + 4 portals
```

**Deployment orchestration**:
```bash
./scripts/deploy.sh [start|stop|restart|status]
```

### Frontend Development

Each portal is a standalone Next.js 14 app with shared UI patterns:

- **Tech stack**: React 18, Next.js 14, TypeScript, TailwindCSS, shadcn/ui
- **State**: React Query (@tanstack/react-query) for server state, Zustand for client state
- **Forms**: react-hook-form + Zod validation
- **Charts**: Recharts for data visualization

**Development commands** (run inside each portal directory):
```bash
npm run dev        # Dev server (port varies: 3000, 3001, 3002, 3003)
npm run build      # Production build
npm run lint       # ESLint check
npm run type-check # TypeScript validation
```

### Docker Compose Architecture

The `docker/docker-compose.yml` orchestrates:
- **1 MariaDB container** with connection pooling (500 max connections, 2GB buffer pool)
- **3 Redis containers** (cache, queue, socketio) for Frappe's architecture
- **Per-module containers** (accounting, crm, selling, buying, stock, manufacturing, hr, education, healthcare)
- **4 portal containers** (saas-admin, tenant-admin, customer-portal, showcase)
- **Nginx reverse proxy** with load balancing configuration

**Key convention**: All services use network `erpnext-network` for inter-service communication.

## Project-Specific Conventions

### Naming Standards

- **Tenant IDs**: Lowercase alphanumeric, 3-30 chars, regex: `^[a-z][a-z0-9_-]{2,29}$`
- **Databases**: `tenant_{tenant_id}` format (e.g., `tenant_acme`)
- **DB Users**: `user_{tenant_id}` format
- **Subdomains**: `{tenant_id}.erp.local` for local dev, `{tenant_id}.yourdomain.com` for production
- **Docker images**: `{registry}/{module}:{version}` (default: `erpnext-saas/{module}:v16`)

### Database Schema Patterns

All schemas follow this structure (see `database-schemas/`):
- **UTF8MB4** charset and collation for full Unicode support
- **BIGINT AUTO_INCREMENT** for primary keys (supports massive scale)
- **Timestamp columns**: `created_at`, `updated_at` (with ON UPDATE trigger)
- **Indexing**: Composite indexes on `(tenant_id, created_at)` for tenant isolation
- **JSON columns**: For flexible metadata (e.g., `module_settings` JSON column in tenants table)

### Configuration Management

**Environment variables** via `configs/common.env`:
- Database credentials default to `DB_ROOT_PASSWORD=your_secure_root_password`
- All ERPNext containers read `FRAPPE_SITE_NAME_HEADER` for multi-site routing
- Redis endpoints: `REDIS_CACHE`, `REDIS_QUEUE`, `REDIS_SOCKETIO` (separate instances)

**Never commit** real credentials - use placeholders in `common.env` and override via Docker secrets in production.

### Portal UI Component Patterns

All portals share component library in `src/components/ui/` (shadcn/ui):
- Use `cn()` utility from `lib/utils.ts` for className merging
- Dark mode via `next-themes` (ThemeProvider in layout)
- Dashboard widgets follow pattern: `stats-card.tsx`, `revenue-chart.tsx`, `tenant-table.tsx`
- Layout structure: `layout/header.tsx` + `layout/sidebar.tsx` + page content

**Example**: To add a new dashboard stat in SaaS Admin Portal:
```tsx
<StatsCard
  title="Metric Name"
  value="123"
  description="Change from last period"
  icon={LucideIcon}
  trend="up|down|neutral"
  trendValue="12.5%"
/>
```

## Integration & Data Flow

### Backend Strategy (ABP + ERPNext)
- .NET ABP handles SaaS platform concerns: auth (JWT), tenant CRUD, module entitlements, billing, audit logs, background jobs
- ERPNext handles ERP concerns: DocTypes, workflows, reports, module logic
- Communication: React portals → ABP (JWT) → ERPNext (API key/cookie) → MariaDB
- Keep `scripts/create_tenant.sh` as the single provisioning entrypoint; ABP should invoke it, not bypass it

### Dual Authentication (Recommended Initial State)
- **ABP JWT**: For platform APIs; store token in `localStorage.access_token`; header `Authorization: Bearer <token>`
- **ERPNext API key/cookie**: For ERP operations; headers `Authorization: token <key>:<secret>` and `X-Frappe-Site-Name: {tenant}.erp.local`; cookies when embedding ERPNext UI
- Why dual, not SSO now: avoids custom Frappe auth hooks, keeps systems decoupled, faster delivery; plan for SSO later if needed

### ABP vs JS-Only Backend (Quick Guidance)
- **Use ABP** if you need enterprise auth, multi-tenancy plumbing, audit logging, background jobs, and you have .NET skills or want ABP commercial features
- **Use Next.js API + Prisma** if you want a single-language (TS) stack with faster onboarding for the current portals
- Either way: respect tenant isolation, call `create_tenant.sh`, keep env-driven config from `configs/common.env`, and stay on `erpnext-network`

### Tenant Provisioning Flow

1. User submits tenant request via SaaS Admin Portal (React)
2. Portal calls backend API (add if missing - currently mocked)
3. Backend invokes `scripts/create_tenant.sh` via subprocess
4. Script creates DB → containers → site initialization
5. Metadata written to `saas_admin.tenants` table
6. Confirmation sent to Tenant Admin Portal

### Module Assignment Flow

1. Tenant admin selects modules via Tenant Admin Portal
2. Updates `tenant_admin.tenant_modules` table (columns: `tenant_id`, `module_id`, `enabled`, `custom_config` JSON)
3. Triggers container orchestration (see `04_container_management_schema.sql`)
4. Spawns/destroys module containers per tenant

### Billing Integration

- Subscription data in `03_subscriptions_billing_schema.sql`
- Plans table: `starter`, `professional`, `enterprise` with module limits
- Usage tracking via `module_usage` table (timestamp + metrics)
- Invoice generation tied to `subscriptions.billing_cycle`

## Key Files Reference

- **[docker/docker-compose.yml](../docker/docker-compose.yml)**: Full service orchestration, health checks, networking
- **[scripts/create_tenant.sh](../scripts/create_tenant.sh)**: Tenant provisioning logic (248 lines)
- **[database-schemas/01_saas_admin_schema.sql](../database-schemas/01_saas_admin_schema.sql)**: Platform-level data model
- `saas-admin-portal/src/app/(dashboard)/dashboard/page.tsx`: Admin dashboard layout pattern
- **[docker/modules/Dockerfile.base](../docker/modules/Dockerfile.base)**: Base image for all module containers
- **[scripts/build_all.sh](../scripts/build_all.sh)**: Automated build pipeline

## Common Pitfalls

1. **Don't bypass tenant creation script** - Manual DB creation will miss container orchestration
2. **Always run from project root** - Scripts use relative paths (`PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"`)
3. **Docker Compose v2 syntax** - Use `docker compose` (space), not `docker-compose` (hyphen)
4. **MariaDB character set** - ERPNext requires `utf8mb4_unicode_ci`, set in docker-compose
5. **Port conflicts** - Ensure 3000-3003, 8000, 3306, 6379 are available before starting

## Testing & Debugging

**Check service health**:
```bash
docker compose -f docker/docker-compose.yml ps
docker logs erpnext-mariadb  # View DB logs
docker logs <container_name>
```

**Access MariaDB**:
```bash
docker exec -it erpnext-mariadb mysql -u root -p{DB_ROOT_PASSWORD}
```

**View tenant data**:
```sql
USE saas_admin;
SELECT tenant_id, organization_name, subdomain, status FROM tenants;
```

**Frontend debugging**: Each portal runs dev server with hot reload - check browser console + Network tab for API issues.
