using AutoMapper;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Data.Mapping
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            this.CreateMap<SetClient, Client>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            this.CreateMap<Client, RegisteredClient>()
                .ForSourceMember(src => src.PasswordHash, opt => opt.DoNotValidate());

            this.CreateMap<Country, GetCountry>()
                .ForMember(dest => dest.CountryIsoCode, opt => opt.MapFrom(src => src.ISO3166));

            this.CreateMap<BasketItemModel, BasketItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            this.CreateMap<Data.Model.ReceiptItem, Domain.Model.ReceiptItem>().ReverseMap();

            this.CreateMap<Data.Model.Receipt, Domain.Model.Receipt>();

            this.CreateMap<MultiBuyDiscount, GetMultiBuyDiscount>();
        }
    }
}
