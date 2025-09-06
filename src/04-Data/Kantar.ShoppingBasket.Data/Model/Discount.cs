namespace Kantar.ShoppingBasket.Data.Model
{
    public class Discount
    {
        public int Id { get; set; }

        public DateTime StartDateUtc { get; set; }

        public DateTime EndDateUtc { get; set; }

        public int AffectedProductId { get; set; }

        public decimal DiscountFactor { get; set; }
    }
}
