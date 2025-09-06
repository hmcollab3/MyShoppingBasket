using Kantar.ShoppingBasket.Application.Adapters.Interfaces;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using System.Threading;

namespace Kantar.ShoppingBasket.Application.Adapters
{
    public class ProductAdapter : IProductAdapter
    {
        private readonly IProductRepository productRepository;

        public ProductAdapter(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Task<(string name, decimal price, string currency)> GetProductPrice(int productId, int countryId, CancellationToken ct)
        {
            return this.productRepository.GetProductPrice(productId, countryId, ct);
        }
    }
}
