using AutoMapper;
using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Data.Model.Enums;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ShoppingBasketContext context;
        private readonly IMapper mapper;

        public BasketRepository(
            ShoppingBasketContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task AddBasketItem(BasketItemModel setBasketItem, CancellationToken ct)
        {
            var existingItem = await context
                .Set<BasketItem>()
                .Where(bi => bi.ProductId == setBasketItem.ProductId &&
                    bi.BasketId == setBasketItem.BasketId)
                .FirstOrDefaultAsync(ct);

            if (existingItem != null)
            {
                if(setBasketItem.Quantity == default)
                {
                    await this.DeleteBasketItem(existingItem.Id, ct);
                    return;
                }

                existingItem.Quantity = setBasketItem.Quantity;
                existingItem.PriceAtCheckout = null;
            }
            else
            {
                if (setBasketItem.Quantity > 0)
                {
                    var basketItem = this.mapper.Map<BasketItem>(setBasketItem);
                    await context.AddAsync(basketItem, ct);
                }
            }

            await context.SaveChangesAsync(ct);
        }

        public async Task<int> CreateBasket(int clientId, int countryId, CancellationToken ct)
        {
            var basket = new Basket
            {
                ClientId = clientId,
                Status = BasketStatus.InProcess.ToString(),
                CountryId = countryId,
            };

            await this.context.Set<Basket>().AddAsync(basket, ct);

            await this.context.SaveChangesAsync(ct);

            return basket.Id;
        }

        public async Task<IEnumerable<BasketItemModel>> GetBasketItems(int basketId, CancellationToken ct)
        {
            return await context.Set<BasketItem>()
                .Where(bi => bi.BasketId == basketId)
                .Join(
                    context.Set<Product>(),
                    bi => bi.ProductId,
                    p => p.Id,
                    (bi, p) => new BasketItemModel
                    {
                        BasketId = bi.BasketId,
                        Currency = bi.Currency,
                        OriginalPrice = bi.OriginalPrice,
                        PriceAtCheckout = bi.PriceAtCheckout,
                        ProductId = bi.ProductId,
                        Quantity = bi.Quantity,
                        Name = p.Name
                    }
                )
                .ToListAsync(ct);
        }

        public Task<int> GetCurrentBasketId(int clientId, int countryId, CancellationToken ct)
        {
            return this.context
                .Set<Basket>()
                .Where(b => 
                    b.ClientId == clientId && 
                    b.Status == BasketStatus.InProcess.ToString() &&
                    b.CountryId == countryId)
                .Select(b => b.Id)
                .FirstOrDefaultAsync(ct);
        }

        public async Task PurchaseBasket(int basketId, CancellationToken ct)
        {
            var basket = await this.context
                .Set<Basket>()
                .Where(b => b.Id == basketId)
                .FirstOrDefaultAsync(ct);

            basket.Status = BasketStatus.Purchased.ToString();

            await this.context.SaveChangesAsync(ct);
        }

        private Task DeleteBasketItem(int Id, CancellationToken ct)
        {
            this.context.Set<BasketItem>()
                .Where(x => x.Id == Id)
                .ExecuteDeleteAsync(ct);

            return Task.CompletedTask;
        }
    }
}
