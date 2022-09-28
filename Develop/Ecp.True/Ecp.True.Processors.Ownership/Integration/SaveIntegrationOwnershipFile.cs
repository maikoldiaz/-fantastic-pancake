// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveIntegrationOwnershipFile.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// cSpell:ignore FICO, Fico, fico, ownershiprule

namespace Ecp.True.Processors.Ownership.Integration
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The Ownership integration with FICO.
    /// </summary>
    public class SaveIntegrationOwnershipFile : ISaveIntegrationOwnershipFile
    {
        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveIntegrationOwnershipFile"/> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public SaveIntegrationOwnershipFile(
            IAzureClientFactory azureClientFactory,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            this.azureClientFactory = azureClientFactory;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Registers the integration fico.
        /// </summary>
        /// <param name="integrationData">
        /// The integration data.
        /// This object should contains:
        /// <list type="bullet">
        /// <item><term>Id</term></item>
        /// <item><term>Data</term></item>
        /// <item><term>Type</term></item>
        /// <item><term>IntegrationType</term></item>
        /// <item><term>PreviousUploadId</term></item>
        /// </list>
        /// </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<string> RegisterIntegrationAsync(IntegrationData integrationData)
        {
            ArgumentValidators.ThrowIfNull(integrationData, nameof(integrationData));

            integrationData.Identifier = Guid.NewGuid();
            integrationData.BlobName = $"{integrationData.Id}_{integrationData.IntegrationType.ToString().ToLowerCase()}.json";
            integrationData.BlobPath = $"ownershiprule/{integrationData.BlobName}";

            await this.SaveOwnershipToBlobAsync(integrationData.Data, integrationData.BlobPath).ConfigureAwait(false);

            await this.SaveResponseFileRegistrationAsync(integrationData).ConfigureAwait(false);

            return integrationData.Identifier.ToString();
        }

        /// <summary>
        /// Saves the Ownership to BLOB asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task SaveOwnershipToBlobAsync(string data, string blobName)
        {
            return this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.Ownership, blobName).CreateBlobAsync(data);
        }

        /// <summary>
        /// Saves the response file registration asynchronous.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task SaveResponseFileRegistrationAsync(IntegrationData form)
        {
            var fileRegistrationResponse = new FileRegistration
            {
                UploadId = form.Identifier.ToString(),
                MessageDate = DateTime.Now,
                Name = $"{form.BlobName}",
                ActionType = FileRegistrationActionType.Insert,
                FileUploadStatus = FileUploadStatus.FINALIZED,
                SystemTypeId = SystemType.FICO,
                SegmentId = null,
                BlobPath = $"{form.BlobPath}",
                HomologationInventoryBlobPath = null,
                HomologationMovementBlobPath = null,
                PreviousUploadId = string.IsNullOrWhiteSpace(form.PreviousUploadId) ? (Guid?)null : Guid.Parse(form.PreviousUploadId),
                IsParsed = false,
                SourceSystem = SystemType.FICO.ToString(),
                SourceTypeId = form.Type,
                IntegrationType = form.IntegrationType,
            };

            return this.fileRegistrationTransactionService.InsertFileRegistrationAsync(fileRegistrationResponse);
        }
    }
}
