using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<DetailedProduct>> GetAllAvailableProducts(int countryId, CancellationToken ct);

        Task<(string name, decimal price, string currency)> GetProductPrice(int productId, int countryId, CancellationToken ct);
    }
}
