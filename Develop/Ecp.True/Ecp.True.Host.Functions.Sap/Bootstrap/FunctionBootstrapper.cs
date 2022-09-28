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

namespace Ecp.True.Host.Functions.Sap.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Retry;
    using Ecp.True.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using SapInterfaces = Ecp.True.Proxies.Sap.Interfaces;
    using SapServices = Ecp.True.Proxies.Sap.Services;

    /// <summary>
    /// The transform bootstrap service.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Setup.Bootstrapper" />
    [ExcludeFromCodeCoverage]
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
            this.RegisterTransient<IFileRegistrationTransactionService, FileRegistrationTransactionService>();
            this.RegisterTransient<ISapProcessor, SapProcessor>();
            this.RegisterTransient<ISapStatusProcessor, SapStatusProcessor>();
            this.RegisterTransient<ISapTrackingProcessor, SapTrackingProcessor>();
            this.RegisterTransient<SapInterfaces.IHttpClientProxy, SapServices.HttpClientProxy>();
            this.RegisterTransient<SapInterfaces.ISapClient, SapServices.SapClient>();
            this.RegisterTransient<SapInterfaces.ISapProxy, SapServices.SapProxy>();
            this.RegisterSingleton<IRetryPolicyFactory, RetryPolicyFactory>();
            this.RegisterSingleton<IRetryHandler, SapRetryHandler>();
            this.RegisterTransient<IRepositoryFactory, RepositoryFactory>();
            this.RegisterTransient<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}