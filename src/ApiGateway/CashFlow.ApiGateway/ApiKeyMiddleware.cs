namespace CashFlow.ApiGateway;

public sealed class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await next(context);
            return;
        }

        var expected = configuration["Gateway:ApiKey"];
        if (string.IsNullOrWhiteSpace(expected) ||
            !context.Request.Headers.TryGetValue("X-API-Key", out var provided) || provided != expected)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or missing API key.");
            return;
        }

        await next(context);
    }
}