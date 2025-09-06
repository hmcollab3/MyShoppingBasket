namespace Kantar.ShoppingBasket.Data.Model
{
    public class BasketItem
    {
        public int Id { get; set; }

        public int BasketId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal? PriceAtCheckout { get; set; }
    }
}
