using AspNetWebApiWithDbContext.Dtos;

namespace AspNetWebApiWithDbContext.Services;
public interface IUserService
{
    Task<List<UserListDto>> GetUsersInRole(string RoleName);
    Task<List<UserListDto>> GetUsers(int count, int page);
}