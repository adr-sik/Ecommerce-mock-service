using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Models.DTOs;
using System.Security.Claims;

namespace Client.Authorization
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;
        public ClaimsPrincipal _currentUser;
        public JwtAuthenticationStateProvider(AuthService authService)
        {
            _authService = authService;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(GetState());
        }

        public async Task<bool> CheckAuthenticationAsync()
        {
            var userClaims = await _authService.GetUserInfoAsync();

            if (userClaims != null)
            {
                Login(userClaims);
                return true;
            }

            LocalLogout();
            return false;
        }

        private AuthenticationState GetState()
        {
            if (this._currentUser != null)
            {
                return new AuthenticationState(this._currentUser);
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public void Login(UserClaimsDTO userClaims)
        {
            var claims = userClaims.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            var identity = new ClaimsIdentity(claims, "apiauth");
            this._currentUser = new ClaimsPrincipal(identity);

            this.NotifyAuthenticationStateChanged(Task.FromResult(GetState()));
        }

        public async Task Logout()
        {
            await _authService.LogoutAsync();

            this._currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            this.NotifyAuthenticationStateChanged(Task.FromResult(GetState()));
        }

        public void LocalLogout()
        {
            this._currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            this.NotifyAuthenticationStateChanged(Task.FromResult(GetState()));
        }

        public Guid GetUserIdAsync()
        {
            var userIdClaim = this._currentUser.FindFirst(ClaimTypes.NameIdentifier);

            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
        }
    }  
}
