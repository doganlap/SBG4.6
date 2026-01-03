using AutoMapper;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Tenants.Dtos;
using AbpSaasPlatform.Subscriptions;
using AbpSaasPlatform.Subscriptions.Dtos;
using AbpSaasPlatform.Modules;
using AbpSaasPlatform.ModuleEntitlements;
using AbpSaasPlatform.ModuleEntitlements.Dtos;
using AbpSaasPlatform.Billing;
using AbpSaasPlatform.Billing.Dtos;
using AbpSaasPlatform.Containers;
using AbpSaasPlatform.Containers.Dtos;
using AbpSaasPlatform.Settings;
using AbpSaasPlatform.Settings.Dtos;
using AbpSaasPlatform.Admin;
using AbpSaasPlatform.Admin.Dtos;

namespace AbpSaasPlatform.Application.Common;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        // Tenants
        CreateMap<Tenant, TenantDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OrganizationName))
            .ForMember(dest => dest.ActivatedAt, opt => opt.MapFrom(src => src.ActivationDate));
        CreateMap<TenantCreateDto, Tenant>()
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.TenantId_Public, opt => opt.Ignore())
            .ForMember(dest => dest.Slug, opt => opt.Ignore())
            .ForMember(dest => dest.Subdomain, opt => opt.Ignore())
            .ForMember(dest => dest.PrimaryEmail, opt => opt.Ignore());
        CreateMap<TenantUpdateDto, Tenant>()
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Name));

        // Subscriptions
        CreateMap<Subscription, SubscriptionDto>()
            .ForMember(dest => dest.PlanName, opt => opt.Ignore()); // Will be populated from Plan navigation
        CreateMap<SubscriptionCreateDto, Subscription>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SubscriptionId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.BasePrice, opt => opt.Ignore())
            .ForMember(dest => dest.FinalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.Currency, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentUsers, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentStorageGb, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentApiCalls, opt => opt.Ignore());
        CreateMap<SubscriptionUpdateDto, Subscription>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Subscription Plans
        CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
        CreateMap<SubscriptionPlanCreateDto, SubscriptionPlan>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<SubscriptionPlanUpdateDto, SubscriptionPlan>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Tenant Modules
        CreateMap<TenantModule, TenantModuleDto>()
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId_Ref));
        CreateMap<TenantModuleCreateDto, TenantModule>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId_Ref, opt => opt.Ignore())
            .ForMember(dest => dest.IsEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.EnabledAt, opt => opt.Ignore());

        // Tenant Roles
        CreateMap<TenantRole, TenantRoleDto>();
        CreateMap<CreateTenantRoleDto, TenantRole>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateTenantRoleDto, TenantRole>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Tenant Settings
        CreateMap<TenantSetting, TenantSettingDto>();
        CreateMap<CreateOrUpdateTenantSettingDto, TenantSetting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Subscription Addons
        CreateMap<SubscriptionAddon, SubscriptionAddonDto>();
        CreateMap<CreateSubscriptionAddonDto, SubscriptionAddon>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateSubscriptionAddonDto, SubscriptionAddon>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Modules
        CreateMap<Module, ModuleDto>();
        CreateMap<CreateModuleDto, Module>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateModuleDto, Module>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Billing Invoices
        CreateMap<BillingInvoice, BillingInvoiceDto>();
        CreateMap<CreateInvoiceDto, BillingInvoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.AmountPaid, opt => opt.Ignore())
            .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.PaidAt, opt => opt.Ignore());
        CreateMap<UpdateInvoiceDto, BillingInvoice>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Billing Payments
        CreateMap<BillingPayment, BillingPaymentDto>();
        CreateMap<CreatePaymentDto, BillingPayment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentDate, opt => opt.Ignore());

        // Usage Records
        CreateMap<UsageRecord, UsageRecordDto>();
        CreateMap<CreateUsageRecordDto, UsageRecord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OverageUserCost, opt => opt.Ignore())
            .ForMember(dest => dest.OverageStorageCost, opt => opt.Ignore())
            .ForMember(dest => dest.OverageApiCost, opt => opt.Ignore())
            .ForMember(dest => dest.TotalOverageCost, opt => opt.Ignore())
            .ForMember(dest => dest.IsBilled, opt => opt.Ignore())
            .ForMember(dest => dest.BilledInvoiceId, opt => opt.Ignore());

        // Container Images
        CreateMap<ContainerImage, ContainerImageDto>();
        CreateMap<CreateContainerImageDto, ContainerImage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateContainerImageDto, ContainerImage>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Container Instances
        CreateMap<ContainerInstance, ContainerInstanceDto>();
        CreateMap<CreateContainerInstanceDto, ContainerInstance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InstanceId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.HealthStatus, opt => opt.Ignore())
            .ForMember(dest => dest.CpuUsagePercent, opt => opt.Ignore())
            .ForMember(dest => dest.MemoryUsageMb, opt => opt.Ignore())
            .ForMember(dest => dest.RestartCount, opt => opt.Ignore())
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore())
            .ForMember(dest => dest.StoppedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastHealthCheck, opt => opt.Ignore())
            .ForMember(dest => dest.ContainerId, opt => opt.Ignore())
            .ForMember(dest => dest.HostIp, opt => opt.Ignore());

        // Container Hosts
        CreateMap<ContainerHost, ContainerHostDto>();
        CreateMap<CreateContainerHostDto, ContainerHost>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsedCpuCores, opt => opt.Ignore())
            .ForMember(dest => dest.UsedMemoryGb, opt => opt.Ignore())
            .ForMember(dest => dest.UsedStorageGb, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentContainers, opt => opt.Ignore())
            .ForMember(dest => dest.LastHeartbeat, opt => opt.Ignore());
        CreateMap<UpdateContainerHostDto, ContainerHost>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Tenant Users
        CreateMap<TenantUser, TenantUserDto>();
        CreateMap<CreateTenantUserDto, TenantUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginIp, opt => opt.Ignore())
            .ForMember(dest => dest.LastActivityAt, opt => opt.Ignore())
            .ForMember(dest => dest.FailedLoginAttempts, opt => opt.Ignore())
            .ForMember(dest => dest.LockedUntil, opt => opt.Ignore())
            .ForMember(dest => dest.ErpnextUserId, opt => opt.Ignore())
            .ForMember(dest => dest.ErpnextUserEmail, opt => opt.Ignore());
        CreateMap<UpdateTenantUserDto, TenantUser>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Platform Settings
        CreateMap<PlatformSetting, PlatformSettingDto>();
        CreateMap<CreateOrUpdateSettingDto, PlatformSetting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Daily Usage Snapshots
        CreateMap<DailyUsageSnapshot, DailyUsageSnapshotDto>();

        // Invoice Line Items
        CreateMap<InvoiceLineItem, InvoiceLineItemDto>();

        // Payment Methods
        CreateMap<PaymentMethod, PaymentMethodDto>();

        // Credits
        CreateMap<Credit, CreditDto>();

        // Credit Transactions
        CreateMap<CreditTransaction, CreditTransactionDto>();

        // Coupons
        CreateMap<Coupon, CouponDto>();

        // Coupon Redemptions
        CreateMap<CouponRedemption, CouponRedemptionDto>();

        // Tax Rates
        CreateMap<TaxRate, TaxRateDto>();

        // Admin Users
        CreateMap<AdminUser, AdminUserDto>();

        // Audit Logs
        CreateMap<AuditLog, AuditLogDto>();

        // System Health
        CreateMap<SystemHealth, SystemHealthDto>();

        // Notifications
        CreateMap<Notification, NotificationDto>();

        // Tenant Departments
        CreateMap<TenantDepartment, TenantDepartmentDto>();

        // Tenant Module Settings
        CreateMap<TenantModuleSetting, TenantModuleSettingDto>();

        // Tenant API Keys
        CreateMap<TenantApiKey, TenantApiKeyDto>();

        // Tenant Webhooks
        CreateMap<TenantWebhook, TenantWebhookDto>();

        // Tenant Audit Logs
        CreateMap<TenantAuditLog, TenantAuditLogDto>();

        // Tenant Activity Logs
        CreateMap<TenantActivityLog, TenantActivityLogDto>();

        // Tenant Notifications
        CreateMap<TenantNotification, TenantNotificationDto>();

        // Tenant Files
        CreateMap<TenantFile, TenantFileDto>();

        // Tenant Deployments
        CreateMap<TenantDeployment, TenantDeploymentDto>();

        // Tenant Databases
        CreateMap<TenantDatabase, TenantDatabaseDto>();

        // Database Backups
        CreateMap<DatabaseBackup, DatabaseBackupDto>();

        // Deployment Logs
        CreateMap<DeploymentLog, DeploymentLogDto>();
    }
}
