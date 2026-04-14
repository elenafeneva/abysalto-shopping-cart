namespace ShoppingCart.Domain.Entities
{
    public class User : BaseEntity
    {
        public User() { }
        public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
