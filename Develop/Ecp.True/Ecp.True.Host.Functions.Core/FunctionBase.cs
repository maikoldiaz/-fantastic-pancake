// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;

    /// <summary>
    /// The functions base type.
    /// </summary>
    public class FunctionBase
    {
        /// <summary>
        /// The semaphore slim.
        /// </summary>
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBase"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        protected FunctionBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Bootstraps the function.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected async Task TryInitializeAsync()
        {
            var sqlTokenProvider = this.Resolve<ISqlTokenProvider>();
            var connectionFactory = this.Resolve<IConnectionFactory>();
            if (connectionFactory.IsReady)
            {
                await sqlTokenProvider.InitializeAsync().ConfigureAwait(false);
                await this.DoInitializeAsync().ConfigureAwait(false);
                return;
            }

            await SemaphoreSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                if (connectionFactory.IsReady)
                {
                    await sqlTokenProvider.InitializeAsync().ConfigureAwait(false);
                    await this.DoInitializeAsync().ConfigureAwait(false);
                    return;
                }

                var configurationHandler = this.Resolve<IConfigurationHandler>();
                await configurationHandler.InitializeAsync().ConfigureAwait(false);

                // Setup storage connection string
                var connectionString = await configurationHandler.GetConfigurationAsync(ConfigurationConstants.ConfigConnectionString).ConfigureAwait(false);
                connectionFactory.SetupStorageConnection(connectionString);

                // Setup sql connection string
                var commandTimeout = await configurationHandler.GetConfigurationAsync<int?>(ConfigurationConstants.CommandTimeoutInSecs).ConfigureAwait(false);
                var sqlConfig = await configurationHandler.GetConfigurationAsync<SqlConnectionSettings>(ConfigurationConstants.SqlConnectionSettings).ConfigureAwait(false);

                sqlConfig.CommandTimeoutInSecs = commandTimeout;
                connectionFactory.SetupSqlConfig(sqlConfig);

                // Setup MSI token for Sql Db Context
                await sqlTokenProvider.InitializeAsync().ConfigureAwait(false);

                // Call initialize template
                await this.DoInitializeAsync().ConfigureAwait(false);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected virtual Task DoInitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>The service.</returns>
        protected TService Resolve<TService>()
            where TService : class
        {
            return (TService)this.serviceProvider.GetService(typeof(TService));
        }

        /// <summary>
        /// Tries the trigger chaos.
        /// </summary>
        /// <param name="chaosValue">The chaos value.</param>
        /// <param name="caller">The caller.</param>
        /// <param name="replyTo">The reply to.</param>
        protected void ProcessMetadata(string chaosValue, string caller, string replyTo)
        {
            var chaosManager = this.Resolve<IChaosManager>();
            this.InitializeChaos(chaosValue);
            this.LogMetadata(replyTo);

            var value = chaosManager.TryTriggerChaos(caller);

            if (!string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(value);
            }
        }

        /// <summary>
        /// Initializes the chaos.
        /// </summary>
        /// <param name="chaosValue">The chaos value.</param>
        protected void InitializeChaos(string chaosValue)
        {
            var chaosManager = this.Resolve<IChaosManager>();
            chaosManager.Initialize(chaosValue);
        }

        private void LogMetadata(string replyTo)
        {
            if (string.IsNullOrWhiteSpace(replyTo))
            {
                return;
            }

            var logger = this.Resolve<ITrueLogger<FunctionBase>>();
            logger.LogInformation($"Message received with property {replyTo}", "DeadLetter");
        }
    }
}
