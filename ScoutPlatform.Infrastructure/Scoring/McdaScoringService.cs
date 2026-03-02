using ScoutPlatform.Application.Common;
using ScoutPlatform.Domain.Scoring;

namespace ScoutPlatform.Infrastructure.Scoring;

internal sealed class McdaScoringService : IScoringService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamProfileRepository _teamProfileRepository;
    private readonly IMetricDefinitionRepository _metricDefinitionRepository;

    public McdaScoringService(
        IPlayerRepository playerRepository,
        ITeamProfileRepository teamProfileRepository,
        IMetricDefinitionRepository metricDefinitionRepository)
    {
        _playerRepository = playerRepository;
        _teamProfileRepository = teamProfileRepository;
        _metricDefinitionRepository = metricDefinitionRepository;
    }

    public async Task<IReadOnlyCollection<PlayerRankingResult>> RankPlayersAsync(RankingContext context, CancellationToken cancellationToken)
    {
        var profile = await _teamProfileRepository.GetDomainByIdAsync(context.TeamProfileId, cancellationToken);
        if (profile is null)
        {
            return Array.Empty<PlayerRankingResult>();
        }

        var weights = await _teamProfileRepository.GetDomainWeightsAsync(context.TeamProfileId, cancellationToken);
        var weightByMetric = weights.ToDictionary(weight => weight.MetricKey, StringComparer.OrdinalIgnoreCase);
        var metricDefinitions = await _metricDefinitionRepository.GetAllAsync(cancellationToken);
        var metricDefinitionByKey = metricDefinitions.ToDictionary(definition => definition.Key, StringComparer.OrdinalIgnoreCase);

        var players = await _playerRepository.SearchAsync(null, null, null, null, cancellationToken);
        var candidateSet = context.CandidatePlayerIds.Count == 0
            ? players.Select(player => player.Id).ToHashSet()
            : context.CandidatePlayerIds.ToHashSet();

        var metrics = await _playerRepository.GetMetricsForPlayersAsync(candidateSet, context.SeasonId, cancellationToken);
        var groupedMetrics = metrics
            .GroupBy(metric => metric.PlayerId)
            .ToDictionary(group => group.Key, group => group.ToDictionary(item => item.MetricKey, item => item.Value, StringComparer.OrdinalIgnoreCase));

        var results = new List<PlayerRankingResult>();

        foreach (var player in players)
        {
            if (!candidateSet.Contains(player.Id))
            {
                continue;
            }

            if (!string.Equals(player.PrimaryPosition, profile.TargetPosition, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (player.MarketValueEur > profile.BudgetMaxEur)
            {
                continue;
            }

            groupedMetrics.TryGetValue(player.Id, out var playerMetrics);
            playerMetrics ??= new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            if (playerMetrics.TryGetValue("minutes", out var minutes) && minutes < profile.MinMinutesPlayed)
            {
                continue;
            }

            var breakdown = new List<MetricBreakdown>();
            decimal weightedSum = 0m;
            decimal totalWeight = 0m;
            var blocked = false;

            foreach (var entry in weightByMetric.Values)
            {
                if (!playerMetrics.TryGetValue(entry.MetricKey, out var rawValue))
                {
                    if (entry.IsHardConstraint)
                    {
                        blocked = true;
                        break;
                    }

                    continue;
                }

                if (entry.IsHardConstraint)
                {
                    if (entry.MinValue.HasValue && rawValue < entry.MinValue.Value)
                    {
                        blocked = true;
                        break;
                    }

                    if (entry.MaxValue.HasValue && rawValue > entry.MaxValue.Value)
                    {
                        blocked = true;
                        break;
                    }
                }

                if (entry.Weight <= 0)
                {
                    continue;
                }

                if (!metricDefinitionByKey.TryGetValue(entry.MetricKey, out var definition))
                {
                    continue;
                }

                var normalized = Normalize(rawValue, definition.MinExpected, definition.MaxExpected, definition.HigherIsBetter);
                var contribution = normalized * entry.Weight;
                weightedSum += contribution;
                totalWeight += entry.Weight;

                breakdown.Add(new MetricBreakdown
                {
                    MetricKey = entry.MetricKey,
                    RawValue = rawValue,
                    NormalizedValue = normalized,
                    Weight = entry.Weight,
                    Contribution = contribution
                });
            }

            if (blocked || totalWeight <= 0)
            {
                continue;
            }

            results.Add(new PlayerRankingResult
            {
                PlayerId = player.Id,
                Score = weightedSum / totalWeight,
                Breakdown = breakdown
            });
        }

        return results
            .OrderByDescending(result => result.Score)
            .ToArray();
    }

    private static decimal Normalize(decimal rawValue, decimal min, decimal max, bool higherIsBetter)
    {
        if (max <= min)
        {
            return 0m;
        }

        var normalized = (rawValue - min) / (max - min);
        normalized = Math.Clamp(normalized, 0m, 1m);

        return higherIsBetter ? normalized : 1m - normalized;
    }
}
