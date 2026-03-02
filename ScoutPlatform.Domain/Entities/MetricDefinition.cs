namespace ScoutPlatform.Domain.Entities;

public sealed class MetricDefinition
{
    public string Key { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal MinExpected { get; init; }
    public decimal MaxExpected { get; init; }
    public bool HigherIsBetter { get; init; }
}
