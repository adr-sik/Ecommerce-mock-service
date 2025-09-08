using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Models.ProductTypes;
using Shared.Models;
using Shared.Models.DTOs;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public abstract class ProductsController<TEntity, TDto, TFilter> : ControllerBase
        where TEntity : Product
        where TDto : ProductDTO
        where TFilter : ProductFilter
    {
        private readonly EcommerceContext _context;
        private readonly IMapper _mapper;

        public ProductsController(EcommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TDto>>> GetAllAsync(
            [FromQuery] TFilter? filter = null,
            string? sortOrder = null,
            string? sortColumn = null,
            int pageNumber = 1)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = ApplyFilter(query, filter);
            }

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {               
                switch (sortColumn.ToLower())
                {
                    case "price":
                        query = sortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true
                            ? query.OrderByDescending(p => p.Price)
                            : query.OrderBy(p => p.Price);
                        break;
                }
            }

            var pagedResponse = await PagedResponse<TEntity>.CreateAsync(query, pageNumber);
            // additional mapping for DTO
            return Ok(pagedResponse.Map<TDto>(_mapper));
        }

        // GET: api/Products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TDto>> GetAsync(int id)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            query = IncludeNavigation(query);

            var product = await query
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TDto>(product));
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        protected async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        protected async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        protected async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        protected abstract IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, TFilter filter);
        protected abstract IQueryable<TEntity> IncludeNavigation(IQueryable<TEntity> query);
    }
}
