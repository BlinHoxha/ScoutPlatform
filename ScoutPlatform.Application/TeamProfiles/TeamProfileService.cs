using ScoutPlatform.Application.Common;
using ScoutPlatform.Application.Common.Services;

namespace ScoutPlatform.Application.TeamProfiles;

public sealed class TeamProfileService : CrudServiceBase<TeamProfileDto, Guid>
{
    private readonly ITeamProfileRepository _repository;

    public TeamProfileService(ITeamProfileRepository repository, IUnitOfWork unitOfWork)
        : base(repository, unitOfWork)
    {
        _repository = repository;
    }

    public Task<TeamProfileDto> CreateAsync(TeamProfileDto profile, CancellationToken cancellationToken)
        => CreateCoreAsync(profile, cancellationToken);

    public Task<TeamProfileDto?> GetByIdAsync(Guid teamProfileId, CancellationToken cancellationToken)
        => GetByIdCoreAsync(teamProfileId, cancellationToken);

    public Task<IReadOnlyCollection<TeamProfileDto>> GetAllAsync(CancellationToken cancellationToken)
        => GetAllCoreAsync(cancellationToken);

    public Task<TeamProfileDto?> UpdateAsync(TeamProfileDto profile, CancellationToken cancellationToken)
        => UpdateCoreAsync(profile, cancellationToken);

    public Task<bool> DeleteAsync(Guid teamProfileId, CancellationToken cancellationToken)
        => DeleteCoreAsync(teamProfileId, cancellationToken);

    public async Task<IReadOnlyCollection<TeamProfileWeightDto>> SetWeightsAsync(Guid teamProfileId, IReadOnlyCollection<TeamProfileWeightDto> weights, CancellationToken cancellationToken)
    {
        var updated = await _repository.SetWeightsAsync(teamProfileId, weights, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return updated;
    }
}
