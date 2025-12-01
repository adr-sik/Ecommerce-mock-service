using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;
using Server.Util;
using Shared.Models.DTOs;
using System.Security.Claims;

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

        [HttpDelete("{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string password)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (id == null) return BadRequest();

                var result = await usersService.DeleteUser(Guid.Parse(id), password);
                if (result == false) return BadRequest("Deletion failed"); else return Ok("User deleted");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("change-password")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(id == null) return BadRequest();

                var result = await usersService.ChangePassword(Guid.Parse(id), oldPassword, newPassword);
                if (result == false) return BadRequest("Password change failed"); else return Ok("Password changed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
