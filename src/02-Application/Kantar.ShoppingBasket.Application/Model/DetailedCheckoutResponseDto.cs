namespace Kantar.ShoppingBasket.Application.Model
{
    public class DetailedCheckoutResponseDto
    {
        public IEnumerable<CheckoutItemDto> CheckoutProducts { get; set; }
    }
}
