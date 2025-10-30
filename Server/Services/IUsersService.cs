using Server.Models;
using Shared.Models.DTOs;

namespace Server.Services
{
    public interface IUsersService
    {
        Task<User?> RegisterAsync(UserDTO request);
    }
}
