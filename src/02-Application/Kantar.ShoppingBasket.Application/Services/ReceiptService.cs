using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Providers.Interfaces;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Domain.Repositories;

namespace Kantar.ShoppingBasket.Application.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository receiptRepository;
        private readonly IFileProvider fileProvider;
        private readonly IMapper mapper;

        public ReceiptService(
            IReceiptRepository receiptRepository,
            IFileProvider fileProvider,
            IMapper mapper)
        {
            this.receiptRepository = receiptRepository;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
        }

        public async Task<byte[]?> GenerateReceipt(int receiptId, CancellationToken ct)
        {
            var detailedReceipt = await receiptRepository.GetDetailedReceipt(receiptId, ct);

            if (detailedReceipt == null)
            {
                return null;
            }

            return this.fileProvider.GenerateFile(detailedReceipt);
        }

        public async Task<IEnumerable<ReceiptDto>> GetAllAvailableReceipts(int clientId, int countryId, CancellationToken ct)
        {
            var result = await this.receiptRepository.GetReceipts(clientId, countryId, ct);

            return this.mapper.Map<IEnumerable<ReceiptDto>>(result);
        }
    }
}
