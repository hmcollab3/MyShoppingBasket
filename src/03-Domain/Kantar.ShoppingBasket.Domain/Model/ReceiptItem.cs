namespace Kantar.ShoppingBasket.Domain.Model
{
    public class ReceiptItem
    {
        public int ReceiptId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal MultiBuyTotalSavings { get; set; }

        public int MultiBuyDiscountedCount { get; set; }

        public decimal ItemTotalCost { get; set; }
    }
}
