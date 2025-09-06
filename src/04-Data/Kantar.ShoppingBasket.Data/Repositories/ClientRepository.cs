using AutoMapper;
using Kantar.ShoppingBasket.Data.Context;
using Kantar.ShoppingBasket.Data.Model;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kantar.ShoppingBasket.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ShoppingBasketContext context;
        private readonly IMapper mapper;

        public ClientRepository(
            ShoppingBasketContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<RegisteredClient> GetRegisteredClient(string clientEmail, string clientHashedPassword, CancellationToken ct)
        {
            var client = await this.context
                .Set<Client>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    c => c.Email == clientEmail && c.PasswordHash == clientHashedPassword,
                    ct);

            if (client == null)
                return null;

            return this.mapper.Map<RegisteredClient>(client);
        }

        public async Task InsertClient(SetClient clientToInsert, CancellationToken ct)
        {
            var entry = this.mapper.Map<Client>(clientToInsert);

            await context.Set<Client>().AddAsync(entry, ct);

            await context.SaveChangesAsync(ct);
        }
    }
}
