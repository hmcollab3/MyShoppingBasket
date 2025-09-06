using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Application.Providers.Interfaces
{
    public interface IFileProvider
    {
        byte[]? GenerateFile(DetailedReceipt receipt);
    }
}
