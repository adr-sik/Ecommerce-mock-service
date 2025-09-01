using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;

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
        public async Task<ActionResult<HeadphonesDTO>> GetAsync(int id)
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
        public async Task<ActionResult<IEnumerable<HeadphonesDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, [FromQuery] HeadphonesFilter? filter = null)
        {
            IQueryable<Headphones> query = _context.Headphones;

            if (filter != null)
            {
                query = ApplyFilter(query, filter);
            }

            var headphones = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(h => h.Images)
                .ToListAsync();
            var headphonesDto = _mapper.Map<List<HeadphonesDTO>>(headphones);

            return Ok(headphonesDto);
        }

        // helper method to apply filters
        private IQueryable<Headphones> ApplyFilter(IQueryable<Headphones> query, HeadphonesFilter filter)
        {
            query = query.Where(h =>
                (string.IsNullOrEmpty(filter.Brand) || h.Brand == filter.Brand) &&
                (string.IsNullOrEmpty(filter.Model) || h.Model.Contains(filter.Model)) &&
                (!filter.MinPrice.HasValue || h.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || h.Price <= filter.MaxPrice.Value) &&
                (!filter.OnSale.HasValue || filter.OnSale == false || h.Sale.HasValue) &&
                (!filter.HeadphoneType.HasValue || h.HeadphoneType == filter.HeadphoneType.Value)
                );
            return query;
        }
    }
}
