using ShoppingCart.API.Entities.DTOs;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.API.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(string email, string password);
        Task<AuthResultDto> RegisterAsync(string firstName, string lastName, string email, string password);
    }
}
