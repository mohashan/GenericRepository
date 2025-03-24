using AspNetWebApiWithDbContext.Dtos;

namespace AspNetWebApiWithDbContext.Services;
public interface IUserService
{
    Task<List<UserListDto>> GetUsersInRole(string RoleName);
}