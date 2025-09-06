namespace Kantar.ShoppingBasket.Application.Model
{
    public class DetailedReceiptDto
    {
        public ReceiptDto Receipt { get; set; }

        public IEnumerable<ReceiptItemDto> ReceiptItems { get; set; }
    }
}
