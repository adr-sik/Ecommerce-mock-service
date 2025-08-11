using Shared.Models.Enums;
using Shared.Models.ProductComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.ProductTypes
{
    public class Headphones : Product
    {
        public HeadphoneType HeadphoneType { get; set; }
        public Wireless? Wireless { get; set; }       
    }
}
