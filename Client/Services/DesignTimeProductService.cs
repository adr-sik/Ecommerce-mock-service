using Shared.Models;

namespace Client.Services
{
    public class DesignTimeProductService : ProductService
    {
        public DesignTimeProductService() : base(null!) { }

        public override Task<List<Product>> GetProducts()
        {
            var mockData = new List<Product>
            {
                new Product { Id = 1, Name = "Mock Product A", Price = 9.99m, Description = "Sample description", Stock = 10 },
                new Product { Id = 2, Name = "Mock Product B", Price = 19.99m, Description = "Another sample", Stock = 5 }
            };
            return Task.FromResult(mockData);
        }
    }
}
