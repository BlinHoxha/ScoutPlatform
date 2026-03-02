namespace ScoutPlatform.Domain.Entities;

public sealed class TeamProfileWeight
{
    public Guid TeamProfileId { get; init; }
    public string MetricKey { get; init; } = string.Empty;
    public decimal Weight { get; init; }
    public bool IsHardConstraint { get; init; }
    public decimal? MinValue { get; init; }
    public decimal? MaxValue { get; init; }
}
