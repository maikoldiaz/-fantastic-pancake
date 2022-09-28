// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductRegistrationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The InventoryProductRegistrationStrategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Registration.RegistrationStrategyBase" />
    public class InventoryProductRegistrationStrategy : RegistrationStrategyBase
    {
        /// <summary>
        /// The insert error message.
        /// </summary>
        private readonly string insertErrorMessage = "System insert of inventory product is not allowed.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductRegistrationStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public InventoryProductRegistrationStrategy(
           ITrueLogger logger,
           IAzureClientFactory azureClientFactory)
           : base(azureClientFactory, logger)
        {
        }

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <returns>
        /// The bool.
        /// </returns>
        public override async Task RegisterAsync(object entity, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var inventoryProduct = (InventoryProduct)entity;

            this.Logger.LogInformation($"Starting inventory registration for InventoryId: {inventoryProduct.InventoryId}");
            await this.RegistrationExecutorAsync(inventoryProduct, unitOfWork).ConfigureAwait(false);
            this.Logger.LogInformation($"Finished inventory registration for InventoryId: {inventoryProduct.InventoryId}");
        }

        /// <inheritdoc/>
        public override void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork)
        {
            this.Logger.LogInformation(this.insertErrorMessage);
        }

        /// <summary>
        /// Writes the inventory product off chain database asynchronous.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <param name="isOwner">if set to <c>true</c> [is owner].</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="eventType">The eventType.</param>
        /// <returns>
        /// The movementTransactionidentifier.
        /// </returns>
        private static async Task<int> WriteInventoryProductOffChainDbAsync(InventoryProduct inventoryProduct, bool isOwner, IUnitOfWork unitOfWork, string eventType, DateTime? dateTime)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            var inventoryProductRepository = unitOfWork.CreateRepository<InventoryProduct>();

            //// Register Inventory product.
            if (isOwner)
            {
                foreach (var item in inventoryProduct.Owners)
                {
                    item.BlockchainInventoryProductTransactionId = inventoryProduct.BlockchainInventoryProductTransactionId;
                    item.BlockchainStatus = StatusType.PROCESSING;
                }
            }

            var transaction = await GetFileRegistrationTransactionAsync(inventoryProduct.FileRegistrationTransactionId.Value, unitOfWork).ConfigureAwait(false);
            inventoryProduct.SystemTypeId = (int)transaction.FileRegistration.SystemTypeId;
            inventoryProduct.InventoryDate = inventoryProduct.InventoryDate.GetValueOrDefault().Date;
            inventoryProduct.BlockchainStatus = StatusType.PROCESSING;

            inventoryProductRepository.Insert(inventoryProduct);

            //// Code to insert in index table to validate the concurrency scenario.
            inventoryProduct.InsertInInventoryMovementIndex(unitOfWork, inventoryProduct.InventoryProductUniqueId, eventType, dateTime);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            return inventoryProduct.InventoryProductId;
        }

        /// <summary>
        /// Negates the movement asynchronous.
        /// </summary>
        /// <param name="inventoryProductObject">The movement object.</param>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="isOwner">The isOwner.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private static async Task<(int, Guid)> NegateInventoryProductAsync(
            InventoryProduct inventoryProductObject,
            int? fileRegistrationTransactionId,
            string eventType,
            bool isOwner,
            IUnitOfWork unitOfWork)
        {
            var negatedInventoryProduct = new InventoryProduct();
            negatedInventoryProduct.CopyFrom(inventoryProductObject);

            inventoryProductObject.Owners.ForEach(own =>
            {
                var owner = new Owner();
                owner.CopyFrom(own);
                owner.BlockchainStatus = StatusType.PROCESSED;
                owner.OwnershipValue = owner.OwnershipValueUnit == Constants.OwnershipPercentageUnit ? Convert.ToDecimal(owner.OwnershipValue.ToString(), CultureInfo.InvariantCulture)
                                       : Convert.ToDecimal(owner.OwnershipValue.ToString(), CultureInfo.InvariantCulture) * -1;
                negatedInventoryProduct.Owners.Add(owner);
            });

            inventoryProductObject.Attributes.ForEach(attr =>
            {
                var attribute = new AttributeEntity();
                attribute.CopyFrom(attr);

                negatedInventoryProduct.Attributes.Add(attribute);
            });

            var previousBlockchainInventoryProductTransactionId = Guid.NewGuid();
            negatedInventoryProduct.FileRegistrationTransactionId = fileRegistrationTransactionId;
            negatedInventoryProduct.EventType = eventType;

            var netVolume = Convert.ToDecimal(inventoryProductObject.ProductVolume.ToString(), CultureInfo.InvariantCulture) * -1;

            negatedInventoryProduct.ProductVolume = netVolume;
            negatedInventoryProduct.BlockchainInventoryProductTransactionId = previousBlockchainInventoryProductTransactionId;
            negatedInventoryProduct.IsDeleted = true;

            var inventoryProductId = await WriteInventoryProductOffChainDbAsync(
                negatedInventoryProduct, isOwner, unitOfWork, Constants.UpdateNegate, inventoryProductObject.CreatedDate).ConfigureAwait(false);
            return (inventoryProductId, previousBlockchainInventoryProductTransactionId);
        }

        private static Task<FileRegistrationTransaction> GetFileRegistrationTransactionAsync(int fileRegistrationTransactionId, IUnitOfWork unitOfWork)
        {
            var fileRegistrationTransactionRepository = unitOfWork.CreateRepository<FileRegistrationTransaction>();
            return fileRegistrationTransactionRepository.FirstOrDefaultAsync(f => f.FileRegistrationTransactionId == fileRegistrationTransactionId, "FileRegistration");
        }

        private static async Task GetAttributesAsync(IUnitOfWork unitOfWork, InventoryProduct inventoryProduct)
        {
            await unitOfWork.CreateRepository<AttributeEntity>().GetAllAsync(x => x.InventoryProductId == inventoryProduct.InventoryProductId).ConfigureAwait(false);
        }

        private static async Task GetOwnersAsync(IUnitOfWork unitOfWork, InventoryProduct inventoryProduct)
        {
            await unitOfWork.CreateRepository<Owner>().GetAllAsync(x => x.InventoryProductId == inventoryProduct.InventoryProductId).ConfigureAwait(false);
        }

        /// <summary>
        /// Registrations the executor asynchronous.
        /// </summary>
        /// <param name="invproductToRegister">The inventory product to register.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private async Task RegistrationExecutorAsync(InventoryProduct invproductToRegister, IUnitOfWork unitOfWork)
        {
            int inventoryProductId = 0;
            Guid? previousBlockchainInventoryProductTransactionId = null;
            var existingInventoryProduct = await this.GetInventoryProductAsync(invproductToRegister.InventoryProductUniqueId, unitOfWork).ConfigureAwait(false);
            if (invproductToRegister.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")) || invproductToRegister.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                (inventoryProductId, previousBlockchainInventoryProductTransactionId) = await NegateInventoryProductAsync(
                    existingInventoryProduct,
                    invproductToRegister.FileRegistrationTransactionId,
                    invproductToRegister.EventType,
                    false,
                    unitOfWork).ConfigureAwait(false);

                this.Logger.LogInformation($"Inserted negated inventory, InventoryProductId: {inventoryProductId}");
            }

            if (invproductToRegister.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")) || invproductToRegister.EventType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                //// For insert and update scenario inserting positive inventory product
                invproductToRegister.BlockchainInventoryProductTransactionId = Guid.NewGuid();
                invproductToRegister.PreviousBlockchainInventoryProductTransactionId = previousBlockchainInventoryProductTransactionId;

                //// Get existing inventory created date..
                var dateTime = existingInventoryProduct?.CreatedDate;

                inventoryProductId = await WriteInventoryProductOffChainDbAsync(invproductToRegister, true, unitOfWork, invproductToRegister.EventType, dateTime).ConfigureAwait(false);
                this.Logger.LogInformation($"Inserted inventory, InventoryProductId: {inventoryProductId}");
            }

            // Send Message to blockchain queue to register it in blockchain.
            await this.SendToBlockchainAsync(inventoryProductId, QueueConstants.BlockchainInventoryProductQueue).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <param name="inventoryProductUniqueId">The inventory product unique identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>the inventory product object.</returns>
        private async Task<InventoryProduct> GetInventoryProductAsync(string inventoryProductUniqueId, IUnitOfWork unitOfWork)
        {
            var inventoryProductRepository = unitOfWork.CreateRepository<InventoryProduct>();
            var inventoryProductEntities = await inventoryProductRepository.GetAllAsync(
                x =>
                x.InventoryProductUniqueId == inventoryProductUniqueId && x.ProductVolume >= 0).ConfigureAwait(false);

            var inventoryProduct = inventoryProductEntities.OrderByDescending(a => a.CreatedDate).FirstOrDefault();

            if (inventoryProduct != null)
            {
                var tasks = new List<Task>();

                // Fetch owners per movement
                tasks.Add(GetOwnersAsync(unitOfWork, inventoryProduct));

                // Fetch attributes per movement
                tasks.Add(GetAttributesAsync(unitOfWork, inventoryProduct));
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            return inventoryProduct;
        }
    }
}
