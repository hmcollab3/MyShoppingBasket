using Kantar.ShoppingBasket.Application.Model;

namespace Kantar.ShoppingBasket.Application.Adapters.Interfaces
{
    public interface IPurchaseAdapter
    {
        Task Purchase(int basketId, IEnumerable<CheckoutItemDto> purchasedItems, CancellationToken ct);
    }
}
