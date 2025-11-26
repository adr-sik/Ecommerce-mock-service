using Client.Authorization;
using Microsoft.JSInterop;
using Shared.Models;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Client.Services
{
    public class CartService : ICartService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly JwtAuthenticationStateProvider _stateProvider;
        private readonly HttpClient _httpClient;

        private const string CartKey = "cart";

        public CartService(IJSRuntime jsRuntime, JwtAuthenticationStateProvider stateProvider, IHttpClientFactory httpClient)
        {
            _jsRuntime = jsRuntime;
            _stateProvider = stateProvider;
            _httpClient = httpClient.CreateClient("WebAPI");
        }

        public async Task<List<CheckoutItemDTO>> GetCart()
        {
            var jsonCart = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", CartKey);

            Console.WriteLine($"jsonCart: {jsonCart}");

            if (string.IsNullOrEmpty(jsonCart))
            {
                return new List<CheckoutItemDTO>();
            }

            var serializedCart = JsonSerializer.Deserialize<List<CheckoutItemDTO>>(jsonCart) ?? new List<CheckoutItemDTO>();
            Console.WriteLine($"jsonCart: {serializedCart}");
            return serializedCart;
        }

        public async Task UpdateCart(int productId, int quantityChange)
        {
            var cart = await GetCart();

            Console.WriteLine(cart.Count);

            var cartItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantityChange;

                if (cartItem.Quantity <= 0)
                {
                    cart.Remove(cartItem);
                }
            }
            else if (quantityChange > 0)
            {
                cart.Add(new CheckoutItemDTO
                {
                    ProductId = productId,
                    Quantity = quantityChange
                });
            }

            var updatedJsonCart = JsonSerializer.Serialize(cart);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", CartKey, updatedJsonCart);
        }

        public async Task<bool> ProceedToCheckout()
        {
            var userId = _stateProvider.GetUserIdAsync();
            var cart = await GetCart();
           
            var response = await _httpClient.PostAsJsonAsync($"/api/Orders/checkout", cart);

            return response.IsSuccessStatusCode;
        }

        public async Task DeleteCart()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", CartKey);
        }
    }
}
