var app = default(WebApplication);
try
{
    app = await WebApplication.CreateBuilder(args)
                  .CreateSkinetBuilder()
                  .RegisterCustomeMidllewares();

    await app.RegisterMidllewares()
             .RunAsync();
}
catch (Exception)
{
    await app.StopAsync();
}


