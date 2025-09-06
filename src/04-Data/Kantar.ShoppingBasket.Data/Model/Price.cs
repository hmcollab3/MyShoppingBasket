namespace Kantar.ShoppingBasket.Data.Model
{
    public class Price
    {
        public int ProductId { get; set; }

        public int CountryId { get; set; }

        public string Currency { get; set; }

        public decimal BasePrice { get; set; }
    }
}
