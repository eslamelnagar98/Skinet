using Microsoft.EntityFrameworkCore.Query;

namespace Core.Interfaces;
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IReadOnlyList<TEntity>> ListAllAsync();
    Task<TEntity> GetEntityWithSpecification(ISpecification<TEntity> specification);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification);
    Task<int> CountAsync(ISpecification<TEntity> specification);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
}

