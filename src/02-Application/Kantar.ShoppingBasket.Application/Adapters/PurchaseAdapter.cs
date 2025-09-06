using AutoMapper;
using Kantar.ShoppingBasket.Application.Adapters.Interfaces;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories.UnitOfWork;

namespace Kantar.ShoppingBasket.Application.Adapters
{
    public class PurchaseAdapter : IPurchaseAdapter
    {
        private readonly IPurchaseUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PurchaseAdapter(
            IPurchaseUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Purchase(int basketId, IEnumerable<CheckoutItemDto> purchasedItems, CancellationToken ct)
        {
            var totalCost = purchasedItems.Select(pi => pi.TotalCost).Sum();
            var currency = purchasedItems.First().Currency;

            var receiptItems = this.mapper.Map<IEnumerable<ReceiptItem>>(purchasedItems);

            await this.unitOfWork.Purchase(basketId, totalCost, currency, receiptItems, ct);
        }
    }
}
