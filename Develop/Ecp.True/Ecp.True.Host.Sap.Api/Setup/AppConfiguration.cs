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

namespace Ecp.True.Host.Sap.Api.Setup
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Common;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Options;

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

            app.UseMvc(SetupRoutes);
        }

        /// <summary>
        /// Setups the routes.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void SetupRoutes(IRouteBuilder builder)
        {
            // DI and Time Zone
            builder.EnableDependencyInjection();
            builder.SetTimeZoneInfo(TimeZoneInfo.Utc);

            builder.MapRoute("default", "{controller=App}/{action=Heartbeat}");
        }
    }
}
