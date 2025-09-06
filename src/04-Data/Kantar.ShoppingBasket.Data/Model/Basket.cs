namespace Kantar.ShoppingBasket.Data.Model
{
    public class Basket
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Status { get; set; }

        public int CountryId { get; set; }
    }
}
