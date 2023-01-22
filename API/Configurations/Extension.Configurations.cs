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
                                        .Select(x => x.ErrorMessage)
                                        .ToList();

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
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRespository<>));
        return services;
    }

    public static async Task ApplyMigrations(this IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<StoreContextSeed>>();
        await using var scoped = services.CreateAsyncScope();
        var serviceProvider = scoped.ServiceProvider;
        try
        {
            using var storeContext = serviceProvider.GetRequiredService<StoreContext>();
            await storeContext.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(storeContext, logger);

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var identityContext = serviceProvider.GetRequiredService<AppIdentityDbContext>();
            await identityContext.Database.MigrateAsync();
            await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
        }
        catch (Exception exception)
        {
            logger.LogError("An Error Occured During Database Migration ,{exception}", exception);
            throw;
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
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins($"{ClientSide.Ip}:{ClientSide.Port}");
            });
        });
    }
}


//  public static async Task ApplyMigrations2<TDbContext, TDbContextSeed>(this IServiceProvider services)
//where TDbContext : DbContext
//where TDbContextSeed : class
//  {
//      await using var scoped = services.CreateAsyncScope();
//      using var dbContext = scoped.ServiceProvider.GetRequiredService<TDbContext>();
//      var logger = services.GetRequiredService<ILogger<TDbContextSeed>>();
//      try
//      {
//          await dbContext.Database.MigrateAsync();
//          var method = typeof(TDbContextSeed).GetMethod(nameof(StoreContextSeed.SeedAsync),
//              BindingFlags.Public | BindingFlags.Static,
//              new Type[] { typeof(TDbContext), typeof(ILogger<TDbContextSeed>) }
//          );
//          await (Task)method.Invoke(null, new object[] { dbContext, logger });
//      }
//      catch (Exception exception)
//      {
//          logger.LogError("An Error Occured During Database Migration ,{exception}", exception);
//      }
//  }