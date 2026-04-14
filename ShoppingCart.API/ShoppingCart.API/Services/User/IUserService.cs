using ShoppingCart.Domain.Entities;

namespace ShoppingCart.API.Services
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
    }
}
