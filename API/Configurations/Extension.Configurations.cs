using Mapster;
using MapsterMapper;
using Microsoft.OpenApi.Models;

namespace API.Configurations;
public static partial class Extension
{
    public static IServiceCollection AddSwaggerAuthorizationSecurity(this IServiceCollection service)
    {
        service.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Auth Bearer Scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            option.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
            option.AddSecurityRequirement(securityRequirement);
        });
        return service;
    }
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
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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

    public static IServiceCollection AddMapster(this IServiceCollection services, IConfiguration configuration)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Apply(new MapsterProfile(configuration));
        services.AddSingleton(config);
        services.AddScoped<MapsterMapper.IMapper, ServiceMapper>();
        return services;
    }
}
