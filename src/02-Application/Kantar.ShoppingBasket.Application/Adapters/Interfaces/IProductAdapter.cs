using Kantar.ShoppingBasket.Application.Model;

namespace Kantar.ShoppingBasket.Application.Adapters.Interfaces
{
    public interface IProductAdapter
    {
        Task<(string name, decimal price, string currency)> GetProductPrice(int productId, int countryId, CancellationToken ct);
    }
}
