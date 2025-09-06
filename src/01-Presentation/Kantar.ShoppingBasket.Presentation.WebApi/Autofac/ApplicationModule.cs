using Autofac;
using Kantar.ShoppingBasket.Application.Adapters;
using Kantar.ShoppingBasket.Application.Adapters.Interfaces;
using Kantar.ShoppingBasket.Application.Providers;
using Kantar.ShoppingBasket.Application.Providers.Interfaces;
using Kantar.ShoppingBasket.Application.Services;
using Kantar.ShoppingBasket.Application.Services.Interfaces;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Autofac
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductService>()
                .As<IProductService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CountryService>()
                .As<ICountryService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BasketService>()
                .As<IBasketService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EncryptionProvider>()
                .As<IEncryptionProvider>()
                .SingleInstance();

            builder.RegisterType<JwtTokenProvider>()
                .As<ITokenProvider>()
                .SingleInstance();

            builder.RegisterType<ProductAdapter>()
                .As<IProductAdapter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DiscountAdapter>()
                .As<IDiscountAdapter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PurchaseAdapter>()
                .As<IPurchaseAdapter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReceiptService>()
                .As<IReceiptService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PDFFileProvider>()
                .As<IFileProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
