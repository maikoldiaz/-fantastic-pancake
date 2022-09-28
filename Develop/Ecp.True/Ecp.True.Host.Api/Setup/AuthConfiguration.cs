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

namespace Ecp.True.Host.Api.Setup
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web;

    /// <summary>
    /// The authentication configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AuthConfiguration
    {
        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentValidators.ThrowIfNull(services, nameof(services));
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));

            services.AddProtectedWebApi(configuration);
            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                // Use the groups claim for populating roles
                options.TokenValidationParameters.RoleClaimType = "groups";
            });

            services.Configure<GraphInfo>(configuration);
        }
    }
}
