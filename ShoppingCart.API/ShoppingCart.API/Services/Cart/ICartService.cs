
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.API.Services
{
    public interface ICartService
    {
        Task<bool> CreateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(Guid cartItemId);
    }
}
