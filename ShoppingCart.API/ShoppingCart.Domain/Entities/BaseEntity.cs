namespace ShoppingCart.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public DateTimeOffset LastModifiedOnUtc { get; set; }
    }
}
