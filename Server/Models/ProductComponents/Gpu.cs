using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.ProductComponents
{
    public class Gpu
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;
        [Required]
        public int VRAM { get; set; }
    }
}
