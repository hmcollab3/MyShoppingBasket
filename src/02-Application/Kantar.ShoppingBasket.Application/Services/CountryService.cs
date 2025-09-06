using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Domain.Repositories;

namespace Kantar.ShoppingBasket.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public CountryService(
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountries(CancellationToken ct)
        {
            var allCountries = await this.countryRepository.GetAllCountries(ct);

            return this.mapper.Map<IEnumerable<CountryDto>>(allCountries);
        }

        public Task<int> GetCountryId(string countryIsoCode, CancellationToken ct)
        {
            return this.countryRepository.GetCountryIdByIsoCode(countryIsoCode, ct);
        }
    }
}
