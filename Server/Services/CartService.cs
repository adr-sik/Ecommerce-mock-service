using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.Models;

namespace Server.Services
{
    public class CartService
    {
    private readonly EcommerceContext _dbContext;

        public CartService(EcommerceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ProceedToCheckout(List<CheckoutItemDTO> items)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            decimal total = 0;

            try
            {
                var itemIds = items.Select(item => item.ProductId).ToList();

                var products = await _dbContext.Products
                    .Where(p => itemIds.Contains(p.Id))
                    .ToListAsync();

                var productDictionary = products.ToDictionary(p => p.Id);

                foreach (var item in items)
                {
                    if (!productDictionary.TryGetValue(item.ProductId, out var product) || product.Stock < item.Quantity)
                    {
                        throw new InvalidOperationException($"Validation failed for product with Id: {item.ProductId}");
                    }

                    decimal unitPrice = product.Sale.HasValue
                        ? product.Price * (1 - (product.Sale.Value / 100M))
                        : product.Price;

                    product.Stock -= item.Quantity;
                    total += unitPrice * item.Quantity;                                      
              }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // TODO: implement
                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
