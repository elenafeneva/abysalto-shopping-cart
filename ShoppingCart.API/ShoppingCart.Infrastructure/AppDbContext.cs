using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set up the Relationship
            base.OnModelCreating(modelBuilder);

            // Configuration for CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItems");
                entity.HasOne(ci => ci.User)          
                    .WithMany(u => u.CartItems)        
                    .HasForeignKey(ci => ci.UserId)    
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            // Configuration for Favorite Product
            modelBuilder.Entity<FavoriteProduct>(entity =>
            {
                entity.ToTable("FavoriteProducts");
                entity.HasOne(ci => ci.User)
                    .WithMany(u => u.FavoriteProducts)
                    .HasForeignKey(ci => ci.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration for User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
