using Shared.Models;

namespace Client.Services
{
    public class ProductService : ServiceBase<Product>
    {
        protected override string Endpoint => "api/products";
        public ProductService(HttpClient httpClient) : base(httpClient) { }
        public virtual Task<List<Product>> GetProducts() => GetAllAsync();
        public Task<Product?> GetProduct(int id) => GetByIdAsync(id);
        public Task<Product?> PutProduct(int id, Product product) => PutAsync(id, product);
        public Task<Product?> PostProduct(Product product) => PostAsync(product);
        public Task<bool> DeleteProduct(int id) => DeleteAsync(id);
    }
}
