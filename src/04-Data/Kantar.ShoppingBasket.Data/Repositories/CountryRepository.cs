using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ShoppingBasketContext context;
        private readonly IMapper mapper;

        public CountryRepository(
            ShoppingBasketContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetCountry>> GetAllCountries(CancellationToken ct)
        {
            return await this.context
                .Set<Country>()
                .ProjectTo<GetCountry>(mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public Task<int> GetCountryIdByIsoCode(string countryCode, CancellationToken ct)
        {
            return this.context
                .Set<Country>()
                .Where(c => c.ISO3166 == countryCode)
                .Select(c => c.Id)
                .FirstOrDefaultAsync(ct);
        }
    }
}
