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

namespace Ecp.True.Host.Api.Setup
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
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
            services.AddApiVersioning();
            services.ConfigureAuth(configuration);

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(Core.SetupOptions.AddLocalizationOptions);

            services.AddOData();
            services
                .AddMvc(Core.SetupOptions.AddMvcOptions)
                .AddDataAnnotationsLocalization(o => Core.SetupOptions.AddDataAnnotationLocalizationOptions(o, SetupOptions.BuildSharedLocalizer))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(SetupOptions.AddJsonOptions)
                .ConfigureApiBehaviorOptions(Core.SetupOptions.AddApiOptions);

            services.AddOpenApiDocument(OpenApiSettings.AddOpenApiConfiguration);

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });
            services.AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>();
        }
    }
}
