// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapTrackingProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Sap.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class SapTrackingProcessor.
    /// </summary>
    public class SapTrackingProcessor : ISapTrackingProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapStatusProcessor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapTrackingProcessor"/> class.
        /// Constructor SapTrackingProcessor.
        /// </summary>
        /// <param name="unitOfWorkFactory">unitOfWork.</param>
        /// <param name="azureClientFactory">azureClientFactory.</param>
        /// <param name="logger">logger.</param>
        public SapTrackingProcessor(
             IUnitOfWorkFactory unitOfWorkFactory,
             IAzureClientFactory azureClientFactory,
             ITrueLogger<SapStatusProcessor> logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Creates the BLOB asynchronous.
        /// </summary>
        /// <param name="processId">processId.</param>
        /// <param name="payload">payload.</param>
        /// <returns>string.</returns>
        public async Task<string> CreateBlobAsync(string processId, object payload)
        {
            var blobName = $"sap/out/{processId}";
            var data = JsonConvert.SerializeObject(payload, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            await this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, blobName).CreateBlobAsync(data).ConfigureAwait(false);
            this.logger.LogInformation($"Blob created successfully {processId}", $"{processId}");
            return blobName;
        }

        /// <summary>
        /// Inserts the sap tracking status.
        /// </summary>
        /// <param name="fileRegistrationId">The file registration identifier.</param>
        /// <param name="statusType">if set to <c>true</c> [status type].</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="blobPath">The BLOB path.</param>
        /// <returns>The Task.</returns>
        public async Task InsertSapTrackingAsync(int fileRegistrationId, bool statusType, string errorMessage, string blobPath)
        {
            var sapTracking = new SapTracking
            {
                FileRegistrationId = fileRegistrationId,
                StatusTypeId = statusType ? StatusType.PROCESSED : StatusType.FAILED,
                OperationalDate = DateTime.UtcNow.ToTrue(),
                Comment = string.Empty,
                ErrorMessage = errorMessage,
                BlobPath = blobPath,
            };
            var sapTrackingRepository = this.unitOfWork.CreateRepository<SapTracking>();
            sapTrackingRepository.Insert(sapTracking);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
