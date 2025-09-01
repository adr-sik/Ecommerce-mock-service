using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;

namespace Server.Controllers
{
    [Route("api/products/laptops")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IMapper _mapper;

        public LaptopsController(EcommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/products/laptops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LaptopDTO>> GetAsync(int id)
        {
            var laptop = await _context.Laptops
                .Include(l => l.Cpu)
                .Include(l => l.Gpu)
                .Include(l => l.Ram)
                .Include(l => l.Display)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (laptop == null)
            {
                return NotFound();
            }

            var laptopDto = _mapper.Map<LaptopDTO>(laptop);
            return Ok(laptopDto);
        }

        // GET: api/products/laptops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaptopDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, [FromQuery] LaptopFilter? filter = null)
        {
            IQueryable<Laptop> query = _context.Laptops;

            if (filter != null)
            {
                query = ApplyFilter(query, filter);
            }

            var laptops = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(l => l.Images)
                .ToListAsync();
            var laptopsDto = _mapper.Map<List<LaptopDTO>>(laptops);

            return Ok(laptopsDto);
        }

        // helper method to apply filters
        private IQueryable<Laptop> ApplyFilter(IQueryable<Laptop> query, LaptopFilter filter)
        {
            query = query.Where(l =>
                (string.IsNullOrEmpty(filter.Brand) || l.Brand == filter.Brand) &&
                (string.IsNullOrEmpty(filter.Model) || l.Model.Contains(filter.Model)) &&
                (!filter.MinPrice.HasValue || l.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || l.Price <= filter.MaxPrice.Value) &&
                (!filter.OnSale.HasValue  || filter.OnSale == false || l.Sale.HasValue) &&
                (!filter.CpuBrand.HasValue || l.Cpu.Brand == filter.CpuBrand.Value) &&
                (string.IsNullOrEmpty(filter.CpuModel) || l.Cpu.Model.Contains(filter.CpuModel)) &&
                (!filter.CpuNumberOfCores.HasValue || l.Cpu.NumberOfCores == filter.CpuNumberOfCores.Value) &&
                (!filter.ScreenSizeInches.HasValue || l.Display.ScreenSizeInches == filter.ScreenSizeInches.Value) &&
                (string.IsNullOrEmpty(filter.Resolution) || l.Display.Resolution == filter.Resolution) &&
                (!filter.RefreshRateHz.HasValue || l.Display.RefreshRateHz == filter.RefreshRateHz.Value) &&
                (string.IsNullOrEmpty(filter.GpuBrand) || l.Gpu.Brand == filter.GpuBrand) &&
                (string.IsNullOrEmpty(filter.GpuModel) || l.Gpu.Model.Contains(filter.GpuModel)) &&
                (!filter.GpuVRAM.HasValue || l.Gpu.VRAM == filter.GpuVRAM.Value) &&
                (!filter.RamCapacity.HasValue || l.Ram.Capacity == filter.RamCapacity.Value)
                );

            return query;
        }
    }
}

/*
TODO
1. optimize queries -> no need to include all dependecies
2. apply strategy pattern to avoid having duplicate controllers
*/

