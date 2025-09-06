namespace Kantar.ShoppingBasket.Presentation.WebApi.Model
{
    public class CurrentBasketModel
    {
        public CurrentBasketModel()
        {
            this.QuantityByProduct = new Dictionary<int, int>();
        }

        public int BasketId { get; set; }

        public Dictionary<int, int> QuantityByProduct { get; set; }
    }
}
