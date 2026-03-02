namespace ScoutPlatform.Application.Players;

public sealed record PlayerSummaryDto(Guid Id, string FullName, string PrimaryPosition, string? CurrentClub, decimal MarketValueEur);
public sealed record PlayerMetricDto(string MetricKey, decimal Value);
