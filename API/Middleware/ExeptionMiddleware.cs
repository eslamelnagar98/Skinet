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
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }
    private async ValueTask HandleException(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, exception.Message);
        httpContext = PrepareHttpContext(httpContext);
        var exceptionResponse = IsDevelopment(exception);
        var jsonContent = PrepareJsonContent(exceptionResponse);
        await httpContext.Response.WriteAsync(jsonContent);
    }
    private HttpContext PrepareHttpContext(HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return httpContext;
    }
    private ApiException IsDevelopment(Exception exception)
    {
        return _environment.IsDevelopment()
            ? new ApiException((int)HttpStatusCode.InternalServerError, exception.Message, exception.StackTrace.ToString())
            : new ApiException((int)HttpStatusCode.InternalServerError);
    }
    private string PrepareJsonContent(ApiException exceptionResponse)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return JsonSerializer.Serialize(exceptionResponse, options);
    }
}

