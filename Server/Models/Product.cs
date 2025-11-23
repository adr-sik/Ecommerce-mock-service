using Server.Models.ProductTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Models
{
    // TODO: Add JsonDerivedType attributes when using System.Text.Json for polymorphic serialization
    //[JsonDerivedType(typeof(Phone), "phone")]
    //[JsonDerivedType(typeof(Laptop), "laptop")]
    //[JsonDerivedType(typeof(Headphones), "headphones")]
    public abstract class Product
    {
        public int Id { get; set; }
        [Required] 
        public string Brand { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock {  get; set; }
        public int? Sale { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public ICollection<Image> Images { get; set; } = new List<Image>();
        [Timestamp]
        public byte[] Version { get; set; }
    }
}
