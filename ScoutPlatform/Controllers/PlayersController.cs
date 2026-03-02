using Microsoft.AspNetCore.Mvc;
using ScoutPlatform.Application.Players;

namespace ScoutPlatform.Controllers;

[ApiController]
[Route("api/v1/players")]
public sealed class PlayersController : ControllerBase
{
    private readonly PlayerService _playerService;

    public PlayersController(PlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PlayerSummaryDto>>> Search(
        [FromQuery] string? search,
        [FromQuery] string? position,
        [FromQuery] int? ageMin,
        [FromQuery] int? ageMax,
        CancellationToken cancellationToken)
    {
        var result = await _playerService.SearchAsync(search, position, ageMin, ageMax, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PlayerSummaryDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var player = await _playerService.GetByIdAsync(id, cancellationToken);
        return player is null ? NotFound() : Ok(player);
    }

    [HttpGet("{id:guid}/metrics")]
    public async Task<ActionResult<IReadOnlyCollection<PlayerMetricDto>>> GetMetrics(Guid id, [FromQuery] int seasonId = 2025, CancellationToken cancellationToken = default)
    {
        var metrics = await _playerService.GetMetricsAsync(id, seasonId, cancellationToken);
        return Ok(metrics);
    }
}
