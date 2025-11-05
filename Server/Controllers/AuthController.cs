using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using Server.Util;
using Shared.Models;
using Shared.Models.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : Controller
    {
        public static User user = new();

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserDTO request)
        {
            var tokenResponse = await authService.LoginAsync(request);
            if (tokenResponse == null)
                return BadRequest("Invalid username or password.");

            authService.SetTokensInsideCookie(tokenResponse, HttpContext);
            var principal = JwtSerialize.Deserialize(tokenResponse.AccessToken!);

            return Ok(new
            {
                username = principal.Identity.Name,
                claims = principal.Claims.Select(c => new { type = c.Type, value = c.Value }).ToList()
            });
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

            var tokenResponse = await authService.RefreshTokensAsync(userId, refreshToken);
            if (tokenResponse == null || tokenResponse.AccessToken is null || tokenResponse.RefreshToken is null)
                return Unauthorized("Session expired, please log in again.");

            authService.SetTokensInsideCookie(tokenResponse, HttpContext);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            string acv = Request.Cookies["AccessToken"] ?? "no token";
            Console.WriteLine("User is authenticated.");
            Console.WriteLine($"Username: {User.Identity?.Name}");
            if (string.IsNullOrEmpty(acv))  
            {
                Console.WriteLine("no token");
            }
            else
            {
                var expiryClaim = User.Claims.FirstOrDefault(c => c.Type == "exp");
                if (expiryClaim != null && long.TryParse(expiryClaim.Value, out long expValue))
                {
                    var expiryDateTime = DateTimeOffset.FromUnixTimeSeconds(expValue);
                    Console.WriteLine($"Token Expiry Time: {expiryDateTime.LocalDateTime} and time now: {DateTime.Now}");
                }
                Console.WriteLine($"AccessToken value: { acv }");
            }
                
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are an admin!");
        }

        [HttpGet("userinfo")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new UserClaimsDTO
                {
                    Username = User.Identity.Name,
                    Claims = User.Claims.Select(c => new ClaimDTO
                    {
                        Type = c.Type,
                        Value = c.Value
                    }).ToList()
                });
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            authService.DeleteCookies(HttpContext);

            return Ok();
        }
    }
}
