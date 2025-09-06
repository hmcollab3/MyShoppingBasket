using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Domain.Repositories
{
    public interface ICountryRepository
    {
        Task<int> GetCountryIdByIsoCode(string countryCode, CancellationToken ct);

        Task<IEnumerable<GetCountry>> GetAllCountries(CancellationToken ct);
    }
}
