using Shared.Models.Enums;

namespace Client.Helpers
{
    public class PhoneFilterHelper : ICpuHelper
    {
        public List<CpuBrand> GetBrands()
        {
            return new List<CpuBrand>() { CpuBrand.Apple, CpuBrand.Qualcomm, CpuBrand.MediaTek, CpuBrand.Google };
        }
    }
}
