namespace ScoutPlatform.Infrastructure.Persistence.Entities;

public sealed class MetricDefinitionEntity : EntityBase
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public bool HigherIsBetter { get; set; }
    public string? Description { get; set; }
    public string? Group { get; set; }
    public string NormalizationStrategy { get; set; } = "MinMax";
    public decimal MinExpected { get; set; }
    public decimal MaxExpected { get; set; }
}
