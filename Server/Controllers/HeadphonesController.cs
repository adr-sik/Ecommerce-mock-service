using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models.DTOs.ProductTypesDTOs;

namespace Server.Controllers
{
    [Route("api/products/headphones")]
    [ApiController]
    public class HeadphonesController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IMapper _mapper;

        public HeadphonesController(EcommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/products/headphones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HeadphonesDTO>> GetHeadphones(int id)
        {
            var headphones = await _context.Headphones
                .Include(h => h.ChargingAccessory)
                .Include(h => h.Images)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (headphones == null)
            {
                return NotFound();
            }

            var headphonesDto = _mapper.Map<HeadphonesDTO>(headphones);
            return Ok(headphonesDto);
        }

        // GET: api/products/headphones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeadphonesDTO>>> GetProducts(int pageNumber = 1, int pageSize = 20)
        {
            IQueryable<Headphones> query = _context.Headphones;

            var headphones = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(h => h.ChargingAccessory)
                .Include(h => h.Images)
                .ToListAsync();
            var headphonesDto = _mapper.Map<List<HeadphonesDTO>>(headphones);

            return Ok(headphonesDto);
        }
    }
}
