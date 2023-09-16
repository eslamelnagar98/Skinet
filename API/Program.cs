var app = default(WebApplication);
try
{
    app = WebApplication.CreateBuilder(args)
                  .CreateSkinetBuilder()
                  .RegisterCustomeMidllewares()
                  .RegisterMidllewares();
    await app.Services.ApplyMigrations();
    await app.RunAsync();
}
catch (Exception)
{
    await app.StopAsync();
}


