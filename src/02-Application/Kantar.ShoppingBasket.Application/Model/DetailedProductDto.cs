namespace Kantar.ShoppingBasket.Application.Model
{
    public class DetailedProductDto : ProductDto
    {
        public string Currency { get; set; }

        public decimal BasePrice { get; set; }
    }
}
