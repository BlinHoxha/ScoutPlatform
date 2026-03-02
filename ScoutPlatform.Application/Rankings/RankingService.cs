using ScoutPlatform.Application.Common;
using ScoutPlatform.Domain.Scoring;

namespace ScoutPlatform.Application.Rankings;

public sealed class RankingService
{
    private readonly IScoringService _scoringService;

    public RankingService(IScoringService scoringService)
    {
        _scoringService = scoringService;
    }

    public async Task<IReadOnlyCollection<PlayerRankingDto>> GetRankingsAsync(Guid teamProfileId, int seasonId, IReadOnlyCollection<Guid> candidatePlayerIds, CancellationToken cancellationToken)
    {
        var context = new RankingContext
        {
            TeamProfileId = teamProfileId,
            SeasonId = seasonId,
            CandidatePlayerIds = candidatePlayerIds
        };

        var results = await _scoringService.RankPlayersAsync(context, cancellationToken);
        return results
            .Select(result => new PlayerRankingDto(
                result.PlayerId,
                result.Score,
                result.Breakdown
                    .Select(metric => new RankingMetricBreakdownDto(metric.MetricKey, metric.RawValue, metric.NormalizedValue, metric.Weight, metric.Contribution))
                    .ToArray()))
            .OrderByDescending(result => result.Score)
            .ToArray();
    }
}
