namespace Kantar.ShoppingBasket.Domain.Model
{
    public class DetailedProduct : PricedProduct
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Currency { get; set; }
    }
}
