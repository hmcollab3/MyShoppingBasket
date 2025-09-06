namespace Kantar.ShoppingBasket.Application.Model
{
    public class ReceiptDto
    {
        public int Id { get; set; }

        public int BasketId { get; set; }

        public decimal TotalCost { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }
    }
}
