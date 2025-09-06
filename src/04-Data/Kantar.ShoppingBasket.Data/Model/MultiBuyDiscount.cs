namespace Kantar.ShoppingBasket.Data.Model
{
    public class MultiBuyDiscount
    {
        public int Id { get; set; }

        public DateTime StartDateUtc { get; set; }

        public DateTime EndDateUtc { get; set; }

        public int AffectedProductId { get; set; }

        public decimal DiscountFactor { get; set; }

        public int TriggeringProductId { get; set; }

        public int TriggerQuantity { get; set; }
    }
}
