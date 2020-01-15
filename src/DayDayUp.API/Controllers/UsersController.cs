using DayDayUp.AccountContext;
using DayDayUp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayDayUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private IUserService _userService;

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new {message = "Username or password is incorrect"});

            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            var users = _userService.GetUser(User.Identity.Name);
            return Ok(users);
        }
    }
}