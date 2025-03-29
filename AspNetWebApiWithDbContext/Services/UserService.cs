using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApiWithDbContext.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserRole> userRoleRepo;

    public UserService(IRepository<UserRole> userRoleRepo)
    {
        this.userRoleRepo = userRoleRepo;
    }
    public async Task<List<UserListDto>> GetUsersInRole(string RoleName)
    {
        var users = userRoleRepo.GetDataQueryable(c => c.Role.Name == RoleName, c => new UserListDto
        {
            Username= c.User.Username,
            FirstName = c.User.FirstName,
            Id = c.User.Id,
            LastName = c.User.LastName,
        }, c => c.OrderBy(d => d.User.LastName), c => c.User, c => c.Role);

        return await users.ToListAsync();

    }
}
