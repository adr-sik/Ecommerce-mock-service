using Shared.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Humanizer;

namespace Client.Services
{
    public class ProductService<T> : ServiceBase<T>, IProductService where T : ProductDTO
    {
        protected override string Endpoint => $"api/products/{typeof(T).Name.ToLowerInvariant().Replace("dto", "").Pluralize()}";
        public ProductService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : base(httpClient, jsonOptions) { }

        // Utilize methods from base class
        public async Task<List<ProductDTO>> GetAllProductsAsync(string? query = "")
        {
            var items = await this.GetAllAsync(query);
            return items.Cast<ProductDTO>().ToList();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {

            var product = await this.GetByIdAsync(id);
            return product;

        }
    }
}