var builder = WebApplication.CreateBuilder(args);
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
    .ConfigureCorsOrigins();
var app = builder.Build();
try
{
    await app.Services.ApplyMigrations();
    app.UseMiddleware<ExeptionMiddleware>();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerDocumentation();
    }
}
catch (Exception)
{
    await app.StopAsync();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

