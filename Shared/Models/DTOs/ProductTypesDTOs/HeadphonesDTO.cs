using Shared.Models.Enums;
using Shared.Models.DTOs.ProductComponentsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductTypesDTOs
{
    public class HeadphonesDTO : ProductDTO
    {
        public HeadphoneType HeadphoneType { get; set; }
        public int BatteryLifeInHours { get; set; }
        public ChargingAccessoryDTO ChargingAccessory { get; set; }
    }
}
