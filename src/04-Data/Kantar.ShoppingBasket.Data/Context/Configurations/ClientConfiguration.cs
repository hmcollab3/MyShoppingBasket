using Kantar.ShoppingBasket.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kantar.ShoppingBasket.Data.Context.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => new
            {
                c.Id,
            });
        }
    }
}
