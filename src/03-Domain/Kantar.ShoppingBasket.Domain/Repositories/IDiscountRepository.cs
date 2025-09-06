using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Domain.Repositories
{
    public interface IDiscountRepository
    {
        Task<decimal?> GetBestDirectDiscount(int productId, CancellationToken ct);

        Task<IEnumerable<GetMultiBuyDiscount>> GetAllMultiBuyDiscounts(int productId, decimal availableDiscount, CancellationToken ct);
    }
}
