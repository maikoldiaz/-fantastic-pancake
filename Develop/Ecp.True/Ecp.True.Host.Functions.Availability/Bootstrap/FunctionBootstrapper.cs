// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionBootstrapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Availability.Bootstrap
{
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Processors.Availability;
    using Ecp.True.Processors.Availability.Interfaces;
    using Ecp.True.Processors.Availability.Services;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The transform bootstrap service.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Setup.Bootstrapper" />
    public class FunctionBootstrapper : Bootstrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBootstrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public FunctionBootstrapper(IServiceCollection services)
            : base(services)
        {
        }

        /// <inheritdoc/>
        protected override void RegisterServices()
        {
            this.RegisterTransient<IAvailabilityProcessor, AvailabilityProcessor>();
            this.RegisterTransient<IHttpClientProxy, HttpClientProxy>();
            this.RegisterTransient<IResourceService, ResourceService>();
            this.RegisterTransient<IMetricService, MetricService>();
            this.RegisterTransient<IAzureManagementApiClient, AzureManagementApiClient>();

            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}
