using Shared.Models.Enums;
using Server.Models.ProductComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.ProductTypes
{
    public class Headphones : Product
    {
        public HeadphoneType HeadphoneType { get; set; }
        public int? BatteryLife { get; set; }
        public ChargingAccessory? ChargingAccessory { get; set; }
    }
}
