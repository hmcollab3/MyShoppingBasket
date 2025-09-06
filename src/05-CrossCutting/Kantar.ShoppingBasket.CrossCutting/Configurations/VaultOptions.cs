namespace Kantar.ShoppingBasket.CrossCutting.Configurations
{
    public class VaultOptions
    {
        public string Address { get; set; }
        public string Token { get; set; }
        public string SecretPath { get; set; }
        public string SecretMount { get; set; }
        public string JwtSecretKey { get; set; }
    }
}
