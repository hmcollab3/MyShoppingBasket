using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Domain.Repositories;
using Kantar.ShoppingBasket.Domain.Repositories.UnitOfWork;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class PurchaseUnitOfWork : IPurchaseUnitOfWork
    {
        private readonly IReceiptRepository receiptRepository;
        private readonly IBasketRepository basketRepository;

        private readonly ShoppingBasketContext context;

        public PurchaseUnitOfWork(
            IReceiptRepository receiptRepository,
            IBasketRepository basketRepository,
            ShoppingBasketContext context)
        {
            this.receiptRepository = receiptRepository;
            this.basketRepository = basketRepository;
            this.context = context;
        }

        public async Task Purchase(int basketId, decimal totalCost, string currency, IEnumerable<Domain.Model.ReceiptItem> items, CancellationToken ct)
        {
            using (var transaction = await context.Database.BeginTransactionAsync(ct))
            {
                try
                {
                    var receiptId = await receiptRepository.CreateReceipt(basketId, totalCost, currency, ct);

                    foreach (var item in items)
                    {
                        item.ReceiptId = receiptId;

                        await this.receiptRepository.CreateReceiptItem(item, ct);
                    }

                    await basketRepository.PurchaseBasket(basketId, ct);
                    await transaction.CommitAsync(ct);
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            }
        }
    }
}
