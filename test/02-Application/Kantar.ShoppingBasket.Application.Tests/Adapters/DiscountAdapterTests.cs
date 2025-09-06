using AutoFixture;
using FluentAssertions;
using Kantar.ShoppingBasket.Application.Adapters;
using Kantar.ShoppingBasket.Application.Adapters.Interfaces;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Moq;

namespace Kantar.ShoppingBasket.Application.Tests.Adapters
{
    [TestClass]
    public class DiscountAdapterTests
    {
        private Mock<IDiscountRepository> discountRepositoryMock;
        private IDiscountAdapter discountAdapter;
        private Fixture fixture;

        [TestInitialize]
        public void Initialize()
        {
            discountRepositoryMock = new Mock<IDiscountRepository>();
            discountAdapter = new DiscountAdapter(discountRepositoryMock.Object);
            fixture = new Fixture();
        }

        [TestMethod]
        public async Task GetAvailableDiscount_DelegatesToRepository_ReturnsValue()
        {
            // Arrange
            int productId = fixture.Create<int>();
            decimal expectedDiscount = fixture.Create<decimal>();
            CancellationToken cancellationToken = CancellationToken.None;
            discountRepositoryMock
                .Setup(r => r.GetBestDirectDiscount(productId, cancellationToken))
                .ReturnsAsync(expectedDiscount);

            // Act
            decimal? actualDiscount = await discountAdapter.GetAvailableDiscount(productId, cancellationToken);

            // Assert
            actualDiscount.Should().Be(expectedDiscount);
        }

        [TestMethod]
        public async Task GetAvailableDiscount_DelegatesToRepository_ReturnsNull()
        {
            // Arrange
            int productId = fixture.Create<int>();
            CancellationToken cancellationToken = CancellationToken.None;
            discountRepositoryMock
                .Setup(r => r.GetBestDirectDiscount(productId, cancellationToken))
                .ReturnsAsync((decimal?)null);

            // Act
            decimal? actualDiscount = await discountAdapter.GetAvailableDiscount(productId, cancellationToken);

            // Assert
            actualDiscount.Should().BeNull();
        }

        [TestMethod]
        public async Task CalculateMultiBuyDiscount_NoDiscounts_ReturnsDefaultDto()
        {
            // Arrange
            int productId = fixture.Create<int>();
            decimal availableDiscount = fixture.Create<decimal>();
            List<BasketItemModel> currentItems = fixture.Create<List<BasketItemModel>>();
            CancellationToken cancellationToken = CancellationToken.None;

            discountRepositoryMock
                .Setup(r => r.GetAllMultiBuyDiscounts(productId, availableDiscount, cancellationToken))
                .ReturnsAsync(new List<GetMultiBuyDiscount>());

            // Act
            MultiBuyDiscountResultDto result = await discountAdapter.CalculateMultiBuyDiscount(productId, availableDiscount, currentItems, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(new MultiBuyDiscountResultDto());
        }

        [TestMethod]
        public async Task CalculateMultiBuyDiscount_AppliesDiscount_WhenEligible()
        {
            // Arrange
            int productId = 1;
            decimal availableDiscount = 0.0m;
            CancellationToken cancellationToken = CancellationToken.None;

            var multiBuyDiscounts = new List<GetMultiBuyDiscount>
            {
                new GetMultiBuyDiscount
                {
                    TriggeringProductId = 1,
                    TriggerQuantity = 2,
                    DiscountFactor = 0.5m,
                    StartDateUtc = DateTime.UtcNow.AddDays(-1),
                    EndDateUtc = DateTime.UtcNow.AddDays(10),
                    AffectedProductId = 1
                }
            };

            var currentItems = new List<BasketItemModel>
            {
                new BasketItemModel { ProductId = 1, Quantity = 4, OriginalPrice = 10m, Name = "Prod1", BasketId = 1, Currency = "USD" },
                new BasketItemModel { ProductId = 2, Quantity = 2, OriginalPrice = 20m, Name = "Prod2", BasketId = 1, Currency = "USD" }
            };

            discountRepositoryMock
                .Setup(r => r.GetAllMultiBuyDiscounts(productId, availableDiscount, cancellationToken))
                .ReturnsAsync(multiBuyDiscounts);

            // Act
            MultiBuyDiscountResultDto result = await discountAdapter.CalculateMultiBuyDiscount(productId, availableDiscount, currentItems, cancellationToken);

            // Assert
            result.DiscountedItemCount.Should().Be(2);
            result.TotalSavings.Should().BeApproximately(10m, 0.001m);
        }

        [TestMethod]
        public async Task CalculateMultiBuyDiscount_NoAffectedItems_ReturnsZeroSavings()
        {
            // Arrange
            int productId = 1;
            decimal availableDiscount = 0.0m;
            CancellationToken cancellationToken = CancellationToken.None;

            var multiBuyDiscounts = new List<GetMultiBuyDiscount>
            {
                new GetMultiBuyDiscount
                {
                    TriggeringProductId = 3,
                    TriggerQuantity = 5,
                    DiscountFactor = 0.2m,
                    StartDateUtc = DateTime.UtcNow.AddDays(-1),
                    EndDateUtc = DateTime.UtcNow.AddDays(10),
                    AffectedProductId = 1
                }
            };

            var currentItems = new List<BasketItemModel>
            {
                new BasketItemModel { ProductId = 3, Quantity = 10, OriginalPrice = 15m, Name = "Prod3", BasketId = 1, Currency = "USD" },
                new BasketItemModel { ProductId = 8, Quantity = 2, OriginalPrice = 30m, Name = "Prod8", BasketId = 1, Currency = "USD" }
            };

            discountRepositoryMock
                .Setup(r => r.GetAllMultiBuyDiscounts(productId, availableDiscount, cancellationToken))
                .ReturnsAsync(multiBuyDiscounts);

            // Act
            MultiBuyDiscountResultDto result = await discountAdapter.CalculateMultiBuyDiscount(productId, availableDiscount, currentItems, cancellationToken);

            // Assert
            result.DiscountedItemCount.Should().Be(0);
            result.TotalSavings.Should().Be(0m);
        }

        [TestMethod]
        public async Task CalculateMultiBuyDiscount_MultipleDiscounts_SelectsBestScenario()
        {
            // Arrange
            int productId = 1;
            decimal availableDiscount = 0m;
            CancellationToken cancellationToken = CancellationToken.None;

            var multiBuyDiscounts = new List<GetMultiBuyDiscount>
            {
                new GetMultiBuyDiscount
                {
                    TriggeringProductId = 1,
                    TriggerQuantity = 1,
                    DiscountFactor = 0.1m,
                    StartDateUtc = DateTime.UtcNow.AddDays(-1),
                    EndDateUtc = DateTime.UtcNow.AddDays(10),
                    AffectedProductId = 1
                },
                new GetMultiBuyDiscount
                {
                    TriggeringProductId = 1,
                    TriggerQuantity = 2,
                    DiscountFactor = 0.2m,
                    StartDateUtc = DateTime.UtcNow.AddDays(-1),
                    EndDateUtc = DateTime.UtcNow.AddDays(10),
                    AffectedProductId = 1
                }
            };

            var currentItems = new List<BasketItemModel>
            {
                new BasketItemModel { ProductId = 1, Quantity = 2, OriginalPrice = 100m, Name = "Prod1", BasketId = 1, Currency = "USD" }
            };

            discountRepositoryMock
                .Setup(r => r.GetAllMultiBuyDiscounts(productId, availableDiscount, cancellationToken))
                .ReturnsAsync(multiBuyDiscounts);

            // Act
            MultiBuyDiscountResultDto result = await discountAdapter.CalculateMultiBuyDiscount(productId, availableDiscount, currentItems, cancellationToken);

            // Assert
            result.DiscountedItemCount.Should().Be(2);
            result.TotalSavings.Should().BeApproximately(20m, 0.001m); 
            /**
             * Explanation
             * Should be 20 because 100m * 2 = 200. Having 2 itemsd we get the highest discount of 0.2, and that should be 200 * 0.2 = 40. However, this is applied only over 1 product, so only 20M is saved.
             */
        }
    }
}