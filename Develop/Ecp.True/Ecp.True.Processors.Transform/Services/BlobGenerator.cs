// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Transform.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The blob generator.
    /// </summary>
    public class BlobGenerator : IBlobGenerator
    {
        /// <summary>
        /// The XML BLOB downloaded.
        /// </summary>
        private readonly string blobGenerationFailed = "Blob generation failed.";

        /// <summary>
        /// The movement ID.
        /// </summary>
        private readonly string blobPath = "BlobPath";

        /// <summary>
        /// The BLOB client.
        /// </summary>
        private readonly IAzureClientFactory clientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<BlobGenerator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobGenerator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="clientFactory">The client factory.</param>
        public BlobGenerator(ITrueLogger<BlobGenerator> logger, IAzureClientFactory clientFactory)
        {
            ArgumentValidators.ThrowIfNull(clientFactory, nameof(clientFactory));

            this.logger = logger;
            this.clientFactory = clientFactory;
        }

        /// <inheritdoc/>
        public Task GenerateBlobsArrayAsync(JArray array, string blobPath, string containerName)
        {
            ArgumentValidators.ThrowIfNull(array, nameof(array));

            var blobEntity = JsonConvert.SerializeObject(array, Formatting.Indented);
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(blobEntity ?? string.Empty));
            return this.clientFactory.GetBlobStorageSaSClient(ContainerName.True, blobPath).CreateBlobAsync(blobEntity);
        }

        /// <inheritdoc/>
        public Task GenerateBlobsArrayAsync(JArray homologatedArray, TrueMessage trueMessage)
        {
            ArgumentValidators.ThrowIfNull(trueMessage, nameof(trueMessage));
            ArgumentValidators.ThrowIfNull(homologatedArray, nameof(homologatedArray));

            var tasks = new List<Task>();
            tasks.AddRange(homologatedArray.Select(h => this.GenerateBlobAndUpdateErrorsAsync(h, trueMessage)));

            return Task.WhenAll(tasks);
        }

        /// <inheritdoc/>
        public Task GenerateBlobsAsync(JObject objectToSave, string blobPath, string containerName)
        {
            ArgumentValidators.ThrowIfNull(objectToSave, nameof(objectToSave));

            var blobEntity = JsonConvert.SerializeObject(objectToSave, Formatting.Indented);
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(blobEntity ?? string.Empty));
            return this.clientFactory.GetBlobStorageSaSClient(ContainerName.True, blobPath).CreateBlobAsync(blobEntity);
        }

        /// <inheritdoc/>
        public Task GenerateBlobsAsync(JObject objectToSave, TrueMessage trueMessage)
        {
            ArgumentValidators.ThrowIfNull(trueMessage, nameof(trueMessage));
            ArgumentValidators.ThrowIfNull(objectToSave, nameof(objectToSave));

            var tasks = new List<Task>
            {
                this.GenerateBlobAndUpdateErrorsAsync(objectToSave, trueMessage),
            };

            return Task.WhenAll(tasks);
        }

        private async Task GenerateBlobAndUpdateErrorsAsync(JToken item, TrueMessage trueMessage)
        {
            try
            {
                if (item[Constants.Type] != null && item[Constants.Type].ToString() == MessageType.Inventory.ToString())
                {
                    var existingIdentifier = item[Constants.InventoryProductUniqueId].ToString();
                    var inventoryUniqueId = IdGenerator.GenerateInventoryProductUniqueId(item);

                    item[Constants.InventoryProductUniqueId] = inventoryUniqueId;
                    item[this.blobPath] = item[this.blobPath].ToString().Replace(existingIdentifier, inventoryUniqueId, StringComparison.InvariantCulture);

                    foreach (var fileRegistrationTransaction in trueMessage.FileRegistration.FileRegistrationTransactions.Where(a => a.SessionId == existingIdentifier))
                    {
                        fileRegistrationTransaction.SessionId = inventoryUniqueId;
                        fileRegistrationTransaction.BlobPath =
                            fileRegistrationTransaction.BlobPath.Replace(existingIdentifier, inventoryUniqueId, StringComparison.InvariantCulture);
                    }
                }

                await this.clientFactory.GetBlobStorageSaSClient(ContainerName.True, item[this.blobPath].ToString()).CreateBlobAsync(item.ToString()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, this.blobGenerationFailed, trueMessage.MessageId);
                trueMessage.PopulatePendingTransactions(ex.Message, item[Constants.Type].ToString(), Constants.TechnicalExceptionParsingErrorMessage);
                if (trueMessage.FileRegistration?.FileRegistrationTransactions?.Count > 0 && item[this.blobPath] != null)
                {
                    trueMessage.FileRegistration.FileRegistrationTransactions.FirstOrDefault(x => x.BlobPath == item[this.blobPath].ToString()).StatusTypeId = StatusType.FAILED;
                }
            }
        }
    }
}
