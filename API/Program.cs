var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonServices()
                .AddStroreDbContext(connectionString)
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

