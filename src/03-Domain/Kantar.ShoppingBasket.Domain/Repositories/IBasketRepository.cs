using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<int> CreateBasket(int clientId, int countryId, CancellationToken ct);

        Task<int> GetCurrentBasketId(int clientId, int countryId, CancellationToken ct);

        Task AddBasketItem(BasketItemModel basketItem, CancellationToken ct);

        Task<IEnumerable<BasketItemModel>> GetBasketItems(int basketId, CancellationToken ct);

        Task PurchaseBasket(int basketId, CancellationToken ct);
    }
}
