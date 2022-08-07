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

    
}

