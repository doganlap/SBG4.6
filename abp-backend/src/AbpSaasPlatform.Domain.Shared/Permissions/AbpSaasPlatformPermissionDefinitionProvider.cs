using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using AbpSaasPlatform.Localization;

namespace AbpSaasPlatform.Permissions;

public class AbpSaasPlatformPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(AbpSaasPlatformPermissions.GroupName, L("Permission:AbpSaasPlatform"));

        // Tenants
        var tenants = group.AddPermission(AbpSaasPlatformPermissions.Tenants.Default, L("Permission:Tenants"));
        tenants.AddChild(AbpSaasPlatformPermissions.Tenants.View, L("Permission:View"));
        tenants.AddChild(AbpSaasPlatformPermissions.Tenants.Create, L("Permission:Create"));
        tenants.AddChild(AbpSaasPlatformPermissions.Tenants.Edit, L("Permission:Edit"));
        tenants.AddChild(AbpSaasPlatformPermissions.Tenants.Delete, L("Permission:Delete"));

        // Tenant Users
        var tenantUsers = group.AddPermission(AbpSaasPlatformPermissions.TenantUsers.Default, L("Permission:TenantUsers"));
        tenantUsers.AddChild(AbpSaasPlatformPermissions.TenantUsers.View, L("Permission:View"));
        tenantUsers.AddChild(AbpSaasPlatformPermissions.TenantUsers.Create, L("Permission:Create"));
        tenantUsers.AddChild(AbpSaasPlatformPermissions.TenantUsers.Edit, L("Permission:Edit"));
        tenantUsers.AddChild(AbpSaasPlatformPermissions.TenantUsers.Delete, L("Permission:Delete"));
        tenantUsers.AddChild(AbpSaasPlatformPermissions.TenantUsers.Manage, L("Permission:Manage"));

        // Tenant Roles
        var tenantRoles = group.AddPermission(AbpSaasPlatformPermissions.TenantRoles.Default, L("Permission:TenantRoles"));
        tenantRoles.AddChild(AbpSaasPlatformPermissions.TenantRoles.View, L("Permission:View"));
        tenantRoles.AddChild(AbpSaasPlatformPermissions.TenantRoles.Create, L("Permission:Create"));
        tenantRoles.AddChild(AbpSaasPlatformPermissions.TenantRoles.Edit, L("Permission:Edit"));
        tenantRoles.AddChild(AbpSaasPlatformPermissions.TenantRoles.Delete, L("Permission:Delete"));

        // Tenant Settings
        var tenantSettings = group.AddPermission(AbpSaasPlatformPermissions.TenantSettings.Default, L("Permission:TenantSettings"));
        tenantSettings.AddChild(AbpSaasPlatformPermissions.TenantSettings.View, L("Permission:View"));
        tenantSettings.AddChild(AbpSaasPlatformPermissions.TenantSettings.Manage, L("Permission:Manage"));

        // Module Entitlements
        var moduleEntitlements = group.AddPermission(AbpSaasPlatformPermissions.ModuleEntitlements.Default, L("Permission:ModuleEntitlements"));
        moduleEntitlements.AddChild(AbpSaasPlatformPermissions.ModuleEntitlements.View, L("Permission:View"));
        moduleEntitlements.AddChild(AbpSaasPlatformPermissions.ModuleEntitlements.Manage, L("Permission:Manage"));

        // Modules
        var modules = group.AddPermission(AbpSaasPlatformPermissions.Modules.Default, L("Permission:Modules"));
        modules.AddChild(AbpSaasPlatformPermissions.Modules.View, L("Permission:View"));
        modules.AddChild(AbpSaasPlatformPermissions.Modules.Create, L("Permission:Create"));
        modules.AddChild(AbpSaasPlatformPermissions.Modules.Edit, L("Permission:Edit"));
        modules.AddChild(AbpSaasPlatformPermissions.Modules.Delete, L("Permission:Delete"));

        // Subscriptions
        var subscriptions = group.AddPermission(AbpSaasPlatformPermissions.Subscriptions.Default, L("Permission:Subscriptions"));
        subscriptions.AddChild(AbpSaasPlatformPermissions.Subscriptions.View, L("Permission:View"));
        subscriptions.AddChild(AbpSaasPlatformPermissions.Subscriptions.Create, L("Permission:Create"));
        subscriptions.AddChild(AbpSaasPlatformPermissions.Subscriptions.Edit, L("Permission:Edit"));
        subscriptions.AddChild(AbpSaasPlatformPermissions.Subscriptions.Delete, L("Permission:Delete"));
        subscriptions.AddChild(AbpSaasPlatformPermissions.Subscriptions.Cancel, L("Permission:Cancel"));

        // Subscription Plans
        var subscriptionPlans = group.AddPermission(AbpSaasPlatformPermissions.SubscriptionPlans.Default, L("Permission:SubscriptionPlans"));
        subscriptionPlans.AddChild(AbpSaasPlatformPermissions.SubscriptionPlans.View, L("Permission:View"));
        subscriptionPlans.AddChild(AbpSaasPlatformPermissions.SubscriptionPlans.Create, L("Permission:Create"));
        subscriptionPlans.AddChild(AbpSaasPlatformPermissions.SubscriptionPlans.Edit, L("Permission:Edit"));
        subscriptionPlans.AddChild(AbpSaasPlatformPermissions.SubscriptionPlans.Delete, L("Permission:Delete"));

        // Subscription Addons
        var subscriptionAddons = group.AddPermission(AbpSaasPlatformPermissions.SubscriptionAddons.Default, L("Permission:SubscriptionAddons"));
        subscriptionAddons.AddChild(AbpSaasPlatformPermissions.SubscriptionAddons.View, L("Permission:View"));
        subscriptionAddons.AddChild(AbpSaasPlatformPermissions.SubscriptionAddons.Create, L("Permission:Create"));
        subscriptionAddons.AddChild(AbpSaasPlatformPermissions.SubscriptionAddons.Edit, L("Permission:Edit"));
        subscriptionAddons.AddChild(AbpSaasPlatformPermissions.SubscriptionAddons.Delete, L("Permission:Delete"));

        // Billing
        var billing = group.AddPermission(AbpSaasPlatformPermissions.Billing.Default, L("Permission:Billing"));
        billing.AddChild(AbpSaasPlatformPermissions.Billing.View, L("Permission:View"));
        billing.AddChild(AbpSaasPlatformPermissions.Billing.Create, L("Permission:Create"));
        billing.AddChild(AbpSaasPlatformPermissions.Billing.Edit, L("Permission:Edit"));
        billing.AddChild(AbpSaasPlatformPermissions.Billing.Manage, L("Permission:Manage"));

        // Usage Metering
        var usageMetering = group.AddPermission(AbpSaasPlatformPermissions.UsageMetering.Default, L("Permission:UsageMetering"));
        usageMetering.AddChild(AbpSaasPlatformPermissions.UsageMetering.View, L("Permission:View"));
        usageMetering.AddChild(AbpSaasPlatformPermissions.UsageMetering.Record, L("Permission:Record"));
        usageMetering.AddChild(AbpSaasPlatformPermissions.UsageMetering.Manage, L("Permission:Manage"));

        // Containers
        var containers = group.AddPermission(AbpSaasPlatformPermissions.Containers.Default, L("Permission:Containers"));
        containers.AddChild(AbpSaasPlatformPermissions.Containers.View, L("Permission:View"));
        containers.AddChild(AbpSaasPlatformPermissions.Containers.Create, L("Permission:Create"));
        containers.AddChild(AbpSaasPlatformPermissions.Containers.Edit, L("Permission:Edit"));
        containers.AddChild(AbpSaasPlatformPermissions.Containers.Delete, L("Permission:Delete"));
        containers.AddChild(AbpSaasPlatformPermissions.Containers.Manage, L("Permission:Manage"));

        // Platform Settings
        var platformSettings = group.AddPermission(AbpSaasPlatformPermissions.PlatformSettings.Default, L("Permission:PlatformSettings"));
        platformSettings.AddChild(AbpSaasPlatformPermissions.PlatformSettings.View, L("Permission:View"));
        platformSettings.AddChild(AbpSaasPlatformPermissions.PlatformSettings.Manage, L("Permission:Manage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSaasPlatformResource>(name);
    }
}
