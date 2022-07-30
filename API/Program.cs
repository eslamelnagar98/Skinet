var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddStroreDbContext(connectionString);
var app = builder.Build();
await ApplyMigrations(app);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task ApplyMigrations(WebApplication app)
{
    await using var scoped = app.Services.CreateAsyncScope();
    using var storeContext = scoped.ServiceProvider.GetRequiredService<StoreContext>();
    try
    {
        await storeContext.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(storeContext, app.Logger);
    }
    catch (Exception exception)
    {
        app.Logger.LogError("An Error Occured During Database Migration ,{exception}", exception);
    }
}
