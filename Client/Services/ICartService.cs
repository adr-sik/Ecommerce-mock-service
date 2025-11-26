using Shared.Models;

namespace Client.Services
{
    public interface ICartService
    {
        public Task<List<CheckoutItemDTO>> GetCart();
    }
}
