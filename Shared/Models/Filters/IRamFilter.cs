using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public interface IRamFilter
    {
        public int? RamCapacity { get; set; }
    }
}
