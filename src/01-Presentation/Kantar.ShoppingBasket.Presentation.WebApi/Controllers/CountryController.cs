using AutoMapper;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService countryService;
        private readonly IMapper mapper;

        public CountryController(
            ICountryService countryService,
            IMapper mapper)
        {
            this.countryService = countryService;
            this.mapper = mapper;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await this.countryService.GetAllCountries(ct);

            return Ok(this.mapper.Map<IEnumerable<CountryModel>>(result));
        }
    }
}
