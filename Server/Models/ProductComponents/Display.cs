using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.ProductComponents
{
    public class Display
    {
        public int Id { get; set; }
        [Required]
        public double ScreenSizeInches { get; set; }
        [Required]
        public string Resolution { get; set; } = null!;
        public int? RefreshRateHz { get; set; }
    }
}
