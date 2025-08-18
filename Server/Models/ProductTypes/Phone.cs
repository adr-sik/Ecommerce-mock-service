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
    public class Phone : Computer
    {
        public string? Color { get; set; }
        public virtual Camera? Camera { get; set; } 
    }
}
