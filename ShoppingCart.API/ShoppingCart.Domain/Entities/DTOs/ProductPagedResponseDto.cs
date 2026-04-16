namespace ShoppingCart.Domain.Entities.DTOs
{
    public class ProductPagedResponseDto
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public int TotalNumberOfProducts { get; set; } = 0;
        
    }
}
