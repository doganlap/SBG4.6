using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Billing.Dtos;

// Daily Usage Snapshot DTOs
public class DailyUsageSnapshotDto : EntityDto<long>
{
    public Guid TenantId { get; set; }
    public DateTime SnapshotDate { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int NewUsers { get; set; }
    public long TotalStorageBytes { get; set; }
    public long StorageDeltaBytes { get; set; }
    public int ApiCalls { get; set; }
    public int ApiErrors { get; set; }
    public int DocumentsCreated { get; set; }
    public int DocumentsUpdated { get; set; }
    public int LoginCount { get; set; }
    public int? AvgResponseTimeMs { get; set; }
}

// Invoice Line Item DTOs
public class InvoiceLineItemDto : EntityDto<long>
{
    public long InvoiceId { get; set; }
    public string ItemType { get; set; }
    public string Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? ReferenceType { get; set; }
    public string? ReferenceId { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public int DisplayOrder { get; set; }
}

// Payment Method DTOs
public class PaymentMethodDto : FullAuditedEntityDto<long>
{
    public Guid TenantId { get; set; }
    public string MethodType { get; set; }
    public string? Provider { get; set; }
    public string? CardBrand { get; set; }
    public string? CardLastFour { get; set; }
    public int? CardExpMonth { get; set; }
    public int? CardExpYear { get; set; }
    public string? CardHolderName { get; set; }
    public string? BankName { get; set; }
    public string? AccountType { get; set; }
    public string? AccountLastFour { get; set; }
    public string? StripePaymentMethodId { get; set; }
    public string? PaypalBillingAgreementId { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public string? Metadata { get; set; }
}

// Credit DTOs
public class CreditDto : FullAuditedEntityDto<long>
{
    public string CreditId { get; set; }
    public Guid TenantId { get; set; }
    public string CreditType { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public string? Description { get; set; }
    public string? Reason { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsExpired { get; set; }
    public long? RelatedInvoiceId { get; set; }
    public long? RelatedPaymentId { get; set; }
    public string? Metadata { get; set; }
}

// Credit Transaction DTOs
public class CreditTransactionDto : AuditedEntityDto<long>
{
    public long CreditId { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public long? InvoiceId { get; set; }
    public string? Description { get; set; }
    public string? ReferenceId { get; set; }
    public DateTime TransactionDate { get; set; }
}

// Coupon DTOs
public class CouponDto : AuditedEntityDto<int>
{
    public string CouponCode { get; set; }
    public string CouponName { get; set; }
    public string? Description { get; set; }
    public string DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public string Currency { get; set; }
    public decimal? MinimumAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public int? MaxRedemptions { get; set; }
    public int? MaxRedemptionsPerUser { get; set; }
    public int CurrentRedemptions { get; set; }
    public string? ApplicablePlans { get; set; }
    public string? ApplicableModules { get; set; }
    public bool IsActive { get; set; }
    public string? Metadata { get; set; }
}

// Coupon Redemption DTOs
public class CouponRedemptionDto : AuditedEntityDto<long>
{
    public int CouponId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public long? InvoiceId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime RedeemedAt { get; set; }
    public string? RedeemedBy { get; set; }
}

// Tax Rate DTOs
public class TaxRateDto : AuditedEntityDto<int>
{
    public string TaxCode { get; set; }
    public string TaxName { get; set; }
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string TaxType { get; set; }
    public bool IsActive { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveUntil { get; set; }
    public string? Metadata { get; set; }
}
