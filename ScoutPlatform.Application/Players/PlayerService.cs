using ScoutPlatform.Application.Common;

namespace ScoutPlatform.Application.Players;

public sealed class PlayerService
{
    private readonly IPlayerRepository _repository;

    public PlayerService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<PlayerSummaryDto>> SearchAsync(string? search, string? position, int? ageMin, int? ageMax, CancellationToken cancellationToken)
        => _repository.SearchAsync(search, position, ageMin, ageMax, cancellationToken);

    public Task<PlayerSummaryDto?> GetByIdAsync(Guid playerId, CancellationToken cancellationToken)
        => _repository.GetByIdAsync(playerId, cancellationToken);

    public Task<IReadOnlyCollection<PlayerMetricDto>> GetMetricsAsync(Guid playerId, int seasonId, CancellationToken cancellationToken)
        => _repository.GetMetricsAsync(playerId, seasonId, cancellationToken);
}
