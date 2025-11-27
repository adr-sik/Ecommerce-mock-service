using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.Models.DTOs;

namespace Server.Services
{
    public class UsersService(EcommerceContext context) : IUsersService
    {
        public async Task<User?> RegisterAsync(UserDTO request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }

            var user = new User();

            user.Username = request.Username;
            GeneratePasswordHash(request.Password, user);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            if (user == null) return false;

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user {userId}: {ex.ToString()}");
                return false;
            }
        }

        public async Task<bool> ChangePassword(Guid userId, string newPassword)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            if (user == null) return false;
            
            try
            {
                GeneratePasswordHash(newPassword, user);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing password fot user {userId}: {ex.ToString()}");
                return false;
            }
        }

        private static void GeneratePasswordHash(string password, User user)
        {
            try
            {
                var hashedPassword = new PasswordHasher<User>()
                    .HashPassword(user, password);

                user.PasswordHash = hashedPassword;
            }
            catch
            {
                throw;
            }
        }
    }
}
