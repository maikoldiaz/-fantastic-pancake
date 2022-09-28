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

namespace Ecp.True.Host.Functions.Blockchain.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Blockchain;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Processors.Blockchain.Services;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Retry;
    using Microsoft.Extensions.DependencyInjection;

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
            // Register
            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IEthereumClient, EthereumClient>();
            this.RegisterSingleton<IRetryPolicyFactory, RetryPolicyFactory>();
            this.RegisterSingleton<IRetryHandler, EthereumRetryHandler>();

            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IBlockchainService, MovementBlockchainService>();
            this.RegisterTransient<IBlockchainService, InventoryProductBlockchainService>();
            this.RegisterTransient<IBlockchainService, OwnerBlockchainService>();
            this.RegisterTransient<IBlockchainService, NodeBlockchainService>();
            this.RegisterTransient<IBlockchainService, NodeConnectionBlockchainService>();
            this.RegisterTransient<IBlockchainService, UnbalanceBlockchainService>();
            this.RegisterTransient<IBlockchainService, OwnershipBlockchainService>();
            this.RegisterTransient<IBlockchainService, OfficialMovementBlockchainService>();
            this.RegisterTransient<IBlockchainServiceProvider, BlockchainServiceProvider>();
            this.RegisterTransient<IBlobOperations, BlobOperations>();
            this.RegisterTransient<IFileRegistrationTransactionService, FileRegistrationTransactionService>();
        }
    }
}
