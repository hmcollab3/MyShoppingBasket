using Kantar.ShoppingBasket.Application.Adapters.Interfaces;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;

namespace Kantar.ShoppingBasket.Application.Adapters
{
    public class DiscountAdapter : IDiscountAdapter
    {
        private readonly IDiscountRepository discountRepository;

        public DiscountAdapter(
            IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        public async Task<MultiBuyDiscountResultDto> CalculateMultiBuyDiscount(int productId, decimal? availableDiscount, IEnumerable<BasketItemModel> currentItems, CancellationToken ct)
        {
            var multiBuyDiscounts = await this.discountRepository.GetAllMultiBuyDiscounts(productId, availableDiscount ?? default, ct);

            if(multiBuyDiscounts?.Any() == false)
            {
                return new MultiBuyDiscountResultDto();
            }

            var bestScenarioTotalSavings = 0m;
            var bestScenarioItemsToDiscount = 0;

            foreach (var discount in multiBuyDiscounts)
            {
                var triggerQty = currentItems
                    .Where(i => i.ProductId == discount.TriggeringProductId)
                    .Sum(i => i.Quantity);

                var affectedQty = currentItems
                    .Where(i => i.ProductId == productId)
                    .Sum(i => i.Quantity);

                int eligibleDiscounts = triggerQty / discount.TriggerQuantity;
                int itemsToDiscount = Math.Min(eligibleDiscounts, affectedQty);

                var affectedItem = currentItems.FirstOrDefault(i => i.ProductId == productId);
                if (affectedItem != null && itemsToDiscount > 0)
                {
                    var itemPrice = affectedItem.OriginalPrice;
                    var totalSavings = itemsToDiscount * itemPrice * discount.DiscountFactor;

                    if (totalSavings > bestScenarioTotalSavings)
                    {
                        bestScenarioTotalSavings = totalSavings;
                        bestScenarioItemsToDiscount = itemsToDiscount;
                    }
                }
            }

            return new MultiBuyDiscountResultDto
            {
                TotalSavings = bestScenarioTotalSavings,
                DiscountedItemCount = bestScenarioItemsToDiscount,
            };
        }

        public async Task<decimal?> GetAvailableDiscount(int productId, CancellationToken ct)
        {
            return await this.discountRepository.GetBestDirectDiscount(productId, ct);
        }
    }
}
