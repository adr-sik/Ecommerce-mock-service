using Shared.Models.DTOs;

namespace Client.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync(string? query);
        //Task<ProductDTO?> GetProductByIdAsync(int id);
    }
}
