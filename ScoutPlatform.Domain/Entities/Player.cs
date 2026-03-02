namespace ScoutPlatform.Domain.Entities;

public sealed class Player
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public string PrimaryPosition { get; init; } = string.Empty;
    public string? CurrentClub { get; init; }
    public decimal MarketValueEur { get; init; }
}
