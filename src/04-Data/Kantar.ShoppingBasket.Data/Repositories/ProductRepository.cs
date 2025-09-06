using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingBasketContext context;

        public ProductRepository(ShoppingBasketContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DetailedProduct>> GetAllAvailableProducts(int countryId, CancellationToken ct)
        {
            return await this.context
                .Set<Product>()
                .Join(
                    this.context.Set<Price>(),
                    product => product.Id,
                    price => price.ProductId,
                    (product, price) => new { Product = product, Price = price })
                .Where(pp => pp.Price.CountryId == countryId)
                .Select(pp => new DetailedProduct
                {
                    Id = pp.Product.Id,
                    Name = pp.Product.Name,
                    Description = pp.Product.Description,
                    BasePrice = pp.Price.BasePrice,
                    Currency = pp.Price.Currency,
                })
                .ToListAsync(ct);
        }

        public async Task<(string name, decimal price, string currency)> GetProductPrice(int productId, int countryId, CancellationToken ct)
        {
            var productPrice = await this.context
                .Set<Product>()
                .Join(
                    this.context.Set<Price>(),
                    product => product.Id,
                    price => price.ProductId,
                    (product, price) => new { Product = product, Price = price })
                .Where(pp => pp.Product.Id == productId && pp.Price.CountryId == countryId)
                .FirstAsync(ct);

            return (productPrice.Product.Name, productPrice.Price.BasePrice, productPrice.Price.Currency);
        }
    }
}
