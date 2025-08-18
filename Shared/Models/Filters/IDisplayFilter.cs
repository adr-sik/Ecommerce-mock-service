using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public interface IDisplayFilter
    {
        public double? ScreenSizeInches { get; set; }
        public string? Resolution { get; set; } 
        public int? RefreshRateHz { get; set; }
    }
}
