// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerReconcilerTests.cs" company="Microsoft">
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
    /// The Owner reconciler tests.
    /// </summary>
    [TestClass]
    public class OwnerReconcilerTests
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ReconciliationSettings settings = new ReconciliationSettings();

        /// <summary>
        /// The reconciler.
        /// </summary>
        private OwnerReconciler reconciler;

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
        /// The mock owner repository.
        /// </summary>
        private Mock<IRepository<Owner>> mockOwnerRepository;

        /// <summary>
        /// The queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> queueClient;

        /// <summary>
        /// The owner.
        /// </summary>
        private List<Owner> owner;

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
            this.mockOwnerRepository = new Mock<IRepository<Owner>>();
            this.mockOwnerRepository.Setup(r => r.UpdateAll(this.owner));
            this.unitOfWorkMock.Setup(u => u.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None));

            this.owner = new List<Owner> { new Owner { OwnerId = 10, RetryCount = 1 } };
            this.mockOwnerRepository.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Owner, bool>>>(), It.IsAny<Expression<Func<Owner, int>>>(), this.settings.DefaultBatch)).ReturnsAsync(this.owner);
            this.mockOwnerRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(this.owner);

            this.queueClient = new Mock<IServiceBusQueueClient>();
            this.azureFactoryMock.Setup(x => x.GetQueueClient(QueueConstants.BlockchainOwnerQueue)).Returns(this.queueClient.Object);
            this.queueClient.Setup(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));

            this.reconciler = new OwnerReconciler(this.factoryMock.Object, this.telemetryMock.Object, this.azureFactoryMock.Object, this.configMock.Object);
        }

        [TestMethod]
        public async Task Reconcile_ShouldGetReconciliationSettingsAsync()
        {
            await this.reconciler.ReconcileAsync().ConfigureAwait(false);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
        }

        /// <summary>
        /// Reconciles the type of the should return.
        /// </summary>
        [TestMethod]
        public void Reconcile_ShouldReturnType()
        {
            Assert.AreEqual(ServiceType.Owner, this.reconciler.Type);
        }

        /// <summary>
        /// Reconciles the should log critical event when failed records are found in last runs asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reconcile_ShouldIncrementRetryCount_WhenInvokedAsync()
        {
            await this.reconciler.ReconcileAsync().ConfigureAwait(false);

            Assert.IsTrue(this.owner.All(a => a.RetryCount == 2));

            this.mockOwnerRepository.Verify(r => r.UpdateAll(this.owner), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Exactly(3));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.mockOwnerRepository.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Owner, bool>>>(), It.IsAny<Expression<Func<Owner, int>>>(), this.settings.DefaultBatch), Times.Exactly(2));
        }

        /// <summary>
        /// Offchains the reconcile should not reconcile when entity is not found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldNotReconcile_WhenEntityIsNotFoundAsync()
        {
            var message = new OffchainMessage { EntityId = 10 };
            this.mockOwnerRepository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(default(Owner));

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.mockOwnerRepository.Verify(r => r.Update(It.IsAny<Owner>()), Times.Never);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);

            this.mockOwnerRepository.Verify(r => r.GetByIdAsync(10), Times.Once);
        }

        /// <summary>
        /// Offchains the reconcile should not reconcile when entity is found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileInOffchain_WhenEntityIsFound_And_BlockchainMovementTransactionId_Does_Not_Exists_Async()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Owner { Id = 10, BlockchainStatus = StatusType.PROCESSING };
            this.mockOwnerRepository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);
            this.mockOwnerRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(entity);

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.mockOwnerRepository.Verify(r => r.Update(It.Is<Owner>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber)), Times.Exactly(1));
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Exactly(1));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.mockOwnerRepository.Verify(r => r.GetByIdAsync(10), Times.Once);
            this.mockOwnerRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Offchains the reconcile should reconcile in offchain when entity is found and blockchain movement transaction identifier exists asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileInOffchain_WhenEntityIsFound_And_BlockchainMovementTransactionId_Exists_Async()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Owner { Id = 10, BlockchainMovementTransactionId = Guid.NewGuid(), BlockchainStatus = StatusType.PROCESSING };
            this.mockOwnerRepository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);
            this.mockOwnerRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Owner, bool>>>())).ReturnsAsync(entity);

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.mockOwnerRepository.Verify(r => r.Update(It.Is<Owner>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber)), Times.Exactly(1));
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Exactly(1));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.mockOwnerRepository.Verify(r => r.GetByIdAsync(10), Times.Once);
        }

        /// <summary>
        /// Resets the should reset failed records when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reset_ShouldResetFailedRecords_WhenInvokedAsync()
        {
            await this.reconciler.ResetAsync(new BlockchainFailures()).ConfigureAwait(false);

            Assert.IsTrue(this.owner.All(a => a.RetryCount == 0));

            this.mockOwnerRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Owner, bool>>>()), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Once);
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
            Assert.AreEqual(nameof(Owner), records.First().Name);
            Assert.AreEqual(this.owner[0].Id, records.First().RecordId);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Once);

            this.mockOwnerRepository.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Owner, bool>>>(), It.IsAny<Expression<Func<Owner, int>>>(), this.settings.DefaultBatch), Times.Once);
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

            Assert.AreEqual(nameof(Owner), records.First().Name);
            Assert.AreEqual(this.owner[0].Id, records.First().RecordId);

            Assert.AreEqual(nameof(Owner), records.Last().Name);
            Assert.AreEqual(this.owner[0].Id, records.Last().RecordId);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Owner>(), Times.Exactly(2));

            this.mockOwnerRepository.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Owner, bool>>>(), It.IsAny<Expression<Func<Owner, int>>>(), this.settings.DefaultBatch), Times.Exactly(2));
        }
    }
}
