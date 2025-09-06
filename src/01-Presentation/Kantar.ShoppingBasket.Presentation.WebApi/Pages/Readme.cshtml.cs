using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Pages
{
    public class ReadmeModel : PageModel
    {
        public string ReadmeText { get; set; }

        public void OnGet()
        {
            ReadmeText = @"
Project Overview

This project was a fun rabbit-hole to step into. While I’m disappointed that I didn’t deliver everything I wanted, I’ve decided to submit what is complete, accepting the limits caused by various factors—including my own choices. 
In this README, I’ll explain my thought process and outline what I would do differently if given another chance.

---

Usability

Home Page

- The main page is the login screen, where users can click a button to either log in or register, each with their own page.
- Upon registering, users are automatically redirected to the login page.
- At the top right corner, users can select the country context for their session.
- Changing the country will always clear the session and prompt the user to log in again.
- The same account can be used regardless of the selected country, but the country will affect the experience moving forward.

Product Display Page

- Products are loaded based on the selected country, with prices varying accordingly.
- Upon visiting, each client is assigned a basket that persists selected items for future reuse.
- Clients can select items to add to their basket and click 'Add To Basket' to save their selection.
- Clients can proceed to checkout or view their purchase history via dedicated buttons.

Checkout

- Clients see a summary of basket items, including applied discounts and final prices both per item and for the entire basket.
- Discounts are applied in the following way:
    - First, the system checks for any valid direct discounts (based on product, discount period, and checkout date).
    - Next, it looks for valid multi-buy discounts, considering the quantity of other items and direct discount rules.
    - If multiple discounts apply, the system chooses the best savings, accounting for both direct and multi-buy discounts.
    - Multi-buy discounts are applied to individual items, with remaining items receiving the direct discount.
- Clients can return to the products page if needed.
- Clicking 'Purchase' processes the order, registers the purchase as a receipt, and redirects the client to the products page.

History

- Clients can view their past receipts (country-aware) for purchases made.
- Each receipt includes an option to generate and download a PDF, which is triggered directly in the browser.

---

What I Would Do Differently

Comments and Code Quality

- I wanted to add comments to all interface methods and inherit them in each implementation.
- Proper documentation, including diagrams, would have helped illustrate the application.
- More validations and checkpoints were needed; the code is exposed, though partially controlled via API usage.
- I dedicated time to testing, but some unnecessary behaviors remain, like redundant page loads and temp data usage.

API Improvements

- I intended to use FluentValidation for model validation.
- I wanted to implement explicit state management for basket and receipt operations, but ended up using cookies as a workaround.
- Logging was on my roadmap (ideally with Serilog and Elasticsearch, fully containerized).
- Middleware for handling request headers and authentication would have streamlined the process.
- Model binding would have made data access and validation easier.
- The code structure became fragmented, but time constraints led me to prioritize finishing a working app over these refinements.

Frontend

- I wanted to refactor page handlers to reduce clutter and adhere to Single Responsibility and Open/Closed Principles.
- My focus on segregating pages and keeping the API clean delayed some simple fixes, such as sharing page logic.

Automated Tests

- I was unable to complete all requested automated tests, but here’s what I intended:
    - Unit Tests: Use data-driven testing and expand beyond the single rushed test class.
    - Integration Tests: Create a dedicated test utilities project for configuration and setup, plus a containerized test database.
    - UI & BDD Tests: Use Selenium for UI tests and Reqnroll (BDD) for backend integration tests.
    - Both test strategies would operate on disposable containers, with each test cleaning up after itself in the integration test database.
    - After all tests, containers would be disposed of.

---

Final Notes

Thank you.
";
        }
    }
}