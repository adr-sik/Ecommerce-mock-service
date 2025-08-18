using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.ProductComponents
{
    public class Cpu
    {
        public int Id { get; set; }
        [Required]
        public CpuBrand Brand { get; set; }

        [Required]
        public string Model { get; set; } = null!;
        [Required]
        public int NumberOfCores { get; set; }
    }
}
