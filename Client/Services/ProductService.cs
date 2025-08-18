using Shared.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Humanizer;

namespace Client.Services
{
    public class ProductService : ServiceBase<ProductDTO>
    {
        protected override string Endpoint => "api/products";
        public ProductService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : base(httpClient, jsonOptions) { }
        public async Task<T> GetProductOfType<T>(int id) where T : class
        {
            var typeName = typeof(T).Name.ToLowerInvariant().Replace("dto", "").Pluralize();

            var response = await _http.GetAsync($"{Endpoint}/{typeName}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
    }
}
