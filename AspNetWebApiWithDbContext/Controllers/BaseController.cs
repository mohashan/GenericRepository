using System.Collections;
using AspNetWebApiWithDbContext.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
        where T : ControllerBase
    {
        protected readonly ILogger<T> logger;
        protected readonly string correlationId;

        protected BaseController(ILogger<T> logger, IHttpContextAccessor contextAccessor):base()
        {
            correlationId = contextAccessor.HttpContext?.Request.Headers["X-Correlation-ID"].ToString()??"Unknown CorrelationId";
            this.logger = logger ?? throw new ArgumentException(nameof(logger));
            logger.LogInformation($"New request received. TraceId: {correlationId}");
        }

        //public virtual async Task<IActionResult> Get(Guid id)
        //{
        //    return Ok();
        //}


        protected IActionResult HandleError(Exception ex)
        {
            logger.LogError(ex, $"An unexpected error occurred. TraceId: {correlationId}");
            return StatusCode(500, Result.Failure(Error.OperationError, correlationId));
        }
    }
}
