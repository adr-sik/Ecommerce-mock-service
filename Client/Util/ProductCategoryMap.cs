using Shared.Models.DTOs;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.Filters;

namespace Client.Util
{
    // Maps string values to concrete types
    public static class ProductCategoryMap
    {
        private static readonly Dictionary<string, Type> categoryToDto = new()
        {
            ["laptops"] = typeof(LaptopDTO),
            ["phones"] = typeof(PhoneDTO),
            ["headphones"] = typeof(HeadphonesDTO)
        };

        public static Type GetDtoType(string category)
        {
            return categoryToDto.TryGetValue(category.ToLowerInvariant(), out var type) ? type : typeof(ProductDTO);
        }

        private static readonly Dictionary<string, Type> categoryToFilter = new()
        {
            ["laptops"] = typeof(LaptopFilter),
            ["phones"] = typeof(PhoneFilter),
            ["headphones"] = typeof(HeadphonesFilter)
        };

        public static Type GetFilterType(string category)
        {
            return categoryToFilter.TryGetValue(category.ToLowerInvariant(), out var type) ? type : typeof(ProductFilter);
        }
    }
}
