namespace API.Configurations;
public static partial class Extension
{
    public static IServiceCollection ConfigureBadRequestBehaviour(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                                        .Where(error => error.Value.Errors.Any())
                                        .SelectMany(x => x.Value.Errors)
                                        .Select(x => x.ErrorMessage).ToList();

                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }

    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRespository<>));
        return services;
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

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static IServiceCollection ConfigureCorsOrigins(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
            });
        });
    }
}

