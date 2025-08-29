using Shared.Models.Enums;

namespace Client.Helpers
{
    public class LaptopFilterHelper : ICpuHelper
    {
        public List<CpuBrand> GetBrands() 
        {
            return new List<CpuBrand>() { CpuBrand.Intel, CpuBrand.AMD };
        }
    }
}
