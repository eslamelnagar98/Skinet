namespace Infrastructure.Extensions;
public static class StartUpExtensions
{
    public static IServiceCollection AddStroreDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StoreContext>(option =>
        {
            option.UseSqlServer(connectionString)
                  .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                   LogLevel.Information)
                  .EnableSensitiveDataLogging();
        });

        return services;
    }

    public static IQueryable<TEntity> EvaluateSpecification<TEntity, Tobj>(
           this IQueryable<TEntity> inputQuery,
           Tobj specification,
           Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate)
    {
        if (specification is bool spec)
        {
            return spec == false ? inputQuery : predicate?.Invoke(inputQuery);
        }
        return specification is null ? inputQuery : predicate?.Invoke(inputQuery);
    }


}

