using Microsoft.AspNetCore.Mvc;
using ScoutPlatform.Application.TeamProfiles;
using ScoutPlatform.Contracts;

namespace ScoutPlatform.Controllers;

[ApiController]
[Route("api/v1/team-profiles")]
public sealed class TeamProfilesController : ControllerBase
{
    private readonly TeamProfileService _teamProfileService;

    public TeamProfilesController(TeamProfileService teamProfileService)
    {
        _teamProfileService = teamProfileService;
    }

    [HttpPost]
    public async Task<ActionResult<TeamProfileDto>> Create(CreateTeamProfileRequest request, CancellationToken cancellationToken)
    {
        var model = new TeamProfileDto(
            Guid.NewGuid(),
            request.OrganizationId,
            request.Name,
            request.Style,
            request.TargetPosition,
            request.BudgetMaxEur,
            request.MinMinutesPlayed);

        var created = await _teamProfileService.CreateAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TeamProfileDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var profile = await _teamProfileService.GetByIdAsync(id, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TeamProfileDto>>> GetAll(CancellationToken cancellationToken)
    {
        var profiles = await _teamProfileService.GetAllAsync(cancellationToken);
        return Ok(profiles);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TeamProfileDto>> Update(Guid id, UpdateTeamProfileRequest request, CancellationToken cancellationToken)
    {
        var model = new TeamProfileDto(
            id,
            request.OrganizationId,
            request.Name,
            request.Style,
            request.TargetPosition,
            request.BudgetMaxEur,
            request.MinMinutesPlayed);

        var updated = await _teamProfileService.UpdateAsync(model, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _teamProfileService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPut("{id:guid}/weights")]
    public async Task<ActionResult<IReadOnlyCollection<TeamProfileWeightDto>>> SetWeights(Guid id, SetTeamProfileWeightsRequest request, CancellationToken cancellationToken)
    {
        var weights = request.Weights
            .Select(item => new TeamProfileWeightDto(item.MetricKey, item.Weight, item.IsHardConstraint, item.MinValue, item.MaxValue))
            .ToArray();

        var result = await _teamProfileService.SetWeightsAsync(id, weights, cancellationToken);
        return Ok(result);
    }
}
