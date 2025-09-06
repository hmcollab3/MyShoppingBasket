using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages.Basket
{
    public class CheckoutModel : PageModel
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CheckoutModel(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            this.client = httpClientFactory.CreateClient("BasketApiClient");
            this.httpContextAccessor = httpContextAccessor;
        }

        public List<CheckoutItemModel> BasketItems { get; set; } = new();
        public decimal TotalCost { get; set; }
        public string Currency { get; set; }

        [TempData]
        public string CheckoutMessage { get; set; }

        public async Task OnGetAsync()
        {
            var basketIdString = httpContextAccessor.HttpContext?.Request.Cookies["BasketId"];
            if (!int.TryParse(basketIdString, out var basketId))
            {
                CheckoutMessage = "Failed to load the checkout details: basket not found.";
                return;
            }

            var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                CheckoutMessage = "Failed to purchase: unauthorized operation.";
                return;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/basket/checkout?basketId={basketId}");
            if (response.IsSuccessStatusCode)
            {
                var checkoutItems = await response.Content.ReadFromJsonAsync<List<CheckoutItemModel>>()
                                 ?? new List<CheckoutItemModel>();

                BasketItems = checkoutItems;
                TotalCost = checkoutItems.Select(si => si.TotalCost).Sum();
                Currency = checkoutItems.FirstOrDefault()?.Currency ?? string.Empty;
                
                TempData["BasketItems"] = JsonConvert.SerializeObject(BasketItems);
            }
            else
            {
                BasketItems = new List<CheckoutItemModel>();
            }
        }

        public async Task<IActionResult> OnPostPurchaseAsync()
        {
            BasketItems = JsonConvert.DeserializeObject<List<CheckoutItemModel>>(TempData["BasketItems"] as string)
                            ?? new List<CheckoutItemModel>();

            var basketIdString = httpContextAccessor.HttpContext?.Request.Cookies["BasketId"];
            if (!BasketItems.Any() || !int.TryParse(basketIdString, out var basketId))
            {
                CheckoutMessage = "Failed to purchase: basket not found.";
                return RedirectToPage();
            }

            var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                CheckoutMessage = "Failed to purchase: unauthorized operation.";
                return RedirectToPage();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var purchaseRequest = new PurchaseRequestModel
            {
                BasketId = basketId,
                CheckoutItemsForPurchase = BasketItems,
            };

            var response = await client.PostAsJsonAsync($"api/basket/purchase", purchaseRequest);

            if (response.IsSuccessStatusCode)
            {
                CheckoutMessage = "Purchase successful!";

                Response.Cookies.Delete("BasketId");

                return RedirectToPage("/Products/ProductListing");
            }
            else
            {
                CheckoutMessage = "Failed to complete purchase.";
                return RedirectToPage();
            }
        }
    }
}