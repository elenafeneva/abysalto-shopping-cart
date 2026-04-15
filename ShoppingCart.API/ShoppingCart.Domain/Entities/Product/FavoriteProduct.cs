namespace ShoppingCart.Domain.Entities
{
    public class FavoriteProduct : BaseEntity
    {
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public virtual User User { get; set; }
    }
}
