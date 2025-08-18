using Shared.Models.DTOs.ProductComponentsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductTypesDTOs
{
    public class PhoneDTO : ProductDTO
    {
        public string? Color { get; set; }
        public DisplayDTO? Display { get; set; }
        public CameraDTO? Camera { get; set; }
        public CpuDTO? Cpu { get; set; }
    }
}
