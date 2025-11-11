using Shared.Models.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Authorization
{
    public partial class AuthService
    {
        private const string Endpoint = "api/Auth";
        public static string Username = String.Empty;

        private readonly HttpClient _httpClient;
        private readonly HttpClient _refreshClient;

        public AuthService(IHttpClientFactory factory) 
        {
            _httpClient = factory.CreateClient("WebAPI");
            _refreshClient = factory.CreateClient("RefreshClient"); 
        }

        public async Task<UserClaimsDTO?> LoginAsync(UserDTO user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", user);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserClaimsDTO>();
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync($"{Endpoint}/logout", null);
        }

        public async Task<bool> TestAuth()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> TestAdmin()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/admin-only");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<UserClaimsDTO> GetUserInfoAsync()
        {
            Console.WriteLine("Getting user info...");
            var response = await _httpClient.GetAsync($"{Endpoint}/userinfo");
            Console.WriteLine($"Success! User: {response?.ToString()}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserClaimsDTO>();
            }

            return null;
        }

        public async Task<bool> RefreshCookies()
        {
            Console.WriteLine($"Attempting token refresh at {DateTime.Now}");
            try
            {
                var response = await _refreshClient.PostAsync($"{Endpoint}/refresh-token", null);
                Console.WriteLine($"Refresh response: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh failed: {ex.Message}");
                return false;
            }
        }
    }
}
