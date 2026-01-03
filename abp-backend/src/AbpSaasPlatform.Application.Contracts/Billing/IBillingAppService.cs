using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Billing.Dtos;

namespace AbpSaasPlatform.Billing;

public interface IBillingAppService : IApplicationService
{
    // Invoices
    Task<BillingInvoiceDto> CreateInvoiceAsync(CreateInvoiceDto input);
    Task<BillingInvoiceDto> GetInvoiceAsync(long id);
    Task<List<BillingInvoiceDto>> GetInvoicesByTenantAsync(Guid tenantId);
    Task<BillingInvoiceDto> UpdateInvoiceAsync(long id, UpdateInvoiceDto input);
    Task<BillingInvoiceDto> MarkInvoicePaidAsync(long id, DateTime paidAt);
    
    // Payments
    Task<BillingPaymentDto> CreatePaymentAsync(CreatePaymentDto input);
    Task<BillingPaymentDto> GetPaymentAsync(long id);
    Task<List<BillingPaymentDto>> GetPaymentsByTenantAsync(Guid tenantId);
    Task<List<BillingPaymentDto>> GetPaymentsByInvoiceAsync(long invoiceId);
}
