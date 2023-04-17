namespace Infrastructure.Extensions;
public static class StartUpExtensions
{

    public static IServiceCollection AddDbContext<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(option =>
        {
            option.UseSqlServer(connectionString)
                  .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                   LogLevel.Information)
                  .EnableSensitiveDataLogging();
        });

        return services;
    }
    public static IServiceCollection AddRedisConnection(this IServiceCollection services, string connectionString)
    {
        return services.AddSingleton<IConnectionMultiplexer>(connection =>
         {
             var configuration = ConfigurationOptions.Parse(connectionString, true);
             return ConnectionMultiplexer.Connect(configuration);
         });
    }

    public static IQueryable<TEntity> EvaluateSpecification<TEntity, Tobj>(
           this IQueryable<TEntity> inputQuery,
           Tobj specification,
           Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate)
    {
        if (specification is bool spec)
        {
            return spec is false ? inputQuery : predicate?.Invoke(inputQuery);
        }
        return specification is null ? inputQuery : predicate?.Invoke(inputQuery);
    }


}

