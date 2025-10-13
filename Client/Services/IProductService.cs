using Shared.Models;
using Shared.Models.DTOs;

namespace Client.Services
{
    // For dynamic ProductDTO type resolution
    public interface IProductService
    {
        Task<PagedResponse<ProductDTO>> GetPagniatedProductsAsync(string? query);
        Task<List<ProductDTO>> GetAllProductsAsync(string? query, string? sort);
        Task<ProductDTO?> GetProductByIdAsync(int id);
    }
}
