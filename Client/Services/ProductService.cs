using Shared.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Humanizer;
using Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;

namespace Client.Services
{
    public class ProductService<T> : ServiceBase<T>, IProductService where T : ProductDTO
    {
        protected override string Endpoint => $"api/products/{typeof(T).Name.ToLowerInvariant().Replace("dto", "").Pluralize()}";
        public ProductService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : base(httpClient, jsonOptions) { }

        // Utilizing methods from base class

        // Get products with pagination
        public async Task<PagedResponse<ProductDTO>> GetPagniatedProductsAsync(
        string? query,
        int? pageNumber)
        {
            var response = await this.GetPagniatedAsync(query, pageNumber);
            return new PagedResponse<ProductDTO> 
            {
                Items = response.Items.Cast<ProductDTO>().ToList(),
                Page = response.Page,
                TotalCount = response.TotalCount
            };
        }

        // Get all products
        public async Task<List<ProductDTO>> GetAllProductsAsync(string? query = "", string? sort = "")
        {
            var items = await this.GetAllAsync(query, sort);
            return items.Cast<ProductDTO>().ToList();
        }

        // Get product by ID
        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await this.GetByIdAsync(id);
            return product;
        }
    }
}