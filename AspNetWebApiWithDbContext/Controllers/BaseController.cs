using System.Collections;
using AspNetWebApiWithDbContext.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
        where T : ControllerBase
    {
        private readonly ILogger<T> logger;
        private readonly string traceId;
        protected BaseController(ILogger<T> logger):base()
        {
            traceId = GetTraceId();
            this.logger = logger ?? throw new ArgumentException(nameof(logger));
            logger.LogInformation($"New request received. TraceId: {traceId}");
        }

        protected string GetTraceId()
        {
            return HttpContext?.Items["TraceId"]?.ToString() ?? "TraceId not available";
        }
        protected IActionResult HandleResponse<TResponse>(TResponse result)
        {

            if (result == null)
            {
                logger.LogWarning($"response of type {typeof(TResponse)} not found. TraceId: {traceId}");
                return NotFound(Result.Failure(Error.NotFoundError,traceId));
            }

            if (typeof(TResponse) is ICollection)
            {
                var count = ((ICollection)result).Count;
                if (count == 0)
                {
                    logger.LogWarning($"Collection of type {typeof(TResponse)} is empty. TraceId: {traceId}");
                    return NotFound(Result.Failure(Error.NotFoundError,traceId));
                }
                HttpContext.Response.Headers.Append("ItemCount",count.ToString());
            }
            logger.LogInformation($"Successfully retrieved the resource. TraceId: {traceId}");
            return Ok(Result.Success(result,traceId));
        }

        protected IActionResult HandleError(Exception ex)
        {
            logger.LogError(ex, $"An unexpected error occurred. TraceId: {traceId}");
            return StatusCode(500, Result.Failure(Error.OperationError, traceId));
        }
    }
}
