using Kantar.ShoppingBasket.Application.Model;

namespace Kantar.ShoppingBasket.Application.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Gets all available products considering the provided country Id.
        /// </summary>
        /// <param name="countryId">The provided country Id</param>
        /// <param name="ct">The <see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<DetailedProductDto>> GetAllAvailableProducts(int countryId, CancellationToken ct);
    }
}
