using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Application.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            this.CreateMap<DetailedProduct, DetailedProductDto>();
            this.CreateMap<GetCountry, CountryDto>();
            this.CreateMap<CheckoutItemDto, ReceiptItem>()
                .ForMember(dest => dest.MultiBuyTotalSavings, opt => opt.MapFrom(src => src.MultiBuyDiscountSavings != null ? src.MultiBuyDiscountSavings.TotalSavings : 0))
                .ForMember(dest => dest.MultiBuyDiscountedCount, opt => opt.MapFrom(src => src.MultiBuyDiscountSavings != null ? src.MultiBuyDiscountSavings.DiscountedItemCount : 0))
                .ForMember(dest => dest.ItemTotalCost, opt => opt.MapFrom(src => src.TotalCost))
                .ForMember(dest => dest.ReceiptId, opt => opt.Ignore());
            this.CreateMap<Receipt, ReceiptDto>();
            this.CreateMap<ReceiptItem, ReceiptItemDto>();
        }
    }
}
