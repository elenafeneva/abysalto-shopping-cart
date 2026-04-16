using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Entities.DTOs;
using ShoppingCart.Infrastructure;
using System.Text.Json.Nodes;

namespace ShoppingCart.API.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;


        public ProductService(IHttpClientFactory httpClientFactory, AppSettings appSettings, AppDbContext appDbContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _appSettings = appSettings;
            _context = appDbContext;
        }

        public async Task<bool> CreateFavoriteProduct(FavoriteProduct favoriteProduct)
        {
            await _context.FavoriteProducts.AddAsync(favoriteProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id, Guid userId)
        {
            var response = await _httpClient.GetAsync($"{_appSettings.DummyProductsUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
            var favoriteProductIds = await GetFavoriteProductIdsByUserIdAsync(userId);

            var product = new ProductDto
            {
                Id = json?["id"]?.GetValue<int>() ?? 0,
                Title = json?["title"]?.GetValue<string>() ?? string.Empty,
                Description = json?["description"]?.GetValue<string>() ?? string.Empty,
                Category = json?["category"]?.GetValue<string>() ?? string.Empty,
                Price = json?["price"]?.GetValue<double>() ?? 0.0,
                DiscountPercentage = json?["discountPercentage"]?.GetValue<double>() ?? 0.0,
                Rating = json?["rating"]?.GetValue<double>() ?? 0.0,
                Stock = json?["stock"]?.GetValue<int>() ?? 0,
                Brand = json?["brand"]?.GetValue<string>() ?? string.Empty,
                Sku = json?["sku"]?.GetValue<string>() ?? string.Empty,
                Weight = json?["weight"]?.GetValue<double>() ?? 0.0,
                Images = json?["images"]?.AsArray().Select(i => i.GetValue<string>()).ToArray() ?? Array.Empty<string>(),
                IsFavorite = favoriteProductIds.Contains(json?["id"]?.GetValue<int>() ?? 0)
            };
            return product;
        }

        public async Task<ProductPagedResponseDto> GetProductsAsync(int limit, int skip, string sortField, string sortOrder, Guid userId)
        {
            var urlParams = $"limit={limit}&skip={skip}";
            if (!string.IsNullOrWhiteSpace(sortField))
                urlParams += $"&sortBy={sortField}&order={sortOrder}";
            var response = await _httpClient.GetAsync($"{_appSettings.DummyProductsUrl}?{urlParams}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
            var productsList = json?["products"]?.AsArray();
            var totalNumberOfProducts = json?["total"]?.AsValue();

            if (productsList is null || !productsList.Any())
                return new ProductPagedResponseDto();

            var favoriteProductIds = await GetFavoriteProductIdsByUserIdAsync(userId);
            var products = productsList.Select(p => new ProductDto
            {
                Id = p["id"]?.GetValue<int>() ?? 0,
                Title = p["title"]?.GetValue<string>() ?? string.Empty,
                Description = p["description"]?.GetValue<string>() ?? string.Empty,
                Category = p["category"]?.GetValue<string>() ?? string.Empty,
                Price = p["price"]?.GetValue<double>() ?? 0.0,
                DiscountPercentage = p["discountPercentage"]?.GetValue<double>() ?? 0.0,
                Rating = p["rating"]?.GetValue<double>() ?? 0.0,
                Stock = p["stock"]?.GetValue<int>() ?? 0,
                Brand = p["brand"]?.GetValue<string>() ?? string.Empty,
                Sku = p["sku"]?.GetValue<string>() ?? string.Empty,
                Weight = p["weight"]?.GetValue<double>() ?? 0.0,
                Images = p["images"]?.AsArray().Select(i => i.GetValue<string>()).ToArray() ?? Array.Empty<string>(),
                IsFavorite = favoriteProductIds.Contains(p["id"]?.GetValue<int>() ?? 0)
            }).ToList();

            var responseProducts = new ProductPagedResponseDto
            {
                Products = products,
                TotalNumberOfProducts = (int)totalNumberOfProducts 
            };
            return responseProducts;
        }

        private async Task<List<int>> GetFavoriteProductIdsByUserIdAsync(Guid userId)
        {
            return await _context.FavoriteProducts
                .Where(fp => fp.UserId == userId)
                .Select(fp => fp.ProductId)
                .ToListAsync();
        }
    }
}
