using Microsoft.EntityFrameworkCore;
using ScoutPlatform.Application.Common;

namespace ScoutPlatform.Infrastructure.Persistence;

internal sealed class EfRepository<TEntity> : ICrudRepository<TEntity, Guid>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(ScoutPlatformDbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity model, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(model, cancellationToken);
        return model;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _dbSet.FindAsync([id], cancellationToken);
        if (existing is null)
        {
            return false;
        }

        _dbSet.Remove(existing);
        return true;
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        => await _dbSet.AsNoTracking().ToArrayAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbSet.FindAsync([id], cancellationToken);

    public Task<TEntity?> UpdateAsync(TEntity model, CancellationToken cancellationToken)
    {
        _dbSet.Update(model);
        return Task.FromResult<TEntity?>(model);
    }
}
