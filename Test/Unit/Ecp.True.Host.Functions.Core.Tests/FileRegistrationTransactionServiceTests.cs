// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationTransactionServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Functions.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Transform.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The file registration transaction service tests.
    /// </summary>
    [TestClass]
    public class FileRegistrationTransactionServiceTests : HomologatorTestBase
    {
        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();

        /// <summary>
        /// The file registration transaction mock.
        /// </summary>
        private readonly Mock<IRepository<FileRegistrationTransaction>> fileRegistrationTransactionRepoMock = new Mock<IRepository<FileRegistrationTransaction>>();

        /// <summary>
        /// The repository factory mock.
        /// </summary>
        private readonly Mock<IRepositoryFactory> repositoryFactoryMock = new Mock<IRepositoryFactory>();

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Mock<ITrueLogger<FileRegistrationTransactionService>> loggerMock = new Mock<ITrueLogger<FileRegistrationTransactionService>>();

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private FileRegistrationTransactionService service;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(this.fileRegistrationTransactionRepoMock.Object);
            this.unitOfWorkFactoryMock.Setup(m => m.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.service = new FileRegistrationTransactionService(this.unitOfWorkFactoryMock.Object, this.repositoryFactoryMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// Gets the file registration transaction should get file registration transaction asynchronous for single entity asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFileRegistrationTransaction_ShouldGetFileRegistrationTransactionAsync_ForSingleEntityAsync()
        {
            this.repositoryFactoryMock.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(this.fileRegistrationTransactionRepoMock.Object);
            this.fileRegistrationTransactionRepoMock.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new FileRegistrationTransaction());

            var result = await this.service.GetFileRegistrationTransactionAsync(1).ConfigureAwait(false);
            this.fileRegistrationTransactionRepoMock.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>(), It.IsAny<string[]>()), Times.Once);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Gets the file registration should get file registration asynchronous for single entity asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFileRegistrationAsync_ShouldGetFileRegistrationAsync_ForSingleEntityAsync()
        {
            var fileRegistrationRepoMock = new Mock<IRepository<FileRegistration>>();
            var fileRegistration = new FileRegistration()
            {
                FileRegistrationId = 1,
            };
            this.repositoryFactoryMock.Setup(m => m.CreateRepository<FileRegistration>()).Returns(fileRegistrationRepoMock.Object);
            fileRegistrationRepoMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<FileRegistration>() { fileRegistration });
            var result = await this.service.GetFileRegistrationAsync("1").ConfigureAwait(false);
            fileRegistrationRepoMock.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>(), It.IsAny<string[]>()), Times.Once);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Update file registration should update.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateFileRegistrationAsync_ShouldUpdateFileRegistrationAsync()
        {
            var fileRegistrationRepoMock = new Mock<IRepository<FileRegistration>>();
            var fileRegistration = new FileRegistration()
            {
                FileRegistrationId = 1,
            };
            this.unitOfWorkMock.Setup(m => m.CreateRepository<FileRegistration>()).Returns(fileRegistrationRepoMock.Object);
            this.unitOfWorkMock.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(this.fileRegistrationTransactionRepoMock.Object);
            this.fileRegistrationTransactionRepoMock.Setup(s => s.GetCountAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>())).ReturnsAsync(0);
            fileRegistrationRepoMock.Setup(x => x.Update(It.IsAny<FileRegistration>()));
            await this.service.UpdateFileRegistrationAsync(fileRegistration).ConfigureAwait(false);
            this.fileRegistrationTransactionRepoMock.Verify(m => m.GetCountAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>()), Times.Once);
            fileRegistrationRepoMock.Verify(x => x.Update(It.IsAny<FileRegistration>()), Times.Once);
        }

        /// <summary>
        /// Gets the file registration should get file registration asynchronous for single entity asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterFailureAsync_ForFileRegistrationInProcessingStatus_ShouldRegisterFailureAsync()
        {
            var fileRegistrationRepoMock = new Mock<IRepository<FileRegistrationTransaction>>();
            var pendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                FileRegistrationTransactionId = 1,
                StatusTypeId = StatusType.PROCESSING,
            };
            this.unitOfWorkMock.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(fileRegistrationRepoMock.Object);
            fileRegistrationRepoMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(fileRegistrationTransaction);
            pendingTransactionRepository.Setup(x => x.Insert(It.IsAny<PendingTransaction>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<PendingTransaction>()).Returns(pendingTransactionRepository.Object);
            await this.service.RegisterFailureAsync(new PendingTransaction(), 1, null, "Error", "Error").ConfigureAwait(false);
            fileRegistrationRepoMock.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Once);
            pendingTransactionRepository.Verify(x => x.Insert(It.IsAny<PendingTransaction>()), Times.Once);
            fileRegistrationRepoMock.Verify(s => s.Update(It.IsAny<FileRegistrationTransaction>()), Times.Once);
            this.loggerMock.Verify(m => m.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Gets the file registration should get file registration asynchronous for single entity asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterFailureAsync_ForFileRegistrationNotInProcessingStatus_ShouldNotRegisterFailureAsync()
        {
            var fileRegistrationRepoMock = new Mock<IRepository<FileRegistrationTransaction>>();
            var pendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                FileRegistrationTransactionId = 1,
                StatusTypeId = StatusType.PROCESSED,
            };
            this.unitOfWorkMock.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(fileRegistrationRepoMock.Object);
            fileRegistrationRepoMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(fileRegistrationTransaction);
            pendingTransactionRepository.Setup(x => x.Insert(It.IsAny<PendingTransaction>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<PendingTransaction>()).Returns(pendingTransactionRepository.Object);
            await this.service.RegisterFailureAsync(new PendingTransaction(), 1, null, "Error", "Error").ConfigureAwait(false);
            fileRegistrationRepoMock.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Once);
            pendingTransactionRepository.Verify(x => x.Insert(It.IsAny<PendingTransaction>()), Times.Never);
            fileRegistrationRepoMock.Verify(s => s.Update(It.IsAny<FileRegistrationTransaction>()), Times.Never);
        }

        /// <summary>
        /// Gets the file registration should get file registration asynchronous for single entity asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterFailureAsync_ForFileRegistrationProcessingStatus_ShouldLogExceptionAsync()
        {
            var fileRegistrationRepoMock = new Mock<IRepository<FileRegistrationTransaction>>();
            var pendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();

            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                FileRegistrationTransactionId = 1,
                StatusTypeId = StatusType.PROCESSING,
            };
            this.unitOfWorkMock.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(fileRegistrationRepoMock.Object);
            fileRegistrationRepoMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(fileRegistrationTransaction);
            pendingTransactionRepository.Setup(x => x.Insert(It.IsAny<PendingTransaction>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<PendingTransaction>()).Returns(pendingTransactionRepository.Object);
            await this.service.RegisterFailureAsync(new PendingTransaction(), 1, new Exception(), "Error", "Error").ConfigureAwait(false);
            fileRegistrationRepoMock.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Once);
            pendingTransactionRepository.Verify(x => x.Insert(It.IsAny<PendingTransaction>()), Times.Once);
            fileRegistrationRepoMock.Verify(s => s.Update(It.IsAny<FileRegistrationTransaction>()), Times.Once);
            this.loggerMock.Verify(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Insert inventory should insert transactions for all the entities.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InsertInventory_ShouldInsertTransactions_ForSingleEntityAsync()
        {
            var fileRegistration = new FileRegistration
            {
                FileRegistrationId = 1,
                BlobPath = "somePath",
            };

            IEnumerable<FileRegistrationTransaction> callbackTransactionList = null;
            this.fileRegistrationTransactionRepoMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()))
                .Callback<IEnumerable<FileRegistrationTransaction>>(list => { callbackTransactionList = list; })
                .Verifiable();
            var inventory = this.GetInventoryJArray();
            var inventoryList = inventory.ToObject<List<InventoryProduct>>();
            await this.service.InsertFileRegistrationTransactionsForInventoryAsync(fileRegistration, this.GetInventoryJArray()).ConfigureAwait(false);
            this.fileRegistrationTransactionRepoMock.Verify(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()), Times.Once);
            Assert.AreEqual(fileRegistration.BlobPath, callbackTransactionList.AsEnumerable().FirstOrDefault().BlobPath);
            Assert.AreEqual(fileRegistration.FileRegistrationId, callbackTransactionList.AsEnumerable().FirstOrDefault().FileRegistrationId);
            Assert.AreEqual(StatusType.PROCESSING, callbackTransactionList.AsEnumerable().FirstOrDefault().StatusTypeId);
            Assert.AreEqual(callbackTransactionList.AsEnumerable().FirstOrDefault().SessionId, inventoryList.AsEnumerable().FirstOrDefault().InventoryProductUniqueId);
        }

        /// <summary>
        /// Insert inventory should insert transactions for all the entitites.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InsertInventory_ShouldInsertTransactions_ForMultipleEntitiesAsync()
        {
            var fileRegistration = new FileRegistration
            {
                FileRegistrationId = 1,
                BlobPath = "somePath",
            };

            IEnumerable<FileRegistrationTransaction> callbackTransactionList = null;
            this.fileRegistrationTransactionRepoMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()))
                .Callback<IEnumerable<FileRegistrationTransaction>>(list => { callbackTransactionList = list; })
                .Verifiable();
            var inventory = this.GetInventoryWithMultipleEntities();
            var inventoryList = inventory.ToObject<List<InventoryProduct>>();
            await this.service.InsertFileRegistrationTransactionsForInventoryAsync(fileRegistration, inventory).ConfigureAwait(false);
            this.fileRegistrationTransactionRepoMock.Verify(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()), Times.Once);
            Assert.AreEqual(fileRegistration.BlobPath, callbackTransactionList.AsEnumerable().FirstOrDefault().BlobPath);
            Assert.AreEqual(fileRegistration.FileRegistrationId, callbackTransactionList.AsEnumerable().FirstOrDefault().FileRegistrationId);
            Assert.AreEqual(StatusType.PROCESSING, callbackTransactionList.AsEnumerable().FirstOrDefault().StatusTypeId);
            Assert.AreEqual(inventoryList.ToList()[1].InventoryProductUniqueId, callbackTransactionList.ToList()[1].SessionId);
        }

        /// <summary>
        /// Insert inventory should insert transactions for all the entitites.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InsertInventory_ShouldInsertTransactions_ForSingleMovementEntityAsync()
        {
            var fileRegistration = new FileRegistration
            {
                FileRegistrationId = 1,
                BlobPath = "somePath",
            };

            IEnumerable<FileRegistrationTransaction> callbackTransactionList = null;
            this.fileRegistrationTransactionRepoMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()))
                .Callback<IEnumerable<FileRegistrationTransaction>>(list => { callbackTransactionList = list; })
                .Verifiable();
            var movements = this.GetMovementJArray();
            var movementList = movements.ToObject<List<Movement>>();
            await this.service.InsertFileRegistrationTransactionsForMovementsAsync(fileRegistration, movements).ConfigureAwait(false);
            this.fileRegistrationTransactionRepoMock.Verify(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()), Times.Once);
            Assert.AreEqual(fileRegistration.BlobPath, callbackTransactionList.AsEnumerable().FirstOrDefault().BlobPath);
            Assert.AreEqual(fileRegistration.FileRegistrationId, callbackTransactionList.AsEnumerable().FirstOrDefault().FileRegistrationId);
            Assert.AreEqual(StatusType.PROCESSING, callbackTransactionList.AsEnumerable().FirstOrDefault().StatusTypeId);
            Assert.AreEqual(movementList.FirstOrDefault().MovementId, callbackTransactionList.AsEnumerable().FirstOrDefault().SessionId);
        }

        [TestMethod]
        public async Task InsertPendingTransactionsAsync_ShouldInsertTransactionsAsync()
        {
            var pendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();
            pendingTransactionRepository.Setup(x => x.InsertAll(It.IsAny<IEnumerable<PendingTransaction>>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<PendingTransaction>()).Returns(pendingTransactionRepository.Object);

            await this.service.InsertPendingTransactionsAsync(new System.Collections.Concurrent.ConcurrentBag<PendingTransaction>()).ConfigureAwait(false);
            pendingTransactionRepository.Verify(x => x.InsertAll(It.IsAny<IEnumerable<PendingTransaction>>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateFileRegistrationTransactionStatusAsync_ShouldUpdateFileRegistrationTransactionStatusAsync()
        {
            var fileRegistrationTransaction = new FileRegistrationTransaction()
            {
                BlobPath = "test",
                FileRegistrationTransactionId = 4,
            };

            var fileRegistrationTransactionRepository = new Mock<IRepository<FileRegistrationTransaction>>();
            fileRegistrationTransactionRepository.Setup(x => x.Update(It.IsAny<FileRegistrationTransaction>()));
            fileRegistrationTransactionRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(fileRegistrationTransaction);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistrationTransaction>()).Returns(fileRegistrationTransactionRepository.Object);

            await this.service.UpdateFileRegistrationTransactionStatusAsync(4, StatusType.FAILED).ConfigureAwait(false);
            fileRegistrationTransactionRepository.Verify(x => x.Update(It.IsAny<FileRegistrationTransaction>()), Times.Once);
        }

        /// <summary>
        /// Insert inventory should insert transactions for all the entitites.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InsertInventory_ShouldInsertTransactions_ForMultipleMovementEntitiesAsync()
        {
            var fileRegistration = new FileRegistration
            {
                FileRegistrationId = 1,
                BlobPath = "somePath",
            };

            IEnumerable<FileRegistrationTransaction> callbackTransactionList = null;
            this.fileRegistrationTransactionRepoMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()))
                .Callback<IEnumerable<FileRegistrationTransaction>>(list => { callbackTransactionList = list; })
                .Verifiable();
            var movements = this.GetMovementEntitiesJArray();
            var movementList = movements.ToObject<List<Movement>>();
            await this.service.InsertFileRegistrationTransactionsForMovementsAsync(fileRegistration, movements).ConfigureAwait(false);
            this.fileRegistrationTransactionRepoMock.Verify(m => m.InsertAll(It.IsAny<IEnumerable<FileRegistrationTransaction>>()), Times.Once);
            Assert.AreEqual(fileRegistration.BlobPath, callbackTransactionList.AsEnumerable().FirstOrDefault().BlobPath);
            Assert.AreEqual(fileRegistration.FileRegistrationId, callbackTransactionList.AsEnumerable().FirstOrDefault().FileRegistrationId);
            Assert.AreEqual(StatusType.PROCESSING, callbackTransactionList.AsEnumerable().FirstOrDefault().StatusTypeId);
            Assert.AreEqual(movementList.FirstOrDefault().MovementId, callbackTransactionList.AsEnumerable().FirstOrDefault().SessionId);
            Assert.AreEqual(fileRegistration.BlobPath, callbackTransactionList.ToList()[1].BlobPath);
            Assert.AreEqual(fileRegistration.FileRegistrationId, callbackTransactionList.ToList()[1].FileRegistrationId);
            Assert.AreEqual(StatusType.PROCESSING, callbackTransactionList.ToList()[1].StatusTypeId);
            Assert.AreEqual(movementList.ToList()[1].MovementId, callbackTransactionList.ToList()[1].SessionId);
        }
    }
}
