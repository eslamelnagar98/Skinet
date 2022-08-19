namespace Core.Specifications;
public class BaseSpecification<TEntity> : ISpecification<TEntity>
{
    public BaseSpecification() { }
    public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = Guard.Against.Null(criteria, nameof(criteria));
    }
    public Expression<Func<TEntity, bool>> Criteria { get; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();

    public Expression<Func<TEntity, object>> OrderBy { get; private set; }

    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPaginEnabled { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) => Includes.Add(includeExpression);

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        => OrderByDescending = orderByDescendingExpression;

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderBy = orderByExpression;

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginEnabled = true;
    }

}

