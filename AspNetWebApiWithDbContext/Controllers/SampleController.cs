using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{

    private readonly ILogger<SampleController> _logger;
    private readonly IRepository repository;

    public SampleController(ILogger<SampleController> logger,IRepository repository)
    {
        _logger = logger;
        this.repository = repository;
    }

    [HttpGet("GetByRole/{roleName}")]
    public async Task<ActionResult<IEnumerable<User>>> Get([FromRoute]string roleName)
    {
        var users = await repository.GetDataAsync<UserRole, UserListDto>
            (filter: c => c.Role.Name == roleName,
            selector: c => new UserListDto{
                Age = c.User.Age,
                FirstName = c.User.FirstName,
                Email = c.User.Email,
                Id = c.User.Id,
                LastName = c.User.LastName,
                RoleName = c.Role.Name,
            },
            orderBy: c => c.OrderBy(d => d.User.FirstName),
            c => c.User,c=>c.Role);

        if(users is null)
        {
            return NotFound();
        }
        HttpContext.Response.Headers.Append("ItemCount",users.Count.ToString());
        return Ok(users);
    }
}
