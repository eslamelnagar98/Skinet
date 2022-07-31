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

    public static void AddStartUpExtentions(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRespository<>));
    }

    public static async Task ApplyMigrations(this IServiceProvider services)
    {
        await using var scoped = services.CreateAsyncScope();
        using var storeContext = scoped.ServiceProvider.GetRequiredService<StoreContext>();
        var logger = services.GetRequiredService<ILogger<StoreContextSeed>>();
        try
        {
            await storeContext.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(storeContext, logger);
        }
        catch (Exception exception)
        {
            logger.LogError("An Error Occured During Database Migration ,{exception}", exception);
        }
    }
}

