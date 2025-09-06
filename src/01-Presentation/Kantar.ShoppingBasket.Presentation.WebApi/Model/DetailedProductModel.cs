namespace Kantar.ShoppingBasket.Presentation.WebApi.Model
{
    public class DetailedProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Currency { get; set; }

        public decimal BasePrice { get; set; }
    }
}
