// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainerConfiguration.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Bootstrap
{
    using Ecp.True.Blockchain.Entities;
    using Ecp.True.Blockchain.Interfaces;
    using Ecp.True.Blockchain.Processor;
    using Ecp.True.Blockchain.Services;
    using Ecp.True.Blockchain.SetUp;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The ContainerConfiguration.
    /// </summary>
    public static class ContainerConfiguration
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The ServiceProvider.
        /// </returns>
        public static ServiceProvider Configure(IConfiguration configuration)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IBlockchainProcessor, BlockchainProcessor>()
                                .AddScoped<IAzureClientFactory, AzureClientFactory>()
                                .AddSingleton<IEthereumClient, EthereumClient>()
                                .AddSingleton<IRetryPolicyFactory, RetryPolicyFactory>()
                                .AddSingleton<IRetryHandler, EthereumRetryHandler>()
                                .AddScoped<IKeyVaultSecretClient, KeyVaultSecretClient>()
                                .AddScoped<ITableStorageClient, TableStorageClient>();

            serviceCollection.AddOptions();
            serviceCollection.Configure<ConfigOptions>(a => configuration.Bind("ConfigOptions", a));

            return serviceCollection.BuildServiceProvider();
        }
    }
}
