using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Controllers
{
    [Route("api/basket")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;
        private readonly IMapper mapper;

        public BasketController(
            IBasketService basketService,
            IMapper mapper)
        {
            this.basketService = basketService;
            this.mapper = mapper;
        }

        [HttpGet("currentitems")]
        public async Task<IActionResult> GetCurrentItemQuantities([FromHeader(Name = "X-Country-Id")] int countryId, CancellationToken ct)
        {
            var clientId = User.FindFirst("client_id")?.Value;

            if (clientId == null)
            {
                return Unauthorized("Invalid client id");
            }

            var result = await this.basketService.GetCurrentItemsQuantities(int.Parse(clientId), countryId, ct);

            return Ok(new CurrentBasketModel
            {
                BasketId = result.basketId,
                QuantityByProduct = result.quantityByProduct,
            });
        }


        [HttpPost("addtobasket")]
        public async Task<IActionResult> AddToBasket([FromHeader(Name = "X-Country-Id")] int countryId, [FromBody] AddToBasketRequestModel addToBasketRequest, CancellationToken ct)
        {
            var dto = this.mapper.Map<AddToBasketRequestDto>(addToBasketRequest);

            await this.basketService.AddToBasket(countryId, dto, ct);

            return Ok();
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] int basketId, CancellationToken ct)
        {
            var result = await this.basketService.Checkout(basketId, ct);

            return Ok(this.mapper.Map<IEnumerable<CheckoutItemModel>>(result));
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromBody] PurchaseRequestModel request, CancellationToken ct)
        {
            var itemsForPurchase = this.mapper.Map<IEnumerable<CheckoutItemDto>>(request.CheckoutItemsForPurchase);

            await this.basketService.Purchase(request.BasketId, itemsForPurchase, ct);

            return Ok();
        }
    }
}
