namespace Core.Interfaces;
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IReadOnlyList<TEntity>> ListAllAsync();
    Task<TEntity> GetEntityWithSpecification(ISpecification<TEntity> specification);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification);
    Task<int> CountAsync(ISpecification<TEntity> specification);
}

