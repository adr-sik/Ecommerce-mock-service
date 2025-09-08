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
    public class PhonesController : ProductsController<Phone, PhoneDTO, PhoneFilter>
    {
        public PhonesController(EcommerceContext context, IMapper mapper) : base(context, mapper) { }

        // helper method to apply filters
        protected override IQueryable<Phone> ApplyFilter(IQueryable<Phone> query, PhoneFilter filter)
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

        protected override IQueryable<Phone> IncludeNavigation(IQueryable<Phone> query)
        {
            return query
                .Include(l => l.Cpu)
                .Include(l => l.Camera)
                .Include(l => l.Display);
        }

    }
}
