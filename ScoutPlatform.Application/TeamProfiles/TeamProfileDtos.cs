namespace ScoutPlatform.Application.TeamProfiles;

public sealed record TeamProfileDto(Guid Id, Guid OrganizationId, string Name, string Style, string TargetPosition, decimal BudgetMaxEur, int MinMinutesPlayed);

public sealed record TeamProfileWeightDto(
    string MetricKey,
    decimal Weight,
    bool IsHardConstraint,
    decimal? MinValue,
    decimal? MaxValue);
