using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Entities.DTOs;

namespace ShoppingCart.API.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync(int limit, int skip, Guid userId);
        Task<ProductDto> GetProductByIdAsync(int id, Guid userId);
        Task<bool> CreateFavoriteProduct(FavoriteProduct favoriteProduct);
    }
}
