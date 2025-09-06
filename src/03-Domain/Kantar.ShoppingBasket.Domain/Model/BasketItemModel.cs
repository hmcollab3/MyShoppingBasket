namespace Kantar.ShoppingBasket.Domain.Model
{
    public class BasketItemModel
    {
        public int BasketId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal? PriceAtCheckout { get; set; }
    }
}
