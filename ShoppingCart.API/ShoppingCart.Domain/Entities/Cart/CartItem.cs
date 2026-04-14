namespace ShoppingCart.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual User User { get; set; }
    }
}
