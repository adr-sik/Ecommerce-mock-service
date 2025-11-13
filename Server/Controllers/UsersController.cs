using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using Shared.Models.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUsersService usersService) : Controller
    {
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var user = await usersService.RegisterAsync(request);
            if (user == null)
                return BadRequest("User already exists.");

            return Ok(user);
        }
    }
}
