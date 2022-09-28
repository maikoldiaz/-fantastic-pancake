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

namespace Ecp.True.Host.Flow.Api.Setup
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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

            services
                .AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
                .AddAzureADBearer(options => configuration.Bind("AzureAd", options));

            services.Configure<JwtBearerOptions>(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            {
                options.TokenValidationParameters.ValidAudiences = new[] { options.Audience, $"api://{options.Audience}" };
            });
        }
    }
}
