using Microsoft.EntityFrameworkCore;
using ScoutPlatform.Application.Common;
using ScoutPlatform.Application.TeamProfiles;
using ScoutPlatform.Domain.Entities;
using ScoutPlatform.Infrastructure.Persistence.Entities;

namespace ScoutPlatform.Infrastructure.Persistence;

internal sealed class EfTeamProfileRepository : ITeamProfileRepository
{
    private readonly ScoutPlatformDbContext _dbContext;
    private readonly EfRepository<TeamProfileEntity> _repository;

    public EfTeamProfileRepository(ScoutPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
        _repository = new EfRepository<TeamProfileEntity>(dbContext);
    }

    public async Task<TeamProfileDto> CreateAsync(TeamProfileDto model, CancellationToken cancellationToken)
    {
        var entity = new TeamProfileEntity
        {
            Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
            OrganizationId = model.OrganizationId,
            Name = model.Name,
            Style = model.Style,
            TargetPosition = model.TargetPosition,
            BudgetMaxEur = model.BudgetMaxEur,
            MinMinutesPlayed = model.MinMinutesPlayed
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return Map(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await _repository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyCollection<TeamProfileDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(Map).ToArray();
    }

    public async Task<TeamProfileDto?> GetByIdAsync(Guid teamProfileId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TeamProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == teamProfileId, cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task<TeamProfileDto?> UpdateAsync(TeamProfileDto model, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.TeamProfiles.FirstOrDefaultAsync(item => item.Id == model.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.OrganizationId = model.OrganizationId;
        existing.Name = model.Name;
        existing.Style = model.Style;
        existing.TargetPosition = model.TargetPosition;
        existing.BudgetMaxEur = model.BudgetMaxEur;
        existing.MinMinutesPlayed = model.MinMinutesPlayed;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        return Map(existing);
    }

    public async Task<IReadOnlyCollection<TeamProfileWeightDto>> SetWeightsAsync(Guid teamProfileId, IReadOnlyCollection<TeamProfileWeightDto> weights, CancellationToken cancellationToken)
    {
        var profileExists = await _dbContext.TeamProfiles.AnyAsync(item => item.Id == teamProfileId, cancellationToken);
        if (!profileExists)
        {
            return Array.Empty<TeamProfileWeightDto>();
        }

        var existing = await _dbContext.TeamProfileWeights
            .Where(item => item.TeamProfileId == teamProfileId)
            .ToArrayAsync(cancellationToken);

        _dbContext.TeamProfileWeights.RemoveRange(existing);

        var entities = weights.Select(weight => new TeamProfileWeightEntity
        {
            Id = Guid.NewGuid(),
            TeamProfileId = teamProfileId,
            MetricKey = weight.MetricKey,
            Weight = weight.Weight,
            IsHardConstraint = weight.IsHardConstraint,
            MinValue = weight.MinValue,
            MaxValue = weight.MaxValue
        }).ToArray();

        await _dbContext.TeamProfileWeights.AddRangeAsync(entities, cancellationToken);

        return entities.Select(MapWeight).ToArray();
    }

    public async Task<IReadOnlyCollection<TeamProfileWeightDto>> GetWeightsAsync(Guid teamProfileId, CancellationToken cancellationToken)
    {
        var weights = await _dbContext.TeamProfileWeights
            .AsNoTracking()
            .Where(weight => weight.TeamProfileId == teamProfileId)
            .Select(weight => new TeamProfileWeightDto(weight.MetricKey, weight.Weight, weight.IsHardConstraint, weight.MinValue, weight.MaxValue))
            .ToArrayAsync(cancellationToken);

        return weights;
    }

    public async Task<TeamProfile?> GetDomainByIdAsync(Guid teamProfileId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.TeamProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == teamProfileId, cancellationToken);

        return entity is null
            ? null
            : new TeamProfile
            {
                Id = entity.Id,
                OrganizationId = entity.OrganizationId,
                Name = entity.Name,
                Style = entity.Style,
                TargetPosition = entity.TargetPosition,
                BudgetMaxEur = entity.BudgetMaxEur,
                MinMinutesPlayed = entity.MinMinutesPlayed
            };
    }

    public async Task<IReadOnlyCollection<TeamProfileWeight>> GetDomainWeightsAsync(Guid teamProfileId, CancellationToken cancellationToken)
        => await _dbContext.TeamProfileWeights
            .AsNoTracking()
            .Where(weight => weight.TeamProfileId == teamProfileId)
            .Select(weight => new TeamProfileWeight
            {
                TeamProfileId = weight.TeamProfileId,
                MetricKey = weight.MetricKey,
                Weight = weight.Weight,
                IsHardConstraint = weight.IsHardConstraint,
                MinValue = weight.MinValue,
                MaxValue = weight.MaxValue
            })
            .ToArrayAsync(cancellationToken);

    private static TeamProfileDto Map(TeamProfileEntity entity)
        => new(entity.Id, entity.OrganizationId, entity.Name, entity.Style, entity.TargetPosition, entity.BudgetMaxEur, entity.MinMinutesPlayed);

    private static TeamProfileWeightDto MapWeight(TeamProfileWeightEntity entity)
        => new(entity.MetricKey, entity.Weight, entity.IsHardConstraint, entity.MinValue, entity.MaxValue);
}
