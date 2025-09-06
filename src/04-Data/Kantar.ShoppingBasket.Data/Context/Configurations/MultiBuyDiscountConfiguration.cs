using Kantar.ShoppingBasket.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kantar.ShoppingBasket.Data.Context.Configurations
{
    public class MultiBuyDiscountConfiguration : IEntityTypeConfiguration<MultiBuyDiscount>
    {
        public void Configure(EntityTypeBuilder<MultiBuyDiscount> builder)
        {
            builder.HasKey(c => new
            {
                c.Id,
            });
        }
    }
}
