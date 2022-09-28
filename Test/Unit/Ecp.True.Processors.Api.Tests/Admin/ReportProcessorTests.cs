// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportProcessorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ticket processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class ReportProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private ReportProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The report execution repository.
        /// </summary>
        private Mock<IRepository<ReportExecution>> reportExecutionRepository;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ReportProcessor>> mockLogger;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.reportExecutionRepository = new Mock<IRepository<ReportExecution>>();
            this.mockLogger = new Mock<ITrueLogger<ReportProcessor>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<int>()));
            this.mockFactory.Setup(m => m.TicketInfoRepository.GetLastTicketIdAsync()).ReturnsAsync(10);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ReportExecution>()).Returns(this.reportExecutionRepository.Object);
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockConfigurationHandler.Setup(c => c.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings)).ReturnsAsync(new ServiceBusSettings());

            this.processor = new ReportProcessor(this.mockLogger.Object, this.mockFactory.Object, this.mockUnitOfWorkFactory.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Validates logistic ticket nodes should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveOperationalDataWithoutCutoffAsync_ShouldExecuteStoredProc_WhenInvokedAsync()
        {
            var cutoff = new ReportExecution();
            this.reportExecutionRepository.Setup(m => m.Insert(It.IsAny<ReportExecution>()));

            await this.processor.SaveOperationalDataWithoutCutoffAsync(cutoff).ConfigureAwait(false);

            this.reportExecutionRepository.Verify(m => m.Insert(It.IsAny<ReportExecution>()), Times.Once);
        }

        [TestMethod]
        public async Task SaveOperationalDataWithoutCutoffAsync_ShouldFailReport_If_PushMessageToServiceBusFails_WhenInvokedAsync()
        {
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<int>())).Throws(new Exception());
            this.reportExecutionRepository.Setup(m => m.Insert(It.IsAny<ReportExecution>()));
            this.reportExecutionRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ReportExecution { ExecutionId = 123 });
            this.reportExecutionRepository.Setup(a => a.Update(It.IsAny<ReportExecution>()));
            var cutoff = new ReportExecution();

            await this.processor.SaveOperationalDataWithoutCutoffAsync(cutoff).ConfigureAwait(false);

            this.reportExecutionRepository.Verify(m => m.Insert(It.IsAny<ReportExecution>()), Times.Once);
            this.reportExecutionRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
            this.reportExecutionRepository.Verify(a => a.Update(It.IsAny<ReportExecution>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ReportExecution>(), Times.Exactly(2));
            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Exactly(2));
        }

        /// <summary>
        /// Validates logistic ticket nodes should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateLogisticTicket_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow, NodeId = 5 };
            IEnumerable<LogisticsTicketValidationResult> validationResult = new List<LogisticsTicketValidationResult>();
            var repoMock = new Mock<IRepository<LogisticsTicketValidationResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(validationResult);
            this.mockFactory.Setup(m => m.CreateRepository<LogisticsTicketValidationResult>()).Returns(repoMock.Object);
            var result = await this.processor.ValidateLogisticTicketAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<LogisticsTicketValidationResult>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Saves the official balance report asynchronous should save official balance execution status and send message to queue when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveOfficialBalanceReportAsync_ShouldSaveOfficialBalanceExecutionStatus_And_SendMessageToQueue_WhenInvokedAsync()
        {
            var officialBalanceExecutionStatus = new ReportExecution();

            this.reportExecutionRepository.Setup(a => a.Insert(It.IsAny<ReportExecution>()));

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<int>()));

            await this.processor.SaveOfficialBalanceReportAsync(officialBalanceExecutionStatus, ReportType.OfficialInitialBalance).ConfigureAwait(false);

            this.reportExecutionRepository.Verify(a => a.Insert(It.IsAny<ReportExecution>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<ReportExecution>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Validates the exists report execution asynchronous returns true when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_ExistsReportExecutionAsync_ReturnsTrue_WhenInvokedAsync()
        {
            var reportExecution = new ReportExecution()
            {
                CategoryId = 1,
                ElementId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ExecutionId = 1,
            };
            var mockRepository = new Mock<IRepository<ReportExecution>>();
            var mockUnitOfwork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(mockUnitOfwork.Object);

            mockRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>())).ReturnsAsync(reportExecution);
            mockUnitOfwork.Setup(m => m.CreateRepository<ReportExecution>()).Returns(mockRepository.Object);

            var result = await this.processor.ExistsReportExecutionAsync(reportExecution, ReportType.BeforeCutOff).ConfigureAwait(false);

            Assert.AreEqual(1, result);
            mockRepository.Verify(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
        }

        /// <summary>
        /// Validates the exists report execution asynchronous returns false when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_ExistsReportExecutionAsync_ReturnsFalse_WhenInvokedAsync()
        {
            var reportExecution = new ReportExecution()
            {
                CategoryId = 1,
                ElementId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ExecutionId = 1,
            };
            var mockRepository = new Mock<IRepository<ReportExecution>>();
            var mockUnitOfwork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(mockUnitOfwork.Object);

            mockRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>())).ReturnsAsync(default(ReportExecution));
            mockUnitOfwork.Setup(m => m.CreateRepository<ReportExecution>()).Returns(mockRepository.Object);

            var result = await this.processor.ExistsReportExecutionAsync(reportExecution, ReportType.BeforeCutOff).ConfigureAwait(false);

            Assert.AreEqual(0, result);
            mockRepository.Verify(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
        }

        [TestMethod]
        public async Task SaveEventSendToSapReportRequestAsync_ShouldAddExecutionToQueue_WhenInvokeAsync()
        {
            var reportExecution = new ReportExecution
            {
                CategoryId = 1,
                ElementId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ExecutionId = 1,
            };

            var mockRepository = new Mock<IRepository<ReportExecution>>();
            var mockUnitOfwork = new Mock<IUnitOfWork>();

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(mockUnitOfwork.Object);

            mockRepository.Setup(a => a.Insert(It.IsAny<ReportExecution>())).Verifiable();
            mockUnitOfwork.Setup(m => m.CreateRepository<ReportExecution>()).Returns(mockRepository.Object);
            mockUnitOfwork.Setup(m => m.SaveAsync(It.IsAny<CancellationToken>())).Verifiable();

            var result = await this.processor.SaveEventSendToSapReportRequestAsync(reportExecution).ConfigureAwait(false);

            Assert.AreEqual(1, result);
            mockRepository.Verify(m => m.Insert(It.IsAny<ReportExecution>()), Times.Once);
            mockUnitOfwork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
        }

        [TestMethod]
        public void SaveEventOfficialNodeStatusReportRequest_ShouldExecuteStoreProcedure_WhenInvoke()
        {
            var officialNodeStatusReportRequest = new OfficialNodeStatusReportRequest
            {
                ElementName = "Segment-name-1",
                ExecutionId = "123",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
            };

            var mockRepository = new Mock<IRepository<Ticket>>();

            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockRepository.Object);

            _ = this.processor.SaveEventOfficialNodeStatusReportRequestAsync(officialNodeStatusReportRequest);

            mockRepository.Verify(a => a.ExecuteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        [TestMethod]
        public async Task SaveUserRolesAndPermissionsReportRequestAsync_ShouldAddExecutionToQueue_WhenInvokeAsync()
        {
            var reportExecution = new ReportExecution
            {
                ExecutionId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ReportTypeId = ReportType.UserRolesAndPermissions,
            };

            var mockRepository = new Mock<IRepository<ReportExecution>>();
            var mockUnitOfwork = new Mock<IUnitOfWork>();

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(mockUnitOfwork.Object);

            mockRepository.Setup(a => a.Insert(It.IsAny<ReportExecution>())).Verifiable();
            mockUnitOfwork.Setup(m => m.CreateRepository<ReportExecution>()).Returns(mockRepository.Object);
            mockUnitOfwork.Setup(m => m.SaveAsync(It.IsAny<CancellationToken>())).Verifiable();

            var result = await this.processor.SaveUserRolesAndPermissionsReportRequestAsync(reportExecution).ConfigureAwait(false);

            Assert.AreEqual(1, result);
            mockRepository.Verify(m => m.Insert(It.IsAny<ReportExecution>()), Times.Once);
            mockUnitOfwork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
        }
    }
}
