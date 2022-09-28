// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Setup
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.UI.Auth;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The service configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureHostServices(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentValidators.ThrowIfNull(services, nameof(services));
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));

            services.AddApplicationInsightsTelemetry();

            services.ConfigureHttpServices();
            services.ConfigureAuth(configuration);
            services
                .AddSignalR()
                .AddAzureSignalR(configuration["Settings:SignalRConnectionString"]);

            services
                .AddMvc(SetupOptions.AddMvcOptions)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(SetupOptions.AddJsonOptions);
        }
    }
}
