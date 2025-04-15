using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics;

namespace AspNetWebApiWithDbContext.Controllers;

[ApiController]
public class BaseCrudController<TEntity,TListDto,TDetailsDto,TUpdateDto,TInsertDto> : BaseController<BaseCrudController<TEntity, TListDto, TDetailsDto, TUpdateDto, TInsertDto>>
    where TEntity : BaseEntity,new()
    where TListDto: IBaseDto<TEntity,TListDto>,new()
    where TDetailsDto: IBaseDto<TEntity,TDetailsDto>,new()
    where TUpdateDto: IBaseDto<TEntity,TUpdateDto>,new()
    where TInsertDto : IBaseDto<TEntity,TInsertDto>,new()
{
    protected readonly GenericRepository<TEntity> repository;

    public BaseCrudController(ILogger<BaseCrudController<TEntity, TListDto, TDetailsDto, TUpdateDto, TInsertDto>> logger,
        GenericRepository<TEntity> repository,IHttpContextAccessor contextAccessor) : base(logger,contextAccessor)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = repository.GetAll().Select(c=>new TListDto().GetDto(c));
        return HandleGetResponse(result);
    }


    [HttpGet("{id}",Name = "GetItem")]
    public async Task<IActionResult> GetItem(Guid id)
    {
        var result = await repository.GetDataQueryable(c => c.Id == id,
            c => new TDetailsDto().GetDto(c), null).FirstOrDefaultAsync();
        return HandleGetResponse(result);
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetPaged([FromRoute]int pageNumber, [FromRoute]int pageSize)
    {
        var result = repository.GetPagedDataQueryable(c=>true,c=>new TListDto().GetDto(c),c=>c.OrderBy(d=>d.Id),pageNumber,pageSize);
        return HandleGetResponse(result);
    }

    [HttpPost()]
    public async Task<IActionResult> Add([FromBody] TInsertDto dto)
    {
        var entity = dto.GetEntity();
        await repository.AddAsync(entity);
        await repository.SaveAsync();
        return HandleCreatedResponse(entity);
    }

    protected IActionResult HandleCreatedResponse(TEntity entity)
    {
        if (entity is null || entity.Id == Guid.Empty)
        {
            logger.LogError($"Insert entity {nameof(entity)} failed. CorrelationId: {correlationId}");
            return BadRequest($"Entity {nameof(entity)} Failed to add. CorrelationId: {correlationId}");
        }
        logger.LogInformation($"Successfully Create the resource. CorrelationId: {correlationId}");

        return CreatedAtRoute("GetItem", new { id = entity.Id }, new TDetailsDto().GetDto(entity));
    }

    protected IActionResult HandleGetResponse<TResponse>(TResponse result)
    {

        if (result == null)
        {
            logger.LogWarning($"response of type {typeof(TResponse)} not found. CorrelationId: {correlationId}");
            return NotFound(Result.Failure(Error.NotFoundError, correlationId));
        }

        if (typeof(TResponse) is ICollection)
        {
            var count = ((ICollection)result).Count;
            if (count == 0)
            {
                logger.LogWarning($"Collection of type {typeof(TResponse)} is empty. CorrelationId: {correlationId}");
                return NotFound(Result.Failure(Error.NotFoundError, correlationId));
            }
            HttpContext.Response.Headers.Append("ItemCount", count.ToString());
        }

        logger.LogInformation($"Successfully retrieved the resource. CorrelationId: {correlationId}");
        return Ok(Result.Success(result, correlationId));
    }

}
