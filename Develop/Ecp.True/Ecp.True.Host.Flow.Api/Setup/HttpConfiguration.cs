// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpConfiguration.cs" company="Microsoft">
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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The service configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpConfiguration
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureHttpServices(this IServiceCollection services)
        {
            ArgumentValidators.ThrowIfNull(services, nameof(services));

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            if (!Debugger.IsAttached)
            {
                services.AddHttpsRedirection(o =>
                {
                    o.HttpsPort = 443;
                });
            }

            services.AddHttpClient(Constants.DefaultHttpClient);
            services.AddHttpContextAccessor();

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.Expiration = HostConstants.DefaultSessionTimeout;
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }
    }
}
