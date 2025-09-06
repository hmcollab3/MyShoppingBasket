namespace Kantar.ShoppingBasket.Application.Model
{
    public class CheckoutItemDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public MultiBuyDiscountResultDto MultiBuyDiscountSavings { get; set; }

        public decimal TotalCost { get; set; }
    }
}
