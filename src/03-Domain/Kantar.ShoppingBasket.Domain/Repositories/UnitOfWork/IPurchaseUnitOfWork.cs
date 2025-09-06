namespace Kantar.ShoppingBasket.Domain.Repositories.UnitOfWork
{
    public interface IPurchaseUnitOfWork
    {
        Task Purchase(int basketId, decimal totalCost, string currency, IEnumerable<Domain.Model.ReceiptItem> items, CancellationToken ct);

    }
}
