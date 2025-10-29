using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using Shared.Models;
using Shared.Models.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : Controller
    {
        public static User user = new();

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
                return BadRequest("User already exists.");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login(UserDTO request)
        {
            var result = await authService.LoginAsync(request);
            if (result == null)
                return BadRequest("Invalid username or password.");

            authService.SetTokensInsideCookie(result, HttpContext);

            return Ok("Login successful");
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token not found.");

            var userId = authService.GetUserIdFromClaims(User);
            if (userId == Guid.Empty)
                return BadRequest("Invalid or missing user.");

            var result = await authService.RefreshTokensAsync(userId, refreshToken);
            if (result == null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Session expired, please log in again.");

            authService.SetTokensInsideCookie(result, HttpContext);

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are an admin!");
        }
    }
}
