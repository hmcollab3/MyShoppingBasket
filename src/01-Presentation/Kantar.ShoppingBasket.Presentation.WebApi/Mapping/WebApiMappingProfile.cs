using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Mapping
{
    public class WebApiMappingProfile : Profile
    {
        public WebApiMappingProfile()
        {
            this.CreateMap<DetailedProductDto, DetailedProductModel>();
            this.CreateMap<LoginRequestModel, LoginRequestDto>();
            this.CreateMap<RegisterClientModel, RegisterClientDto>();
            this.CreateMap<CountryDto, CountryModel>();
            this.CreateMap<AddToBasketRequestModel, AddToBasketRequestDto>();
            this.CreateMap<CheckoutItemDto, CheckoutItemModel>().ReverseMap();
            this.CreateMap<MultiBuyDiscountResultDto, MultiBuyDiscountResultModel>().ReverseMap();
            this.CreateMap<ReceiptDto, ReceiptModel>();
        }
    }
}
