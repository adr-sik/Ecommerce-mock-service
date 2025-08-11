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

            };
            return Task.FromResult(mockData);
        }
    }
}
