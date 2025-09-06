using Kantar.ShoppingBasket.Application.Model;

namespace Kantar.ShoppingBasket.Application.Services.Interfaces
{
    public interface IReceiptService
    {
        Task<IEnumerable<ReceiptDto>> GetAllAvailableReceipts(int clientId, int countryId, CancellationToken ct);

        Task<byte[]?> GenerateReceipt(int receiptId, CancellationToken ct);
    }
}
