// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The function bootstrap service.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ConfigurationBootstrapper configuration;

        /// <summary>
        /// The data access.
        /// </summary>
        private readonly DataAccessBootsrapper dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        protected Bootstrapper(IServiceCollection services)
            : base(services)
        {
            this.configuration = new ConfigurationBootstrapper(services);
            this.dataAccess = new DataAccessBootsrapper(services);
        }

        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        public void RegisterDependencies()
        {
            this.configuration.Register();
            this.dataAccess.Register();
            this.RegisterServices();
        }

        /// <summary>
        /// Registers the services.
        /// </summary>
        protected abstract void RegisterServices();
    }
}
