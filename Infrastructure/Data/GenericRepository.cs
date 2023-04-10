using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
namespace Infrastructure.Data;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly StoreContext _storeContext;
    public GenericRepository(StoreContext storeContext)
    {
        _storeContext = Guard.Against.Null(storeContext, nameof(storeContext));
    }
    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _storeContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<IReadOnlyList<TEntity>> ListAllAsync()
    {
        return await _storeContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetEntityWithSpecification(ISpecification<TEntity> specification)
    {
        return await (await ApplySpecification(specification)).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification)
    {
        return await (await ApplySpecification(specification)).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<TEntity> specification)
    {
        return await _storeContext.Set<TEntity>()
            .Where(specification.Criteria)
            .AsNoTracking()
            .CountAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _storeContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task UpdateAsync(Expression<Func<TEntity, bool>> predicate,
                             Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
    {
        await _storeContext.Set<TEntity>().Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await _storeContext.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
    }

    private async Task<IQueryable<TEntity>> ApplySpecification(ISpecification<TEntity> specification)
    {
        return await SpecificationEvaluator<TEntity>.GetQuery(_storeContext.Set<TEntity>().AsQueryable(), specification);
    }


}

