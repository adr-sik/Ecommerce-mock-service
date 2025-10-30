using Shared.Models.DTOs;
using System.Text.Json;

namespace Client.Services
{
    public class UserService : ServiceBase<UserDTO>
    {
        protected override string Endpoint => "api/users";
        public UserService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : base(httpClient, jsonOptions) { }

        // Register a new user
        public async Task<bool> RegisterAsync(UserDTO user)
        {
            var response = await _http.PostAsJsonAsync($"{Endpoint}/register", user);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        // Utilizing methods from base class
    }
}
