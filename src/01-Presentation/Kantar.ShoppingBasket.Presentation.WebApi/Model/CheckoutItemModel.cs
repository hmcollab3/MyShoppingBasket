namespace Kantar.ShoppingBasket.Presentation.WebApi.Model
{
    public class CheckoutItemModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public MultiBuyDiscountResultModel MultiBuyDiscountSavings { get; set; }

        public decimal TotalCost { get; set; }
    }
}
