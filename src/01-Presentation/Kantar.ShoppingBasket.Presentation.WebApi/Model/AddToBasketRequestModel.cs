namespace Kantar.ShoppingBasket.Presentation.WebApi.Model
{
    public class AddToBasketRequestModel
    {
        public AddToBasketRequestModel()
        {
            this.QuantityByProductId = new Dictionary<int, int>();
        }

        public int BasketId { get; set; }

        public Dictionary<int, int> QuantityByProductId { get; set; }
    }
}
