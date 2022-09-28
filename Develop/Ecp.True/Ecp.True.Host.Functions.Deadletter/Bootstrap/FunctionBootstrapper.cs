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

namespace Ecp.True.Host.Functions.Deadletter.Bootstrap
{
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Deadletter;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Processors.Deadletter.Reconciler;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Retry;

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
            this.RegisterTransient<IDeadletterProcessor, DeadletterProcessor>();
            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IEthereumClient, EthereumClient>();
            this.RegisterSingleton<IRetryPolicyFactory, RetryPolicyFactory>();
            this.RegisterSingleton<IRetryHandler, EthereumRetryHandler>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();

            // Reconcilers
            this.RegisterTransient<IReconciler, InventoryProductReconciler>();
            this.RegisterTransient<IReconciler, MovementReconciler>();
            this.RegisterTransient<IReconciler, NodeConnectionReconciler>();
            this.RegisterTransient<IReconciler, NodeReconciler>();
            this.RegisterTransient<IReconciler, OwnershipReconciler>();
            this.RegisterTransient<IReconciler, UnbalanceReconciler>();
            this.RegisterTransient<IReconcileService, ReconcileService>();
            this.RegisterTransient<IReconciler, OwnerReconciler>();

            // Failure Handlers
            this.RegisterTransient<IFailureHandlerFactory, FailureHandlerFactory>();
            this.RegisterTransient<IFailureHandler, CutOffFailureHandler>();
            this.RegisterTransient<IFailureHandler, OwnershipFailureHandler>();
            this.RegisterTransient<IFailureHandler, LogisticsFailureHandler>();
        }
    }
}
