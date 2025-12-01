using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.Models.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IMapper _mapper;

        public CartController(EcommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("load-items")]
        public async Task<ActionResult<ProductDTO>> GetAllByIdAsync([FromQuery] List<int> productIds)
        {
            IQueryable<Product> query = _context.Set<Product>();

            if (productIds == null)
                return BadRequest();

            var products = await query
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            return Ok(_mapper.Map<List<ProductDTO>>(products));
        }
    }
}
