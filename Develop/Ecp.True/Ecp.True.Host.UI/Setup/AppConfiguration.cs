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

namespace Ecp.True.Host.UI.Setup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Common;
    using Ecp.True.Host.UI.Hub;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.Extensions.Hosting;

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
        /// <param name="env">The env.</param>
        public static void ConfigureApplication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            ArgumentValidators.ThrowIfNull(app, nameof(app));
            ArgumentValidators.ThrowIfNull(env, nameof(env));

            app.UseMiddleware(typeof(RequestHandler));
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ConfigFile = @"webpack.config.dev.js",
                    EnvironmentVariables = new Dictionary<string, string>
                    {
                       { "NODE_OPTIONS", "--max-old-space-size=4086" },
                    },
                });
                TelemetryDebugWriter.IsTracingDisabled = true;
            }
            else
            {
                app.UseHsts(SetupOptions.AddHstsOptions);
                app.UseStaticFiles(SetupOptions.BuildStaticFileOptions());
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            SetupHeaders(app);

            // The authentication middleware is placed before SignalR and Mvc.
            app.UseAuthentication();

            app.UseAzureSignalR(routes => routes.MapHub<ConcurrentEdit>("/concurrentEdit"));
            app.UseMvc(builder => SetupRoutes(builder));
        }

        /// <summary>
        /// Setups the routes.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void SetupRoutes(IRouteBuilder builder)
        {
            // DI and Time Zone
            builder.SetTimeZoneInfo(TimeZoneInfo.Utc);
            builder.MapRoute("default", "{controller=Account}/{action=SignIn}");
        }

        private static void SetupHeaders(IApplicationBuilder app)
        {
            // Registered before static files to always set header
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());

            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());

            // Registered after static files, to set headers for dynamic content.
            app.UseXfo(xfo => xfo.SameOrigin());

            // All these below whitelist is intentionally hardcoded. If something else needs to be added, should be done below
            app.UseCsp(opts => opts
               .DefaultSources(s => s.Self())
               .ScriptSources(s => s.Self().CustomSources("*.microsoft.com"))
               .StyleSources(s => s.Self().UnsafeInline().CustomSources("fonts.googleapis.com"))
               .FontSources(f => f.Self().CustomSources("fonts.gstatic.com", "data:"))
               .ImageSources(i => i.Self().CustomSources("data:"))
               .ConnectSources(c =>
                    c.Self().CustomSources("*.windows.net", "*.powerbi.com", "*.microsoftonline.com", "*.microsoft.com", "wss:", "ws:", "*.service.signalr.net", "*.services.visualstudio.com"))
               .FrameSources(f => f.Self().CustomSources("*.powerbi.com", "*.microsoft.com")));
        }
    }
}