using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public interface ICpuFilter
    {
        CpuBrand? CpuBrand { get; set; }
        string? CpuModel { get; set; }
        int? CpuNumberOfCores { get; set; }
    }
}
