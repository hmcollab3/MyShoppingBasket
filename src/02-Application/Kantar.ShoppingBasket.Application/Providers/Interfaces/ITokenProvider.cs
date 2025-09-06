using Kantar.ShoppingBasket.Application.Model.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Kantar.ShoppingBasket.Application.Providers.Interfaces
{
    public interface ITokenProvider
    {
        Task<JwtSecurityToken> GenerateToken(int clientId, ClientRole role);
    }
}
