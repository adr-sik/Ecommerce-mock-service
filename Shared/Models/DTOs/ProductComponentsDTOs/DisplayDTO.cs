using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductComponentsDTOs
{
    public class DisplayDTO
    {
        public int Id { get; set; }
        public double ScreenSizeInches { get; set; }
        public string Resolution { get; set; } = null!;
        public int? RefreshRateHz { get; set; }

        public override string ToString()
        {
            return $"{ScreenSizeInches}\" {Resolution} {RefreshRateHz}Hz";
        }
    }
}
