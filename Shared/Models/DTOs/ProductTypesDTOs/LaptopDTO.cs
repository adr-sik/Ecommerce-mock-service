using Shared.Models.DTOs.ProductComponentsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductTypesDTOs
{
    public class LaptopDTO : ProductDTO
    {        
        public CpuDTO? Cpu { get; set; }
        public GpuDTO? Gpu { get; set; }
        public RamDTO? Ram { get; set; }
        public DisplayDTO? Display { get; set; }
    }
}
