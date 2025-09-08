using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;

namespace Server.Controllers
{
    [Route("api/products/[controller]")]
    [ApiController]
    public class LaptopsController : ProductsController<Laptop, LaptopDTO, LaptopFilter>
    {
        public LaptopsController(EcommerceContext context, IMapper mapper) : base(context, mapper) { }

        protected override IQueryable<Laptop> ApplyFilter(IQueryable<Laptop> query, LaptopFilter filter)
        {
            query = query.Where(l =>
                (string.IsNullOrEmpty(filter.Brand) || l.Brand == filter.Brand) &&
                (string.IsNullOrEmpty(filter.Model) || l.Model.Contains(filter.Model)) &&
                (!filter.MinPrice.HasValue || l.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || l.Price <= filter.MaxPrice.Value) &&
                (!filter.OnSale.HasValue || filter.OnSale == false || l.Sale.HasValue) &&
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

        protected override IQueryable<Laptop> IncludeNavigation(IQueryable<Laptop> query)
        {
            return query
                .Include(l => l.Cpu)
                .Include(l => l.Gpu)
                .Include(l => l.Ram)
                .Include(l => l.Display);
        }
    }
}


