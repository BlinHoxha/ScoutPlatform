namespace ScoutPlatform.Domain.Entities;

public sealed class PlayerMetric
{
    public Guid PlayerId { get; init; }
    public int SeasonId { get; init; }
    public string MetricKey { get; init; } = string.Empty;
    public decimal Value { get; init; }
}
