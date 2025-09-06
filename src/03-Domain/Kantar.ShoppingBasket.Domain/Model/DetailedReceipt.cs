namespace Kantar.ShoppingBasket.Domain.Model
{
    public class DetailedReceipt
    {
        public Receipt Receipt { get; set; }

        public IEnumerable<ReceiptItem> ReceiptItems { get; set; }
    }
}
