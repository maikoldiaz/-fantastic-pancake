// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipReconcilerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Deadletter.Tests.Reconciler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Deadletter.Reconciler;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ownership reconciler tests.
    /// </summary>
    [TestClass]
    public class OwnershipReconcilerTests
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ReconciliationSettings settings = new ReconciliationSettings();

        /// <summary>
        /// The reconciler.
        /// </summary>
        private OwnershipReconciler reconciler;

        /// <summary>
        /// The factory mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> factoryMock;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The configuration mock.
        /// </summary>
        private Mock<IConfigurationHandler> configMock;

        /// <summary>
        /// The telemetry mock.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The azure factory mock.
        /// </summary>
        private Mock<IAzureClientFactory> azureFactoryMock;

        /// <summary>
        /// The repo mock.
        /// </summary>
        private Mock<IRepository<Ownership>> repoMock;

        /// <summary>
        /// The queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> queueClient;

        /// <summary>
        /// The movements.
        /// </summary>
        private List<Ownership> entities;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.factoryMock = new Mock<IUnitOfWorkFactory>();
            this.configMock = new Mock<IConfigurationHandler>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.azureFactoryMock = new Mock<IAzureClientFactory>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();

            this.factoryMock.Setup(f => f.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.configMock.Setup(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings)).ReturnsAsync(this.settings);
            this.telemetryMock.Setup(t => t.TrackEvent(Constants.Critical, It.IsAny<string>(), null, It.IsAny<IDictionary<string, double>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.repoMock = new Mock<IRepository<Ownership>>();
            this.repoMock.Setup(r => r.UpdateAll(this.entities));
            this.unitOfWorkMock.Setup(u => u.CreateRepository<Ownership>()).Returns(this.repoMock.Object);
            this.unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None));

            this.entities = new List<Ownership> { new Ownership { InventoryProductId = 10, MovementTransactionId = 10, OwnershipId = 1, RetryCount = 1 } };
            this.repoMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, int>>>(), this.settings.DefaultBatch)).ReturnsAsync(this.entities);
            this.repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(this.entities);

            this.queueClient = new Mock<IServiceBusQueueClient>();
            this.azureFactoryMock.Setup(x => x.GetQueueClient(QueueConstants.BlockchainOwnershipQueue)).Returns(this.queueClient.Object);
            this.queueClient.Setup(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));

            this.reconciler = new OwnershipReconciler(this.factoryMock.Object, this.telemetryMock.Object, this.azureFactoryMock.Object, this.configMock.Object);
        }

        /// <summary>
        /// Reconciles the should log critical event when failed records are found in last runs asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reconcile_ShouldLogCriticalEvent_WhenFailedRecordsAreFoundInLastRunsAsync()
        {
            await this.reconciler.ReconcileAsync().ConfigureAwait(false);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.telemetryMock.Verify(t => t.TrackEvent(Constants.Critical, EventName.OwnershipReconcileFailed.ToString("G"), null, It.Is<IDictionary<string, double>>(x => x["Count"] == this.entities.Count), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Reconciles the should log critical event when failed records are found in last runs asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reconcile_ShouldWriteToOwnershipQueue_WhenReconciledRecordsAreFoundAsync()
        {
            await this.reconciler.ReconcileAsync().ConfigureAwait(false);

            this.azureFactoryMock.Verify(x => x.GetQueueClient(QueueConstants.BlockchainOwnershipQueue), Times.Exactly(2));
            this.queueClient.Verify(q => q.QueueSessionMessageAsync(1, "10"));
        }

        /// <summary>
        /// Reconciles the should log critical event when failed records are found in last runs asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reconcile_ShouldIncrementRetryCount_WhenInvokedAsync()
        {
            await this.reconciler.ReconcileAsync().ConfigureAwait(false);

            Assert.IsTrue(this.entities.All(a => a.RetryCount == 2));

            this.repoMock.Verify(r => r.UpdateAll(this.entities), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Exactly(3));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, int>>>(), this.settings.DefaultBatch), Times.Exactly(2));
        }

        /// <summary>
        /// Offchains the reconcile should not reconcile when entity is not found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldNotReconcile_WhenEntityIsNotFoundAsync()
        {
            var message = new OffchainMessage { EntityId = 10 };
            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct")).ReturnsAsync(default(Ownership));

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.IsAny<Ownership>()), Times.Never);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);

            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct"), Times.Once);
        }

        /// <summary>
        /// Offchains the reconcile should reconcile when entity is found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileInOffchain_WhenEntityIsFoundWithMovementTypeAsync()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Ownership { OwnershipId = 10, MessageTypeId = MessageType.MovementOwnership, MovementTransaction = new Movement { BlockchainMovementTransactionId = Guid.NewGuid() }, BlockchainStatus = StatusType.PROCESSING };

            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct")).ReturnsAsync(entity);
            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>()));

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.Is<Ownership>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber && n.BlockchainMovementTransactionId == entity.MovementTransaction.BlockchainMovementTransactionId && n.BlockchainInventoryProductTransactionId == null)), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct"), Times.Once);
            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Offchains the reconcile should reconcile when entity is found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileInOffchain_WhenEntityIsFoundWithInventoryTypeAsync()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Ownership { OwnershipId = 10, MessageTypeId = MessageType.InventoryOwnership, BlockchainStatus = StatusType.PROCESSING, InventoryProduct = new InventoryProduct { BlockchainInventoryProductTransactionId = Guid.NewGuid() } };

            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct")).ReturnsAsync(entity);
            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>()));

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.Is<Ownership>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber && n.BlockchainInventoryProductTransactionId == entity.InventoryProduct.BlockchainInventoryProductTransactionId && n.BlockchainMovementTransactionId == null)), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct"), Times.Once);
            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Offchains the reconcile should reconcile when entity is found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileNegatedRecordInOffchain_WhenEntityIsFoundAsync()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Ownership { OwnershipId = 10, PreviousBlockchainOwnershipId = Guid.NewGuid(), BlockchainStatus = StatusType.PROCESSING };

            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct")).ReturnsAsync(entity);
            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(new Ownership());

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.Is<Ownership>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber)), Times.Exactly(2));
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Exactly(2));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), "MovementTransaction", "InventoryProduct"), Times.Once);
            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ownership, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Resets the should reset failed records when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reset_ShouldResetFailedRecords_WhenInvokedAsync()
        {
            await this.reconciler.ResetAsync(new BlockchainFailures()).ConfigureAwait(false);

            Assert.IsTrue(this.entities.All(a => a.RetryCount == 0));

            this.repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>()), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);
        }

        /// <summary>
        /// Gets the failures should get critical failed records when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFailures_ShouldGetCriticalFailedRecords_WhenInvokedAsync()
        {
            var records = await this.reconciler.GetFailuresAsync(true, null).ConfigureAwait(false);

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(nameof(Ownership), records.First().Name);
            Assert.AreEqual(this.entities[0].OwnershipId, records.First().RecordId);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Once);

            this.repoMock.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, int>>>(), this.settings.DefaultBatch), Times.Once);
        }

        /// <summary>
        /// Gets the failures should get critical failed records when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFailures_ShouldGetAllFailedRecords_WhenInvokedAsync()
        {
            var records = await this.reconciler.GetFailuresAsync(false, null).ConfigureAwait(false);

            Assert.AreEqual(2, records.Count());

            Assert.AreEqual(nameof(Ownership), records.First().Name);
            Assert.AreEqual(this.entities[0].OwnershipId, records.First().RecordId);

            Assert.AreEqual(nameof(Ownership), records.Last().Name);
            Assert.AreEqual(this.entities[0].OwnershipId, records.Last().RecordId);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Ownership>(), Times.Exactly(2));

            this.repoMock.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<Expression<Func<Ownership, int>>>(), this.settings.DefaultBatch), Times.Exactly(2));
        }
    }
}
