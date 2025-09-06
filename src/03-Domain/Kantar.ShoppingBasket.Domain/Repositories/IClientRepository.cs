using Kantar.ShoppingBasket.Domain.Model;

namespace Kantar.ShoppingBasket.Domain.Repositories
{
    public interface IClientRepository
    {
        Task InsertClient(SetClient clientToInsert, CancellationToken ct);

        Task<RegisteredClient> GetRegisteredClient(string clientEmail, string clientHashedPassword, CancellationToken ct);
    }
}
