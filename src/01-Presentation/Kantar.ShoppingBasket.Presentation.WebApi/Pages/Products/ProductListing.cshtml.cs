using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages.Products
{
    public class ProductListingModel : PageModel
    {
        private readonly HttpClient client;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductListingModel(
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            this.client = httpClientFactory.CreateClient("BasketApiClient");
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Dictionary<DetailedProductModel, int> Products { get; set; } = new();

        [BindProperty]
        public List<int> ProductIds { get; set; }
        [BindProperty]
        public List<int> Quantities { get; set; }

        [TempData]
        public string AddToBasketMessage { get; set; }

        public async Task OnGetAsync()
        {
            var countryIdString = httpContextAccessor.HttpContext?.Request.Cookies["CountryId"];
            if (!int.TryParse(countryIdString, out var countryId))
            {
                Products = new Dictionary<DetailedProductModel, int>();
                return;
            }

            client.DefaultRequestHeaders.Add("X-Country-Id", countryId.ToString());

            var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                Products = new Dictionary<DetailedProductModel, int>();
                return;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/product/available");
            if (response.IsSuccessStatusCode)
            {
                var availableProducts = await response.Content.ReadFromJsonAsync<List<DetailedProductModel>>() ?? new List<DetailedProductModel>();

                var currentQuantities = new CurrentBasketModel();
                if (availableProducts.Count > 0)
                {
                    var currentItemsResponse = await client.GetAsync($"api/basket/currentitems");
                    if (currentItemsResponse.IsSuccessStatusCode)
                    {
                        currentQuantities = await currentItemsResponse.Content.ReadFromJsonAsync<CurrentBasketModel>() ?? new CurrentBasketModel();
                    }
                    else
                    {
                        Products = new Dictionary<DetailedProductModel, int>();
                        return;
                    }
                }

                Products = new Dictionary<DetailedProductModel, int>();
                foreach (var product in availableProducts)
                {
                    int qty = 0;
                    if (currentQuantities.QuantityByProduct.TryGetValue(product.Id, out int foundQty))
                    {
                        qty = foundQty;
                    }
                    Products.Add(product, qty);
                }

                Response.Cookies.Append("BasketId", currentQuantities.BasketId.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            }
            else
            {
                Products = new Dictionary<DetailedProductModel, int>();
            }
        }

        public async Task<IActionResult> OnPostAddToBasketAsync()
        {
            var basketIdString = httpContextAccessor.HttpContext?.Request.Cookies["BasketId"];
            if (!int.TryParse(basketIdString, out var basketId))
            {
                AddToBasketMessage = "Failed to add items: basket not found.";
                return RedirectToPage();
            }

            var addToBasketRequest = new AddToBasketRequestModel
            {
                BasketId = basketId,
            };

            for (int i = 0; i < ProductIds.Count; i++)
            {
                addToBasketRequest.QuantityByProductId.Add(ProductIds[i], Quantities[i]);
            }
            
            var countryIdString = httpContextAccessor.HttpContext?.Request.Cookies["CountryId"];
            if (!int.TryParse(countryIdString, out var countryId))
            {
                AddToBasketMessage = "Failed to add items: country not selected.";
                return RedirectToPage();
            }
            
            client.DefaultRequestHeaders.Add("X-Country-Id", countryId.ToString());

            var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                AddToBasketMessage = "Failed to add items: unauthorized operation.";
                return RedirectToPage();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync($"api/basket/addtobasket", addToBasketRequest);

            if (response.IsSuccessStatusCode)
            {
                AddToBasketMessage = "Items successfully added to basket!";
            }
            else
            {
                AddToBasketMessage = "Failed to add items to basket.";
            }

            return RedirectToPage();
        }
    }
}
