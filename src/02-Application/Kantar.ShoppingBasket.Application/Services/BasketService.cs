using AutoMapper;
using Kantar.ShoppingBasket.Application.Adapters.Interfaces;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;

namespace Kantar.ShoppingBasket.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IPurchaseAdapter purchaseAdapter;
        private readonly IDiscountAdapter discountAdapter;
        private readonly IProductAdapter productAdapter;
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketService(
            IPurchaseAdapter purchaseAdapter,
            IDiscountAdapter discountAdapter,
            IProductAdapter productAdapter,
            IBasketRepository basketRepository,
            IMapper mapper)
        {
            this.purchaseAdapter = purchaseAdapter;
            this.discountAdapter = discountAdapter;
            this.productAdapter = productAdapter;
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        public Task AddToBasket(int countryId, AddToBasketRequestDto addToBasketRequest, CancellationToken ct)
        {
            return this.AddItemsToBasket(addToBasketRequest.BasketId, countryId, addToBasketRequest, ct);
        }

        public async Task<IEnumerable<CheckoutItemDto>> Checkout(int basketId, CancellationToken ct)
        {
            var currentItems = await this.basketRepository.GetBasketItems(basketId, ct);

            if (currentItems?.Any() == false) 
            {
                return new List<CheckoutItemDto>();
            }

            var checkoutItems = new List<CheckoutItemDto>();

            foreach (var currentItem in currentItems) 
            {
                var availableDiscount = await this.discountAdapter.GetAvailableDiscount(currentItem.ProductId, ct);

                var multiBuyDiscountSavings = await this.discountAdapter.CalculateMultiBuyDiscount(currentItem.ProductId, availableDiscount, currentItems, ct);

                var checkoutItem = new CheckoutItemDto
                {
                    Name = currentItem.Name,
                    Price = currentItem.OriginalPrice,
                    Currency = currentItem.Currency,
                    Discount = availableDiscount ?? default,
                    MultiBuyDiscountSavings = multiBuyDiscountSavings,
                    Quantity = currentItem.Quantity,
                };

                this.CalculateTotalCost(checkoutItem);

                checkoutItems.Add(checkoutItem);
            }

            return checkoutItems;
        }

        public async Task Purchase(int basketId, IEnumerable<CheckoutItemDto> purchasedItems, CancellationToken ct)
        {
            await this.purchaseAdapter.Purchase(basketId, purchasedItems, ct);
        }

        public async Task<(int basketId, Dictionary<int, int> quantityByProduct)> GetCurrentItemsQuantities(int clientId, int countryId, CancellationToken ct)
        {
            var currentBasket = await this.GetCurrentBasketId(clientId, countryId, ct);

            if(currentBasket.isNewBasket)
            {
                return (currentBasket.basketId, new Dictionary<int, int>());
            }
            
            var currentItems = await this.basketRepository.GetBasketItems(currentBasket.basketId, ct);

            return (currentBasket.basketId, currentItems.ToDictionary(d => d.ProductId, d => d.Quantity));
        }

        private void CalculateTotalCost(CheckoutItemDto checkoutItem)
        {
            int multiBuyCount = checkoutItem.MultiBuyDiscountSavings?.DiscountedItemCount ?? 0;
            decimal multiBuyTotalSavings = checkoutItem.MultiBuyDiscountSavings?.TotalSavings ?? 0;
            int directDiscountCount = checkoutItem.Quantity - multiBuyCount;

            decimal multiBuyItemsTotal = (multiBuyCount * checkoutItem.Price) - multiBuyTotalSavings;
            decimal directDiscountItemsTotal = directDiscountCount * checkoutItem.Price * (1 - checkoutItem.Discount);

            checkoutItem.TotalCost = multiBuyItemsTotal + directDiscountItemsTotal;
        }

        private async Task<(bool isNewBasket, int basketId)> GetCurrentBasketId(int clientId, int countryId, CancellationToken ct)
        {
            var currentBasketId = await this.basketRepository.GetCurrentBasketId(clientId, countryId, ct);
            var isNewBasket = false;

            if (currentBasketId == default)
            {
                currentBasketId = await this.basketRepository.CreateBasket(clientId, countryId, ct);
                isNewBasket = true;
            }

            return (isNewBasket, currentBasketId);
        }

        private async Task AddItemsToBasket(int basketId, int countryId, AddToBasketRequestDto addToBasketRequest, CancellationToken ct)
        {
            foreach (var entry in addToBasketRequest.QuantityByProductId)
            {
                var productPrice = await this.productAdapter.GetProductPrice(entry.Key, countryId, ct);

                var basketItem = new BasketItemModel
                {
                    BasketId = basketId,
                    ProductId = entry.Key,
                    Quantity = entry.Value,
                    Currency = productPrice.currency,
                    OriginalPrice = productPrice.price,
                };

                await this.basketRepository.AddBasketItem(basketItem, ct);
            }
        }
    }
}
