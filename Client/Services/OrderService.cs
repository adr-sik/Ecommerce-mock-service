using Shared.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Client.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("api/orders") ?? new List<Order>();
        }

        public async Task<Order?> GetOrder(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/orders/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Order>();
                }
                return null;
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine($"Error fetching order {id}: {ex.Message}");
                return null;
            }
        }
    }
}
