// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Operation;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Inventory validator .
    /// </summary>
    /// <seealso cref="IInventoryValidatorService" />
    public class InventoryValidator : BaseValidator, IInventoryValidator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<InventoryValidator> logger;

        /// <summary>
        /// The inventory validator service.
        /// </summary>
        private readonly IBlobOperations blobOperations;

        /// <summary>
        /// The composite validator factory.
        /// </summary>
        private readonly ICompositeValidatorFactory compositeValidatorFactory;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The invalid message.
        /// </summary>
        private readonly string validationMessage = "Inventory validation started";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validated = "The homologated entity is validated";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validatedInventory = "The inventory entity is validated";

        /// <summary>
        /// The failed.
        /// </summary>
        private readonly string failed = "The homologated entity is failed";

        /// <summary>
        /// The validate create.
        /// </summary>
        private readonly string validateCreate = "The inventory product already exists in the system";

        /// <summary>
        /// The validate update delete.
        /// </summary>
        private readonly string validateUpdateDelete = "The inventory product to adjust does not exist";

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryValidator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blobOperations">The blobOperations.</param>
        /// <param name="compositeValidatorFactory">The composite validator factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public InventoryValidator(
            ITrueLogger<InventoryValidator> logger,
            IBlobOperations blobOperations,
            ICompositeValidatorFactory compositeValidatorFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
            : base(azureClientFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.logger = logger;
            this.blobOperations = blobOperations;
            this.compositeValidatorFactory = compositeValidatorFactory;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Validate the inventory asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="homologatedToken">The homologated token.</param>
        /// <returns>
        /// The [True] if validation passes, [False] otherwise.
        /// </returns>
        public async Task<(bool isValid, InventoryProduct inventory)> ValidateInventoryAsync(FileRegistrationTransaction fileRegistrationTransaction, JToken homologatedToken)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            this.logger.LogInformation(this.validationMessage, fileRegistrationTransaction.UploadId);

            var inventory = this.blobOperations.GetHomologatedObject<InventoryProduct>(homologatedToken, fileRegistrationTransaction.UploadId);
            if (inventory.Item1 != null)
            {
                var isValid = await this.DoValidateInventoryAsync(fileRegistrationTransaction, inventory.Item1).ConfigureAwait(false);
                return (isValid, inventory.Item1);
            }
            else
            {
                var pendingTransaction = homologatedToken.ToPendingTransaction(fileRegistrationTransaction, inventory.Item2);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                    pendingTransaction,
                    fileRegistrationTransaction.FileRegistrationTransactionId,
                    inventory.Item3,
                    Constants.InvalidDataType,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);

                return (false, null);
            }
        }

        /// <summary>
        /// validate if the inventory product contains version.
        /// </summary>
        /// <param name="inventoryProduct">inventoryProduct.</param>
        /// <returns>bool.</returns>
        private static bool ContainsVersion(InventoryProduct inventoryProduct)
        {
            if (inventoryProduct.ScenarioId == ScenarioType.OFFICER)
            {
                return !string.IsNullOrEmpty(inventoryProduct.Version);
            }

            return true;
        }

        /// <summary>
        /// Does the validate inventory asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <param name="inventoryProduct">The inventory.</param>
        private async Task<bool> DoValidateInventoryAsync(FileRegistrationTransaction fileRegistrationTransaction, InventoryProduct inventoryProduct)
        {
            var result = await this.compositeValidatorFactory.InventoryCompositeValidator.ValidateAsync(inventoryProduct).ConfigureAwait(false);
            this.logger.LogInformation(this.validated, fileRegistrationTransaction.UploadId);

            if (!result.IsSuccess)
            {
                await this.DoRegisterFailureAsync(
                inventoryProduct, fileRegistrationTransaction, result.ErrorInfo.Select(r => r.Message), this.failed, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            this.logger.LogInformation(this.validatedInventory, fileRegistrationTransaction.UploadId);
            return await this.ValidateInventoryAsync(inventoryProduct, fileRegistrationTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the inventory asynchronous.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <exception cref="System.InvalidOperationException">Unknown event type {movement.EventType}.</exception>
        private async Task<bool> ValidateInventoryAsync(InventoryProduct inventoryProduct, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            if (inventoryProduct.EventType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                return await this.ValidateCreateInventoryAsync(inventoryProduct, fileRegistrationTransaction).ConfigureAwait(false);
            }

            if (inventoryProduct.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")))
            {
                return await this.ValidateUpdateDeleteInventoryAsync(inventoryProduct, fileRegistrationTransaction, Constants.InventoryUpdateConflict).ConfigureAwait(false);
            }

            if (inventoryProduct.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                return await this.ValidateUpdateDeleteInventoryAsync(inventoryProduct, fileRegistrationTransaction, Constants.InventoryDeleteConflict).ConfigureAwait(false);
            }

            throw new InvalidOperationException($"Unknown event type {inventoryProduct.EventType}");
        }

        /// <summary>
        /// Validates the create inventory asynchronous.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        private async Task<bool> ValidateCreateInventoryAsync(InventoryProduct inventoryProduct, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            var result = await this.unitOfWork.InventoryProductRepository.GetLatestInventoryProductAsync(inventoryProduct.InventoryProductUniqueId).ConfigureAwait(false);
            var errors = new List<string>();

            if (!(result == null ||
                result.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G"))))
            {
                errors.Add(Constants.InventoryCreateConflict);
            }

            if (!ContainsVersion(inventoryProduct))
            {
                errors.Add(Constants.VersionRequired);
            }

            if (errors.Any())
            {
                await this.DoRegisterFailureAsync(
                inventoryProduct, fileRegistrationTransaction, errors, this.validateCreate, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the update delete inventory asynchronous.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>True or false.</returns>
        private async Task<bool> ValidateUpdateDeleteInventoryAsync(InventoryProduct inventoryProduct, FileRegistrationTransaction fileRegistrationTransaction, string errorMessage)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));

            var result = await this.unitOfWork.InventoryProductRepository.GetLatestInventoryProductAsync(inventoryProduct.InventoryProductUniqueId).ConfigureAwait(false);
            var errors = new List<string>();

            if (!(result != null &&
                result.ProductVolume.HasValue &&
                !result.ProductVolume.IsNegative() &&
                !result.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G"))))
            {
                errors.Add(errorMessage);
            }

            if (!ContainsVersion(inventoryProduct))
            {
                errors.Add(Constants.VersionRequired);
            }

            if (errors.Any())
            {
                await this.DoRegisterFailureAsync(
                inventoryProduct, fileRegistrationTransaction, errors, this.validateUpdateDelete, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Does the register failure.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="fileRegistrationTransaction">The file registration  session message.</param>
        private async Task DoRegisterFailureAsync(
            InventoryProduct inventoryProduct,
            FileRegistrationTransaction fileRegistrationTransaction,
            IEnumerable<string> errorInfos,
            string errorMessage,
            params object[] args)
        {
            var pendingTransaction = inventoryProduct.ToPendingTransaction(fileRegistrationTransaction, errorInfos);
            await this.fileRegistrationTransactionService.RegisterFailureAsync(
                pendingTransaction,
                fileRegistrationTransaction.FileRegistrationTransactionId,
                null,
                errorMessage,
                args).ConfigureAwait(false);
        }
    }
}