using Microsoft.EntityFrameworkCore;
using Faker;
namespace AspNetWebApiWithDbContext.Domain;

public interface IDbSeeder
{
    public void Seed();
}

public class DbSeeder : IDbSeeder
{
    private readonly MyDbContext dbContext;

    public DbSeeder(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void Seed()
    {
        if (dbContext.Users.Any())
        {
            return;
        }
        var userRoles = new UserRole[50];
        var roles = new Role[3];

        roles[0] = new Role() { Id = Guid.NewGuid(), Name = "Admin" };
        roles[1] = new Role() { Id = Guid.NewGuid(), Name = "SuperUser" };
        roles[2] = new Role() { Id = Guid.NewGuid(), Name = "User" };


        for (int i = 0; i < 50; i++)
        {

            userRoles[i] = new UserRole
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Age = RandomNumber.Next(),
                    Email = Internet.Email(),
                    FirstName = Name.First(),
                    LastName = Name.Last(),
                    Password = RandomNumber.Next(1000001, 9999999).ToString(),
                    Username = Internet.UserName(),
                },
                Role = roles[Faker.RandomNumber.Next(0, 2)],
            };
        }
        dbContext.UserRoles.AddRange(userRoles);
        dbContext.SaveChanges();

    }
}
