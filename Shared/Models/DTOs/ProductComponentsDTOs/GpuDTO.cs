using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductComponentsDTOs
{
    public class GpuDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int VRAM { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} {VRAM} GB";
        }
    }
}
