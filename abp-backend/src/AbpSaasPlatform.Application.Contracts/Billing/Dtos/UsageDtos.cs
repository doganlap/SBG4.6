using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Billing.Dtos;

public class CreateUsageRecordDto
{
    public Guid TenantId { get; set; }
    public Guid SubscriptionId { get; set; }
    public DateTime UsagePeriodStart { get; set; }
    public DateTime UsagePeriodEnd { get; set; }
    public int UserCount { get; set; }
    public long StorageUsedBytes { get; set; }
    public int ApiCalls { get; set; }
    public long BandwidthBytes { get; set; }
    public string? ModuleUsage { get; set; }
}

public class UsageRecordDto : EntityDto<long>
{
    public Guid TenantId { get; set; }
    public Guid SubscriptionId { get; set; }
    public DateTime UsagePeriodStart { get; set; }
    public DateTime UsagePeriodEnd { get; set; }
    public int UserCount { get; set; }
    public long StorageUsedBytes { get; set; }
    public int ApiCalls { get; set; }
    public long BandwidthBytes { get; set; }
    public string? ModuleUsage { get; set; }
    public decimal OverageUserCost { get; set; }
    public decimal OverageStorageCost { get; set; }
    public decimal OverageApiCost { get; set; }
    public decimal TotalOverageCost { get; set; }
    public bool IsBilled { get; set; }
    public long? BilledInvoiceId { get; set; }
}

public class UsageSummaryDto
{
    public Guid TenantId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int TotalUsers { get; set; }
    public long TotalStorageBytes { get; set; }
    public int TotalApiCalls { get; set; }
    public long TotalBandwidthBytes { get; set; }
    public decimal TotalOverageCost { get; set; }
    public int RecordCount { get; set; }
}
