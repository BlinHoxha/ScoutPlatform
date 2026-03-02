using ScoutPlatform.Application.Common;

namespace ScoutPlatform.Infrastructure.Persistence;

internal sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ScoutPlatformDbContext _dbContext;

    public EfUnitOfWork(ScoutPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
