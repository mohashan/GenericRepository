using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers;

[ApiController]
public class BaseCrudController<TEntity,TListDto,TDetailsDto,TUpdateDto,TInsertDto> : BaseController<BaseCrudController<TEntity, TListDto, TDetailsDto, TUpdateDto, TInsertDto>>
    where TEntity : BaseEntity,new()
    where TListDto: IBaseDto<TEntity,TListDto>,new()
    where TDetailsDto: IBaseDto<TEntity,TDetailsDto>,new()
    where TUpdateDto: IBaseDto<TEntity,TUpdateDto>,new()
    where TInsertDto : IBaseDto<TEntity,TInsertDto>,new()
{
    private readonly GenericRepository<TEntity> repository;

    public BaseCrudController(ILogger<BaseCrudController<TEntity, TListDto, TDetailsDto, TUpdateDto, TInsertDto>> logger,
        GenericRepository<TEntity> repository) : base(logger)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = repository.GetAll().Select(c=>new TListDto().GetDto(c));
        return HandleResponse(result);
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<IActionResult> Get(int pageNumber, int pageSize)
    {
        var result = repository.GetPagedDataQueryable(c=>true,c=>new TListDto().GetDto(c),c=>c.OrderBy(d=>d.Id),pageNumber,pageSize);
        return HandleResponse(result);
    }
}
