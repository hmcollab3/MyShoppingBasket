using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Model.Enums;
using Kantar.ShoppingBasket.Application.Providers.Interfaces;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Domain.Model;
using Kantar.ShoppingBasket.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kantar.ShoppingBasket.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientRepository clientRepository;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ITokenProvider tokenProvider;
        private readonly IMapper mapper;

        public AuthService(
            IClientRepository clientRepository,
            IEncryptionProvider encryptionProvider,
            ITokenProvider tokenProvider,
            IMapper mapper)
        {
            this.clientRepository = clientRepository;
            this.encryptionProvider = encryptionProvider;
            this.tokenProvider = tokenProvider;
            this.mapper = mapper;
        }

        public async Task<JwtSecurityToken> Login(LoginRequestDto loginRequest, CancellationToken ct)
        {
            var hashedPassword = this.encryptionProvider.Encrypt(loginRequest.Password);

            var validClient = await this.clientRepository.GetRegisteredClient(loginRequest.Email, hashedPassword, ct);

            if (validClient != null)
            {
                var clientRole = Enum.TryParse<ClientRole>(validClient.Role, out var role) ?
                        role :
                        ClientRole.Client;

                return await this.tokenProvider.GenerateToken(validClient.Id, clientRole);
            }

            return null;
        }

        public async Task RegisterClient(RegisterClientDto registerClient, CancellationToken ct)
        {  
            var hashedPassword = this.encryptionProvider.Encrypt(registerClient.Password);

            var clientToInsert = new SetClient
            {
                Name = registerClient.Name,
                Email = registerClient.Email,
                PasswordHash = hashedPassword,
                Role = Enum.TryParse<ClientRole>(registerClient.Role, out _) ?
                    registerClient.Role :
                    ClientRole.Client.ToString(),
            };

            await this.clientRepository.InsertClient(clientToInsert, ct);
        }
    }
}
