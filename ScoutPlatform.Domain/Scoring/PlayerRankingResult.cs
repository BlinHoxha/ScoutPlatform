namespace ScoutPlatform.Domain.Scoring;

public sealed class PlayerRankingResult
{
    public Guid PlayerId { get; init; }
    public decimal Score { get; init; }
    public required IReadOnlyCollection<MetricBreakdown> Breakdown { get; init; }
}
