using Client.Services;

namespace Client.Util
{
    public class ProductServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;


        public ProductServiceResolver (IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProductService? GetService(string category)
        {
            Type? dtoType = ProductCategoryMap.GetDtoType(category);
            if (dtoType == null) return null;

            var serviceType = typeof(ProductService<>).MakeGenericType(dtoType);
            return (IProductService)_serviceProvider.GetService(serviceType)!;
        }
    }
}
