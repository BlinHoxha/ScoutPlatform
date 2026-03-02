using Microsoft.EntityFrameworkCore;
using ScoutPlatform.Application.Common;
using ScoutPlatform.Domain.Entities;

namespace ScoutPlatform.Infrastructure.Persistence;

internal sealed class EfMetricDefinitionRepository : IMetricDefinitionRepository
{
    private readonly ScoutPlatformDbContext _dbContext;

    public EfMetricDefinitionRepository(ScoutPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<MetricDefinition>> GetAllAsync(CancellationToken cancellationToken)
        => await _dbContext.MetricDefinitions
            .AsNoTracking()
            .Select(item => new MetricDefinition
            {
                Key = item.Key,
                Name = item.Name,
                MinExpected = item.MinExpected,
                MaxExpected = item.MaxExpected,
                HigherIsBetter = item.HigherIsBetter
            })
            .ToArrayAsync(cancellationToken);
}
