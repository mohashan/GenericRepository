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

    public Task<User> GetUser(Guid id)
    {
        return userRepo.GetDataQueryable(c=>c.Id == id,c=>c).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsers(int count, int page)
    {
        var users = userRepo.GetPagedDataQueryable(c => true,
            c => c, c => c.OrderBy(d => d.LastName), page, count);

        return await users.ToListAsync();
    }

    public async Task<List<User>> GetUsersInRole(string RoleName)
    {
        var users = userRoleRepo.GetDataQueryable(c => c.Role.Name == RoleName, c => c.User, c => c.OrderBy(d => d.User.LastName), c => c.User, c => c.Role);

        return await users.ToListAsync();

    }
}
