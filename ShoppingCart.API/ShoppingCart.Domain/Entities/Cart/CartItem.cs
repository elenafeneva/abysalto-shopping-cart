namespace ShoppingCart.Domain.Entities
{
    public class CartItem : BaseEntity
    {

        public CartItem(Guid userId, int productId, int quantity = 1)
        {
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual User User { get; set; }
    }
}
