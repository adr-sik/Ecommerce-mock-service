using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public ICollection<User> Referred { get; set; } = new List<User>();
        public Guid? ReferralId { get; set; }
        public User? Referral { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public string Role { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        // TODO: Change Role to enum
    }
}
