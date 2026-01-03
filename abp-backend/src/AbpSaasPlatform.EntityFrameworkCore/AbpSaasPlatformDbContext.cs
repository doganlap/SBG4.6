using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Modules;
using AbpSaasPlatform.Subscriptions;
using AbpSaasPlatform.Settings;
using AbpSaasPlatform.Billing;
using AbpSaasPlatform.Containers;
using AbpSaasPlatform.Admin;
using AbpSaasPlatform.EntityFrameworkCore.Configurations;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace AbpSaasPlatform.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class AbpSaasPlatformDbContext : AbpDbContext<AbpSaasPlatformDbContext>
{
    // Tenants
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantModule> TenantModules { get; set; }
    public DbSet<TenantSetting> TenantSettings { get; set; }
    public DbSet<TenantUser> TenantUsers { get; set; }
    public DbSet<TenantRole> TenantRoles { get; set; }
    
    // Subscriptions
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public DbSet<SubscriptionAddon> SubscriptionAddons { get; set; }
    public DbSet<TenantAddon> TenantAddons { get; set; }
    
    // Modules
    public DbSet<Module> Modules { get; set; }
    public DbSet<ModuleFeature> ModuleFeatures { get; set; }
    public DbSet<ModuleConfiguration> ModuleConfigurations { get; set; }
    
    // Billing
    public DbSet<BillingInvoice> BillingInvoices { get; set; }
    public DbSet<BillingPayment> BillingPayments { get; set; }
    public DbSet<UsageRecord> UsageRecords { get; set; }
    public DbSet<DailyUsageSnapshot> DailyUsageSnapshots { get; set; }
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Credit> Credits { get; set; }
    public DbSet<CreditTransaction> CreditTransactions { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CouponRedemption> CouponRedemptions { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    
    // Containers
    public DbSet<ContainerImage> ContainerImages { get; set; }
    public DbSet<ContainerInstance> ContainerInstances { get; set; }
    public DbSet<ContainerHost> ContainerHosts { get; set; }
    public DbSet<TenantDeployment> TenantDeployments { get; set; }
    public DbSet<TenantDatabase> TenantDatabases { get; set; }
    public DbSet<DatabaseBackup> DatabaseBackups { get; set; }
    public DbSet<DeploymentLog> DeploymentLogs { get; set; }
    
    // Admin
    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<SystemHealth> SystemHealth { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    // Settings
    public DbSet<PlatformSetting> PlatformSettings { get; set; }

    public AbpSaasPlatformDbContext(DbContextOptions<AbpSaasPlatformDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Tenants
        builder.ConfigureTenant();
        builder.ConfigureTenantModule();
        builder.ConfigureTenantSetting();
        builder.ConfigureTenantUser();
        builder.ConfigureTenantRole();
        
        // Subscriptions
        builder.ConfigureSubscription();
        builder.ConfigureSubscriptionPlan();
        builder.ConfigureSubscriptionAddon();
        builder.ConfigureTenantAddon();
        
        // Modules
        builder.ConfigureModule();
        builder.ConfigureModuleFeature();
        builder.ConfigureModuleConfiguration();
        
        // Billing
        builder.ConfigureBillingInvoice();
        builder.ConfigureBillingPayment();
        builder.ConfigureUsageRecord();
        builder.ConfigureDailyUsageSnapshot();
        builder.ConfigureInvoiceLineItem();
        builder.ConfigurePaymentMethod();
        builder.ConfigureCredit();
        builder.ConfigureCreditTransaction();
        builder.ConfigureCoupon();
        builder.ConfigureCouponRedemption();
        builder.ConfigureTaxRate();
        
        // Containers
        builder.ConfigureContainerImage();
        builder.ConfigureContainerInstance();
        builder.ConfigureContainerHost();
        builder.ConfigureTenantDeployment();
        builder.ConfigureTenantDatabase();
        builder.ConfigureDatabaseBackup();
        builder.ConfigureDeploymentLog();
        
        // Admin
        builder.ConfigureAdminUser();
        builder.ConfigureAuditLog();
        builder.ConfigureSystemHealth();
        builder.ConfigureNotification();
        
        // Tenant Additional
        builder.ConfigureTenantDepartment();
        builder.ConfigureTenantModuleSetting();
        builder.ConfigureTenantApiKey();
        builder.ConfigureTenantWebhook();
        builder.ConfigureTenantAuditLog();
        builder.ConfigureTenantActivityLog();
        builder.ConfigureTenantNotification();
        builder.ConfigureTenantFile();
        
        // Settings
        builder.ConfigurePlatformSetting();
        
        // ABP Framework
        builder.ConfigurePermissionManagement();
    }
}
