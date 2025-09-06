namespace Kantar.ShoppingBasket.Domain.Model
{
    public class GetMultiBuyDiscount
    {
        public DateTime StartDateUtc { get; set; }

        public DateTime EndDateUtc { get; set; }

        public int AffectedProductId { get; set; }

        public decimal DiscountFactor { get; set; }

        public int TriggeringProductId { get; set; }

        public int TriggerQuantity { get; set; }
    }
}
