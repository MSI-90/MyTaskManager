using Microsoft.AspNetCore.Mvc;
using MyTaskManager.Services.Interfaces;

namespace MyTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IResult> Register()
        {
            return Results.Ok();
        }

        [HttpPost("login")]
        public async Task<IResult> Login()
        {
            return Results.Ok();
        }
    }
}
