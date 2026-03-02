namespace ScoutPlatform.Infrastructure.Persistence.Entities;

public sealed class PlayerEntity : EntityBase
{
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string PrimaryPosition { get; set; } = string.Empty;
    public string? CurrentClub { get; set; }
    public decimal MarketValueEur { get; set; }

    public ICollection<PlayerMetricEntity> Metrics { get; set; } = [];
}
