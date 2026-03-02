namespace ScoutPlatform.Infrastructure.Persistence.Entities;

public sealed class SuitabilityScoreEntity : EntityBase
{
    public Guid TeamProfileId { get; set; }
    public Guid PlayerId { get; set; }
    public decimal Score { get; set; }
    public int ScoreVersion { get; set; }
    public string BreakdownJson { get; set; } = "{}";
    public DateTime CalculatedAtUtc { get; set; } = DateTime.UtcNow;
}
