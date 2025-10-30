using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models.DTOs;
using System.Security.Claims;

namespace Client.Authorization
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public JwtAuthenticationStateProvider(AuthService authService)
        {
            _authService = authService;
        }

        public string DisplayName => _currentUser.Identity?.Name;
        public bool IsLoggedIn => _currentUser.Identity?.IsAuthenticated ?? false;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public async Task<bool> CheckAuthenticationAsync()
        {
            try
            {
                var userClaims = await _authService.GetUserInfoAsync();

                if (userClaims != null)
                {
                    Login(userClaims);
                    return true;
                }
            }
            catch (HttpRequestException)
            {

            }
            Logout();
            return false;
        }

        public void Login(UserClaimsDTO userClaims)
        {
            var claims = userClaims.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            var identity = new ClaimsIdentity(claims, "apiauth");
            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public void Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}
