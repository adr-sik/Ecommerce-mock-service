namespace Server.Models.ProductComponents
{
    public class ChargingAccessory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? BatteryCapacity { get; set; }
        public bool IsFastCharging { get; set; }
        public string Port { get; set; } = null!;
    }
}
