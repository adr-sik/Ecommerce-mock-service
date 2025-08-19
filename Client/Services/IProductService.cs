using Shared.Models.DTOs;

namespace Client.Services
{
    // For dynamic ProductDTO type resolution
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync(string? query);
        Task<ProductDTO?> GetProductByIdAsync(int id);
    }
}
