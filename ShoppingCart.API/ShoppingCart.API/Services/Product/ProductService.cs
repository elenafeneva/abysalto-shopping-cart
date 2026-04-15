using ShoppingCart.Domain.Entities.DTOs;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ShoppingCart.API.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;


        public ProductService(IHttpClientFactory httpClientFactory, AppSettings appSettings)
        {
            _httpClient = httpClientFactory.CreateClient();
            _appSettings = appSettings;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_appSettings.DummyProductsUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
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
                Images = json?["images"]?.AsArray().Select(i => i.GetValue<string>()).ToArray() ?? Array.Empty<string>()
            };
            return product;
        }

        public async Task<List<ProductDto>> GetProductsAsync(int limit, int skip)
        {
            var response = await _httpClient.GetAsync($"{_appSettings.DummyProductsUrl}?limit={limit}&skip={skip}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
            var productsList = json?["products"]?.AsArray();

            if (productsList is null || !productsList.Any())
                return new List<ProductDto>();

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
                Images = p["images"]?.AsArray().Select(i => i.GetValue<string>()).ToArray() ?? Array.Empty<string>()
            }).ToList();
            return products;
        }

    }
}
