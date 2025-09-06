using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Application.Adapters.Interfaces
{
    public interface IDiscountAdapter
    {
        Task<decimal?> GetAvailableDiscount(int productId, CancellationToken ct);

        Task<MultiBuyDiscountResultDto> CalculateMultiBuyDiscount(int productId, decimal? availableDiscount, IEnumerable<BasketItemModel> currentItems, CancellationToken ct);
    }
}
