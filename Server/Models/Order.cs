using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }
        public string? Comments { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
