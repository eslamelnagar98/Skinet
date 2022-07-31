using Core.Specifications;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(
            IQueryable<TEntity> inputQuery,
            ISpecification<TEntity> specifcation)
        {
            var query = inputQuery;
            if (specifcation.Criteria is not null)
            {
                query = query.Where(specifcation.Criteria);
            }

            query = specifcation.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
