using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElegentAPINMN.Data
{
    public class ShopAuthDbContext : IdentityDbContext
    {
        public ShopAuthDbContext(DbContextOptions<ShopAuthDbContext> options): base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var customerRoleId = "11df2da6-10ef-4b25-a24a-324d17ec7cfd";
            var adminRoleId = "45ec6e0b-9541-4954-b4a1-94250dd4cbc7";

            var roles= new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = customerRoleId,
                    ConcurrencyStamp = customerRoleId,
                    Name = "Customer",
                    NormalizedName = "Customer".ToUpper()
                },

                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
