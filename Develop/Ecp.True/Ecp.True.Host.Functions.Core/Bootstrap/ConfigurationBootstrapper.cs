// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationBootstrapper.cs" company="Microsoft">
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
    using Ecp.True.Chaos;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.NoSql;
    using Ecp.True.Host.Functions.Core.Setup;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore;
    using Ecp.True.KeyStore.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The configuration bootstrap service.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Setup.BootstrapperBase" />
    [ExcludeFromCodeCoverage]
    public class ConfigurationBootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBootstrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public ConfigurationBootstrapper(IServiceCollection services)
            : base(services)
        {
        }

        /// <summary>
        /// Boostraps the configuration.
        /// </summary>
        public void Register()
        {
            this.RegisterSingleton<IApplicationInsightsResolver, ApplicationInsightsResolver>();
            this.RegisterSingleton<ITelemetry, Telemetry>();
            this.RegisterSingleton<IResolver, FunctionResolver>();
            this.RegisterScoped(typeof(ITrueLogger<>), typeof(TrueLogger<>));

            this.RegisterSingleton<IConfigurationHandler, ConfigurationHandler>();
            this.RegisterSingleton<IConfigurationStoreFactory, ConfigurationStoreFactory>();
            this.RegisterSingleton<ITableProvider, TableProvider>();
            this.RegisterTransient<IFileConfigurationStore, FileConfigurationStore>();
            this.RegisterSingleton<ISecretManagementProvider, SecretManagementProvider>();
            this.RegisterSingleton<IKeyVaultContext, KeyVaultContext>();
            this.RegisterSingleton<IKeyVaultClientFactory, KeyVaultClientFactory>();
            this.RegisterSingleton<IKeyVaultExtensionsWrapper, KeyVaultExtensionsWrapper>();
            this.RegisterScoped<IBusinessContext, BusinessContext>();
            this.RegisterScoped<IChaosManager, ChaosManager>();
            this.RegisterScoped<IDeadLetterManager, DeadLetterManager>();
            this.RegisterSingleton<ITokenProvider, TokenProvider>();

            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();

            this.RegisterSingleton<IMessageSerializerSettingsFactory, TrueDurableFunctionMessageSerializer>();
        }
    }
}
