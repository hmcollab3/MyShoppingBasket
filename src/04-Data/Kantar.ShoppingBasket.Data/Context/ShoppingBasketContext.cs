using Kantar.ShoppingBasket.Data.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Kantar.ShoppingBasket.Data.Context
{
    public class ShoppingBasketContext : DbContext
    {
        private readonly string schema;

        public ShoppingBasketContext([NotNull] DbContextOptions<ShoppingBasketContext> options)
            : base(options)
        {
            this.schema = "Kantar";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schema);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new PriceConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new BasketConfiguration());
            modelBuilder.ApplyConfiguration(new BasketItemConfiguration());
            modelBuilder.ApplyConfiguration(new DirectDiscountConfiguration());
            modelBuilder.ApplyConfiguration(new MultiBuyDiscountConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptItemConfiguration());
        }
    }
}
