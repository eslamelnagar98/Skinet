var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonServices()
                .AddStroreDbContext(dbConnectionString)
                .AddRedisConnection(redisConnectionString)
                .AddAutoMapper(typeof(MappingProfiles))
                .ConfigureBadRequestBehaviour()
                .ConfigureCorsOrigins();
var app = builder.Build();
await app.Services.ApplyMigrations();
app.UseMiddleware<ExeptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();

