using Kantar.ShoppingBasket.Application.Model.Enums;
using Kantar.ShoppingBasket.Application.Providers.Interfaces;
using Kantar.ShoppingBasket.CrossCutting.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace Kantar.ShoppingBasket.Application.Providers
{
    public class JwtTokenProvider : ITokenProvider
    {
        private readonly VaultOptions vaultOptions;

        public JwtTokenProvider(IOptions<VaultOptions> vaultOptions)
        {
            this.vaultOptions = vaultOptions.Value;
        }

        public async Task<JwtSecurityToken> GenerateToken(int clientId, ClientRole role)
        {
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultOptions.Token);
            var vaultClientSettings = new VaultClientSettings(vaultOptions.Address, authMethod);

            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            var secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: vaultOptions.SecretPath,
                mountPoint: vaultOptions.SecretMount
            );

            string jwtSecret = null;
            if (secret?.Data?.Data != null && secret.Data.Data.TryGetValue(vaultOptions.JwtSecretKey, out var secretObj))
            {
                jwtSecret = secretObj?.ToString().Trim();
            }

            var claims = new[]
            {
                new Claim("client_id", clientId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);
        }
    }
}