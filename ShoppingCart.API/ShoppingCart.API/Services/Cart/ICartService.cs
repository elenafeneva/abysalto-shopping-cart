
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Entities.DTOs;

namespace ShoppingCart.API.Services
{
    public interface ICartService
    {
        Task<bool> CreateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(int productId, Guid userId);
        Task<List<ProductDto>> GetCartProductsByUserId(Guid userId);
    }
}
