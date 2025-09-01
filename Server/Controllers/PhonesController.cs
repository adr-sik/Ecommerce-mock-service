using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;

namespace Server.Controllers
{
    [Route("api/products/phones")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IMapper _mapper;

        public PhonesController(EcommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/products/phones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneDTO>> GetPhone(int id)
        {
            var phone = await _context.Phones
                .Include(l => l.Cpu)
                .Include(l => l.Camera)
                .Include(l => l.Display)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (phone == null)
            {
                return NotFound();
            }

            var phoneDto = _mapper.Map<PhoneDTO>(phone);
            return Ok(phoneDto);
        }

        // GET: api/products/phones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneDTO>>> GetPhones(int pageNumber = 1, int pageSize = 20, [FromQuery] PhoneFilter? filter = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (filter != null)
            {
                query = ApplyFilter(query, filter);
            }

            var phones = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(l => l.Images)
                .ToListAsync();
            var phonesDto = _mapper.Map<List<PhoneDTO>>(phones);

            return Ok(phonesDto);
        }

        // helper method to apply filters
        private IQueryable<Phone> ApplyFilter(IQueryable<Phone> query, PhoneFilter filter)
        {
            query = query.Where(p =>
                (string.IsNullOrEmpty(filter.Brand) || p.Brand == filter.Brand) &&
                (string.IsNullOrEmpty(filter.Model) || p.Model.Contains(filter.Model)) &&
                (!filter.MinPrice.HasValue || p.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || p.Price <= filter.MaxPrice.Value) &&
                (!filter.OnSale.HasValue || filter.OnSale == false || p.Sale.HasValue) &&
                (!filter.CpuBrand.HasValue || p.Cpu.Brand == filter.CpuBrand.Value) &&
                (string.IsNullOrEmpty(filter.Color) || p.Color == filter.Color)
            );
            return query;
        }
    }
}
