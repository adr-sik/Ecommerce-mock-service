using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models.DTOs.ProductTypesDTOs;

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
        public async Task<ActionResult<IEnumerable<PhoneDTO>>> GetPhones(int pageNumber = 1, int pageSize = 20)
        {
            IQueryable<Phone> query = _context.Phones;

            var phones = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(l => l.Cpu)
                .Include(l => l.Camera)
                .Include(l => l.Display)
                .Include(l => l.Images)
                .ToListAsync();
            var phonesDto = _mapper.Map<List<PhoneDTO>>(phones);

            return Ok(phonesDto);
        }
    }
}
