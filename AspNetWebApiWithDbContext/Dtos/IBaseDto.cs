using AspNetWebApiWithDbContext.Domain;

namespace AspNetWebApiWithDbContext.Dtos;

public interface IBaseDto<T,TDto>
    where T : BaseEntity,new()
    where TDto : IBaseDto<T,TDto>,new()
{
    T GetEntity();

    TDto GetDto(T entity);
}
