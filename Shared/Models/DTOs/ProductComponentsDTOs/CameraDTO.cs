using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs.ProductComponentsDTOs
{
    public class CameraDTO
    {
        public int Id { get; set; }
        public int Megapixels { get; set; }

        public override string ToString()
        {
            return $"{Megapixels} MP";
        }
    }
}
