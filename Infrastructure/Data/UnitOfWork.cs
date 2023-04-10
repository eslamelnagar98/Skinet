using Core.Interfaces;

namespace Infrastructure.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _storeContext;
    private Dictionary<string, object> _repositories = new();

    public UnitOfWork(StoreContext storeContext)
    {
        _storeContext = Guard.Against.Null(storeContext, nameof(storeContext));
    }
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;
        if (_repositories.TryGetValue(type, out var instance))
        {
            return instance as IGenericRepository<TEntity>;
        }
        var repositoryType = typeof(GenericRepository<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _storeContext)
                                as IGenericRepository<TEntity>;
        _repositories.TryAdd(type, repositoryInstance);
        return repositoryInstance;
    }

    public async Task<int> Complete()
    {
       return await _storeContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _storeContext.Dispose();
    }
}
