using ScoutPlatform.Application.Players;
using ScoutPlatform.Application.TeamProfiles;
using ScoutPlatform.Domain.Entities;

namespace ScoutPlatform.Application.Common;

public interface IReadRepository<TModel, in TId>
{
    Task<TModel?>GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TModel>> GetAllAsync(CancellationToken cancellationToken);
}

public interface ICrudRepository<TModel, in TId> : IReadRepository<TModel, TId>
{
    Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken);
    Task<TModel?> UpdateAsync(TModel model, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IPlayerRepository : IReadRepository<PlayerSummaryDto, Guid>
{
    Task<IReadOnlyCollection<PlayerSummaryDto>> SearchAsync(string? search, string? position, int? ageMin, int? ageMax, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PlayerMetricDto>> GetMetricsAsync(Guid playerId, int seasonId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PlayerMetric>> GetMetricsForPlayersAsync(IEnumerable<Guid> playerIds, int seasonId, CancellationToken cancellationToken);
}

public interface ITeamProfileRepository : ICrudRepository<TeamProfileDto, Guid>
{
    Task<IReadOnlyCollection<TeamProfileWeightDto>> SetWeightsAsync(Guid teamProfileId, IReadOnlyCollection<TeamProfileWeightDto> weights, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TeamProfileWeightDto>> GetWeightsAsync(Guid teamProfileId, CancellationToken cancellationToken);
    Task<TeamProfile?> GetDomainByIdAsync(Guid teamProfileId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TeamProfileWeight>> GetDomainWeightsAsync(Guid teamProfileId, CancellationToken cancellationToken);
}

public interface IMetricDefinitionRepository
{
    Task<IReadOnlyCollection<MetricDefinition>> GetAllAsync(CancellationToken cancellationToken);
}
