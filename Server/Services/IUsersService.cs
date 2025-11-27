using Server.Models;
using Shared.Models.DTOs;

namespace Server.Services
{
    public interface IUsersService
    {
        Task<User?> RegisterAsync(UserDTO request);
        Task<bool> DeleteUser(Guid userId);
        Task<bool> ChangePassword(Guid userId, string newPassword);
    }
}
