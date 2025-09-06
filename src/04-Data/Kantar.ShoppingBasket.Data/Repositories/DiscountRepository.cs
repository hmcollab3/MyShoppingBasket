using AutoMapper;
using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ShoppingBasketContext context;
        private readonly IMapper mapper;

        public DiscountRepository(
            ShoppingBasketContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<decimal?> GetBestDirectDiscount(int productId, CancellationToken ct)
        {
            var result = await this.context
                .Set<Discount>()
                .Where(dd => dd.AffectedProductId == productId &&
                    dd.StartDateUtc < DateTime.UtcNow &&
                    dd.EndDateUtc > DateTime.UtcNow)
                .OrderByDescending(dd => dd.DiscountFactor)
                .FirstOrDefaultAsync(ct);

            return result?.DiscountFactor;
        }

        public async Task<IEnumerable<GetMultiBuyDiscount>> GetAllMultiBuyDiscounts(int productId, decimal availableDiscount, CancellationToken ct)
        {
            var result = await this.context
                .Set<MultiBuyDiscount>()
                .Where(dd => dd.AffectedProductId == productId &&
                    dd.StartDateUtc < DateTime.UtcNow &&
                    dd.EndDateUtc > DateTime.UtcNow &&
                    dd.DiscountFactor > availableDiscount)
                .OrderByDescending(dd => dd.DiscountFactor)
                .ToListAsync(ct);

            return this.mapper.Map<IEnumerable<GetMultiBuyDiscount>>(result);
        }
    }
}
