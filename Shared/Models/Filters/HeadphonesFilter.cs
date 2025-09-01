using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public class HeadphonesFilter : ProductFilter
    {
        public HeadphoneType? HeadphoneType { get; set; }
    }
}
