namespace ScoutPlatform.Domain.Scoring;

public sealed class MetricBreakdown
{
    public required string MetricKey { get; init; }
    public decimal RawValue { get; init; }
    public decimal NormalizedValue { get; init; }
    public decimal Weight { get; init; }
    public decimal Contribution { get; init; }
}
