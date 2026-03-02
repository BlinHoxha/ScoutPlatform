namespace ScoutPlatform.Infrastructure.Persistence.Entities;

public sealed class TeamProfileEntity : EntityBase
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    public string TargetPosition { get; set; } = string.Empty;
    public decimal BudgetMaxEur { get; set; }
    public int MinMinutesPlayed { get; set; }

    public ICollection<TeamProfileWeightEntity> Weights { get; set; } = [];
}
