namespace ScoutPlatform.Contracts;

public sealed record RankingsQuery(
    int SeasonId = 2025,
    string? CandidatePlayerIds = null);
