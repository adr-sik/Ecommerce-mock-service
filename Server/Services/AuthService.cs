using Azure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;
using Shared.Models;
using Shared.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly EcommerceContext _dbContext;

        private static readonly int AccessTokenExpiryTime = 15; // minutes
        private static readonly int RefreshTokenExpiryTime = 7; // days

        public AuthService(EcommerceContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
            
        private static CookieOptions GetBaseAccessCookieOptions() => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/Auth"
        };

        private static CookieOptions GetBaseRefreshCookieOptions() => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/Auth/refresh-token"
        };

        public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<TokenResponseDTO?> RefreshTokensAsync(string refreshToken)
        {
            var user = await ValidateRefreshTokenAsync(refreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDTO> CreateTokenResponse(User? user)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            return new TokenResponseDTO
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user is null)
            {
                return null;
            }

            if (user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                user.RefreshToken = null;
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error revoking expired refresh token in DB: {ex.Message}");
                }
                return null;
            }

            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(RefreshTokenExpiryTime); // test
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }

        public Guid GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdClaim?.Value, out var userId))
            {
                return userId;
            }

            return Guid.Empty;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(AccessTokenExpiryTime),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public void SetTokensInsideCookie(TokenResponseDTO tokens)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var accessOptions = GetBaseAccessCookieOptions();
            accessOptions.Expires = DateTimeOffset.Now.AddMinutes(AccessTokenExpiryTime);
            httpContext.Response.Cookies.Append("AccessToken", tokens.AccessToken, accessOptions);

            var refreshOptions = GetBaseRefreshCookieOptions();
            refreshOptions.Expires = DateTimeOffset.Now.AddDays(RefreshTokenExpiryTime);
            httpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, refreshOptions);
        }
        public void DeleteCookies()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var accessDeleteOptions = GetBaseAccessCookieOptions();
            httpContext.Response.Cookies.Delete("AccessToken", accessDeleteOptions);

            var refreshDeleteOptions = GetBaseRefreshCookieOptions();
            httpContext.Response.Cookies.Delete("RefreshToken", refreshDeleteOptions);
        }
    }
}