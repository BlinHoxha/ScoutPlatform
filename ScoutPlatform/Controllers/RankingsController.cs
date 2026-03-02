using Microsoft.AspNetCore.Mvc;
using ScoutPlatform.Application.Rankings;
using ScoutPlatform.Contracts;

namespace ScoutPlatform.Controllers;

[ApiController]
[Route("api/v1/team-profiles/{teamProfileId:guid}")]
public sealed class RankingsController : ControllerBase
{
    private readonly RankingService _rankingService;

    public RankingsController(RankingService rankingService)
    {
        _rankingService = rankingService;
    }

    [HttpPost("scores/recalculate")]
    public IActionResult Recalculate(Guid teamProfileId)
        => Accepted(new { TeamProfileId = teamProfileId, Status = "queued" });

    [HttpGet("rankings")]
    public async Task<ActionResult<IReadOnlyCollection<PlayerRankingDto>>> GetRankings(Guid teamProfileId, [FromQuery] RankingsQuery query, CancellationToken cancellationToken)
    {
        var candidatePlayerIds = ParseCandidateIds(query.CandidatePlayerIds);
        var rankings = await _rankingService.GetRankingsAsync(teamProfileId, query.SeasonId, candidatePlayerIds, cancellationToken);
        return Ok(rankings);
    }

    [HttpGet("rankings/{playerId:guid}")]
    public async Task<ActionResult<PlayerRankingDto>> GetRankingForPlayer(Guid teamProfileId, Guid playerId, [FromQuery] int seasonId = 2025, CancellationToken cancellationToken = default)
    {
        var rankings = await _rankingService.GetRankingsAsync(teamProfileId, seasonId, [playerId], cancellationToken);
        var playerRanking = rankings.FirstOrDefault();
        return playerRanking is null ? NotFound() : Ok(playerRanking);
    }

    private static IReadOnlyCollection<Guid> ParseCandidateIds(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<Guid>();
        }

        return value
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(static input => Guid.TryParse(input, out var id) ? id : Guid.Empty)
            .Where(static id => id != Guid.Empty)
            .ToArray();
    }
}
