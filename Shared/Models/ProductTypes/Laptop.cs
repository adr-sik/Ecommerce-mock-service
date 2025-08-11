using Shared.Models.ProductComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.ProductTypes
{
    public class Laptop : Product
    {
        public Display? Display { get; set; }
        public Cpu? Cpu { get; set; }
        public Gpu? Gpu { get; set; }
        public int Ram { get; set; }
    }
}
