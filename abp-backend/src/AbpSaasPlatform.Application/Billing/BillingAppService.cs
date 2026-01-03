using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Billing;
using AbpSaasPlatform.Billing.Dtos;

namespace AbpSaasPlatform.Application.Billing;

public class BillingAppService : ApplicationService, IBillingAppService
{
    private readonly IRepository<BillingInvoice, long> _invoiceRepository;
    private readonly IRepository<BillingPayment, long> _paymentRepository;

    public BillingAppService(
        IRepository<BillingInvoice, long> invoiceRepository,
        IRepository<BillingPayment, long> paymentRepository)
    {
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<BillingInvoiceDto> CreateInvoiceAsync(CreateInvoiceDto input)
    {
        var invoice = new BillingInvoice
        {
            InvoiceId = $"INV-{DateTime.UtcNow:yyyy-MM}-{GuidGenerator.Create().ToString("N")[..6].ToUpper()}",
            TenantId = input.TenantId,
            SubscriptionId = input.SubscriptionId,
            InvoiceType = input.InvoiceType,
            Status = "draft",
            InvoiceDate = input.InvoiceDate,
            DueDate = input.DueDate,
            Subtotal = input.Subtotal,
            TaxAmount = input.TaxAmount,
            TaxPercent = input.TaxPercent,
            DiscountAmount = input.DiscountAmount,
            TotalAmount = input.TotalAmount,
            AmountDue = input.TotalAmount,
            Currency = input.Currency,
            PeriodStart = input.PeriodStart,
            PeriodEnd = input.PeriodEnd,
            LineItems = input.LineItems,
            Notes = input.Notes
        };

        await _invoiceRepository.InsertAsync(invoice, autoSave: true);
        return ObjectMapper.Map<BillingInvoice, BillingInvoiceDto>(invoice);
    }

    public async Task<BillingInvoiceDto> GetInvoiceAsync(long id)
    {
        var invoice = await _invoiceRepository.GetAsync(id);
        return ObjectMapper.Map<BillingInvoice, BillingInvoiceDto>(invoice);
    }

    public async Task<List<BillingInvoiceDto>> GetInvoicesByTenantAsync(Guid tenantId)
    {
        var invoices = await _invoiceRepository.GetListAsync(x => x.TenantId == tenantId);
        return ObjectMapper.Map<List<BillingInvoice>, List<BillingInvoiceDto>>(invoices.OrderByDescending(x => x.InvoiceDate).ToList());
    }

    public async Task<BillingInvoiceDto> UpdateInvoiceAsync(long id, UpdateInvoiceDto input)
    {
        var invoice = await _invoiceRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.Status))
            invoice.Status = input.Status;
        if (input.DueDate.HasValue)
            invoice.DueDate = input.DueDate.Value;
        if (input.TaxAmount.HasValue)
            invoice.TaxAmount = input.TaxAmount.Value;
        if (input.TaxPercent.HasValue)
            invoice.TaxPercent = input.TaxPercent.Value;
        if (input.DiscountAmount.HasValue)
            invoice.DiscountAmount = input.DiscountAmount.Value;
        if (input.TotalAmount.HasValue)
        {
            invoice.TotalAmount = input.TotalAmount.Value;
            invoice.AmountDue = input.TotalAmount.Value - invoice.AmountPaid;
        }
        if (!string.IsNullOrEmpty(input.Notes))
            invoice.Notes = input.Notes;

        await _invoiceRepository.UpdateAsync(invoice, autoSave: true);
        return ObjectMapper.Map<BillingInvoice, BillingInvoiceDto>(invoice);
    }

    public async Task<BillingInvoiceDto> MarkInvoicePaidAsync(long id, DateTime paidAt)
    {
        var invoice = await _invoiceRepository.GetAsync(id);
        invoice.Status = "paid";
        invoice.PaidAt = paidAt;
        invoice.AmountPaid = invoice.TotalAmount;
        invoice.AmountDue = 0;

        await _invoiceRepository.UpdateAsync(invoice, autoSave: true);
        return ObjectMapper.Map<BillingInvoice, BillingInvoiceDto>(invoice);
    }

    public async Task<BillingPaymentDto> CreatePaymentAsync(CreatePaymentDto input)
    {
        var payment = new BillingPayment
        {
            PaymentId = $"PAY-{DateTime.UtcNow:yyyy-MM}-{GuidGenerator.Create().ToString("N")[..6].ToUpper()}",
            TenantId = input.TenantId,
            InvoiceId = input.InvoiceId,
            Amount = input.Amount,
            Currency = input.Currency,
            Status = "pending",
            PaymentMethod = input.PaymentMethod,
            CardBrand = input.CardBrand,
            CardLastFour = input.CardLastFour,
            StripePaymentId = input.StripePaymentId,
            StripeChargeId = input.StripeChargeId,
            Description = input.Description,
            Metadata = input.Metadata,
            PaymentDate = DateTime.UtcNow
        };

        await _paymentRepository.InsertAsync(payment, autoSave: true);
        
        // Update invoice if linked
        if (input.InvoiceId.HasValue)
        {
            var invoice = await _invoiceRepository.GetAsync(input.InvoiceId.Value);
            invoice.AmountPaid += input.Amount;
            invoice.AmountDue = invoice.TotalAmount - invoice.AmountPaid;
            if (invoice.AmountDue <= 0)
            {
                invoice.Status = "paid";
                invoice.PaidAt = DateTime.UtcNow;
            }
            await _invoiceRepository.UpdateAsync(invoice, autoSave: true);
        }

        return ObjectMapper.Map<BillingPayment, BillingPaymentDto>(payment);
    }

    public async Task<BillingPaymentDto> GetPaymentAsync(long id)
    {
        var payment = await _paymentRepository.GetAsync(id);
        return ObjectMapper.Map<BillingPayment, BillingPaymentDto>(payment);
    }

    public async Task<List<BillingPaymentDto>> GetPaymentsByTenantAsync(Guid tenantId)
    {
        var payments = await _paymentRepository.GetListAsync(x => x.TenantId == tenantId);
        return ObjectMapper.Map<List<BillingPayment>, List<BillingPaymentDto>>(payments.OrderByDescending(x => x.PaymentDate).ToList());
    }

    public async Task<List<BillingPaymentDto>> GetPaymentsByInvoiceAsync(long invoiceId)
    {
        var payments = await _paymentRepository.GetListAsync(x => x.InvoiceId == invoiceId);
        return ObjectMapper.Map<List<BillingPayment>, List<BillingPaymentDto>>(payments.OrderByDescending(x => x.PaymentDate).ToList());
    }
}
