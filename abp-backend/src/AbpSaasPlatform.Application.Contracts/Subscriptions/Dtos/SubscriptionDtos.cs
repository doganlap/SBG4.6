using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Subscriptions.Dtos;

public class SubscriptionCreateDto
{
    public Guid TenantId { get; set; }
    public int PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string BillingCycle { get; set; } = "monthly";
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
}

public class SubscriptionUpdateDto
{
    public int? PlanId { get; set; }
    public string? Status { get; set; }
    public DateTime? EndDate { get; set; }
    public string? CancellationReason { get; set; }
}

public class SubscriptionDto : EntityDto<Guid>
{
    public string SubscriptionId { get; set; }
    public Guid TenantId { get; set; }
    public int PlanId { get; set; }
    public string PlanName { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? TrialStartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public string BillingCycle { get; set; }
    public int BillingAnchorDay { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public decimal BasePrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalPrice { get; set; }
    public string Currency { get; set; }
    public int CurrentUsers { get; set; }
    public decimal CurrentStorageGb { get; set; }
    public int CurrentApiCalls { get; set; }
    public string? StripeSubscriptionId { get; set; }
    public string? StripeCustomerId { get; set; }
}
