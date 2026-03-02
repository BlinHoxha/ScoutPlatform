namespace ScoutPlatform.Infrastructure.Persistence.Entities;

public sealed class TeamProfileWeightEntity : EntityBase
{
    public Guid TeamProfileId { get; set; }
    public TeamProfileEntity TeamProfile { get; set; } = null!;
    public string MetricKey { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public bool IsHardConstraint { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
}
