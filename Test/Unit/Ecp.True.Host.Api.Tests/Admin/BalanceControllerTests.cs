// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceControllerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The node controller tests.
    /// </summary>
    [TestClass]
    public class BalanceControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private BalanceController controller;

        /// <summary>
        /// The QueueProcessor.
        /// </summary>
        private QueueProcessor queueProcessor;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IBalanceProcessor> balanceProcessorMock;

        /// <summary>
        /// The mock cut Off processor.
        /// </summary>
        private Mock<IQueueProcessor> queuProcessorMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<TicketProcessor>> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> azureClientFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.balanceProcessorMock = new Mock<IBalanceProcessor>();
            this.queuProcessorMock = new Mock<IQueueProcessor>();
            this.logger = new Mock<ITrueLogger<TicketProcessor>>();
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.controller = new BalanceController(this.balanceProcessorMock.Object, this.queuProcessorMock.Object);
            this.queueProcessor = new QueueProcessor(this.logger.Object, this.azureClientFactory.Object);
        }

        /// <summary>
        /// Nodes the name exists asynchronous should invoke processor to return true.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task ValidateUnbalanceTicketAsync_ShouldInvokeProcessor_ToReturnTheNodeEntitiesAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow,
                CategoryElementId = 1,
            };
            IEnumerable<OwnershipInitialInventoryNode> nodeList = new List<OwnershipInitialInventoryNode>
            {
                new OwnershipInitialInventoryNode
                {
                    NodeName = "someNode",
                    InventoryDate = DateTime.UtcNow,
                    Type = 1,
                },
            };
            this.balanceProcessorMock.Setup(m => m.ValidateOwnershipInitialInventoryAsync(ticketLocal)).ReturnsAsync(nodeList);

            var result = await this.controller.ValidateInitialInventoryAsync(ticketLocal).ConfigureAwait(false);

            var entityExistsResult = result as EntityResult;
            var nodes = (IEnumerable<OwnershipInitialInventoryNode>)entityExistsResult.Value;

            // Assert
            Assert.IsNotNull(entityExistsResult);

            this.balanceProcessorMock.Verify(c => c.ValidateOwnershipInitialInventoryAsync(ticketLocal), Times.Once());
            Assert.AreEqual(nodeList.FirstOrDefault().NodeName, nodes.FirstOrDefault().NodeName);
            Assert.AreEqual(nodeList.FirstOrDefault().InventoryDate, nodes.FirstOrDefault().InventoryDate);
            Assert.AreEqual(nodeList.FirstOrDefault().Type, nodes.FirstOrDefault().Type);
        }

        /// <summary>
        /// Gets unbalances for a ticket when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetUnbalancesAsync_ReturnsUnbalances_WhenInvokedAsync()
        {
            var unbalances = new List<UnbalanceComment>() { new UnbalanceComment(), new UnbalanceComment() };
            this.balanceProcessorMock.Setup(m => m.CalculateAsync(It.IsAny<UnbalanceRequest>())).Returns(Task.FromResult(unbalances.AsEnumerable()));

            var result = await this.controller.GetUnbalancesAsync(new UnbalanceRequest()).ConfigureAwait(false);

            // Assert or verify
            this.balanceProcessorMock.Verify(c => c.CalculateAsync(It.IsAny<UnbalanceRequest>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Queries the systemUnbalances returns systemUnbalances when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryTickets_ReturnsSystemUnbalance_WhenInvokedAsync()
        {
            var systemUnbalances = new[] { new SystemUnbalance() }.AsQueryable();
            this.balanceProcessorMock.Setup(m => m.QueryAllAsync<SystemUnbalance>(null)).ReturnsAsync(systemUnbalances);

            var result = await this.controller.QuerySystemUnbalanceAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, systemUnbalances);

            this.balanceProcessorMock.Verify(c => c.QueryAllAsync<SystemUnbalance>(null), Times.Once());
        }

        /// <summary>
        /// Gets the transfer points asynchronous returns movements marked official when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetTransferPointsAsync_ReturnsMovementsMarkedOfficial_WhenInvokedAsync()
        {
            var transferPointMovements = new List<OfficialTransferPointMovement>() { new OfficialTransferPointMovement(), new OfficialTransferPointMovement() };
            this.balanceProcessorMock.Setup(m => m.GetTransferPointsAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(transferPointMovements.AsEnumerable()));

            var result = await this.controller.GetTransferPointsAsync(new Ticket()).ConfigureAwait(false);

            // Assert or verify
            this.balanceProcessorMock.Verify(c => c.GetTransferPointsAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the sap tracking errors asynchronous returns sap tracking errors when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetSapTrackingErrorsAsync_ReturnsSapTrackingErrors_WhenInvokedAsync()
        {
            var errors = new List<SapTrackingError>() { new SapTrackingError(), new SapTrackingError() };
            this.balanceProcessorMock.Setup(m => m.GetSapTrackingErrorsAsync(It.IsAny<int>())).Returns(Task.FromResult(errors.AsEnumerable()));

            var result = await this.controller.GetSapTrackingErrorsAsync(It.IsAny<int>()).ConfigureAwait(false);

            // Assert or verify
            this.balanceProcessorMock.Verify(c => c.GetSapTrackingErrorsAsync(It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the sap tracking errors asynchronous returns sap tracking errors when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetFirstTimeNodes_WhenInvokedAsync()
        {
            var nodeIds = new List<int>() { 1 };
            this.balanceProcessorMock.Setup(m => m.GetFirstTimeNodesAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(nodeIds.AsEnumerable()));

            var result = await this.controller.GetFirstTimeNodesAsync(It.IsAny<Ticket>()).ConfigureAwait(false);

            // Assert or verify
            this.balanceProcessorMock.Verify(c => c.GetFirstTimeNodesAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the sap tracking errors asynchronous returns sap tracking errors when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task RecalculateCutOffBalanc_WhenInvokedAsync()
        {
            this.queuProcessorMock.Setup(m => m.PushQueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));

            var result = await this.controller.RecalculateCutOffBalanceAsync(It.IsAny<int>()).ConfigureAwait(false);

            // Assert or verify
            this.queuProcessorMock.Verify(c => c.PushQueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }
    }
}