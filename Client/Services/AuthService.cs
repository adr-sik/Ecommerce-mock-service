using Shared.Models.DTOs;
using System.Text.Json;

namespace Client.Services
{
    public class AuthService<T> : ServiceBase<T>, IAuthService where T : UserDTO
    {
        protected override string Endpoint => $"api/auth";
        public AuthService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : base(httpClient, jsonOptions) { }

        public async Task<UserDTO?> LoginAsync(UserDTO user)
        {
            var response = await _http.PostAsJsonAsync($"{Endpoint}/login", user);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDTO>(_jsonOptions);
            }
            return null;
        }
    }
}
