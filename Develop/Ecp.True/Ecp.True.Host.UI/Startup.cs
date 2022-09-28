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

namespace Ecp.True.Host.UI
{
    using System.Diagnostics;
    using Autofac;
    using Ecp.True.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Setup;
    using Ecp.True.Ioc;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The startup.
    /// </summary>
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
        /// The configure services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ArgumentValidators.ThrowIfNull(services, nameof(services));
            services.ConfigureHostServices(this.Configuration);
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ArgumentValidators.ThrowIfNull(app, nameof(app));
            ArgumentValidators.ThrowIfNull(env, nameof(env));

            if (Debugger.IsAttached)
            {
                PortFinder.Instance.Configure(env.ContentRootPath);
            }

            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{env?.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

            this.Configuration = builder.Build();
            app.ConfigureApplication(env);
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
