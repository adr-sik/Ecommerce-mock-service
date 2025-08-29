using Server.Models.ProductComponents;

namespace Server.Models.ProductTypes
{
    public abstract class Computer : Product
    {
        public Cpu? Cpu { get; set; }
        public Display? Display { get; set; }
    }
}
