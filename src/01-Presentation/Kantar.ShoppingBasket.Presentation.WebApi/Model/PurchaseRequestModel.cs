namespace Kantar.ShoppingBasket.Presentation.WebApi.Model
{
    public class PurchaseRequestModel
    {
        public int BasketId { get; set; }

        public IEnumerable<CheckoutItemModel> CheckoutItemsForPurchase { get; set; }
    }
}
