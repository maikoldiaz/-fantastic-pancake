// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ticket processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class TicketProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private TicketProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The cache handler.
        /// </summary>
        private Mock<ICacheHandler<string>> mockCacheHandler;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<TicketProcessor>> mockLogger;

        /// <summary>
        /// The mock failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> mockFailureHandlerFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockCacheHandler = new Mock<ICacheHandler<string>>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockFactory.Setup(m => m.TicketInfoRepository.GetLastTicketIdAsync()).ReturnsAsync(10);
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockLogger = new Mock<ITrueLogger<TicketProcessor>>();
            this.mockFailureHandlerFactory = new Mock<IFailureHandlerFactory>();

            this.mockUnitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<int>()));
            this.mockQueueClient.Setup(c => c.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);
            this.mockConfigurationHandler.Setup(c => c.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings)).ReturnsAsync(new ServiceBusSettings());
            this.processor = new TicketProcessor(this.mockLogger.Object, this.mockFailureHandlerFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockFactory.Object, this.mockBusinessContext.Object, this.mockConfigurationHandler.Object, this.mockAzureClientFactory.Object, this.mockCacheHandler.Object);
        }

        /// <summary>
        /// Gets the ticket by identifier returns ticket when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetTicketById_ReturnsTicket_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ticket);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);

            var ticketResult = await this.processor.GetTicketByIdAsync(1).ConfigureAwait(false);

            Assert.IsNotNull(ticket);
            Assert.IsTrue(object.Equals(ticket, ticketResult));
            this.mockFactory.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
            repoMock.Verify(m => m.GetByIdAsync(1), Times.Once);
        }

        /// <summary>
        /// Gets the node by identifier asynchronous should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetTicketInfoAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var inventories = new Dictionary<string, int>();
            var movements = new Dictionary<string, int>();
            var generatedMovements = new Dictionary<string, int>();
            var ticketInfo = new TicketInfo(ticket, inventories, movements, generatedMovements);
            this.mockFactory.Setup(m => m.TicketInfoRepository.GetTicketInfoAsync(ticket.TicketId)).ReturnsAsync(ticketInfo);

            var result = await this.processor.GetTicketInfoAsync(ticket.TicketId).ConfigureAwait(false);
            Assert.AreEqual(result, ticketInfo);
            this.mockFactory.Verify(m => m.TicketInfoRepository.GetTicketInfoAsync(ticket.TicketId), Times.Once);
        }

        /// <summary>
        /// Gets the movement inventory count in date range.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Validate_Cutoff_Async()
        {
            var movementRepositoryMock = new Mock<IRepository<Movement>>();
            var inventoryRepositoryMock = new Mock<IRepository<InventoryProduct>>();
            movementRepositoryMock.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(10);
            inventoryRepositoryMock.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(10);
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(movementRepositoryMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<InventoryProduct>()).Returns(inventoryRepositoryMock.Object);
            const bool expectedResult = true;
            var ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now, Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var actualResult = await this.processor.ValidateCutOffAsync(ticket).ConfigureAwait(false);
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Gets the movement inventory count in date range.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Validate_Cutoff__ShouldReturnFalse_Async()
        {
            var movementRepositoryMock = new Mock<IRepository<Movement>>();
            var inventoryRepositoryMock = new Mock<IRepository<InventoryProduct>>();
            movementRepositoryMock.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(0);
            inventoryRepositoryMock.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(10);
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(movementRepositoryMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<InventoryProduct>()).Returns(inventoryRepositoryMock.Object);
            const bool expectedResult = false;
            var ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now, Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var actualResult = await this.processor.ValidateCutOffAsync(ticket).ConfigureAwait(false);
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Validates the delta already running asynchronous returns true when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_DeltaAlreadyRunningAsync_ReturnsTrue_WhenInvokedAsync()
        {
            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);

            var result = await this.processor.ExistsDeltaTicketAsync(1).ConfigureAwait(false);

            Assert.AreEqual(true, result);
            repoTicketMock.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the delta already running asynchronous returns false when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_DeltaAlreadyRunningAsync_ReturnsFalse_WhenInvokedAsync()
        {
            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(() => 0);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);

            var result = await this.processor.ExistsDeltaTicketAsync(1).ConfigureAwait(false);

            Assert.AreEqual(false, result);
            repoTicketMock.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the cutoff already running asynchronous returns true when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_CutoffAlreadyRunningAsync_ReturnsTrue_WhenInvokedAsync()
        {
            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);
            var ticket = new Ticket
            {
                CategoryElementId = 123,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                TicketTypeId = TicketType.Cutoff,
            };
            var result = await this.processor.ExistsTicketAsync(ticket).ConfigureAwait(false);

            Assert.AreEqual(true, result);
            repoTicketMock.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the cutoff already running asynchronous returns false when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_CutoffAlreadyRunningAsync_ReturnsFalse_WhenInvokedAsync()
        {
            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(() => 0);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);

            var ticket = new Ticket
            {
                CategoryElementId = 123,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                TicketTypeId = TicketType.Cutoff,
            };
            var result = await this.processor.ExistsTicketAsync(ticket).ConfigureAwait(false);

            Assert.AreEqual(false, result);
            repoTicketMock.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Exactly(2));
        }

        /// <summary>
        /// ses the save operational cut off asynchronous should create ticket when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task SaveOperationalCutOffAsync_ShouldCreateTicket_WhenInvokedAsync()
        {
            var ticket = new Ticket();
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>();
            var pendingTransactionErrors = new List<PendingTransactionError>();
            var failedLogisticsMovements = new List<int>();

            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FailedLogisticsMovements = failedLogisticsMovements;

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);

            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// ses the save operational cut off asynchronous should create ticket when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task SaveCutOffAsync_ShouldCreateTicket_WhenInvokedAsync()
        {
            var ticket = new Ticket() { TicketTypeId = TicketType.Cutoff, CategoryElementId = 1, Status = StatusType.PROCESSED };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>()
            {
                new UnbalanceComment
                {
                    NodeId = 1,
                    ProductId = "2",
                    Unbalance = 10,
                    Units = "Barrels",
                    UnbalancePercentage = 10,
                    ControlLimit = 10,
                    Comment = string.Empty,
                },
            };
            var pendingTransactionErrors = new List<PendingTransactionError>()
            {
                new PendingTransactionError
                {
                    ErrorId = 10,
                    Comment = string.Empty,
                },
            };

            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });

            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<Ticket> { ticket });
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);

            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// Saves the cut off asynchronous should create ticket for ticket type logistics when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task SaveCutOffAsync_ShouldCreateTicket_ForTicketType_Logistics_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.Logistics };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>();
            var pendingTransactionErrors = new List<PendingTransactionError>();
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should create ticket for ticket type logisticMovements when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldCreateTicket_ForTicketType_LogisticMovements_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.LogisticMovements };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>();
            var pendingTransactionErrors = new List<PendingTransactionError>();
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// Saves the cut off asynchronous should create ticket for ticket type delta when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task SaveCutOffAsync_ShouldCreateTicket_ForTicketType_Delta_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.Delta };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>();
            var pendingTransactionErrors = new List<PendingTransactionError>();
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// Saves the operational cut off asynchronous should throw cutoff already running when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task SaveOperationalCutOffAsync_ShouldThrowCutoffAlreadyRunning_WhenInvokedAsync()
        {
            var ticket = new Ticket() { TicketTypeId = TicketType.Cutoff, CategoryElementId = 1, Status = StatusType.PROCESSING };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>()
            {
                new UnbalanceComment
                {
                    NodeId = 1,
                    ProductId = "2",
                    Unbalance = 10,
                    Units = "Barrels",
                    UnbalancePercentage = 10,
                    ControlLimit = 10,
                    Comment = string.Empty,
                },
            };
            var pendingTransactionErrors = new List<PendingTransactionError>()
            {
                new PendingTransactionError
                {
                    ErrorId = 10,
                    Comment = string.Empty,
                },
            };

            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });

            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);

            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the operational cut off asynchronous should throw delta already running when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task SaveOperationalCutOffAsync_ShouldThrowDeltaAlreadyRunning_WhenInvokedAsync()
        {
            var ticket = new Ticket() { TicketTypeId = TicketType.Cutoff, CategoryElementId = 1, Status = StatusType.PROCESSING };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>()
            {
                new UnbalanceComment
                {
                    NodeId = 1,
                    ProductId = "2",
                    Unbalance = 10,
                    Units = "Barrels",
                    UnbalancePercentage = 10,
                    ControlLimit = 10,
                    Comment = string.Empty,
                },
            };
            var pendingTransactionErrors = new List<PendingTransactionError>()
            {
                new PendingTransactionError
                {
                    ErrorId = 10,
                    Comment = string.Empty,
                },
            };

            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });

            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            repoTicketMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);

            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the error details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetDeltaExceptionsDetails_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            int ticketId = 1233;
            IEnumerable<DeltaExceptions> errorDetails = new List<DeltaExceptions>();
            var repoMock = new Mock<IRepository<DeltaExceptions>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(errorDetails);
            this.mockFactory.Setup(m => m.CreateRepository<DeltaExceptions>()).Returns(repoMock.Object);
            var result = await this.processor.GetDeltaExceptionsDetailsAsync(ticketId, TicketType.Delta).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<DeltaExceptions>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for errors when invoked with first batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateCutOffComment_ShouldUpdateCommentForErrors_WhenInvokedWithFirstBatchAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 1,
                Errors = new List<PendingTransactionError>()
                {
                    new PendingTransactionError
                    {
                        ErrorId = 10,
                        Comment = string.Empty,
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.SetupSequence(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()))
                .ReturnsAsync(string.Empty)
                .ReturnsAsync(batch.SessionId);
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<UpdateCutOffComment>());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Exactly(2));
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for errors when invoked with second batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateCutOffComment_ShouldUpdateCommentForErrors_WhenInvokedWithSecondBatchAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 1,
                Errors = new List<PendingTransactionError>()
                {
                    new PendingTransactionError
                    {
                        ErrorId = 10,
                        Comment = string.Empty,
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.Setup(x => x.GetAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(batch.SessionId);
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<UpdateCutOffComment>());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Once);
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for errors when invoked with first batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateCutOffComment_ShouldNotUpdateCommentForErrors_WhenInvokedWithFirstBatchAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 1,
                Errors = new List<PendingTransactionError>()
                {
                    new PendingTransactionError
                    {
                        ErrorId = 10,
                        Comment = string.Empty,
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.SetupSequence(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()))
                .ReturnsAsync(string.Empty)
                .ReturnsAsync(string.Empty);
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<UpdateCutOffComment>());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Exactly(2));
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Never);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Never);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for errors when invoked with first batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateCutOffComment_ShouldNotUpdateCommentForErrors_WhenInvokedWithAnotherBatchInProgressAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 1,
                Errors = new List<PendingTransactionError>()
                {
                    new PendingTransactionError
                    {
                        ErrorId = 10,
                        Comment = string.Empty,
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.Setup(x => x.GetAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync("23456");
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<UpdateCutOffComment>());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Once);
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Never);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Never);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for unbalances when invoked with first batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateCutOffComment_ShouldUpdateCommentForUnbalances_WhenInvokedWithFirstBatchAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 2,
                Unbalances = new List<UnbalanceComment>()
                {
                    new UnbalanceComment
                    {
                        NodeId = 1,
                        ProductId = "2",
                        Unbalance = 10,
                        Units = "Barrels",
                        UnbalancePercentage = 10,
                        ControlLimit = 10,
                        Comment = string.Empty,
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.SetupSequence(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()))
                .ReturnsAsync(string.Empty)
                .ReturnsAsync(batch.SessionId);
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<UpdateCutOffComment>());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Exactly(2));
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for transfer points when invoked with first batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateCutOffComment_ShouldUpdateCommentForTransferPoints_WhenInvokedWithFirstBatchAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 3,
                TransferPoints = new List<TransferPoints>()
                {
                    new TransferPoints
                    {
                        MovementTransactionId = 1,
                        SapTrackingId = 1,
                        Comment = "comment",
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.SetupSequence(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()))
                .ReturnsAsync(string.Empty)
                .ReturnsAsync(batch.SessionId);
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<UpdateCutOffComment>());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Exactly(2));
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Never);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Updates the cut off comment should update comment for transfer points when invoked with first batch asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateCutOffComment_ShouldDeleteCacheKey_WhenExceptionOccursAsync()
        {
            var batch = new OperationalCutOffBatch
            {
                SessionId = "session",
                SegmentId = "1234",
                Type = 3,
                TransferPoints = new List<TransferPoints>()
                {
                    new TransferPoints
                    {
                        MovementTransactionId = 1,
                        SapTrackingId = 1,
                        Comment = "comment",
                    },
                },
            };
            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) };

            this.mockCacheHandler.SetupSequence(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()))
                .ReturnsAsync(string.Empty)
                .ReturnsAsync(batch.SessionId);
            this.mockCacheHandler.Setup(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>())).ReturnsAsync(true);
            this.mockCacheHandler.Setup(x => x.SetAsync(batch.SegmentId, batch.SessionId, It.IsAny<string>(), cacheOptions)).Returns(Task.CompletedTask);
            var repoMock = new Mock<IRepository<UpdateCutOffComment>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Throws(new Exception());
            this.mockFactory.Setup(m => m.CreateRepository<UpdateCutOffComment>()).Returns(repoMock.Object);
            await this.processor.UpdateCommentAsync(batch).ConfigureAwait(false);

            this.mockCacheHandler.Verify(x => x.GetAsync(batch.SegmentId, It.IsAny<string>()), Times.Exactly(2));
            this.mockCacheHandler.Verify(x => x.DeleteAsync(batch.SegmentId, It.IsAny<string>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<UpdateCutOffComment>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the success details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ApproveOfficialNodeDeltaSuccessAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var deltaNode = new DeltaNode()
            {
                DeltaNodeId = 1,
                Ticket = new Ticket()
                {
                    Status = StatusType.DELTA,
                },
            };
            var deltaApprover = new DeltaNodeApproval()
            {
                Approvers = "test",
                Level = 1,
                NodeId = 1,
            };
            var repoDeltaApprover = new Mock<IRepository<DeltaNodeApproval>>();
            repoDeltaApprover.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNodeApproval, bool>>>())).ReturnsAsync(deltaApprover);
            this.mockFactory.Setup(x => x.CreateRepository<DeltaNodeApproval>()).Returns(repoDeltaApprover.Object);
            var repoMock = new Mock<IRepository<DeltaNode>>();
            this.mockUnitOfWork.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string>())).ReturnsAsync(deltaNode);
            repoMock.Setup(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).Verifiable();
            var result = await this.processor.SendDeltaNodeForApprovalAsync(new DeltaNodeStatusRequest() { NodeId = 1, SegmentId = 1 }).ConfigureAwait(false);

            Assert.IsTrue(result.IsValidOfficialDeltaNode);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<DeltaNode>(), Times.Once);
            repoMock.Verify(m => m.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            repoMock.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string>()), Times.Once);
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Gets the fail details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ApproveOfficialNodeDeltaFailAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var deltaNode = new DeltaNode()
            {
                DeltaNodeId = 1,
                Ticket = new Ticket()
                {
                    Status = StatusType.DELTA,
                },
            };
            var deltaApprover = new DeltaNodeApproval()
            {
                Approvers = "test",
                Level = 1,
                NodeId = 1,
            };
            var repoDeltaApprover = new Mock<IRepository<DeltaNodeApproval>>();
            repoDeltaApprover.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNodeApproval, bool>>>())).ReturnsAsync(deltaApprover);
            this.mockFactory.Setup(x => x.CreateRepository<DeltaNodeApproval>()).Returns(repoDeltaApprover.Object);
            var sqlException = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var messageField = typeof(SqlException).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
            messageField.SetValue(sqlException, Constants.ApproveOfficialNodeDeltaFail);
            var repoMock = new Mock<IRepository<DeltaNode>>();
            this.mockUnitOfWork.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string>())).ReturnsAsync(deltaNode);
            repoMock.Setup(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).Throws(sqlException);
            var result = await this.processor.SendDeltaNodeForApprovalAsync(new DeltaNodeStatusRequest() { NodeId = 1, SegmentId = 1 }).ConfigureAwait(false);

            Assert.IsFalse(result.IsValidOfficialDeltaNode);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<DeltaNode>(), Times.Once);
            repoMock.Verify(m => m.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            repoMock.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string>()), Times.Once);
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Gets the exception details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ApproveOfficialNodeDeltaExceptionAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var deltaNode = new DeltaNode()
            {
                DeltaNodeId = 1,
                Ticket = new Ticket()
                {
                    Status = StatusType.DELTA,
                },
            };
            var deltaApprover = new DeltaNodeApproval()
            {
                Approvers = "test",
                Level = 1,
                NodeId = 1,
            };
            var repoDeltaApprover = new Mock<IRepository<DeltaNodeApproval>>();
            repoDeltaApprover.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNodeApproval, bool>>>())).ReturnsAsync(deltaApprover);
            this.mockFactory.Setup(x => x.CreateRepository<DeltaNodeApproval>()).Returns(repoDeltaApprover.Object);
            var repoMock = new Mock<IRepository<DeltaNode>>();
            this.mockUnitOfWork.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>(), It.IsAny<string>())).ReturnsAsync(deltaNode);
            repoMock.Setup(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).Throws(new Exception());
            await this.processor.SendDeltaNodeForApprovalAsync(new DeltaNodeStatusRequest() { NodeId = 1, SegmentId = 1 }).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<DeltaNode>(), Times.Once);
            repoMock.Verify(m => m.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            repoMock.Verify(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should create ticket for ticket type official delta when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldCreateTicket_ForTicketType_OfficialDelta_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.OfficialDelta };
            var saveTicket = new SaveTicketResult();
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should create ticket for ticket type official logistics when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldCreateTicket_ForTicketType_OfficialLogistics_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.OfficialLogistics };
            var saveTicket = new SaveTicketResult();
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueSessionMessageAsync(It.IsAny<QueueMessage>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should fail ticket for push message to service bus fails for ticket type logistics when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldFailTicket_For_PushMessageToServiceBusFails_ForTicketType_Logistics_WhenInvokedAsync()
        {
            // Arrange
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.Logistics };
            var saveTicket = new SaveTicketResult { TicketId = 123 };
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<QueueMessage>())).ThrowsAsync(new Exception());

            var failureHandler = new Mock<IFailureHandler>();
            failureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(failureHandler.Object);

            // Act
            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            // Assert
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueMessageAsync(It.IsAny<QueueMessage>()), Times.Once);
            failureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
            this.mockFailureHandlerFactory.Verify(a => a.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should fail ticket for push message to service bus fails for ticket type official logistics when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldFailTicket_For_PushMessageToServiceBusFails_ForTicketType_OfficialLogistics_WhenInvokedAsync()
        {
            // Arrange
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.OfficialLogistics };
            var saveTicket = new SaveTicketResult { TicketId = 123 };
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.mockQueueClient.Setup(c => c.QueueSessionMessageAsync(It.IsAny<QueueMessage>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var failureHandler = new Mock<IFailureHandler>();
            failureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(failureHandler.Object);

            // Act
            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            // Assert
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueSessionMessageAsync(It.IsAny<QueueMessage>(), It.IsAny<string>()), Times.Once);
            failureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
            this.mockFailureHandlerFactory.Verify(a => a.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should fail ticket for push message to service bus fails for ticket type delta when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldFailTicket_For_PushMessageToServiceBusFails_ForTicketType_Delta_WhenInvokedAsync()
        {
            // Arrange
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.Delta };
            var saveTicket = new SaveTicketResult { TicketId = 123 };
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.mockQueueClient.Setup(c => c.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var failureHandler = new Mock<IFailureHandler>();
            failureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(failureHandler.Object);

            // Act
            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            // Assert
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            failureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
            this.mockFailureHandlerFactory.Verify(a => a.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should fail ticket for push message to service bus fails for ticket type official delta when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldFailTicket_For_PushMessageToServiceBusFails_ForTicketType_OfficialDelta_WhenInvokedAsync()
        {
            // Arrange
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.OfficialDelta };
            var saveTicket = new SaveTicketResult { TicketId = 123 };
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.mockQueueClient.Setup(c => c.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var failureHandler = new Mock<IFailureHandler>();
            failureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(failureHandler.Object);

            // Act
            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            // Assert
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            failureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
            this.mockFailureHandlerFactory.Verify(a => a.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should fail ticket for push message to service bus fails for ticket type cutoff when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldFailTicket_For_PushMessageToServiceBusFails_ForTicketType_Cutoff_WhenInvokedAsync()
        {
            // Arrange
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.Cutoff };
            var saveTicket = new SaveTicketResult { TicketId = 123 };
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.mockQueueClient.Setup(c => c.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var failureHandler = new Mock<IFailureHandler>();
            failureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(failureHandler.Object);

            var ticketRepository = new Mock<IRepository<Ticket>>();
            ticketRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(0);
            this.mockFactory.Setup(a => a.CreateRepository<Ticket>()).Returns(ticketRepository.Object);

            // Act
            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            // Assert
            ticketRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Exactly(3));
            this.mockFactory.Verify(a => a.CreateRepository<Ticket>(), Times.Exactly(2));

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            failureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
            this.mockFailureHandlerFactory.Verify(a => a.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Saves the ticket asynchronous should fail ticket for push message to service bus fails for ticket type ownership when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldFailTicket_For_PushMessageToServiceBusFails_ForTicketType_Ownership_WhenInvokedAsync()
        {
            // Arrange
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.Ownership };
            var saveTicket = new SaveTicketResult { TicketId = 123 };
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = new List<UnbalanceComment>();
            operationalCutOff.FailedLogisticsMovements = new List<int>();
            operationalCutOff.PendingTransactionErrors = new List<PendingTransactionError>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings { StandardUncertaintyPercentage = 0.2M, ControlLimit = 1.98M, };
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.mockQueueClient.Setup(c => c.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            var failureHandler = new Mock<IFailureHandler>();
            failureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(failureHandler.Object);

            // Act
            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            // Assert
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
            this.mockQueueClient.Verify(q => q.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            failureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
            this.mockFailureHandlerFactory.Verify(a => a.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Gets the success details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetDeltaNodesForReopenAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var response = new List<DeltaNodeReopenResponse>() { new DeltaNodeReopenResponse() { Node = "Node", Segment = "Segment", DeltaNode = 1 } };
            var repoMock = new Mock<IRepository<DeltaNodeReopenResponse>>();
            this.mockFactory.Setup(f => f.CreateRepository<DeltaNodeReopenResponse>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(response);
            var result = await this.processor.GetDeltaNodesForReopenAsync(1).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<DeltaNodeReopenResponse>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the success details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ReopenDeltaNodesAsync_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var deltaNode = new DeltaNode()
            {
                DeltaNodeId = 1,
                Status = OwnershipNodeStatusType.APPROVED,
            };

            var repoMock = new Mock<IRepository<DeltaNode>>();
            this.mockUnitOfWork.Setup(x => x.CreateRepository<DeltaNode>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(deltaNode);
            repoMock.Setup(r => r.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).Verifiable();
            await this.processor.ReopenDeltaNodesAsync(new DeltaNodeReopenRequest() { DeltaNodeId = new List<int>() { 1 }, Comment = "Test" }).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<DeltaNode>(), Times.Once);
            repoMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Saves the cut off asynchronous should create ticket exclude first time node when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task SaveCutOffAsync_ShouldCreateTicket_ExcludeFirstTimeNode_WhenInvokedAsync()
        {
            var ticket = new Ticket() { TicketTypeId = TicketType.Cutoff, CategoryElementId = 1, Status = StatusType.PROCESSED };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>()
            {
                new UnbalanceComment
                {
                    NodeId = 1,
                    ProductId = "2",
                    Unbalance = 10,
                    Units = "Barrels",
                    UnbalancePercentage = 10,
                    ControlLimit = 10,
                    Comment = string.Empty,
                },
            };
            var pendingTransactionErrors = new List<PendingTransactionError>()
            {
                new PendingTransactionError
                {
                    ErrorId = 10,
                    Comment = string.Empty,
                },
            };

            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FirstTimeNodes = new List<int> { 1 };
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });

            var repoTicketMock = new Mock<IRepository<Ticket>>();
            repoTicketMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<Ticket> { ticket });
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicketMock.Object);
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);

            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }

        /// <summary>
        /// Saves the operational ticket asynchronous should fail ticket for push message to service bus fails for ticket type ownership when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveTicketAsync_ShouldCreateTicket_ForTicketType_OperationalLogisticMovements_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, OwnerId = 30, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(1).Date, TicketTypeId = TicketType.LogisticMovements, ScenarioTypeId = ScenarioType.OPERATIONAL };
            var saveTicket = new SaveTicketResult();
            var unbalances = new List<UnbalanceComment>();
            var pendingTransactionErrors = new List<PendingTransactionError>();
            var operationalCutOff = new OperationalCutOff();
            operationalCutOff.Ticket = ticket;
            operationalCutOff.Unbalances = unbalances;
            operationalCutOff.PendingTransactionErrors = pendingTransactionErrors;
            operationalCutOff.FailedLogisticsMovements = new List<int>();

            var repoMock = new Mock<IRepository<SaveTicketResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SaveTicketResult> { saveTicket });
            this.mockFactory.Setup(m => m.CreateRepository<SaveTicketResult>()).Returns(repoMock.Object);
            this.mockBusinessContext.Setup(a => a.UserId).Returns("AdminUser");
            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            await this.processor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SaveTicketResult>(), Times.Once);
        }
    }
}