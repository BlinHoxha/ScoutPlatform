namespace ScoutPlatform.Contracts;

public sealed record CreateTeamProfileRequest(
    Guid OrganizationId,
    string Name,
    string Style,
    string TargetPosition,
    decimal BudgetMaxEur,
    int MinMinutesPlayed);

public sealed record UpdateTeamProfileRequest(
    Guid OrganizationId,
    string Name,
    string Style,
    string TargetPosition,
    decimal BudgetMaxEur,
    int MinMinutesPlayed);

public sealed record SetTeamProfileWeightsRequest(IReadOnlyCollection<SetTeamProfileWeightItem> Weights);

public sealed record SetTeamProfileWeightItem(
    string MetricKey,
    decimal Weight,
    bool IsHardConstraint,
    decimal? MinValue,
    decimal? MaxValue);
