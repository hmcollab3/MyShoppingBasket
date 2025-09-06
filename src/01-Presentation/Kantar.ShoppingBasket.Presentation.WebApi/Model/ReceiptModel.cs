namespace Kantar.ShoppingBasket.Presentation.WebApi.Model
{
    public class ReceiptModel
    {
        public int Id { get; set; }

        public int BasketId { get; set; }

        public decimal TotalCost { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }
    }
}
