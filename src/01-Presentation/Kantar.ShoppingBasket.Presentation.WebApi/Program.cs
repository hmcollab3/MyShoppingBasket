using Autofac;
using Autofac.Extensions.DependencyInjection;
using Kantar.ShoppingBasket.CrossCutting.Configurations;
using Kantar.ShoppingBasket.Presentation.WebApi.Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace Kantar.ShoppingBasket.Presentation.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            ConfigureServices(builder, config);

            Start(builder);
        }

        private static void ConfigureServices(WebApplicationBuilder builder, IConfiguration config)
        {
            builder.Services.AddControllers();
            builder.Services.AddRazorPages();

            ConfigureAuth(builder, config);
            ConfigureHttpClient(builder, config);
            ConfigureModules(builder, config);
        }

        private static void Start(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureModules(WebApplicationBuilder builder, IConfiguration config)
        {
            builder.Host
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutoMapperModule());
                    builder.RegisterModule(new DataModule(config));
                    builder.RegisterModule(new ApplicationModule());
                });
        }

        private static void ConfigureHttpClient(WebApplicationBuilder builder, IConfiguration config)
        {
            builder.Services.AddHttpClient("BasketApiClient", client =>
            {
                client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]);
            });

            builder.Services.AddHttpContextAccessor();
        }

        private static void ConfigureAuth(WebApplicationBuilder builder, IConfiguration config)
        {
            string jwtSecret = GetJwtToken(config);

            builder.Services.Configure<VaultOptions>(config.GetSection("Vault"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });
        }

        public static string GetJwtToken(IConfiguration config)
        {
            var vaultAddress = config["Vault:Address"];
            var vaultToken = config["Vault:Token"];
            var secretPath = config["Vault:SecretPath"];
            var secretMount = config["Vault:SecretMount"];
            var jwtSecretKey = config["Vault:JwtSecretKey"];

            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken);
            var vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);
            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            var secret = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: secretPath,
                mountPoint: secretMount
            ).GetAwaiter().GetResult();

            return secret.Data.Data[jwtSecretKey].ToString();
        }
    }
}
