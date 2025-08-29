using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
    }
}
