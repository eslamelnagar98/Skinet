namespace API.Middleware;
public class ExeptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExeptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger, IHostEnvironment environment)
    {
        _next = Guard.Against.Null(next, nameof(next));
        _logger = Guard.Against.Null(logger, nameof(logger));
        _environment = Guard.Against.Null(environment, nameof(environment));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = IsDevelopment(ex);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    private ApiException IsDevelopment(Exception ex)
    {
        return _environment.IsDevelopment()
            ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
            : new ApiException((int)HttpStatusCode.InternalServerError);
    }
}

