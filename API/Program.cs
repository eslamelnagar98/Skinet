var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStartUpExtentions();
builder.Services.AddStroreDbContext(connectionString);
builder.Services.AddAutoMapper(typeof(MappingProfiles));
var app = builder.Build();
await app.Services.ApplyMigrations();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();

