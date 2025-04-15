using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using AspNetWebApiWithDbContext.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiWithDbContext.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : BaseController<SampleController>
{

    private readonly IUserService userService;

    public SampleController(ILogger<SampleController> logger, IUserService userService,IHttpContextAccessor contextAccessor):base(logger,contextAccessor)
    {
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

    [HttpGet("GetUsers")]
    public async Task<ActionResult<IEnumerable<User>>> Get([FromQuery] int count, [FromQuery] int page)
    {
        var users = await userService.GetUsers(count,page);

        if (users is null)
        {
            return NotFound();
        }
        HttpContext.Response.Headers.Append("ItemCount", users.Count.ToString());
        return Ok(users);
    }

}
