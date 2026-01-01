using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.Models;

namespace Server.Services
{
    public class CartService : ICartService
    {
    private readonly EcommerceContext _context;

        public CartService(EcommerceContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Product>> LoadCartItemsAsync(List<int> productIds)
        {
            IQueryable<Product> query = _context.Set<Product>();

            if (productIds == null)
                return new List<Product>();

            var products = await query
                .Where(p => productIds.Contains(p.Id))
                .Include(p => p.Images)
                .ToListAsync();

            return products;
        }
    }
}
