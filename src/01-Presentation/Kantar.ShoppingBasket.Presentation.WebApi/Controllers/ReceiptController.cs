using AutoMapper;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Controllers
{
    [Route("api/receipt")]
    [ApiController]
    [Authorize]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService receiptService;
        private readonly IMapper mapper;

        public ReceiptController(
            IReceiptService receiptService,
            IMapper mapper)
        {
            this.receiptService = receiptService;
            this.mapper = mapper;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetReceiptHistory([FromHeader(Name = "X-Country-Id")] int countryId, CancellationToken ct)
        {
            var clientId = User.FindFirst("client_id")?.Value;

            if (clientId == null)
            {
                return Unauthorized("Invalid client id");
            }

            var receipts = await this.receiptService.GetAllAvailableReceipts(int.Parse(clientId), countryId, ct);

            return Ok(this.mapper.Map<IEnumerable<ReceiptModel>>(receipts));
        }

        [HttpGet("{receiptId}/download")]
        public async Task<IActionResult> GetSelectedReceipt([FromRoute] int receiptId, CancellationToken ct)
        {
            var receipt = await receiptService.GenerateReceipt(receiptId, ct);
            if (receipt == null)
            {
                return NotFound();
            }

            return File(receipt, "application/pdf", $"receipt_{receiptId}.pdf");
        }
    }
}
