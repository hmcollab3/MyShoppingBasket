using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Domain.Repositories;

namespace Kantar.ShoppingBasket.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DetailedProductDto>> GetAllAvailableProducts(int countryId, CancellationToken ct)
        {
            var entires = await this.productRepository.GetAllAvailableProducts(countryId, ct);

            return this.mapper.Map<IEnumerable<DetailedProductDto>>(entires);
        }
    }
}