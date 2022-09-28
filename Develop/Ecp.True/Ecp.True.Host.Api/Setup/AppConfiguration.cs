// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppConfiguration.cs" company="Microsoft">
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
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Api.OData;
    using Ecp.True.Host.Common;
    using Ecp.True.Host.Core;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Options;
    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;

    /// <summary>
    /// The application configuration extension.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AppConfiguration
    {
        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void ConfigureApplication(this IApplicationBuilder app)
        {
            ArgumentValidators.ThrowIfNull(app, nameof(app));

            app.UseMiddleware(typeof(RequestHandler));
            app.UseForwardedHeaders();

            if (Debugger.IsAttached)
            {
                app.UseDeveloperExceptionPage();
                TelemetryDebugWriter.IsTracingDisabled = true;
            }

            // Registered before static files to always set header
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());

            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());

            // Registered after static files, to set headers for dynamic content.
            app.UseXfo(xfo => xfo.Deny());

            // Registered after static files, to set headers only for dynamic content.
            app.UseNoCacheHttpHeaders();

            // Register this earlier if there's middleware that might redirect.
            app.UseRedirectValidation(r => r.AllowSameHostRedirectsToHttps());

            app.UseOpenApi();
            app.UseSwaggerUi3();

            var options = (IOptions<RequestLocalizationOptions>)app.ApplicationServices.GetService(typeof(IOptions<RequestLocalizationOptions>));
            app.UseRequestLocalization(options.Value);
            app.UseAuthentication();

            app.UseMvc(builder => SetupRoutes(app, builder));
        }

        /// <summary>
        /// Setups the routes.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="builder">The builder.</param>
        private static void SetupRoutes(IApplicationBuilder app, IRouteBuilder builder)
        {
            // Setup ODATA
            var edmModel = ODataModel.GetEdmModel(app?.ApplicationServices);

            builder.MapODataServiceRoute(HostConstants.OData, "odata", b =>
                b.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(EdmModel), sp => edmModel)
                 .AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), sp => new StringAsEnumResolver()));

            builder.MapODataServiceRoute("ODataVersioned", "odata/v1", edmModel);
            builder.Select().Filter().Expand().Count().OrderBy().MaxTop(1000);

            // DI and Time Zone
            builder.EnableDependencyInjection();
            builder.SetTimeZoneInfo(TimeZoneInfo.Utc);

            builder.MapRoute("default", "{controller=App}/{action=Heatbeat}");
        }
    }
}
