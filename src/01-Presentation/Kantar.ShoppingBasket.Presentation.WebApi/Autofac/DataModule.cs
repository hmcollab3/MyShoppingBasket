using Autofac;
using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Repositories;
using Kantar.ShoppingBasket.Domain.Repositories;
using Kantar.ShoppingBasket.Domain.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Autofac
{
    public class DataModule : Module
    {
        private readonly IConfiguration configuration;

        public DataModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            this.RegisterDbContext(builder);

            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientRepository>()
                .As<IClientRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryRepository>()
                .As<ICountryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BasketRepository>()
                .As<IBasketRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DiscountRepository>()
                .As<IDiscountRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReceiptRepository>()
                .As<IReceiptRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PurchaseUnitOfWork>()
                .As<IPurchaseUnitOfWork>()
                .InstancePerLifetimeScope();
        }

        private void RegisterDbContext(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ShoppingBasketContext>();

                    optionsBuilder.UseSqlServer(
                        configuration.GetConnectionString("SqlServer")
                    );
                    return new ShoppingBasketContext(optionsBuilder.Options);
                })
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
