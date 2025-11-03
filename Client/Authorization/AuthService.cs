using Shared.Models.DTOs;

namespace Client.Authorization
{
    public partial class AuthService
    {
        private readonly HttpClient _http;
        private const string Endpoint = "api/Auth";
        public static string Username = String.Empty;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<UserClaimsDTO?> LoginAsync(UserDTO user)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", user);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserClaimsDTO>();
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            await _http.PostAsync("api/auth/logout", null);
        }

        public async Task<bool> TestAuth()
        {
            var response = await _http.GetAsync($"{Endpoint}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> TestAdmin()
        {
            var response = await _http.GetAsync($"{Endpoint}/admin-only");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<UserClaimsDTO> GetUserInfoAsync()
        {
            var response = await _http.GetAsync("api/auth/userinfo");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserClaimsDTO>();
            }

            return null;
        }
    }
}
