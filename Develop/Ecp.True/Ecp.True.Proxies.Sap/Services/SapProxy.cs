// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapProxy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Proxies.Sap.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Ecp.True.Proxies.Sap.Request;
    using Ecp.True.Proxies.Sap.Response;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The sap proxy.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Sap.Interfaces.ISapProxy" />
    public class SapProxy : ISapProxy
    {
        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly ISapClient sapClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapProxy> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapProxy"/> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="sapClient">The sap client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public SapProxy(
            IConfigurationHandler configurationHandler,
            ISapClient sapClient,
            ITrueLogger<SapProxy> logger,
            IAzureClientFactory azureClientFactory)
        {
            this.configurationHandler = configurationHandler;
            this.sapClient = sapClient;
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Gets the Mapping  asynchronous.
        /// </summary>
        /// <returns>
        /// The sap mappings.
        /// </returns>
        public async Task<IEnumerable<SapMappingResponse>> GetMappingsAsync()
        {
            var configuration = await this.configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            this.sapClient.Initialize(configuration);
            var result = await this.sapClient.GetAsync(configuration.MappingPath).ConfigureAwait(false);
            return await result.Content.DeserializeHttpContentAsync<IEnumerable<SapMappingResponse>>().ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the movement transfer point asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <returns>The boolean.</returns>
        [Obsolete("This Method is Deprecated", false)]
        public async Task<SapTrackingStatus> UpdateMovementTransferPointAsync(SapMovementRequest movement, int movementTransactionId)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var configuration = await this.configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            this.sapClient.Initialize(configuration);
            var payload = new List<SapMovementRequest> { movement };
            await this.LogRequestAsync(payload, movementTransactionId).ConfigureAwait(false);
            var result = await this.sapClient.PostAsync(configuration.TransferPointPath, payload).ConfigureAwait(false);
            var errors = await result.Content.DeserializeHttpContentAsync<SapTrackingStatus>().ConfigureAwait(false);
            errors.IsSuccess = result.IsSuccessStatusCode;
            return errors;
        }

        /// <summary>
        /// Updates the upload status asynchronous.
        /// </summary>
        /// <param name="sapUpload">The sap upload.</param>
        /// <returns>
        /// The boolean.
        /// </returns>
        public async Task<bool> UpdateUploadStatusAsync(SapUploadStatus sapUpload)
        {
            var configuration = await this.configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            this.sapClient.Initialize(configuration);
            var result = await this.sapClient.PostAsync(configuration.UploadStatusPath, sapUpload).ConfigureAwait(false);
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Updates the upload status contract asynchronous.
        /// </summary>
        /// <param name="sapUpload">The sap upload.</param>
        /// <returns>
        /// The boolean.
        /// </returns>
        public async Task<bool> UpdateUploadStatusContractAsync(SapUploadStatusContract sapUpload)
        {
            var configuration = await this.configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            this.sapClient.Initialize(configuration);

            string json = JsonConvert.SerializeObject(new SapUploadStatusContractOutput { SapUploadStatusContract = sapUpload }, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented,
            });

            var result = await this.sapClient.PostAsync(configuration.UploadStatusContractPath, JsonConvert.DeserializeObject(json)).ConfigureAwait(false);
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Send operating balance information with property and official deltas.
        /// </summary>
        /// <param name="sapUpload">The sap upload.</param>
        /// <returns>
        /// The boolean.
        /// </returns>
        public async Task<bool> SendLogisticMovementAsync(object sapUpload)
        {
            var configuration = await this.configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            this.sapClient.Initialize(configuration);
            var result = await this.sapClient.PostAsync(configuration.SendLogisticMovementsPath, sapUpload).ConfigureAwait(false);
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Creates the sap BLOB asynchronous.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <returns>The path.</returns>
        private async Task LogRequestAsync(object payload, int movementTransactionId)
        {
            var blobPath = string.Format(CultureInfo.InvariantCulture, Constants.TransferPointBlobStoragePath, movementTransactionId);
            var data = JsonConvert.SerializeObject(payload);
            await this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, blobPath).CreateBlobAsync(data).ConfigureAwait(false);
            this.logger.LogInformation($"Blob created successfully for movement : {movementTransactionId}", data);
        }
    }
}
