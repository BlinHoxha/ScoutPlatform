namespace ScoutPlatform.Infrastructure.Persistence.Entities;

public sealed class PlayerMetricEntity : EntityBase
{
    public Guid PlayerId { get; set; }
    public PlayerEntity Player { get; set; } = null!;
    public int SeasonId { get; set; }
    public string? CompetitionId { get; set; }
    public string MetricKey { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal? Minutes { get; set; }
    public string? Source { get; set; }
    public DateTime CollectedAtUtc { get; set; } = DateTime.UtcNow;
}
