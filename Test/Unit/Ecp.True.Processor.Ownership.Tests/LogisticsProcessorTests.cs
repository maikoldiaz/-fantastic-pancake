// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipProcessorTests.
    /// </summary>
    [TestClass]
    public class LogisticsProcessorTests
    {
        /// <summary>
        /// The mock failure handler.
        /// </summary>
        private readonly Mock<IFailureHandler> mockFailureHandler = new Mock<IFailureHandler>();

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The unit of work Factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The logistics service.
        /// </summary>
        private Mock<ILogisticsService> mockLogisticsService;

        /// <summary>
        /// The logistics processor.
        /// </summary>
        private LogisticsProcessor logisticsProcessor;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<LogisticsProcessor>> mockLogger;

        /// <summary>
        /// The mock failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> mockFailureHandlerFactory;

        /// <summary>
        /// The mock excel service.
        /// </summary>
        private Mock<IExcelService> mockExcelService;

        /// <summary>
        /// The sap ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> logisticRepository;

        /// <summary>
        /// The sap Logistic Movement repository.
        /// </summary>
        private Mock<IRepository<LogisticMovement>> logisticMovementRepository;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The Interface mock processor.
        /// </summary>
        private Mock<ILogisticsProcessor> mockProcessor;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock logistic service.
        /// </summary>
        private Mock<ILogisticsService> mockLogisticService;

        /// <summary>
        /// The business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.unitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockLogger = new Mock<ITrueLogger<LogisticsProcessor>>();
            this.mockLogisticsService = new Mock<ILogisticsService>();
            this.mockFailureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            this.mockExcelService = new Mock<IExcelService>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockProcessor = new Mock<ILogisticsProcessor>();
            this.logisticRepository = new Mock<IRepository<Ticket>>();
            this.logisticMovementRepository = new Mock<IRepository<LogisticMovement>>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockLogisticService = new Mock<ILogisticsService>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.logisticsProcessor = new LogisticsProcessor(this.unitOfWorkFactory.Object, this.mockFactory.Object, this.mockLogisticsService.Object, this.mockLogger.Object, this.mockAzureClientFactory.Object, this.mockFailureHandlerFactory.Object, this.mockBusinessContext.Object);
        }

        [TestMethod]
        public async Task GenerateOfficialLogisticsAsync_GenerateExcelAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement { MovementTypeId = 1 } };
            var transformedMovements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement { MovementId = "1" } };

            var repoMockDetails = new Mock<IRepository<GenericLogisticsMovement>>();
            repoMockDetails.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(u => u.CreateRepository<GenericLogisticsMovement>()).Returns(repoMockDetails.Object);
            this.mockLogisticsService.Setup(r => r.TransformAsync(It.IsAny<List<GenericLogisticsMovement>>(), It.IsAny<Ticket>(), It.IsAny<SystemType>(), It.IsAny<ScenarioType>())).ReturnsAsync(transformedMovements);
            this.mockFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);

            await this.logisticsProcessor.GenerateOfficialLogisticsAsync(ticket, (int)SystemType.SIV).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(u => u.CreateRepository<GenericLogisticsMovement>(), Times.Once);
            this.mockFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task GenerateOfficialLogisticsAsync_DoNotGenerateExcelAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var movements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement { MovementTypeId = 1 } };
            var transformedMovements = new List<GenericLogisticsMovement>();

            var repoMockDetails = new Mock<IRepository<GenericLogisticsMovement>>();
            repoMockDetails.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(u => u.CreateRepository<GenericLogisticsMovement>()).Returns(repoMockDetails.Object);
            this.mockLogisticsService.Setup(r => r.TransformAsync(It.IsAny<List<GenericLogisticsMovement>>(), It.IsAny<Ticket>(), It.IsAny<SystemType>(), It.IsAny<ScenarioType>())).ReturnsAsync(transformedMovements);
            this.mockExcelService.Setup(e => e.ExportAndUploadLogisticsExcelAsync(
                It.IsAny<DataSet>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()));
            this.mockFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);

            await this.logisticsProcessor.GenerateOfficialLogisticsAsync(ticket, (int)SystemType.SIV).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(u => u.CreateRepository<GenericLogisticsMovement>(), Times.Once);
            this.mockExcelService.Verify(
                e => e.ExportAndUploadLogisticsExcelAsync(
                It.IsAny<DataSet>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
            this.mockFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Never);
        }

        [TestMethod]
        public async Task GenerateOfficialLogisticsAsync_NoMovementsFailureAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var movements = new List<GenericLogisticsMovement>();
            var transformedMovements = new List<GenericLogisticsMovement>();

            var repoMockDetails = new Mock<IRepository<GenericLogisticsMovement>>();
            repoMockDetails.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(u => u.CreateRepository<GenericLogisticsMovement>()).Returns(repoMockDetails.Object);
            this.mockLogisticsService.Setup(r => r.TransformAsync(It.IsAny<List<GenericLogisticsMovement>>(), It.IsAny<Ticket>(), It.IsAny<SystemType>(), It.IsAny<ScenarioType>())).ReturnsAsync(transformedMovements);
            this.mockExcelService.Setup(e => e.ExportAndUploadLogisticsExcelAsync(
                It.IsAny<DataSet>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()));
            this.mockFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);

            await this.logisticsProcessor.GenerateOfficialLogisticsAsync(ticket, (int)SystemType.SIV).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(u => u.CreateRepository<GenericLogisticsMovement>(), Times.Once);
            this.mockExcelService.Verify(
                e => e.ExportAndUploadLogisticsExcelAsync(
                It.IsAny<DataSet>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Never);
            this.mockFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        [TestMethod]
        public async Task GetTicketAsync_ShouldReturenTicketAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), "Owner", "CategoryElement")).ReturnsAsync(ticket);
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);

            var ticketOutput = await this.logisticsProcessor.GetTicketAsync(ticket.TicketId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(ticketOutput);
            repoMockTicket.Verify(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), "Owner", "CategoryElement"), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
        }

        /// <summary>
        /// Test of CancelBatchAsync of class Logistic Movement.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task LogisticCancelBatch_CancelTicketAsync_ToReturnSuccessAsync()
        {
            var ticket = new Ticket() { TicketId = 90 };
            var logisticMovements = new List<LogisticMovement>() { new LogisticMovement { TicketId = 90, LogisticMovementId = 1 },  new LogisticMovement { TicketId = 90, LogisticMovementId = 2 } };
            int ticketIdTest = 1;
            this.mockProcessor.Setup(m => m.CancelBatchAsync(It.IsAny<int>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(this.logisticRepository.Object);
            this.mockUnitOfWork.Setup(u => u.CreateRepository<LogisticMovement>()).Returns(this.logisticMovementRepository.Object);
            this.logisticRepository.Setup(x => x.Update(It.IsAny<Ticket>()));
            this.logisticRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(ticket);
            this.logisticMovementRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>())).ReturnsAsync(logisticMovements);
            await this.logisticsProcessor.CancelBatchAsync(ticketIdTest).ConfigureAwait(false);
            this.logisticRepository.Verify(x => x.Update(It.IsAny<Ticket>()), Times.Once);
        }

        /// <summary>
        /// This Procedure is used to validate that the nodes for the next case.
        /// Validating available nodes.
        /// Validating nodes with submission to SAP.
        /// Approved nodes.
        /// Predecessor nodes with submission to SAP.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateLogisticMovementNode_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            IEnumerable<TicketNode> arrayAll = new List<TicketNode>() { new TicketNode() { NodeId = 123 } };
            var ticket = new Ticket { CategoryElementId = 1, StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow, ScenarioTypeId = ScenarioType.OPERATIONAL, TicketNodes = arrayAll };
            IEnumerable<NodesForSegmentResult> validationResult = new List<NodesForSegmentResult>();
            var repoMock = new Mock<IRepository<NodesForSegmentResult>>();
            this.mockProcessor.Setup(m => m.LogisticMovementNodeValidationsAsync(It.IsAny<Ticket>()));
            this.mockFactory.Setup(m => m.CreateRepository<NodesForSegmentResult>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(validationResult);
            var result = await this.logisticsProcessor.LogisticMovementNodeValidationsAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<NodesForSegmentResult>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Update Logistic Movements Ticket Should ThrowArgumentException When LogisticMovementsTicketRequest IsNull.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Confirm_LogisticMovements_ShouldThrowArgumentException_WhenLogisticMovementsTicketRequestIsNullAsync()
        {
            await this.logisticsProcessor.ConfirmLogisticMovementsAsync(new LogisticMovementsTicketRequest()).ConfigureAwait(false);
        }

        /// <summary>
        /// Forward Logistic Movements Ticket Should ThrowArgumentException When LogisticMovementsTicketRequest IsNull.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Forward_LogisticMovements_ShouldThrowArgumentException_WhenLogisticMovementsTicketRequestIsNullAsync()
        {
            await this.logisticsProcessor.ForwardLogisticMovementsAsync(new LogisticMovementsTicketRequest()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets logistic movement detail should execute query when invoked.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task GetLogisticMovementDetailAsync_ShouldExecuteQuery_WhenInvokedAsync()
        {
            var logisticMovement = new[] { new SapLogisticMovementDetail() };
            var repoMock = new Mock<IRepository<SapLogisticMovementDetail>>();
            this.mockFactory.Setup(m => m.CreateRepository<SapLogisticMovementDetail>()).Returns(repoMock.Object);
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(logisticMovement);
            var result = await this.logisticsProcessor.GetLogisticMovementDetailAsync(It.IsAny<int>()).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<SapLogisticMovementDetail>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets logistic movement detail should execute query when invoked.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task FailedLogisticMovementDetailAsync_ShouldExecuteQuery_WhenInvokedAsync()
        {
            var logisticMovement = new[] { new SapLogisticMovementDetail() };
            var repoMock = new Mock<IRepository<SapLogisticMovementDetail>>();
            IEnumerable<TicketNode> arrayAll = new List<TicketNode>() { new TicketNode() { NodeId = 123 } };
            var ticket = new Ticket { CategoryElementId = 1, StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow, ScenarioTypeId = ScenarioType.OPERATIONAL, TicketNodes = arrayAll };

            this.mockFactory.Setup(m => m.CreateRepository<SapLogisticMovementDetail>()).Returns(repoMock.Object);
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(logisticMovement);
            var result = await this.logisticsProcessor.FailedLogisticMovementAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<SapLogisticMovementDetail>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Confirm Logistic Movements Ticket Should send sapRequest to queue when the data is valid.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task Confirm_LogisticMovements_ShouldSendSapRequestToQueue_WhenInvokedAsync()
        {
            var clientMock = new Mock<IServiceBusQueueClient>();
            var repoMock = new Mock<IRepository<UpdateLogisticMovements>>();
            clientMock.Setup(m => m.QueueScheduleMessageAsync(It.IsAny<LogisticQueueMessage>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockFactory.Setup(m => m.CreateRepository<UpdateLogisticMovements>()).Returns(repoMock.Object);
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(clientMock.Object);

            var logisticMovement = new LogisticMovementsTicketRequest() { TicketId = 123, Movements = new List<int>() };
            await this.logisticsProcessor.ConfirmLogisticMovementsAsync(logisticMovement).ConfigureAwait(false);

            this.mockFactory.Verify(m => m.CreateRepository<UpdateLogisticMovements>(), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Forward Logistic Movements Ticket Should send sapRequest to queue when the data is valid.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task Forward_LogisticMovements_ShouldSendSapRequestToQueue_WhenInvokedAsync()
        {
            var clientMock = new Mock<IServiceBusQueueClient>();

            var repoMockTicket = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            repoMockTicket.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket());

            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(u => u.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            repoMockLogisticMovement.Setup(u => u.FirstOrDefaultAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>())).ReturnsAsync(new LogisticMovement());

            clientMock.Setup(m => m.QueueScheduleMessageAsync(It.IsAny<LogisticQueueMessage>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(clientMock.Object);

            var logisticMovement = new LogisticMovementsTicketRequest() { TicketId = 123, Movements = new List<int> { 18623 } };
            await this.logisticsProcessor.ForwardLogisticMovementsAsync(logisticMovement).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Forward Logistic Movements Ticket Should send sapRequest to queue when the data is valid.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Forward_LogisticMovements_ShouldTicketException_WhenInvokedAsync()
        {
            var clientMock = new Mock<IServiceBusQueueClient>();

            var repoMockTicket = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            repoMockTicket.Setup(u => u.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());

            clientMock.Setup(m => m.QueueScheduleMessageAsync(It.IsAny<LogisticQueueMessage>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(clientMock.Object);

            var logisticMovement = new LogisticMovementsTicketRequest() { TicketId = 123, Movements = new List<int> { 18623 } };
            await this.logisticsProcessor.ForwardLogisticMovementsAsync(logisticMovement).ConfigureAwait(false);
        }

        /// <summary>
        /// Forward Logistic Movements Ticket Should send sapRequest to queue when the data is valid.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Forward_LogisticMovements_ShouldLogisticMovementExcepction_WhenInvokedAsync()
        {
            var clientMock = new Mock<IServiceBusQueueClient>();

            var repoMockTicket = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            repoMockTicket.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket());

            var repoMockLogisticMovement = new Mock<IRepository<LogisticMovement>>();
            this.mockUnitOfWork.Setup(u => u.CreateRepository<LogisticMovement>()).Returns(repoMockLogisticMovement.Object);
            repoMockLogisticMovement.Setup(u => u.FirstOrDefaultAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>())).Throws(new Exception());

            clientMock.Setup(m => m.QueueScheduleMessageAsync(It.IsAny<LogisticQueueMessage>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(clientMock.Object);

            var logisticMovement = new LogisticMovementsTicketRequest() { TicketId = 123, Movements = new List<int> { 18623 } };
            await this.logisticsProcessor.ForwardLogisticMovementsAsync(logisticMovement).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GenerateOfficialLogisticsAsync_ExceptionAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var transformedMovements = new List<GenericLogisticsMovement> { new GenericLogisticsMovement { MovementId = "1" } };

            var repoMockDetails = new Mock<IRepository<GenericLogisticsMovement>>();
            repoMockDetails.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Throws(new Exception());
            this.mockUnitOfWork.Setup(u => u.CreateRepository<GenericLogisticsMovement>()).Returns(repoMockDetails.Object);
            this.mockLogisticsService.Setup(r => r.TransformAsync(It.IsAny<List<GenericLogisticsMovement>>(), It.IsAny<Ticket>(), It.IsAny<SystemType>(), It.IsAny<ScenarioType>())).ReturnsAsync(transformedMovements);
            this.mockFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.mockFailureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockFailureHandler.Object);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);

            await this.logisticsProcessor.GenerateOfficialLogisticsAsync(ticket, (int)SystemType.SIV).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(u => u.CreateRepository<GenericLogisticsMovement>(), Times.Once);
            this.mockFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Once);
        }
    }
}
