using Microsoft.EntityFrameworkCore;
using ScoutPlatform.Application.Common;
using ScoutPlatform.Application.Players;
using ScoutPlatform.Domain.Entities;

namespace ScoutPlatform.Infrastructure.Persistence;

internal sealed class EfPlayerRepository : IPlayerRepository
{
    private readonly ScoutPlatformDbContext _dbContext;

    public EfPlayerRepository(ScoutPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<PlayerSummaryDto>> GetAllAsync(CancellationToken cancellationToken)
        => await _dbContext.Players
            .AsNoTracking()
            .Select(player => new PlayerSummaryDto(player.Id, player.FullName, player.PrimaryPosition, player.CurrentClub, player.MarketValueEur))
            .ToArrayAsync(cancellationToken);

    public async Task<PlayerSummaryDto?> GetByIdAsync(Guid playerId, CancellationToken cancellationToken)
        => await _dbContext.Players
            .AsNoTracking()
            .Where(player => player.Id == playerId)
            .Select(player => new PlayerSummaryDto(player.Id, player.FullName, player.PrimaryPosition, player.CurrentClub, player.MarketValueEur))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyCollection<PlayerSummaryDto>> SearchAsync(string? search, string? position, int? ageMin, int? ageMax, CancellationToken cancellationToken)
    {
        var query = _dbContext.Players.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(player => EF.Functions.ILike(player.FullName, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(position))
        {
            query = query.Where(player => player.PrimaryPosition == position);
        }

        var candidates = await query
            .Select(player => new PlayerSummaryWithDob(
                new PlayerSummaryDto(player.Id, player.FullName, player.PrimaryPosition, player.CurrentClub, player.MarketValueEur),
                player.DateOfBirth))
            .ToArrayAsync(cancellationToken);

        if (!ageMin.HasValue && !ageMax.HasValue)
        {
            return candidates.Select(item => item.Summary).ToArray();
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return candidates
            .Where(item =>
            {
                var age = GetAge(item.DateOfBirth, today);
                return (!ageMin.HasValue || age >= ageMin.Value) && (!ageMax.HasValue || age <= ageMax.Value);
            })
            .Select(item => item.Summary)
            .ToArray();
    }

    public async Task<IReadOnlyCollection<PlayerMetricDto>> GetMetricsAsync(Guid playerId, int seasonId, CancellationToken cancellationToken)
        => await _dbContext.PlayerMetrics
            .AsNoTracking()
            .Where(metric => metric.PlayerId == playerId && metric.SeasonId == seasonId)
            .Select(metric => new PlayerMetricDto(metric.MetricKey, metric.Value))
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<PlayerMetric>> GetMetricsForPlayersAsync(IEnumerable<Guid> playerIds, int seasonId, CancellationToken cancellationToken)
    {
        var ids = playerIds.ToHashSet();
        return await _dbContext.PlayerMetrics
            .AsNoTracking()
            .Where(metric => ids.Contains(metric.PlayerId) && metric.SeasonId == seasonId)
            .Select(metric => new PlayerMetric
            {
                PlayerId = metric.PlayerId,
                SeasonId = metric.SeasonId,
                MetricKey = metric.MetricKey,
                Value = metric.Value
            })
            .ToArrayAsync(cancellationToken);
    }

    private static int GetAge(DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    private sealed record PlayerSummaryWithDob(PlayerSummaryDto Summary, DateOnly DateOfBirth);
}
