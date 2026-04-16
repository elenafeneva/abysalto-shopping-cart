using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Entities.DTOs;
using ShoppingCart.Infrastructure;

namespace ShoppingCart.API.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;


        public CartService(AppDbContext appDbContext, IProductService productService)
        {
            _context = appDbContext;
            _productService = productService;
        }

        public async Task<bool> CreateCartItemAsync(CartItem cartItem)
        {
            var existingCartItem = await _context.CartItems
                .Where(c => c.UserId == cartItem.UserId && c.ProductId == cartItem.ProductId)
                .FirstOrDefaultAsync();

            if (existingCartItem is not null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
            }
            else
            {
                await _context.CartItems.AddAsync(cartItem);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(int productId, Guid userId)
        {
            await _context.CartItems
                .Where(c => c.ProductId == productId && c.UserId == userId)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<ProductDto>> GetCartProductsByUserId(Guid userId)
        {
            var cartProducts = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var cartProductIds = cartProducts.Select(c => c.ProductId).ToList();

            List<ProductDto> products = new List<ProductDto>();
            foreach(int productId in cartProductIds)
            {
                var product = await _productService.GetProductByIdAsync(productId, userId);
                var quantity = cartProducts.Where(c => c.ProductId == productId).FirstOrDefault()?.Quantity;
                product.Quantity = quantity ?? 0;
                products.Add(product);
            }
            return products;
        }
    }
}
