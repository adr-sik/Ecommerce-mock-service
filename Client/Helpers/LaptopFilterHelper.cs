using Shared.Models.Enums;

namespace Client.Helpers
{
    public class LaptopFilterHelper : ICpuHelper
    {
        public List<CpuBrand> GetCpuBrands() 
        {
            return new List<CpuBrand>() { CpuBrand.Intel, CpuBrand.AMD };
        }
        public List<string> GetGpuBrands()
        {
            return new List<string>() { "Nvidia", "AMD", "Radeon" };
        }
        public List<int> GetRamRanges()
        {
            return new List<int>() { 8, 16, 32 };
        }
    }
}
