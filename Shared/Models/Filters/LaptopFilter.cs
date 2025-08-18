using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public class LaptopFilter : ProductFilter, ICpuFilter, IDisplayFilter, IGpuFilter, IRamFilter
    {
        public CpuBrand? CpuBrand { get; set; }
        public string? CpuModel { get; set; }
        public int? CpuNumberOfCores { get; set; }
        public double? ScreenSizeInches { get; set; }
        public string? Resolution { get; set; }
        public int? RefreshRateHz { get; set; }
        public string? GpuBrand { get; set; }
        public string? GpuModel { get; set; }
        public int? GpuVRAM { get; set; }
        public int? RamCapacity { get; set; }

    }
}
