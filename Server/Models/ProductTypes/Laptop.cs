using Server.Models.ProductComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.ProductTypes
{
    public class Laptop : Computer
    {
        public virtual Gpu? Gpu { get; set; }
        public Ram Ram { get; set; }
    }
}
