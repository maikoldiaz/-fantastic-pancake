// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Microsoft.AspNetCore.Mvc.ApiController]
[assembly: Microsoft.AspNetCore.Mvc.ApiConventionType(typeof(Ecp.True.Host.Shared.WebApiConventions))]

namespace Ecp.True.Host.Api
{
    using System.Diagnostics.CodeAnalysis;
    using Autofac;
    using Ecp.True.Core;
    using Ecp.True.Host.Api.Setup;
    using Ecp.True.Ioc;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ArgumentValidators.ThrowIfNull(services, nameof(services));
            services.ConfigureHostServices(this.Configuration);
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env?.ContentRootPath)
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{env?.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

            this.Configuration = builder.Build();
            app.ConfigureApplication();
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            IoCManager.RegisterByConvention(builder);
        }
    }
}
