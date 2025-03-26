namespace AspNetWebApiWithDbContext.Middlewares
{
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;
        public TraceIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items["TraceId"] = context.TraceIdentifier;
            await _next(context);
        }
    }
}
