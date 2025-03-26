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
        var users = userRoleRepo.GetDataAsync(c => c.Role.Name == RoleName, c => new UserListDto
        {
            RoleName = c.Role.Name,
            Age = c.User.Age,
            Email = c.User.Email,
            FirstName = c.User.FirstName,
            Id = c.User.Id,
            LastName = c.User.LastName,
        }, c => c.OrderBy(d => d.User.LastName), c => c.User, c => c.Role);

        return await users.ToListAsync();
        
    }
}
