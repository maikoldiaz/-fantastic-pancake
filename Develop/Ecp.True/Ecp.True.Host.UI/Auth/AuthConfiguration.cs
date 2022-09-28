// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Auth
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Auth.Token;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Azure.Storage;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;

    /// <summary>
    /// The authentication configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AuthConfiguration
    {
        /// <summary>
        /// The eco petrol API scope.
        /// </summary>
        private const string EcoPetrolApiScope = "EcoPetrolApi:EcoPetrolApiScope";

        /// <summary>
        /// The graph API scope.
        /// </summary>
        private const string GraphApiScope = "GraphApi:GraphScope";

        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentValidators.ThrowIfNull(services, nameof(services));
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddSignIn("AzureAD", configuration, options => configuration.Bind("AzureAD", options));

            if (Debugger.IsAttached)
            {
                ConfigureLocalAuthCache(services, configuration);
            }
            else
            {
                ConfigureDistAuthCache(services, configuration);
            }

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                ConfigureOpenIdConnectOptions(options, configuration);
            });

            services.AddSingleton<ITicketStore, RedisCacheTicketStore>();
            services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>();

            services.Configure<GraphInfo>(configuration);
        }

        private static void ConfigureLocalAuthCache(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddWebAppCallsProtectedWebApi(configuration, new[] { configuration[EcoPetrolApiScope], configuration[GraphApiScope] })
                .AddInMemoryTokenCaches();
        }

        private static void ConfigureDistAuthCache(IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["Settings:RedisConnectionString"];

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));

            var storageAccount = CloudStorageAccount.Parse(configuration["Settings:StorageConnectionString"]);
            var keyVaultName = configuration["Settings:VaultName"];

            services.AddDataProtection()
                .SetApplicationName("True_WebApp")
                .ProtectKeysWithAzureKeyVault(keyVaultClient, $"https://{keyVaultName}.vault.azure.net/keys/dataprotection")
                .PersistKeysToAzureBlobStorage(storageAccount, "/dataprotectionkeys/keys.xml");

            services
                .AddWebAppCallsProtectedWebApi(configuration, new[] { configuration[EcoPetrolApiScope], configuration[GraphApiScope] })
                .AddRedisTokenCaches(redisConnectionString, "Token");
        }

        private static void ConfigureOpenIdConnectOptions(OpenIdConnectOptions options, IConfiguration configuration)
        {
            // Use the groups claim for populating roles.
            options.TokenValidationParameters.RoleClaimType = "groups";

            options.Events.OnRedirectToIdentityProvider = context =>
            {
                if (!Debugger.IsAttached)
                {
                    var security = configuration["AzureAd:SecurityLevel"];

                    // Force the user to login everytime.
                    context.ProtocolMessage.Prompt = security;
                }

                return Task.CompletedTask;
            };

            options.Events.OnRemoteFailure = context =>
            {
                if (context?.Failure?.Message?.Contains("AADSTS50105", System.StringComparison.OrdinalIgnoreCase) == true)
                {
                    context.HandleResponse();
                    context.Response.Redirect("error/unauthorized");
                }

                return Task.CompletedTask;
            };
        }
    }
}
