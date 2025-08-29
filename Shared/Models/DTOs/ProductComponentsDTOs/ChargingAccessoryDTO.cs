using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductComponentsDTOs
{
    public class ChargingAccessoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? BatteryCapacity { get; set; }
        public bool IsFastCharging { get; set; }
        public string Port { get; set; } = null!;

        public override string ToString()
        {
            string result = "";

            result += $"Port: {Port}";

            if (BatteryCapacity.HasValue)
            {
                result += $", Battery: {BatteryCapacity}mAh";
            }

            if (IsFastCharging)
            {
                result += ", Fast-Charging";
            }
            return result;
        }
    }
}
