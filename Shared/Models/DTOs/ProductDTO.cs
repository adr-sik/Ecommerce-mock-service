using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Shared.Models.DTOs.ProductTypesDTOs;

namespace Shared.Models.DTOs
{
    [JsonDerivedType(typeof(PhoneDTO), "phone")]
    [JsonDerivedType(typeof(LaptopDTO), "laptop")]
    [JsonDerivedType(typeof(HeadphonesDTO), "headphones")]
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int? Sale { get; set; }
        public ICollection<ImageDTO> Images { get; set; } = new List<ImageDTO>();

        public string GetPriceWithSale()
        {
            if (Sale.HasValue && Sale.Value > 0)
            {
                return (Price - (Price * Sale.Value / 100)).ToString("0.00");
            }
            return "No sale for this product";
        }
    }
}
