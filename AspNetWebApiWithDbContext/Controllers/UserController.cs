using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using AspNetWebApiWithDbContext.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseCrudController<User,UserListDto,UserDetailsDto,UserUpdateDto,UserInsertDto>
{
    public UserController(ILogger<UserController> logger, GenericRepository<User> userRepo):base(logger,userRepo)
    {
    }
}
