using AutoMapper;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductController(
            IProductService productService,
            IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableProducts([FromHeader(Name = "X-Country-Id")] int countryId, CancellationToken ct)
        {
            var result = await this.productService.GetAllAvailableProducts(countryId, ct);

            if(result?.Any() == false)
            {
                return NotFound("No products were available for the provided country");
            }

            return Ok(this.mapper.Map<IEnumerable<DetailedProductModel>>(result));
        }
    }
}
