// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationTransactionService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The file registration transaction service.
    /// </summary>
    public class FileRegistrationTransactionService : IFileRegistrationTransactionService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<FileRegistrationTransactionService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationTransactionService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="logger">The logger.</param>
        public FileRegistrationTransactionService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory repositoryFactory,
            ITrueLogger<FileRegistrationTransactionService> logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.repositoryFactory = repositoryFactory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<FileRegistration> GetFileRegistrationAsync(string uploadId)
        {
            var fileRegistrationRepository = this.repositoryFactory.CreateRepository<FileRegistration>();
            var fileRegistrations = await fileRegistrationRepository.GetAllAsync(x => x.UploadId == uploadId).ConfigureAwait(false);
            return fileRegistrations.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<FileRegistrationTransaction> GetFileRegistrationTransactionAsync(int fileRegistrationTransactionId)
        {
            var repository = this.repositoryFactory.CreateRepository<FileRegistrationTransaction>();
            return repository.FirstOrDefaultAsync(x => x.FileRegistrationTransactionId == fileRegistrationTransactionId, "FileRegistration");
        }

        /// <inheritdoc/>
        public Task InsertFileRegistrationAsync(FileRegistration fileRegistration)
        {
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistration>();
            fileRegistrationTransactionRepository.Insert(fileRegistration);
            return this.unitOfWork.SaveAsync(CancellationToken.None);
        }

        /// <inheritdoc/>
        public async Task UpdateFileRegistrationAsync(FileRegistration fileRegistration)
        {
            ArgumentValidators.ThrowIfNull(fileRegistration, nameof(fileRegistration));

            var transactionRepo = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var transactions = await transactionRepo.GetCountAsync(x => x.FileRegistrationId == fileRegistration.FileRegistrationId).ConfigureAwait(false);

            // If the transactions are already present, we don't need to do anything as this step is already complete
            if (transactions == 0)
            {
                this.unitOfWork.CreateRepository<FileRegistration>().Update(fileRegistration);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task InsertFileRegistrationTransactionsForInventoryAsync(FileRegistration fileRegistration, JArray inventory)
        {
            ArgumentValidators.ThrowIfNull(fileRegistration, nameof(fileRegistration));
            ArgumentValidators.ThrowIfNull(inventory, nameof(inventory));
            var inventoryList = inventory.ToObject<List<InventoryProduct>>();
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var fileRegistrationTransactions = new List<FileRegistrationTransaction>();
            foreach (var inventoryItem in inventoryList)
            {
                fileRegistrationTransactions.Add(GenerateFileRegistration(fileRegistration, null, inventoryItem));
            }

            fileRegistrationTransactionRepository.InsertAll(fileRegistrationTransactions);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task InsertFileRegistrationTransactionsForMovementsAsync(FileRegistration fileRegistration, JArray movements)
        {
            ArgumentValidators.ThrowIfNull(fileRegistration, nameof(fileRegistration));
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));
            var movementList = movements.ToObject<List<Movement>>();
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var fileRegistrationTransactions = new List<FileRegistrationTransaction>();
            foreach (var movementItem in movementList)
            {
                fileRegistrationTransactions.Add(GenerateFileRegistration(fileRegistration, movementItem, null));
            }

            fileRegistrationTransactionRepository.InsertAll(fileRegistrationTransactions);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Inserts the pending transactions asynchronous.
        /// </summary>
        /// <param name="pendingTransactions">The pending transactions.</param>
        /// <returns>The task.</returns>
        public async Task InsertPendingTransactionsAsync(ConcurrentBag<PendingTransaction> pendingTransactions)
        {
            var pendingTransactionRepository = this.unitOfWork.CreateRepository<PendingTransaction>();
            pendingTransactionRepository.InsertAll(pendingTransactions);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task UpdateFileRegistrationTransactionStatusAsync(int fileRegistrationTransactionId, StatusType statusType)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransactionId, nameof(fileRegistrationTransactionId));
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var fileRegistrationTransaction = await fileRegistrationTransactionRepository.
                GetByIdAsync(fileRegistrationTransactionId).ConfigureAwait(false);
            if (fileRegistrationTransaction != null)
            {
                fileRegistrationTransaction.StatusTypeId = statusType;
            }

            fileRegistrationTransactionRepository.Update(fileRegistrationTransaction);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the failure asynchronous.
        /// </summary>
        /// <param name="pendingTransaction">The pending transaction.</param>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorParams">The error parameters.</param>
        /// <returns>The task.</returns>
        public async Task RegisterFailureAsync(PendingTransaction pendingTransaction, int fileRegistrationTransactionId, object exception, string errorMessage, params object[] errorParams)
        {
            this.unitOfWork.Clear();
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var fileRegistrationTransaction = await fileRegistrationTransactionRepository.GetByIdAsync(fileRegistrationTransactionId).ConfigureAwait(false);

            if (fileRegistrationTransaction == null)
            {
                return;
            }

            if (fileRegistrationTransaction.StatusTypeId == StatusType.PROCESSING || fileRegistrationTransaction.StatusTypeId == StatusType.FAILED)
            {
                if (exception != null)
                {
                    this.logger.LogError(exception as Exception, errorMessage, errorParams);
                }
                else
                {
                    this.logger.LogError(errorMessage, errorParams);
                }

                this.unitOfWork.CreateRepository<PendingTransaction>().Insert(pendingTransaction);

                fileRegistrationTransaction.StatusTypeId = StatusType.FAILED;
                fileRegistrationTransactionRepository.Update(fileRegistrationTransaction);

                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Generating the file registration item.
        /// </summary>
        /// <param name="fileRegistration">The file registration.</param>
        /// <param name="movementItem">The movement item.</param>
        /// <param name="inventoryProduct">The inventory.</param>
        /// <returns>The task.</returns>
        private static FileRegistrationTransaction GenerateFileRegistration(FileRegistration fileRegistration, Movement movementItem, InventoryProduct inventoryProduct)
        {
            return new FileRegistrationTransaction
            {
                SessionId = movementItem != null ? movementItem.MovementId : inventoryProduct.InventoryProductUniqueId,
                FileRegistrationId = fileRegistration.FileRegistrationId,
                BlobPath = fileRegistration.BlobPath,
                StatusTypeId = StatusType.PROCESSING,
            };
        }
    }
}
