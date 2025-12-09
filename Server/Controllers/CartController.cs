using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("load-items")]
        public async Task<ActionResult<ProductDTO>> GetCartItems([FromQuery] List<int> productIds)
        {
            var products = await _cartService.LoadCartItemsAsync(productIds);
            if (products == null || !products.Any()) 
                return BadRequest();

            return Ok(_mapper.Map<List<ProductDTO>>(products));
        }
    }
}
