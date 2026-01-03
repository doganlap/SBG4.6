using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Billing.Dtos;

namespace AbpSaasPlatform.Billing;

public interface IUsageMeteringAppService : IApplicationService
{
    Task<UsageRecordDto> RecordUsageAsync(CreateUsageRecordDto input);
    Task<UsageRecordDto> GetUsageRecordAsync(long id);
    Task<List<UsageRecordDto>> GetUsageRecordsByTenantAsync(Guid tenantId, DateTime? startDate = null, DateTime? endDate = null);
    Task<UsageRecordDto> GetCurrentPeriodUsageAsync(Guid tenantId);
    Task<UsageSummaryDto> GetUsageSummaryAsync(Guid tenantId, DateTime startDate, DateTime endDate);
}
