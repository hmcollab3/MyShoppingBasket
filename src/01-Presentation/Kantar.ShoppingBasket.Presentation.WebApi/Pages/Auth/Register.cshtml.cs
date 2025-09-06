using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public RegisterClientModel Input { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        private readonly HttpClient client;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            this.client = httpClientFactory.CreateClient("BasketApiClient");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await client.PostAsJsonAsync("api/auth/register", Input);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                SuccessMessage = json.GetProperty("message").GetString() ?? "Registration successful.";
                return RedirectToPage("/Auth/Login");
            }

            ErrorMessage = "Registration failed";
            return Page();
        }
    }
}
