namespace Infrastructure.Data;
public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
                                               ISpecification<TEntity> specifcation)
    {
        var query = inputQuery;
        query = query.EvaluateSpecification(specifcation.Criteria, input =>
                                    input.Where(specifcation.Criteria))
                     .EvaluateSpecification(specifcation.OrderBy, input =>
                                    input.OrderBy(specifcation.OrderBy))
                     .EvaluateSpecification(specifcation.OrderByDescending, input =>
                                    input.OrderByDescending(specifcation.OrderByDescending))
                     .EvaluateSpecification(specifcation.IsPaginEnabled, input =>
                                            input.Skip(specifcation.Skip).Take(specifcation.Take));

        query = specifcation.Includes.Aggregate(query, (current, include) => current.Include(include));
        return query;
    }

}

