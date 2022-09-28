// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ticket controller test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class ReportControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private ReportController controller;

        /// <summary>
        /// The ticket processor.
        /// </summary>
        private Mock<IReportProcessor> reportProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.reportProcessor = new Mock<IReportProcessor>();
            this.controller = new ReportController(this.reportProcessor.Object);
        }

        /// <summary>
        /// Validates the ownership nodes asynchronous should invoke processor to return validation result.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ValidateLogisticTicket_ShouldInvokeProcessor_ToReturnValidationResultAsync()
        {
            var ticket = new Ticket();
            IEnumerable<LogisticsTicketValidationResult> logisticValidationResult = new List<LogisticsTicketValidationResult>();
            this.reportProcessor.Setup(m => m.ValidateLogisticTicketAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(logisticValidationResult));
            var result = await this.controller.ValidateLogisticTicketAsync(ticket).ConfigureAwait(false);

            this.reportProcessor.Verify(c => c.ValidateLogisticTicketAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        [TestMethod]
        public async Task SaveOperationalDataWithoutCutoffAsync_ShouldCallProcessor_WhenInvokedAsync()
        {
            var cutoff = new ReportExecution
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
            };

            this.reportProcessor.Setup(m => m.SaveOperationalDataWithoutCutoffAsync(It.IsAny<ReportExecution>())).ReturnsAsync(10);
            var result = await this.controller.SaveOperationalDataWithoutCutoffAsync(cutoff).ConfigureAwait(false);

            this.reportProcessor.Verify(c => c.SaveOperationalDataWithoutCutoffAsync(It.IsAny<ReportExecution>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        [TestMethod]
        public async Task SaveTicketNodeStatusAsync_ShouldCallProcessor_WhenInvokedAsync()
        {
            var cutoff = new TicketNodeStatusData
            {
                ElementName = "TestElement",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                ExecutionId = "Test",
            };

            this.reportProcessor.Setup(m => m.SaveTicketNodeStatusAsync(It.IsAny<TicketNodeStatusData>())).Returns(Task.CompletedTask);
            var result = await this.controller.SaveTicketNodeStatusAsync(cutoff).ConfigureAwait(false);

            this.reportProcessor.Verify(c => c.SaveTicketNodeStatusAsync(It.IsAny<TicketNodeStatusData>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        [TestMethod]
        public async Task SaveEventContractReportRequestAsync_ShouldCallProcessor_WhenInvokedAsync()
        {
            var request = new EventContractReportRequest
            {
                ElementName = "TestElement",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                ExecutionId = "Test",
                Node = "TestNode",
            };

            this.reportProcessor.Setup(m => m.SaveEventContractReportRequestAsync(It.IsAny<EventContractReportRequest>())).Returns(Task.CompletedTask);
            var result = await this.controller.SaveEventContractReportRequestAsync(request).ConfigureAwait(false);

            this.reportProcessor.Verify(c => c.SaveEventContractReportRequestAsync(It.IsAny<EventContractReportRequest>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Creates the node configuration report request asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SaveNodeConfigurationReportRequestAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var nodeConfigurationReportRequest = new NodeConfigurationReportRequest();
            this.reportProcessor.Setup(m => m.SaveNodeConfigurationReportRequestAsync(nodeConfigurationReportRequest));

            var result = await this.controller.SaveNodeConfigurationReportRequestAsync(nodeConfigurationReportRequest).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.reportProcessor.Verify(c => c.SaveNodeConfigurationReportRequestAsync(nodeConfigurationReportRequest), Times.Once());
        }

        /// <summary>
        /// Saves the official initial report asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveOfficialInitialReportAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var officialBalanceExecutionStatus = new ReportExecution();
            this.reportProcessor.Setup(m => m.SaveOfficialBalanceReportAsync(It.IsAny<ReportExecution>(), ReportType.OfficialInitialBalance));

            var result = await this.controller.SaveOfficialInitialReportAsync(officialBalanceExecutionStatus).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.reportProcessor.Verify(m => m.SaveOfficialBalanceReportAsync(It.IsAny<ReportExecution>(), ReportType.OfficialInitialBalance), Times.Once);
        }

        /// <summary>
        /// Validates the exists report execution asynchronous returns true when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_ExistsReportExecutionAsync_ReturnsTrue_WhenInvokedAsync()
        {
            ReportExecution reportExecution = new ReportExecution()
            {
                CategoryId = 1,
                ElementId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                ExecutionId = 1,
            };
            this.reportProcessor.Setup(m => m.ExistsReportExecutionAsync(It.IsAny<ReportExecution>(), It.IsAny<ReportType>())).ReturnsAsync(reportExecution.ExecutionId);
            var result = await this.controller.ExistsReportExecutionAsync(reportExecution, ReportType.BeforeCutOff).ConfigureAwait(true);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.AreEqual(1, actionResult.Value);
        }

        /// <summary>
        /// Validates the exists report execution asynchronous returnsfalse when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_ExistsReportExecutionAsync_Returnsfalse_WhenInvokedAsync()
        {
            ReportExecution reportExecution = new ReportExecution();
            this.reportProcessor.Setup(m => m.ExistsReportExecutionAsync(It.IsAny<ReportExecution>(), It.IsAny<ReportType>())).ReturnsAsync(0);
            var result = await this.controller.ExistsReportExecutionAsync(reportExecution, ReportType.BeforeCutOff).ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.AreEqual(reportExecution.ExecutionId, actionResult.Value);
        }

        /// <summary>
        /// Queries the report execution asynchronous when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task QueryReportExecutionAsync_WhenInvokedAsync()
        {
            var reports = new List<ReportExecutionEntity>() { new ReportExecutionEntity() }.AsQueryable();
            this.reportProcessor.Setup(m => m.QueryViewAsync<ReportExecutionEntity>()).ReturnsAsync(reports);
            var result = await this.controller.QueryReportExecutionAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");

            this.reportProcessor.Verify(c => c.QueryViewAsync<ReportExecutionEntity>(), Times.Once());
        }

        /// <summary>
        /// Saves the official node report asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveNonOperationalSegmentOwnershipReportAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var reportExecution = new ReportExecution();
            this.reportProcessor.Setup(m => m.SaveOfficialBalanceReportAsync(It.IsAny<ReportExecution>(), ReportType.OperativeBalance));

            var result = await this.controller.SaveNonOperationalSegmentOwnershipReportAsync(reportExecution).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.reportProcessor.Verify(m => m.SaveOfficialBalanceReportAsync(It.IsAny<ReportExecution>(), ReportType.OperativeBalance), Times.Once);
        }

        [TestMethod]
        public async Task SaveMovementSendToSapReportRequestAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var reportExecution = new ReportExecution();
            this.reportProcessor.Setup(m => m.SaveEventSendToSapReportRequestAsync(It.IsAny<ReportExecution>()));

            var result = await this.controller.SaveMovementSendToSapReportRequestAsync(reportExecution).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.reportProcessor.Verify(m => m.SaveEventSendToSapReportRequestAsync(It.IsAny<ReportExecution>()), Times.Once);
        }

        [TestMethod]
        public async Task SaveOfficialNodeStatusReportRequestAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var reportExecution = new OfficialNodeStatusReportRequest();
            this.reportProcessor.Setup(m => m.SaveEventOfficialNodeStatusReportRequestAsync(It.IsAny<OfficialNodeStatusReportRequest>()));

            var result = await this.controller.SaveOfficialNodeStatusReportRequestAsync(reportExecution).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.reportProcessor.Verify(m => m.SaveEventOfficialNodeStatusReportRequestAsync(It.IsAny<OfficialNodeStatusReportRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task SaveUserRolesAndPermissionsReportRequestAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var reportExecution = new ReportExecution();
            this.reportProcessor.Setup(m => m.SaveUserRolesAndPermissionsReportRequestAsync(It.IsAny<ReportExecution>()));

            var result = await this.controller.SaveUserRolesAndPermissionsReportRequestAsync(reportExecution).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.reportProcessor.Verify(m => m.SaveUserRolesAndPermissionsReportRequestAsync(It.IsAny<ReportExecution>()), Times.Once);
        }
    }
}
