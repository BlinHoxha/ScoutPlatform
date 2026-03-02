using ScoutPlatform.Application.Common;

namespace ScoutPlatform.Application.Common.Services;

public abstract class CrudServiceBase<TModel, TId>
{
    private readonly ICrudRepository<TModel, TId> _repository;
    private readonly IUnitOfWork _unitOfWork;

    protected CrudServiceBase(ICrudRepository<TModel, TId> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    protected Task<TModel?> GetByIdCoreAsync(TId id, CancellationToken cancellationToken)
        => _repository.GetByIdAsync(id, cancellationToken);

    protected Task<IReadOnlyCollection<TModel>> GetAllCoreAsync(CancellationToken cancellationToken)
        => _repository.GetAllAsync(cancellationToken);

    protected async Task<TModel> CreateCoreAsync(TModel model, CancellationToken cancellationToken)
    {
        var created = await _repository.CreateAsync(model, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return created;
    }

    protected async Task<TModel?> UpdateCoreAsync(TModel model, CancellationToken cancellationToken)
    {
        var updated = await _repository.UpdateAsync(model, cancellationToken);
        if (updated is null)
        {
            return default;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return updated;
    }

    protected async Task<bool> DeleteCoreAsync(TId id, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return false;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    protected Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _unitOfWork.SaveChangesAsync(cancellationToken);
}
