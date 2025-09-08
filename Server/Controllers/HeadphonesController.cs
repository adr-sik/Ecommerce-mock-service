using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.ProductTypes;
using Shared.Models;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;

namespace Server.Controllers
{
    [Route("api/products/[controller]")]
    [ApiController]
    public class HeadphonesController : ProductsController<Headphones, HeadphonesDTO, HeadphonesFilter>
    {
        public HeadphonesController(EcommerceContext context, IMapper mapper) : base(context, mapper) { }

        // helper method to apply filters
        protected override IQueryable<Headphones> ApplyFilter(IQueryable<Headphones> query, HeadphonesFilter filter)
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

        protected override IQueryable<Headphones> IncludeNavigation(IQueryable<Headphones> query)
        {
            return query
                .Include(h => h.ChargingAccessory);
        }
    }
}
