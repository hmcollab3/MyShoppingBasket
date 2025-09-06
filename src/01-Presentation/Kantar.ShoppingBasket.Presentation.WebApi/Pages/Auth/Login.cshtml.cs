using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty] public LoginRequestModel Input { get; set; }
        public string ErrorMessage { get; set; }

        private readonly HttpClient client;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            this.client = httpClientFactory.CreateClient("BasketApiClient");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await client.PostAsJsonAsync("api/auth/login", Input);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                var token = json.GetProperty("token").GetString();

                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                Response.Cookies.Delete("BasketId");

                return RedirectToPage("/Products/ProductListing");
            }
            ErrorMessage = "Invalid login.";
            return Page();
        }
    }
}
