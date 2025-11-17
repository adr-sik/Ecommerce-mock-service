using Azure.Core;
using Microsoft.AspNetCore.Antiforgery;
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
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public static User user = new();

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserDTO request)
        {
            var tokenResponse = await _authService.LoginAsync(request);
            if (tokenResponse == null)
                return BadRequest("Invalid username or password.");

            _authService.SetTokensInsideCookie(tokenResponse);
            var principal = JwtSerialize.Deserialize(tokenResponse.AccessToken!);

            return Ok(new
            {
                username = principal.Identity.Name,
                claims = principal.Claims.Select(c => new { type = c.Type, value = c.Value }).ToList()
            });
        }

        [HttpPost("refresh-token")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["RefreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Unauthorized();

                var tokenResponse = await _authService.RefreshTokensAsync(refreshToken);

                if (tokenResponse is null)
                    return Unauthorized();

                _authService.SetTokensInsideCookie(tokenResponse);
                Console.WriteLine($"Tokens refreshed and set in cookies. {tokenResponse.RefreshToken} \n {tokenResponse.AccessToken}");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _authService.DeleteCookies();

            return Ok();
        }
    }
}
