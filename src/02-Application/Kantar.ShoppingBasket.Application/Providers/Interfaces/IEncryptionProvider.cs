namespace Kantar.ShoppingBasket.Application.Providers.Interfaces
{
    public interface IEncryptionProvider
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);
    }
}
