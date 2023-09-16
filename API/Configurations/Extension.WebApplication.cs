namespace API.Configurations;
public partial class Extension
{
    public static WebApplication CreateSkinetBuilder(this WebApplicationBuilder builder)
    {
        var userIdentityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
        var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddCommonServices()
            .AddDbContext<AppIdentityDbContext>(userIdentityConnectionString)
            .AddDbContext<StoreContext>(dbConnectionString)
            .AddRedisConnection(redisConnectionString)
            .AddIdentityServices()
            .AddJwtBearerAuthentication(builder.Configuration)
            .AddAutoMapper(typeof(MappingProfiles))
            .ConfigureBadRequestBehaviour()
            .ConfigureCorsOrigins()
            .AddSwaggerAuthorizationSecurity()
            .AddMapster(builder.Configuration);
        return builder.Build();
    }
    public static WebApplication RegisterMidllewares(this WebApplication app)
    {
        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseCors("CorsPolicy");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
    public static WebApplication RegisterCustomeMidllewares(this WebApplication app)
    {
        app.UseMiddleware<ExeptionMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerDocumentation();
        }
        return app;
    }

}
