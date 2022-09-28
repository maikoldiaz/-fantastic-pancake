// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractValidator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
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
    /// The Contract validator.
    /// </summary>
    /// <seealso cref="IContractValidator" />
    public class ContractValidator : BaseValidator, IContractValidator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ContractValidator> logger;

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
        private readonly string validationMessage = "Contract validation started";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validated = "The homologated contract is validated";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validatedContract = "The contract entity is validated";

        /// <summary>
        /// The failed.
        /// </summary>
        private readonly string failed = "The homologated contract is failed";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractValidator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blobOperations">The blobOperations.</param>
        /// <param name="compositeValidatorFactory">The composite validator factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public ContractValidator(
            ITrueLogger<ContractValidator> logger,
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
        /// Validate the contract asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="homologatedToken">The homologated token.</param>
        /// <returns>
        /// The [True] if validation passes, [False] otherwise.
        /// </returns>
        public async Task<(bool isValid, Contract contractObject)> ValidateContractAsync(FileRegistrationTransaction fileRegistrationTransaction, JToken homologatedToken)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            ArgumentValidators.ThrowIfNull(homologatedToken, nameof(homologatedToken));

            this.logger.LogInformation(this.validationMessage, fileRegistrationTransaction.UploadId);
            var isDelete = GetIsDeleteContract(homologatedToken, fileRegistrationTransaction);

            Tuple<Contract, List<string>, object> contractObject = isDelete
                ? this.blobOperations.DoGetContractToDelete<Contract>(homologatedToken)
                : this.blobOperations.GetHomologatedObject<Contract>(homologatedToken, fileRegistrationTransaction.UploadId);

            if (contractObject.Item1 != null)
            {
                contractObject.Item1.ActionType = (string)homologatedToken["ActionType"]
                    ?? Convert.ToString(fileRegistrationTransaction.ActionType.Value, CultureInfo.InvariantCulture);

                var isValid = isDelete
                    ? await this.ValidateUpdateDeleteContractAsync(contractObject.Item1, fileRegistrationTransaction).ConfigureAwait(false)
                    : await this.DoValidateContractAsync(fileRegistrationTransaction, contractObject.Item1).ConfigureAwait(false);

                return (isValid, contractObject.Item1);
            }
            else
            {
                var pendingTransaction = homologatedToken.ToPendingTransaction(fileRegistrationTransaction, contractObject.Item2);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                                    pendingTransaction,
                                    fileRegistrationTransaction.FileRegistrationTransactionId,
                                    contractObject.Item3,
                                    Constants.InvalidDataType,
                                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return (false, null);
            }
        }

        /// <summary>
        /// Validate if action is delete.
        /// </summary>
        /// <param name="homologatedToken"> The homologate token.</param>
        /// <param name="fileRegistrationTransaction"> The file registration.</param>
        /// <returns>True if action is delete.</returns>
        private static bool GetIsDeleteContract(JToken homologatedToken, FileRegistrationTransaction fileRegistrationTransaction)
        {
            if (homologatedToken["SourceSystem"].ToString().EqualsIgnoreCase("EXCEL"))
            {
                return fileRegistrationTransaction.ActionType == Entities.Enumeration.FileRegistrationActionType.Delete;
            }

            if (string.IsNullOrEmpty((string)homologatedToken["ActionType"]))
            {
                return false;
            }

            return ((string)homologatedToken["ActionType"]).EqualsIgnoreCase(EventType.Delete.ToString("G"));
        }

        /// <summary>
        /// Validates the contract frequency.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <returns>The boolean.</returns>
        private static bool ValidateContractFrequency(Contract contractObject)
        {
            return string.IsNullOrEmpty(contractObject.Frequency)
                || contractObject.Frequency == Constants.Daily
                || contractObject.Frequency == Constants.Biweekly
                || contractObject.Frequency == Constants.Monthly;
        }

        /// <summary>
        /// Validates the contract status.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <returns>The boolean.</returns>
        private static bool ValidateContractStatus(Contract contractObject)
        {
            return contractObject.Status == Constants.Active || contractObject.Status == Constants.Unauthorized;
        }

        /// <summary>
        /// Does the get contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <returns>The task.</returns>
        private async Task<Contract> DoGetContractAsync(Contract contractObject)
        {
            var contractRepository = this.unitOfWork.CreateRepository<Contract>();
            return await contractRepository.FirstOrDefaultAsync(
               x => x.DocumentNumber == contractObject.DocumentNumber &&
                    x.Position == contractObject.Position &&
                    x.IsDeleted == false).ConfigureAwait(false);
        }

        /// <summary>
        /// Does the get contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <returns>The task.</returns>
        private async Task<Contract> DoGetContractToDeleteAsync(Contract contractObject)
        {
            var contractRepository = this.unitOfWork.CreateRepository<Contract>();
            var contract = await contractRepository.GetAllAsync(
           x => x.DocumentNumber == contractObject.DocumentNumber &&
                x.Position == contractObject.Position).ConfigureAwait(false);
            return contract.LastOrDefault();
        }

        /// <summary>
        /// Validates the contract date asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateContractDateAsync(Contract contractObject)
        {
            var contractRepository = this.unitOfWork.CreateRepository<Contract>();
            var contractObjects = await contractRepository.GetAllAsync(
             x => x.MovementTypeId == contractObject.MovementTypeId &&
                  x.SourceNodeId == contractObject.SourceNodeId &&
                  x.DestinationNodeId == contractObject.DestinationNodeId &&
                  x.ProductId == contractObject.ProductId &&
                  x.IsDeleted == false).ConfigureAwait(false);

            return !contractObjects.Any(x => x.StartDate <= contractObject.EndDate && contractObject.StartDate <= x.EndDate);
        }

        /// <summary>
        /// Does the validate contract asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="contractObject">The contract object.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> DoValidateContractAsync(FileRegistrationTransaction fileRegistrationTransaction, Contract contractObject)
        {
            this.logger.LogInformation(this.validated, fileRegistrationTransaction.UploadId);
            var result = await this.compositeValidatorFactory.ContractCompositeValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                await this.DoRegisterFailureAsync(
                contractObject, fileRegistrationTransaction, result.ErrorInfo.Select(r => r.Message), this.failed, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            this.logger.LogInformation(this.validatedContract, fileRegistrationTransaction.UploadId);
            return await this.ValidateContractAsync(contractObject, fileRegistrationTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateContractAsync(Contract contractObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));
            string actionType = Convert.ToString(fileRegistrationTransaction.ActionType.Value, CultureInfo.InvariantCulture);

            var isFrequencyValid = ValidateContractFrequency(contractObject);
            if (!isFrequencyValid)
            {
                await this.DoRegisterFailureAsync(
                    contractObject,
                    fileRegistrationTransaction,
                    new[] { Constants.ValueNotFoundFrequency },
                    Constants.ValueNotFoundFrequency,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            var isStatusValid = ValidateContractStatus(contractObject);
            if (!isStatusValid)
            {
                await this.DoRegisterFailureAsync(
                    contractObject,
                    fileRegistrationTransaction,
                    new[] { Constants.ValueNotFoundStatus },
                    Constants.ValueNotFoundStatus,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            if (actionType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                return await this.ValidateCreateContractAsync(contractObject, fileRegistrationTransaction).ConfigureAwait(false);
            }

            if (actionType.EqualsIgnoreCase(EventType.Update.ToString("G")) || actionType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                return await this.ValidateUpdateDeleteContractAsync(contractObject, fileRegistrationTransaction).ConfigureAwait(false);
            }

            throw new InvalidOperationException($"Unknown event type {actionType}");
        }

        /// <summary>
        /// Validates the create contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateCreateContractAsync(Contract contractObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));

            var existing = await this.DoGetContractAsync(contractObject).ConfigureAwait(false);
            if (existing != null)
            {
                await this.DoRegisterFailureAsync(
                    contractObject,
                    fileRegistrationTransaction,
                    new[] { Constants.ContractAlreadyExists },
                    Constants.ContractAlreadyExists,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            var isContractValid = await this.ValidateContractDateAsync(contractObject).ConfigureAwait(false);
            if (!isContractValid)
            {
                await this.DoRegisterFailureAsync(
                    contractObject,
                    fileRegistrationTransaction,
                    new[] { Constants.PurchaseAndSellPeriodAlreadyExists },
                    Constants.PurchaseAndSellPeriodAlreadyExists,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the update delete contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateUpdateDeleteContractAsync(Contract contractObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));

            return contractObject.ActionType.EqualsIgnoreCase(EventType.Delete.ToString("G"))
                ? await this.ValidateDeleteContractAsync(contractObject, fileRegistrationTransaction).ConfigureAwait(false)
                : await this.ValidateUpdateContractAsync(contractObject, fileRegistrationTransaction).ConfigureAwait(false);
        }

        private async Task<bool> ValidateUpdateContractAsync(Contract contractObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            var existing = await this.DoGetContractAsync(contractObject).ConfigureAwait(false);
            if (existing == null)
            {
                await this.DoRegisterFailureAsync(
                contractObject, fileRegistrationTransaction, new[] { Constants.ContractNotFound }, Constants.ContractNotFound, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate contract to Delete.
        /// </summary>
        /// <param name="contractObject"> The contract to delete.</param>
        /// <param name="fileRegistrationTransaction">the file registration transaction.</param>
        /// <returns>The result.</returns>
        private async Task<bool> ValidateDeleteContractAsync(Contract contractObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            var contractToDelete = await this.DoGetContractToDeleteAsync(contractObject).ConfigureAwait(false);

            if (contractToDelete == null)
            {
                await this.DoRegisterFailureAsync(
                contractObject, fileRegistrationTransaction, new[] { Constants.ContractNotFound }, Constants.ContractNotFound, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }
            else
            {
                if (contractToDelete.IsDeleted.Value)
                {
                    await this.DoRegisterFailureAsync(
                contractObject, fileRegistrationTransaction, new[] { Constants.ContractWasDeleted }, Constants.ContractWasDeleted, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Does the register failure.
        /// </summary>
        /// <param name="contractObject">The contract.</param>
        /// <param name="fileRegistrationTransaction">The file registration  session message.</param>
        private async Task DoRegisterFailureAsync(
            Contract contractObject,
            FileRegistrationTransaction fileRegistrationTransaction,
            IEnumerable<string> errorInfos,
            string errorMessage,
            params object[] args)
        {
            var pendingTransaction = contractObject.ToPendingTransaction(fileRegistrationTransaction, errorInfos);
            await this.fileRegistrationTransactionService.RegisterFailureAsync(
                pendingTransaction,
                fileRegistrationTransaction.FileRegistrationTransactionId,
                null,
                errorMessage,
                args).ConfigureAwait(false);
        }
    }
}