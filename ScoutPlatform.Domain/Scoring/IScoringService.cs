namespace ScoutPlatform.Domain.Scoring;

public interface IScoringService
{
    Task<IReadOnlyCollection<PlayerRankingResult>> RankPlayersAsync(RankingContext context, CancellationToken cancellationToken);
}
