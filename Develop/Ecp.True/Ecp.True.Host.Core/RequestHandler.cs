// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestHandler.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Common
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Core;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// Request Handling Middleware For following things:
    /// 1. Sets Activity ID in the request.
    /// 2. Maps resolver to Request level container.
    /// 3. Handle exception and add logs.
    /// 4. Bootstrap factories.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class RequestHandler : IDisposable
    {
        /// <summary>
        /// The failed to write error details.
        /// </summary>
        private readonly string failedToWriteErrorDetails = "Cannot write error details in response";

        /// <summary>
        /// The next.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// The thread limiter.
        /// </summary>
        private readonly SemaphoreSlim semaphoreSlim;

        /// <summary>
        /// The flag to indicate whether it is initialized.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="shouldBootStrap">The flag to indicate whether to bootstrap.</param>
        public RequestHandler(RequestDelegate next)
        {
            this.next = next;
            this.semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">Generate request Id.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task InvokeAsync(HttpContext context, IConfigurationHandler configuration)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));

            try
            {
                // 1. Setup application factories like connection and SQL
                await this.BootstrapFactoriesAsync(context).ConfigureAwait(false);
                await SetupAzureProxyAsync(context).ConfigureAwait(false);

                // 3. Setup SQL Access token
                await SetUpSqlAccessTokenAsync(context).ConfigureAwait(false);

                // 4. Setup chaos manager
                SetupChaosManager(context);

                // 5. Execute the next middleware on pipeline.
                await this.next(context).ConfigureAwait(false);

                // 6. Change the response code in case of ASP NET Identity middleware returns 302 for Idle browser client.
                ValidateLogoutRedirect(context);
            }
            catch (Exception ex)
            {
                await this.LogExceptionAsync(context, ex).ConfigureAwait(false);
                var throwExceptions = await configuration.GetConfigurationAsync<bool>(ConfigurationConstants.ThrowExceptions).ConfigureAwait(false);
                if (throwExceptions)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.semaphoreSlim.Dispose();
        }

        private static Task SetUpSqlAccessTokenAsync(HttpContext context)
        {
            var sqlTokenProvider = (ISqlTokenProvider)context.RequestServices.GetService(typeof(ISqlTokenProvider));
            return sqlTokenProvider.InitializeAsync();
        }

        private static void SetupChaosManager(HttpContext context)
        {
            var chaosManager = (IChaosManager)context.RequestServices.GetService(typeof(IChaosManager));
            chaosManager.Initialize(context.GetChaosHeaderValue());
        }

        private static void ValidateLogoutRedirect(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status302Found && context.Request.Headers.ContainsKey(Constants.TrueOriginHeader))
            {
                context.Response.Headers[Constants.RedirectPathOnAuthFailureHeader] = Constants.RedirectPathOnAuthFailurePath;
                context.Response.StatusCode = 401;
            }
        }

        private static async Task SetupAzureProxyAsync(HttpContext context)
        {
            var configurationHandler = (IConfigurationHandler)context.RequestServices.GetService(typeof(IConfigurationHandler));

            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);
            var blockchainSettings = await configurationHandler.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings).ConfigureAwait(false);
            var analysisSettings = await configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);

            var azureConfiguration = new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings);
            azureConfiguration.QuorumProfile = new QuorumProfile
            {
                Address = blockchainSettings.EthereumAccountAddress,
                PrivateKey = blockchainSettings.EthereumAccountKey,
                RpcEndpoint = blockchainSettings.RpcEndpoint,
                ClientId = blockchainSettings.ClientId,
                ClientSecret = blockchainSettings.ClientSecret,
                TenantId = blockchainSettings.TenantId,
                ResourceId = blockchainSettings.ResourceId,
            };

            var azureClientFactory = (IAzureClientFactory)context.RequestServices.GetService(typeof(IAzureClientFactory));
            azureClientFactory.Initialize(azureConfiguration);
        }

        private async Task LogExceptionAsync(HttpContext context, Exception exception)
        {
            var logger = (ITrueLogger<RequestHandler>)context.RequestServices.GetService(typeof(ITrueLogger<RequestHandler>));
            logger.LogError(exception, exception.Message);

            if (context.Response.HasStarted)
            {
                logger.LogError(exception, this.failedToWriteErrorDetails);
            }
            else
            {
                var code = HttpStatusCode.InternalServerError;
                if (exception is UnauthorizedAccessException)
                {
                    code = HttpStatusCode.Unauthorized;
                }

                var result = new { exception.Message, RequestId = Trace.CorrelationManager.ActivityId.ToString("D", CultureInfo.InvariantCulture) };
                try
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)code;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result), CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception responseEx)
                {
                    logger.LogError(responseEx, this.failedToWriteErrorDetails);
                }
            }
        }

        /// <summary>
        /// Bootstraps the factories.
        /// </summary>
        /// <param name="context">The context.</param>
        private async Task BootstrapFactoriesAsync(HttpContext context)
        {
            if (this.isInitialized)
            {
                return;
            }

            try
            {
                await this.semaphoreSlim.WaitAsync().ConfigureAwait(false);

                if (this.isInitialized)
                {
                    return;
                }

                ArgumentValidators.ThrowIfNull(context, nameof(context));

                var configurationHandler = (IConfigurationHandler)context.RequestServices.GetService(typeof(IConfigurationHandler));
                await configurationHandler.InitializeAsync().ConfigureAwait(false);

                var connectionString = await configurationHandler.GetConfigurationAsync(ConfigurationConstants.ConfigConnectionString).ConfigureAwait(false);

                var connectionFactory = (IConnectionFactory)context.RequestServices.GetService(typeof(IConnectionFactory));
                connectionFactory.SetupStorageConnection(connectionString);

                var sqlConfig = await configurationHandler.GetConfigurationAsync<SqlConnectionSettings>(ConfigurationConstants.SqlConnectionSettings).ConfigureAwait(false);
                connectionFactory.SetupSqlConfig(sqlConfig);

                var systemSettings = await configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);
                EnableTrueQueryAttribute.Initialize(systemSettings);

                var cacheBootstrapper = (ICacheBootstrapper)context.RequestServices.GetService(typeof(ICacheBootstrapper));
                var cacheSettings = await configurationHandler.GetConfigurationAsync<CacheSettings>(ConfigurationConstants.CacheSettings).ConfigureAwait(false);
                cacheBootstrapper.Setup(CacheMode.DistributedOnly, TimeSpan.FromMinutes(cacheSettings.Expiration), cacheSettings.RedisConnectionString, cacheSettings.Sliding);

                this.isInitialized = true;
            }
            catch (Exception exception)
            {
                var logger = (ITrueLogger<RequestHandler>)context.RequestServices.GetService(typeof(ITrueLogger<RequestHandler>));
                logger.LogError(exception, exception.Message);
            }
            finally
            {
                this.semaphoreSlim.Release();
            }
        }
    }
}