using Shared.Models.Enums;

namespace Client.Helpers
{
    public class PhoneFilterHelper : ICpuHelper
    {
        public List<CpuBrand> GetCpuBrands()
        {
            return new List<CpuBrand>() { CpuBrand.Apple, CpuBrand.Qualcomm, CpuBrand.MediaTek, CpuBrand.Google };
        }

        public List<double> GetScreenSizes() 
        {
            return new List<double>() { };
        }

        public List<string> GetColors()
        {
            return new List<string> { "Black", "White", "Red", "Blue" };
        }
    }
}
