namespace ScoutPlatform.Application.Rankings;

public sealed record RankingMetricBreakdownDto(string MetricKey, decimal RawValue, decimal NormalizedValue, decimal Weight, decimal Contribution);
public sealed record PlayerRankingDto(Guid PlayerId, decimal Score, IReadOnlyCollection<RankingMetricBreakdownDto> Breakdown);
