using Server.Models;
using Shared.Models.DTOs;

namespace Server.Services
{
    public interface IUsersService
    {
        Task<User?> RegisterAsync(UserDTO request);
        Task<bool> DeleteUser(Guid userId, string password);
        Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword);
    }
}
