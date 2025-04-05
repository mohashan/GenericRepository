using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApiWithDbContext.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserRole> userRoleRepo;
    private readonly IRepository<User> userRepo;

    public UserService(IRepository<UserRole> userRoleRepo, IRepository<User> userRepo)
    {

        this.userRoleRepo = userRoleRepo;
        this.userRepo = userRepo;
    }

    public async Task<List<UserListDto>> GetUsers(int count, int page)
    {
        var users = userRepo.GetPagedDataQueryable(c => true,
            c => new UserListDto
            {
                Username = c.Username,
                FirstName = c.FirstName,
                Id = c.Id,
                LastName = c.LastName,
            }, c => c.OrderBy(d => d.LastName), page, count);

        return await users.ToListAsync();
    }

    public async Task<List<UserListDto>> GetUsersInRole(string RoleName)
    {
        var users = userRoleRepo.GetDataQueryable(c => c.Role.Name == RoleName, c => new UserListDto
        {
            Username = c.User.Username,
            FirstName = c.User.FirstName,
            Id = c.User.Id,
            LastName = c.User.LastName,
        }, c => c.OrderBy(d => d.User.LastName), c => c.User, c => c.Role);

        return await users.ToListAsync();

    }
}
