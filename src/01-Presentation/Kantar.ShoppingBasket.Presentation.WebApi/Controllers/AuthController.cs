using AutoMapper;
using Kantar.ShoppingBasket.Application.Model;
using Kantar.ShoppingBasket.Application.Services.Interfaces;
using Kantar.ShoppingBasket.Presentation.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AuthController(
            IAuthService authService,
            IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request, CancellationToken ct)
        {
            try
            {
                var dto = this.mapper.Map<LoginRequestDto>(request);

                var token = await this.authService.Login(dto, ct);

                if (token != null)
                {
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }
            catch
            {
                return StatusCode(500, "Failed to retrieve JWT secret from Vault.");
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterClientModel request, CancellationToken ct)
        {
            try
            {
                var dto = this.mapper.Map<RegisterClientDto>(request);

                await this.authService.RegisterClient(dto, ct);

                return Ok(new { message = "Registration successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
