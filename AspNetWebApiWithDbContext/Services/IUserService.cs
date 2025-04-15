using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;

namespace AspNetWebApiWithDbContext.Services;

public interface IUserService
{
    Task<List<User>> GetUsersInRole(string RoleName);
    Task<List<User>> GetUsers(int count, int page);

    Task<User> GetUser(Guid id);
}