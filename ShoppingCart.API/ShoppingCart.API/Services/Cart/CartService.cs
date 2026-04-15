using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Infrastructure;

namespace ShoppingCart.API.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;


        public CartService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<bool> CreateCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(Guid cartItemId)
        {
            await _context.CartItems.Where(c => c.Id == cartItemId).ExecuteDeleteAsync();
            return true;
        }
    }
}
