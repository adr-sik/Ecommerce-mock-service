using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await usersService.DeleteUser(id);
                if (result == false) return BadRequest("Deletion failed"); else return Ok("User deleted");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, string newPassword)
        {
            try
            {
                var result = await usersService.ChangePassword(id, newPassword);
                if (result == false) return BadRequest("Password change failed"); else return Ok("Password changed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
