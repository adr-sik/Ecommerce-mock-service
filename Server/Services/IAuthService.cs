using Server.Models;
using Shared.Models;
using Shared.Models.DTOs;
using System.Security.Claims;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO request);
        Task<TokenResponseDTO?> LoginAsync(UserDTO request);
        Task<TokenResponseDTO?> RefreshTokensAsync(Guid userId, string refreshToken);
        void SetTokensInsideCookie(TokenResponseDTO tokens, HttpContext context);
        Guid GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
