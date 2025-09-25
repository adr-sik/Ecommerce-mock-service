using Shared.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Humanizer;
using Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Client.Services
{
    public class ProductService<T> : ServiceBase<T>, IProductService where T : ProductDTO
    {
        protected override string Endpoint => $"api/products/{typeof(T).Name.ToLowerInvariant().Replace("dto", "").Pluralize()}";
        public ProductService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : base(httpClient, jsonOptions) { }

        // Get products with pagination
        public async Task<PagedResponse<ProductDTO>> GetPagniatedProductsAsync(
        string? query,
        int pageNumber)
        {
            query = string.IsNullOrEmpty(query) ? $"?pageNumber={pageNumber}" : $"{query}&pageNumber={pageNumber}";
            try
            {
                var response = await _http.GetAsync($"{Endpoint}{query}");
                if (response.IsSuccessStatusCode)
                {
                    var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<ProductDTO>>(_jsonOptions);
                    return pagedResponse;
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching paginated items from {Endpoint}: {ex.Message}");
                return default;
            }
        }

        // Utilize methods from base class
        public async Task<List<ProductDTO>> GetAllProductsAsync(string? query = "", string? sort = "")
        {
            var items = await this.GetAllAsync(query, sort);
            return items.Cast<ProductDTO>().ToList();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {

            var product = await this.GetByIdAsync(id);
            return product;

        }
    }
}