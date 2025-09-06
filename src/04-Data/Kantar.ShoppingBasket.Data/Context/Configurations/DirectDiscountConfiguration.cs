using Kantar.ShoppingBasket.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kantar.ShoppingBasket.Data.Context.Configurations
{
    public class DirectDiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(c => new
            {
                c.Id,
            });
        }
    }
}
