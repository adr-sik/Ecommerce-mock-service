using Shared.Models.ProductComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.ProductTypes
{
    public class Phone : Product
    {
        public string? Color { get; set; }
        public Display? Display { get; set; }
        public Camera? Camera { get; set; }
        public Cpu? Cpu { get; set; }
    }
}
