namespace Kantar.ShoppingBasket.Data.Model
{
    public class Receipt
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public decimal TotalCost { get; set; }
        public string Currency { get; set; }
        public DateTime CreationDateUtc { get; set; }
    }
}
