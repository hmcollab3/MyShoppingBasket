using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages.Basket
{
    public class HistoryModel : PageModel
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HistoryModel(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            this.client = httpClientFactory.CreateClient("BasketApiClient");
            this.httpContextAccessor = httpContextAccessor;
        }

        public List<ReceiptModel> Receipts { get; set; } = new();

        [TempData]
        public string HistoryMessage { get; set; }

        public async Task OnGetAsync()
        {
            var countryIdString = httpContextAccessor.HttpContext?.Request.Cookies["CountryId"];
            if (!int.TryParse(countryIdString, out var countryId))
            {
                Receipts = new List<ReceiptModel>();
                return;
            }

            client.DefaultRequestHeaders.Add("X-Country-Id", countryId.ToString());

            var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                HistoryMessage = "Unable to load history: unauthorized operation.";
                return;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/receipt/history");
            if (response.IsSuccessStatusCode)
            {
                var receipts = await response.Content.ReadFromJsonAsync<List<ReceiptModel>>()
                                 ?? new List<ReceiptModel>();

                Receipts = receipts;
                HistoryMessage = string.Empty;
            }
            else
            {
                Receipts = new List<ReceiptModel>();
                HistoryMessage = "Failed to load history.";
            }
        }

        public async Task<IActionResult> OnPostDownloadReceiptAsync(int receiptId)
        {
            var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                HistoryMessage = "Unable to download receipt: unauthorized operation.";
                return RedirectToPage();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/receipt/{receiptId}/download");
            if (response.IsSuccessStatusCode)
            {
                var pdfBytes = await response.Content.ReadAsByteArrayAsync();
                return File(pdfBytes, "application/pdf", $"receipt_{receiptId}.pdf");
            }
            else
            {
                HistoryMessage = "Failed to generate/download receipt.";
                return RedirectToPage();
            }
        }
    }
}
