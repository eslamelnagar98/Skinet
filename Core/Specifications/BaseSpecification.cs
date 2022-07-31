namespace Core.Specifications;
public class BaseSpecification<TEntity> : ISpecification<TEntity>
{
    public BaseSpecification()
    {

    }
    public Expression<Func<TEntity, bool>> Criteria { get; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
    public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = Guard.Against.Null(criteria, nameof(criteria));
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        => Includes.Add(includeExpression);
    
}

