using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using AspNetWebApiWithDbContext.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{

    private readonly ILogger<SampleController> _logger;
    private readonly IUserService userService;

    public SampleController(ILogger<SampleController> logger, IUserService userService)
    {
        _logger = logger;
        this.userService = userService;
    }

    [HttpGet("GetByRole/{roleName}")]
    public async Task<ActionResult<IEnumerable<User>>> Get([FromRoute]string roleName)
    {
        var users = await userService.GetUsersInRole(roleName);

        if(users is null)
        {
            return NotFound();
        }
        HttpContext.Response.Headers.Append("ItemCount",users.Count.ToString());
        return Ok(users);
    }
}
