using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Domain.Repositories
{
    public interface IReceiptRepository
    {
        Task<int> CreateReceipt(int basketId, decimal totalCost, string currency, CancellationToken ct);

        Task CreateReceiptItem(ReceiptItem receiptItem, CancellationToken ct);

        Task<IEnumerable<Receipt>> GetReceipts(int clientId, int countryId, CancellationToken ct);

        Task<DetailedReceipt> GetDetailedReceipt(int receiptId, CancellationToken ct);
    }
}
