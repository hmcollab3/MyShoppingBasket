using AutoMapper;
using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ShoppingBasketContext context;
        private readonly IMapper mapper;

        public ReceiptRepository(
            ShoppingBasketContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> CreateReceipt(int basketId, decimal totalCost, string currency, CancellationToken ct)
        {
            var receipt = new Data.Model.Receipt
            {
                BasketId = basketId,
                TotalCost = totalCost,
                Currency = currency,
                CreationDateUtc = DateTime.UtcNow,
            };

            await this.context.Set<Data.Model.Receipt>().AddAsync(receipt, ct);

            await this.context.SaveChangesAsync(ct);

            return receipt.Id;
        }

        public async Task CreateReceiptItem(Domain.Model.ReceiptItem receiptItem, CancellationToken ct)
        {
            var input = this.mapper.Map<Data.Model.ReceiptItem>(receiptItem);

            await this.context.Set<Data.Model.ReceiptItem>().AddAsync(input, ct);

            await this.context.SaveChangesAsync(ct);
        }

        public async Task<DetailedReceipt> GetDetailedReceipt(int receiptId, CancellationToken ct)
        {
            var receipt = await context.Set<Data.Model.Receipt>()
                .Where(r => r.Id == receiptId)
                .FirstOrDefaultAsync(ct);

            if (receipt == null)
                return null;

            var receiptItems = await context.Set<Data.Model.ReceiptItem>()
                .Where(ri => ri.ReceiptId == receiptId)
                .ToListAsync(ct);

            return new DetailedReceipt
            {
                Receipt = this.mapper.Map<Domain.Model.Receipt>(receipt),
                ReceiptItems = this.mapper.Map<IEnumerable<Domain.Model.ReceiptItem>>(receiptItems),
            };
        }

        public async Task<IEnumerable<Domain.Model.Receipt>> GetReceipts(int clientId, int countryId, CancellationToken ct)
        {
            var result = await context.Set<Data.Model.Receipt>()
                .Join(
                    context.Set<Basket>(),
                    receipt => receipt.BasketId,
                    basket => basket.Id,
                    (receipt, basket) => new { receipt, basket }
                )
                .Where(rb => rb.basket.ClientId == clientId && rb.basket.CountryId == countryId)
                .Select(rb => new Domain.Model.Receipt
                {
                    Id = rb.receipt.Id,
                    BasketId = rb.receipt.BasketId,
                    Currency = rb.receipt.Currency,
                    TotalCost = rb.receipt.TotalCost,
                    Date = rb.receipt.CreationDateUtc,
                })
                .ToListAsync(ct);

            return result;
        }
    }
}
