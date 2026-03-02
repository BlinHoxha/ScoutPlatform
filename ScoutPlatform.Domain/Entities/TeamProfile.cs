namespace ScoutPlatform.Domain.Entities;

public sealed class TeamProfile
{
    public Guid Id { get; init; }
    public Guid OrganizationId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Style { get; init; } = string.Empty;
    public string TargetPosition { get; init; } = string.Empty;
    public decimal BudgetMaxEur { get; init; }
    public int MinMinutesPlayed { get; init; }
}
