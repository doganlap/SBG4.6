namespace AbpSaasPlatform;

public static class AbpSaasPlatformPermissions
{
    public const string GroupName = "AbpSaasPlatform";

    public static class Tenants
    {
        public const string Default = GroupName + ".Tenants";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
    }

    public static class TenantUsers
    {
        public const string Default = GroupName + ".TenantUsers";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Manage = Default + ".Manage";
    }

    public static class TenantRoles
    {
        public const string Default = GroupName + ".TenantRoles";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class TenantSettings
    {
        public const string Default = GroupName + ".TenantSettings";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }

    public static class ModuleEntitlements
    {
        public const string Default = GroupName + ".ModuleEntitlements";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }

    public static class Modules
    {
        public const string Default = GroupName + ".Modules";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Subscriptions
    {
        public const string Default = GroupName + ".Subscriptions";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Cancel = Default + ".Cancel";
    }

    public static class SubscriptionPlans
    {
        public const string Default = GroupName + ".SubscriptionPlans";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class SubscriptionAddons
    {
        public const string Default = GroupName + ".SubscriptionAddons";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Billing
    {
        public const string Default = GroupName + ".Billing";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Manage = Default + ".Manage";
    }

    public static class UsageMetering
    {
        public const string Default = GroupName + ".UsageMetering";
        public const string View = Default + ".View";
        public const string Record = Default + ".Record";
        public const string Manage = Default + ".Manage";
    }

    public static class Containers
    {
        public const string Default = GroupName + ".Containers";
        public const string View = Default + ".View";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Manage = Default + ".Manage";
    }

    public static class PlatformSettings
    {
        public const string Default = GroupName + ".PlatformSettings";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage";
    }
}
