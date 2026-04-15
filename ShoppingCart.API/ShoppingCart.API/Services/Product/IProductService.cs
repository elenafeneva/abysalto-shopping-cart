using ShoppingCart.Domain.Entities.DTOs;

namespace ShoppingCart.API.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync(int limit, int skip);
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
