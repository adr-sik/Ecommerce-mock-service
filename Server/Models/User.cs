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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Surname { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public ICollection<User> Referred { get; set; } = new List<User>();
        public int? ReferralId { get; set; }
        public User? Referral { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
