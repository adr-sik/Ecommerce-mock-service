using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductComponentsDTOs
{
    public class CpuDTO
    {
        public int Id { get; set; }
        public CpuBrand Brand { get; set; }
        public string Model { get; set; } = null!;
        public int NumberOfCores { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} ({NumberOfCores} cores)";
        }
    }
}
