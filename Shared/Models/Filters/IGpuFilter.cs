using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public interface IGpuFilter
    {
        public string? GpuBrand { get; set; }
        public string? GpuModel { get; set; }
        public int? GpuVRAM { get; set; }
    }
}
