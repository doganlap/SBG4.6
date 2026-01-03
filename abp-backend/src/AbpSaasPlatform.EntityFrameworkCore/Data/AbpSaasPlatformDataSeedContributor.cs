using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using AbpSaasPlatform.Subscriptions;
using AbpSaasPlatform.Modules;
using AbpSaasPlatform.Settings;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Data;

public class AbpSaasPlatformDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<SubscriptionPlan, int> _subscriptionPlanRepository;
    private readonly IRepository<Module, int> _moduleRepository;
    private readonly IRepository<PlatformSetting, int> _platformSettingRepository;
    private readonly IRepository<TenantRole, int> _tenantRoleRepository;
    private readonly IGuidGenerator _guidGenerator;

    public AbpSaasPlatformDataSeedContributor(
        IRepository<SubscriptionPlan, int> subscriptionPlanRepository,
        IRepository<Module, int> moduleRepository,
        IRepository<PlatformSetting, int> platformSettingRepository,
        IRepository<TenantRole, int> tenantRoleRepository,
        IGuidGenerator guidGenerator)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _moduleRepository = moduleRepository;
        _platformSettingRepository = platformSettingRepository;
        _tenantRoleRepository = tenantRoleRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedSubscriptionPlansAsync();
        await SeedModulesAsync();
        await SeedPlatformSettingsAsync();
        await SeedDefaultTenantRolesAsync();
    }

    private async Task SeedSubscriptionPlansAsync()
    {
        if (await _subscriptionPlanRepository.GetCountAsync() > 0)
        {
            return; // Already seeded
        }

        // Starter Plan
        var starterPlan = new SubscriptionPlan
        {
            PlanCode = "starter",
            PlanName = "Starter",
            Description = "Perfect for small businesses getting started",
            PriceMonthly = 499.00m,
            PriceYearly = 4789.00m,
            Currency = "USD",
            MaxUsers = 5,
            MaxStorageGb = 10,
            MaxApiCallsPerMonth = 10000,
            IncludedModules = "[\"accounting\", \"crm\", \"selling\", \"buying\"]",
            IsActive = true,
            IsPublic = true,
            IsTrialAvailable = true,
            TrialDays = 14,
            DisplayOrder = 1
        };
        await _subscriptionPlanRepository.InsertAsync(starterPlan, autoSave: true);

        // Professional Plan
        var professionalPlan = new SubscriptionPlan
        {
            PlanCode = "professional",
            PlanName = "Professional",
            Description = "For growing organizations with more needs",
            PriceMonthly = 1500.00m,
            PriceYearly = 14400.00m,
            Currency = "USD",
            MaxUsers = 50,
            MaxStorageGb = 50,
            MaxApiCallsPerMonth = 100000,
            IncludedModules = "[\"accounting\", \"crm\", \"selling\", \"buying\", \"stock\", \"manufacturing\", \"hr\", \"support\"]",
            IsActive = true,
            IsPublic = true,
            IsTrialAvailable = true,
            TrialDays = 14,
            DisplayOrder = 2,
            BadgeText = "Popular"
        };
        await _subscriptionPlanRepository.InsertAsync(professionalPlan, autoSave: true);

        // Enterprise Plan
        var enterprisePlan = new SubscriptionPlan
        {
            PlanCode = "enterprise",
            PlanName = "Enterprise",
            Description = "For large organizations with custom needs",
            PriceMonthly = 4500.00m,
            PriceYearly = 43200.00m,
            Currency = "USD",
            MaxUsers = -1, // Unlimited
            MaxStorageGb = 500,
            MaxApiCallsPerMonth = -1, // Unlimited
            IncludedModules = "[\"all\"]",
            IsActive = true,
            IsPublic = true,
            IsTrialAvailable = true,
            TrialDays = 14,
            DisplayOrder = 3,
            BadgeText = "Best Value"
        };
        await _subscriptionPlanRepository.InsertAsync(enterprisePlan, autoSave: true);
    }

    private async Task SeedModulesAsync()
    {
        if (await _moduleRepository.GetCountAsync() > 0)
        {
            return; // Already seeded
        }

        var modules = new[]
        {
            new Module { ModuleCode = "accounting", ModuleName = "Accounting", Description = "Financial management, ledgers, reports", Category = "core", Icon = "Calculator", DisplayOrder = 1, IsFree = true, IsActive = true },
            new Module { ModuleCode = "crm", ModuleName = "CRM", Description = "Customer relationship management", Category = "core", Icon = "Users", DisplayOrder = 2, IsFree = true, IsActive = true },
            new Module { ModuleCode = "selling", ModuleName = "Selling", Description = "Sales orders, quotations, invoices", Category = "core", Icon = "ShoppingCart", DisplayOrder = 3, IsFree = true, IsActive = true },
            new Module { ModuleCode = "buying", ModuleName = "Buying", Description = "Purchase orders, supplier management", Category = "core", Icon = "Truck", DisplayOrder = 4, IsFree = true, IsActive = true },
            new Module { ModuleCode = "stock", ModuleName = "Stock", Description = "Inventory management, warehouses", Category = "core", Icon = "Package", DisplayOrder = 5, IsFree = true, IsActive = true },
            new Module { ModuleCode = "manufacturing", ModuleName = "Manufacturing", Description = "Production planning, BOM, work orders", Category = "core", Icon = "Factory", DisplayOrder = 6, IsFree = true, IsActive = true },
            new Module { ModuleCode = "projects", ModuleName = "Projects", Description = "Project management, tasks, timesheets", Category = "core", Icon = "ClipboardList", DisplayOrder = 7, IsFree = true, IsActive = true },
            new Module { ModuleCode = "support", ModuleName = "Support", Description = "Helpdesk, tickets, SLA", Category = "core", Icon = "Headphones", DisplayOrder = 8, IsFree = true, IsActive = true },
            new Module { ModuleCode = "website", ModuleName = "Website", Description = "Web pages, blog, portal", Category = "core", Icon = "Globe", DisplayOrder = 9, IsFree = true, IsActive = true },
            new Module { ModuleCode = "ecommerce", ModuleName = "E-commerce", Description = "Online store, shopping cart", Category = "core", Icon = "CreditCard", DisplayOrder = 10, IsFree = true, IsActive = true },
            new Module { ModuleCode = "pos", ModuleName = "POS", Description = "Point of sale system", Category = "core", Icon = "CreditCard", DisplayOrder = 11, IsFree = true, IsActive = true },
            new Module { ModuleCode = "assets", ModuleName = "Assets", Description = "Fixed assets, depreciation", Category = "core", Icon = "Building", DisplayOrder = 12, IsFree = true, IsActive = true },
            new Module { ModuleCode = "hr", ModuleName = "HRMS", Description = "Employee management, payroll, attendance", Category = "standalone", Icon = "Users", DisplayOrder = 13, IsFree = false, AddonPriceMonthly = 99.00m, IsActive = true },
            new Module { ModuleCode = "education", ModuleName = "Education", Description = "Schools, courses, students", Category = "domain", Icon = "GraduationCap", DisplayOrder = 14, IsFree = false, AddonPriceMonthly = 199.00m, IsActive = true },
            new Module { ModuleCode = "healthcare", ModuleName = "Healthcare", Description = "Patients, appointments, lab tests", Category = "domain", Icon = "Stethoscope", DisplayOrder = 15, IsFree = false, AddonPriceMonthly = 299.00m, IsActive = true },
            new Module { ModuleCode = "payments", ModuleName = "Payments", Description = "Payment gateway integrations", Category = "standalone", Icon = "CreditCard", DisplayOrder = 16, IsFree = false, AddonPriceMonthly = 49.00m, IsActive = true },
            new Module { ModuleCode = "lms", ModuleName = "LMS", Description = "Learning Management System", Category = "standalone", Icon = "GraduationCap", DisplayOrder = 17, IsFree = false, AddonPriceMonthly = 149.00m, IsActive = true },
            new Module { ModuleCode = "helpdesk", ModuleName = "Helpdesk", Description = "Customer support portal", Category = "standalone", Icon = "Headphones", DisplayOrder = 18, IsFree = false, AddonPriceMonthly = 79.00m, IsActive = true },
            new Module { ModuleCode = "wiki", ModuleName = "Wiki", Description = "Knowledge base and documentation", Category = "standalone", Icon = "FileText", DisplayOrder = 19, IsFree = false, AddonPriceMonthly = 59.00m, IsActive = true },
            new Module { ModuleCode = "insights", ModuleName = "Insights", Description = "Business intelligence and analytics", Category = "standalone", Icon = "BarChart", DisplayOrder = 20, IsFree = false, AddonPriceMonthly = 199.00m, IsActive = true },
            new Module { ModuleCode = "builder", ModuleName = "Builder", Description = "Visual website/app builder", Category = "standalone", Icon = "Puzzle", DisplayOrder = 21, IsFree = false, AddonPriceMonthly = 149.00m, IsActive = true },
            new Module { ModuleCode = "crm_app", ModuleName = "CRM App", Description = "Standalone CRM application", Category = "standalone", Icon = "Users", DisplayOrder = 22, IsFree = false, AddonPriceMonthly = 99.00m, IsActive = true }
        };

        foreach (var module in modules)
        {
            await _moduleRepository.InsertAsync(module, autoSave: false);
        }

        await _moduleRepository.GetDbContext().SaveChangesAsync();
    }

    private async Task SeedPlatformSettingsAsync()
    {
        if (await _platformSettingRepository.GetCountAsync() > 0)
        {
            return; // Already seeded
        }

        var settings = new[]
        {
            new PlatformSetting { SettingKey = "platform.name", SettingValue = "ERPNext SaaS Platform", SettingType = "string", Category = "general", Description = "Platform name", IsUserEditable = false },
            new PlatformSetting { SettingKey = "platform.version", SettingValue = "1.0.0", SettingType = "string", Category = "general", Description = "Platform version", IsUserEditable = false },
            new PlatformSetting { SettingKey = "billing.currency", SettingValue = "USD", SettingType = "string", Category = "billing", Description = "Default billing currency", IsUserEditable = true },
            new PlatformSetting { SettingKey = "billing.tax_rate", SettingValue = "0", SettingType = "decimal", Category = "billing", Description = "Default tax rate percentage", IsUserEditable = true },
            new PlatformSetting { SettingKey = "trial.days", SettingValue = "14", SettingType = "int", Category = "subscriptions", Description = "Default trial period in days", IsUserEditable = false },
            new PlatformSetting { SettingKey = "container.default_image", SettingValue = "erpnext/erpnext:latest", SettingType = "string", Category = "containers", Description = "Default ERPNext container image", IsUserEditable = false },
            new PlatformSetting { SettingKey = "email.from_address", SettingValue = "noreply@erpnext-saas.com", SettingType = "string", Category = "email", Description = "Default from email address", IsUserEditable = true },
            new PlatformSetting { SettingKey = "email.from_name", SettingValue = "ERPNext SaaS", SettingType = "string", Category = "email", Description = "Default from email name", IsUserEditable = true }
        };

        foreach (var setting in settings)
        {
            await _platformSettingRepository.InsertAsync(setting, autoSave: false);
        }

        await _platformSettingRepository.GetDbContext().SaveChangesAsync();
    }

    private async Task SeedDefaultTenantRolesAsync()
    {
        if (await _tenantRoleRepository.GetCountAsync() > 0)
        {
            return; // Already seeded
        }

        var roles = new[]
        {
            new TenantRole
            {
                RoleCode = "admin",
                RoleName = "Administrator",
                Description = "Full access to all features and settings",
                Level = 100,
                IsSystemRole = true,
                IsActive = true,
                Permissions = "[\"all\"]"
            },
            new TenantRole
            {
                RoleCode = "manager",
                RoleName = "Manager",
                Description = "Access to management features and reports",
                Level = 50,
                IsSystemRole = true,
                IsActive = true,
                Permissions = "[\"view_reports\", \"manage_users\", \"view_analytics\"]"
            },
            new TenantRole
            {
                RoleCode = "user",
                RoleName = "User",
                Description = "Standard user access",
                Level = 10,
                IsSystemRole = true,
                IsActive = true,
                Permissions = "[\"view_dashboard\", \"create_documents\", \"edit_own_documents\"]"
            },
            new TenantRole
            {
                RoleCode = "viewer",
                RoleName = "Viewer",
                Description = "Read-only access",
                Level = 1,
                IsSystemRole = true,
                IsActive = true,
                Permissions = "[\"view_dashboard\", \"view_documents\"]"
            }
        };

        foreach (var role in roles)
        {
            await _tenantRoleRepository.InsertAsync(role, autoSave: false);
        }

        await _tenantRoleRepository.GetDbContext().SaveChangesAsync();
    }
}
