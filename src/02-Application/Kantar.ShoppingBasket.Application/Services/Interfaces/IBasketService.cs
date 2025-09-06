using Kantar.ShoppingBasket.Application.Model;

namespace Kantar.ShoppingBasket.Application.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddToBasket(int countryId, AddToBasketRequestDto addToBasketRequest, CancellationToken ct);

        Task<IEnumerable<CheckoutItemDto>> Checkout(int basketId, CancellationToken ct);

        Task Purchase(int basketId, IEnumerable<CheckoutItemDto> itemsForPurchase, CancellationToken ct);

        Task<(int basketId, Dictionary<int, int> quantityByProduct)> GetCurrentItemsQuantities(int clientId, int countryId, CancellationToken ct);
    }
}
