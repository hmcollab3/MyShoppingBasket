using Kantar.ShoppingBasket.Application.Model;

namespace Kantar.ShoppingBasket.Application.Services.Interfaces
{
    public interface ICountryService
    {
        Task<int> GetCountryId(string countryIsoCode, CancellationToken ct);

        Task<IEnumerable<CountryDto>> GetAllCountries(CancellationToken ct);
    }
}
