using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;
using Shared.Models;
using Shared.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
    public class AuthService(EcommerceContext context, IConfiguration configuration) : IAuthService
    {
        private static readonly int AccessTokenExpiryTime = 15; // minutes
        private static readonly int RefreshTokenExpiryTime = 7; // days

        private static readonly CookieOptions AccessCookieOptions  = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/Auth",
            Expires = DateTimeOffset.Now.AddMinutes(AccessTokenExpiryTime)
        };

        private static readonly CookieOptions RefreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/Auth/refresh-token",
            Expires = DateTimeOffset.Now.AddDays(RefreshTokenExpiryTime)
        };

        public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
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

        public async Task<TokenResponseDTO?> RefreshTokensAsync(Guid userId, string refreshToken)
        {
            var user = await ValidateRefreshTokenAsync(userId, refreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDTO> CreateTokenResponse(User? user)
        {
            return new TokenResponseDTO
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
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
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(RefreshTokenExpiryTime);
            await context.SaveChangesAsync();
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
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(AccessTokenExpiryTime),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public void SetTokensInsideCookie(TokenResponseDTO tokens, HttpContext context)
        {
            context.Response.Cookies.Append("AccessToken", tokens.AccessToken, AccessCookieOptions);
            context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, RefreshCookieOptions);
        }
        public void DeleteCookies(HttpContext context)
        {
            context.Response.Cookies.Delete("AccessToken", AccessCookieOptions);
            context.Response.Cookies.Delete("RefreshToken", RefreshCookieOptions);
        }
    }
}