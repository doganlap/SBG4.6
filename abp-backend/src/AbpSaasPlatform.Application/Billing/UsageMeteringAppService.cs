using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Billing;
using AbpSaasPlatform.Billing.Dtos;

namespace AbpSaasPlatform.Application.Billing;

public class UsageMeteringAppService : ApplicationService, IUsageMeteringAppService
{
    private readonly IRepository<UsageRecord, long> _usageRecordRepository;

    public UsageMeteringAppService(IRepository<UsageRecord, long> usageRecordRepository)
    {
        _usageRecordRepository = usageRecordRepository;
    }

    public async Task<UsageRecordDto> RecordUsageAsync(CreateUsageRecordDto input)
    {
        // Check if record already exists for this period
        var existing = await _usageRecordRepository.FirstOrDefaultAsync(
            x => x.TenantId == input.TenantId &&
                 x.UsagePeriodStart == input.UsagePeriodStart &&
                 x.UsagePeriodEnd == input.UsagePeriodEnd);

        if (existing != null)
        {
            // Update existing record
            existing.UserCount = input.UserCount;
            existing.StorageUsedBytes = input.StorageUsedBytes;
            existing.ApiCalls = input.ApiCalls;
            existing.BandwidthBytes = input.BandwidthBytes;
            existing.ModuleUsage = input.ModuleUsage;
            
            await _usageRecordRepository.UpdateAsync(existing, autoSave: true);
            return ObjectMapper.Map<UsageRecord, UsageRecordDto>(existing);
        }

        var usageRecord = new UsageRecord
        {
            TenantId = input.TenantId,
            SubscriptionId = input.SubscriptionId,
            UsagePeriodStart = input.UsagePeriodStart,
            UsagePeriodEnd = input.UsagePeriodEnd,
            UserCount = input.UserCount,
            StorageUsedBytes = input.StorageUsedBytes,
            ApiCalls = input.ApiCalls,
            BandwidthBytes = input.BandwidthBytes,
            ModuleUsage = input.ModuleUsage,
            IsBilled = false
        };

        await _usageRecordRepository.InsertAsync(usageRecord, autoSave: true);
        return ObjectMapper.Map<UsageRecord, UsageRecordDto>(usageRecord);
    }

    public async Task<UsageRecordDto> GetUsageRecordAsync(long id)
    {
        var record = await _usageRecordRepository.GetAsync(id);
        return ObjectMapper.Map<UsageRecord, UsageRecordDto>(record);
    }

    public async Task<List<UsageRecordDto>> GetUsageRecordsByTenantAsync(Guid tenantId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = await _usageRecordRepository.GetQueryableAsync();
        var records = query.Where(x => x.TenantId == tenantId);
        
        if (startDate.HasValue)
            records = records.Where(x => x.UsagePeriodStart >= startDate.Value);
        if (endDate.HasValue)
            records = records.Where(x => x.UsagePeriodEnd <= endDate.Value);

        var list = records.OrderByDescending(x => x.UsagePeriodStart).ToList();
        return ObjectMapper.Map<List<UsageRecord>, List<UsageRecordDto>>(list);
    }

    public async Task<UsageRecordDto> GetCurrentPeriodUsageAsync(Guid tenantId)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        var record = await _usageRecordRepository.FirstOrDefaultAsync(
            x => x.TenantId == tenantId &&
                 x.UsagePeriodStart == startOfMonth &&
                 x.UsagePeriodEnd == endOfMonth);

        if (record == null)
        {
            return new UsageRecordDto
            {
                TenantId = tenantId,
                UsagePeriodStart = startOfMonth,
                UsagePeriodEnd = endOfMonth,
                UserCount = 0,
                StorageUsedBytes = 0,
                ApiCalls = 0,
                BandwidthBytes = 0
            };
        }

        return ObjectMapper.Map<UsageRecord, UsageRecordDto>(record);
    }

    public async Task<UsageSummaryDto> GetUsageSummaryAsync(Guid tenantId, DateTime startDate, DateTime endDate)
    {
        var records = await _usageRecordRepository.GetListAsync(
            x => x.TenantId == tenantId &&
                 x.UsagePeriodStart >= startDate &&
                 x.UsagePeriodEnd <= endDate);

        return new UsageSummaryDto
        {
            TenantId = tenantId,
            PeriodStart = startDate,
            PeriodEnd = endDate,
            TotalUsers = records.Any() ? records.Max(x => x.UserCount) : 0,
            TotalStorageBytes = records.Any() ? records.Max(x => x.StorageUsedBytes) : 0,
            TotalApiCalls = records.Sum(x => x.ApiCalls),
            TotalBandwidthBytes = records.Sum(x => x.BandwidthBytes),
            TotalOverageCost = records.Sum(x => x.TotalOverageCost),
            RecordCount = records.Count
        };
    }
}
