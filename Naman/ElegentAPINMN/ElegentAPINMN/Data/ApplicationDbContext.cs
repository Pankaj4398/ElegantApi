using ElegentAPINMN.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ElegentAPINMN.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingSession> ShoppingSessions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<DiscountProduct> DiscountProduct { get; set; }

    }
}
