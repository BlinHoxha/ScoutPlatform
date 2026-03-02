namespace ScoutPlatform.Domain.Scoring;

public sealed class RankingContext
{
    public Guid TeamProfileId { get; init; }
    public int SeasonId { get; init; }
    public required IReadOnlyCollection<Guid> CandidatePlayerIds { get; init; }
}
