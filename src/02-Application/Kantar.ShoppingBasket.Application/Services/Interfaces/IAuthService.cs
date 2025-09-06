using Kantar.ShoppingBasket.Application.Model;
using System.IdentityModel.Tokens.Jwt;

namespace Kantar.ShoppingBasket.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterClient(RegisterClientDto registerClient, CancellationToken ct);

        Task<JwtSecurityToken> Login(LoginRequestDto loginRequest, CancellationToken ct);
    }
}
