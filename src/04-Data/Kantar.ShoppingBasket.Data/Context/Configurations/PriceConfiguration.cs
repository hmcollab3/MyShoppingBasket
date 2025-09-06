using Kantar.ShoppingBasket.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kantar.ShoppingBasket.Data.Context.Configurations
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.HasKey(c => new
            {
                c.ProductId,
                c.CountryId
            });
        }
    }
}
