using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages.Handler
{
    public class SetCountryModel : PageModel
    {
        public IActionResult OnPost(string country)
        {
            Response.Cookies.Append("CountryId", country, new CookieOptions
            {
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("BasketId");

            return RedirectToPage("/Index");
        }
    }
}
