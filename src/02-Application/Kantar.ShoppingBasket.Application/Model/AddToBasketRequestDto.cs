namespace Kantar.ShoppingBasket.Application.Model
{
    public class AddToBasketRequestDto
    {
        public int BasketId { get; set; }

        public Dictionary<int, int> QuantityByProductId { get; set; }
    }
}
