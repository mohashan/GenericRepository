namespace AspNetWebApiWithDbContext.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers.Append("X-Correlation-ID", correlationId);
        }
        using (var logScope = context.RequestServices.GetRequiredService<ILogger<CorrelationIdMiddleware>>()
            .BeginScope(new Dictionary<string, object> { { "CorrelationId", correlationId } }))
        {
            context.Response.Headers.Append("X-Correlation-ID", correlationId);
            await _next(context);
        }
    }
}

