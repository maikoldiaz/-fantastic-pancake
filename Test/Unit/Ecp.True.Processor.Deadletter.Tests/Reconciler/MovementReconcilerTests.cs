// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementReconcilerTests.cs" company="Microsoft">
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
    /// The movement reconciler tests.
    /// </summary>
    [TestClass]
    public class MovementReconcilerTests
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ReconciliationSettings settings = new ReconciliationSettings();

        /// <summary>
        /// The reconciler.
        /// </summary>
        private MovementReconciler reconciler;

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
        private Mock<IRepository<Movement>> repoMock;

        /// <summary>
        /// The queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> queueClient;

        /// <summary>
        /// The movements.
        /// </summary>
        private List<Movement> movements;

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

            this.repoMock = new Mock<IRepository<Movement>>();
            this.repoMock.Setup(r => r.UpdateAll(this.movements));
            this.unitOfWorkMock.Setup(u => u.CreateRepository<Movement>()).Returns(this.repoMock.Object);
            this.unitOfWorkMock.Setup(u => u.SaveAsync(CancellationToken.None));

            this.movements = new List<Movement> { new Movement { MovementId = "10", MovementTransactionId = 1, RetryCount = 1 } };
            this.repoMock.Setup(r => r.OrderByAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<Expression<Func<Movement, int>>>(), this.settings.DefaultBatch)).ReturnsAsync(this.movements);
            this.repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(this.movements);

            this.queueClient = new Mock<IServiceBusQueueClient>();
            this.azureFactoryMock.Setup(x => x.GetQueueClient(QueueConstants.BlockchainMovementQueue)).Returns(this.queueClient.Object);
            this.queueClient.Setup(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));

            this.reconciler = new MovementReconciler(this.factoryMock.Object, this.telemetryMock.Object, this.azureFactoryMock.Object, this.configMock.Object);
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
            this.telemetryMock.Verify(t => t.TrackEvent(Constants.Critical, EventName.MovementReconcileFailed.ToString("G"), null, It.Is<IDictionary<string, double>>(x => x["Count"] == this.movements.Count), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Reconciles the should log critical event when failed records are found in last runs asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reconcile_ShouldWriteToMovementQueue_WhenReconciledRecordsAreFoundAsync()
        {
            await this.reconciler.ReconcileAsync().ConfigureAwait(false);

            this.azureFactoryMock.Verify(x => x.GetQueueClient(QueueConstants.BlockchainMovementQueue), Times.Once);
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

            Assert.IsTrue(this.movements.All(a => a.RetryCount == 2));

            this.repoMock.Verify(r => r.UpdateAll(this.movements), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Exactly(3));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<Expression<Func<Movement, int>>>(), this.settings.DefaultBatch), Times.Exactly(2));
        }

        /// <summary>
        /// Offchains the reconcile should not reconcile when entity is not found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldNotReconcile_WhenEntityIsNotFoundAsync()
        {
            var message = new OffchainMessage { EntityId = 10 };
            this.repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(default(Movement));

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.IsAny<Movement>()), Times.Never);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Never);

            this.repoMock.Verify(r => r.GetByIdAsync(10), Times.Once);
        }

        /// <summary>
        /// Offchains the reconcile should not reconcile when entity is found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileInOffchain_WhenEntityIsFoundAsync()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Movement { MovementTransactionId = 10, BlockchainStatus = StatusType.PROCESSING };
            this.repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);
            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>()));

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.Is<Movement>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber)), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Once);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.GetByIdAsync(10), Times.Once);
            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Offchains the reconcile should not reconcile when entity is found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OffchainReconcile_ShouldReconcileNegatedRecordInOffchain_WhenEntityIsFoundAsync()
        {
            var message = new OffchainMessage { EntityId = 10, TransactionHash = "Hash", BlockNumber = "123" };
            var entity = new Movement { MovementTransactionId = 10, PreviousBlockchainMovementTransactionId = Guid.NewGuid(), BlockchainStatus = StatusType.PROCESSING };

            this.repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);
            this.repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(new Movement());

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.repoMock.Verify(r => r.Update(It.Is<Movement>(n => n.BlockchainStatus == message.Status && n.TransactionHash == message.TransactionHash && n.BlockNumber == message.BlockNumber)), Times.Exactly(2));
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Exactly(2));
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);

            this.repoMock.Verify(r => r.GetByIdAsync(10), Times.Once);
            this.repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Movement, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Resets the should reset failed records when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Reset_ShouldResetFailedRecords_WhenInvokedAsync()
        {
            await this.reconciler.ResetAsync(new BlockchainFailures()).ConfigureAwait(false);

            Assert.IsTrue(this.movements.All(a => a.RetryCount == 0));

            this.repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>()), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Once);
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
            Assert.AreEqual(nameof(Movement), records.First().Name);
            Assert.AreEqual(this.movements[0].MovementTransactionId, records.First().RecordId);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Once);

            this.repoMock.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<Expression<Func<Movement, int>>>(), this.settings.DefaultBatch), Times.Once);
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

            Assert.AreEqual(nameof(Movement), records.First().Name);
            Assert.AreEqual(this.movements[0].MovementTransactionId, records.First().RecordId);

            Assert.AreEqual(nameof(Movement), records.Last().Name);
            Assert.AreEqual(this.movements[0].MovementTransactionId, records.Last().RecordId);

            this.configMock.Verify(c => c.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings), Times.Once);
            this.unitOfWorkMock.Verify(u => u.CreateRepository<Movement>(), Times.Exactly(2));

            this.repoMock.Verify(r => r.OrderByAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<Expression<Func<Movement, int>>>(), this.settings.DefaultBatch), Times.Exactly(2));
        }

        /// <summary>
        /// Reconciles the type of the should return.
        /// </summary>
        [TestMethod]
        public void Reconcile_ShouldReturnType()
        {
            Assert.AreEqual(ServiceType.Movement, this.reconciler.Type);
        }
    }
}
