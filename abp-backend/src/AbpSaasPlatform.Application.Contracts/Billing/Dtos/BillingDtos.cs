using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Billing.Dtos;

public class CreateInvoiceDto
{
    public Guid TenantId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public string InvoiceType { get; set; } = "subscription";
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; } = 0;
    public decimal TaxPercent { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public string? LineItems { get; set; }
    public string? Notes { get; set; }
}

public class UpdateInvoiceDto
{
    public string? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TaxPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Notes { get; set; }
}

public class BillingInvoiceDto : EntityDto<long>
{
    public string InvoiceId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public string InvoiceType { get; set; }
    public string Status { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public string Currency { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public string? LineItems { get; set; }
    public string? PaymentMethod { get; set; }
    public string? StripeInvoiceId { get; set; }
    public string? PdfUrl { get; set; }
    public string? Notes { get; set; }
}

public class CreatePaymentDto
{
    public Guid TenantId { get; set; }
    public long? InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentMethod { get; set; } = "card";
    public string? CardBrand { get; set; }
    public string? CardLastFour { get; set; }
    public string? StripePaymentId { get; set; }
    public string? StripeChargeId { get; set; }
    public string? Description { get; set; }
    public string? Metadata { get; set; }
}

public class BillingPaymentDto : EntityDto<long>
{
    public string PaymentId { get; set; }
    public Guid TenantId { get; set; }
    public long? InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public string PaymentMethod { get; set; }
    public string? CardBrand { get; set; }
    public string? CardLastFour { get; set; }
    public string? StripePaymentId { get; set; }
    public string? StripeChargeId { get; set; }
    public string? Description { get; set; }
    public string? FailureReason { get; set; }
    public DateTime PaymentDate { get; set; }
}
