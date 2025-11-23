using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EcommerceContext _dbContext;

        public OrdersController(EcommerceContext context)
        {
            _dbContext = context;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<decimal?>> ProcessOrder(List<CheckoutItemDTO> items, Guid userId)
        {
            var result = await ProceedToCheckout(items, userId);

            return result;
        }

        [HttpPost("concurrency-test")]
        public async Task<ActionResult<decimal?>> TestConcurrency(List<CheckoutItemDTO> items, Guid userId)
        {
            var result = await ProceedToCheckout(items, userId, sleep:true);

            return result;
        }

        private async Task<decimal?> ProceedToCheckout(List<CheckoutItemDTO> items, Guid userId, int maxRetries = 3, bool sleep = false)
        {
            // retry in case of DbUpdateConcurrencyException
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    return await ExecuteCheckout(items, userId, sleep);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (attempt == maxRetries - 1)
                    {
                        throw new InvalidOperationException("Checkout failed due to concurrent updates. Please try again.", ex);
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));
                }
            }
            return null;
        }

        private async Task<decimal?> ExecuteCheckout(List<CheckoutItemDTO> items, Guid userId, bool sleep)
        {
            _dbContext.ChangeTracker.Clear();
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            decimal total = 0;

            try
            {
                var itemIds = items.Select(item => item.ProductId).ToList();
                var products = await _dbContext.Products
                    .Where(p => itemIds.Contains(p.Id))
                    .ToListAsync();

                var productDictionary = products.ToDictionary(p => p.Id);

                var newOrder = new Order
                {
                    UserId = userId,
                    Created = DateTime.UtcNow,
                    OrderProducts = new List<OrderProduct>()
                };

                foreach (var item in items)
                {
                    if (item.Quantity <= 0)
                    {
                        throw new InvalidOperationException($"Invalid quantity for product {item.ProductId}");
                    }

                    if (!productDictionary.TryGetValue(item.ProductId, out var product))
                    {
                        throw new InvalidOperationException($"Product not found: {item.ProductId}");
                    }

                    if (product.Stock < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {item.ProductId}. Available: {product.Stock}, Requested: {item.Quantity}");
                    }

                    decimal unitPrice = product.Sale.HasValue
                        ? product.Price * (1 - (product.Sale.Value / 100M))
                        : product.Price;

                    product.Stock -= item.Quantity;

                    decimal lineTotal = unitPrice * item.Quantity;
                    total += Math.Round(lineTotal, 2, MidpointRounding.AwayFromZero);

                    var newOrderProduct = new OrderProduct
                    {
                        ProductId = item.ProductId,
                        Amount = item.Quantity,
                        UnitPrice = unitPrice,
                    };

                    newOrder.OrderProducts.Add(newOrderProduct);
                }

                // for concurrency testing
                if (sleep == true) Thread.Sleep(5000);

                newOrder.Total = total;
                _dbContext.Orders.Add(newOrder);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return total;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }       
        }
    }
}
