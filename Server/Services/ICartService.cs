using Server.Models;
using Shared.Models;

namespace Server.Services
{
    public interface ICartService
    {
        public Task<List<Product>> LoadCartItemsAsync(List<int> productIds);
        //public Task<bool> ProceedToCheckout(List<CheckoutItemDTO> items);
    }
}
