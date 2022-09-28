// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticControllerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ticket controller test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class LogisticControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private LogisticController controller;

        /// <summary>
        /// The ticket processor.
        /// </summary>
        private Mock<ILogisticsProcessor> logisticProcessor;

        [TestInitialize]
        public void Initialize()
        {
            this.logisticProcessor = new Mock<ILogisticsProcessor>();
            this.controller = new LogisticController(this.logisticProcessor.Object);
        }

        /// <summary>
        /// Validates the logistic nodes availables.
        /// </summary>
        /// <returns>The Task ticket.</returns>
        [TestMethod]
        public async Task ValidateNodeAvailablesLogisticMovement_ShouldInvokeProcessor_ToReturnValidationResultAsync()
        {
            var ticket = new Ticket();
            IEnumerable<NodesForSegmentResult> ticketResult = new List<NodesForSegmentResult>();
            this.logisticProcessor.Setup(m => m.LogisticMovementNodeValidationsAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(ticketResult));
            var result = await this.controller.ValidateNodeAvailablesLogisticMovementAsync(ticket).ConfigureAwait(false);

            this.logisticProcessor.Verify(c => c.LogisticMovementNodeValidationsAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Get logistic movement for ticket identifier asynchronous should call store procedure.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueryLogisticMovementAsync_ShouldCallProcedure_ToReturnSuccessAsync()
        {
            var logisticMovement = new List<SapLogisticMovementDetail>() { new SapLogisticMovementDetail() }.AsQueryable();
            this.logisticProcessor.Setup(t => t.GetLogisticMovementDetailAsync(It.IsAny<int>())).ReturnsAsync(logisticMovement);
            var result = await this.controller.QueryLogisticMovementAsync(default(int)).ConfigureAwait(false);

            Assert.AreEqual(logisticMovement, result);
            this.logisticProcessor.Verify(x => x.GetLogisticMovementDetailAsync(It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Get failed logistic movement for ticket identifier asynchronous should call store procedure.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task QueryFailedLogisticMovementAsync_ShouldCallProcedure_ToReturnSuccessAsync()
        {
            DateTime startDate = DateTime.Now.AddDays(-2);
            DateTime endDate = DateTime.Now;
            int[] ticketNodes = new int[1];
            var ticket = new Ticket();
            ticket.CategoryElementId = default(int);
            ticket.StartDate = startDate;
            ticket.EndDate = endDate;
            ticket.ScenarioTypeId = (ScenarioType)default(int);
            ticket.OwnerId = default(int);
            ticket.TicketNodes = ticketNodes.Select(x => new Entities.Admin.TicketNode { NodeId = x });
            var logisticMovement = new List<SapLogisticMovementDetail>() { new SapLogisticMovementDetail() }.AsQueryable();
            this.logisticProcessor.Setup(t => t.FailedLogisticMovementAsync(ticket)).ReturnsAsync(logisticMovement);
            await this.controller.QueryFailedLogisticMovementAsync(default(int), startDate, endDate, default(int), default(int), ticketNodes).ConfigureAwait(false);
            this.logisticProcessor.Verify(x => x.FailedLogisticMovementAsync(It.IsAny<Ticket>()), Times.Once());
        }

        /// <summary>
        /// Test of CancelBatchAsync of class Logistic Movement.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task LogisticCancelBatch_ShouldCancelTicketAsync_ToReturnSuccessAsync()
        {
            int ticketIdTest = 1;
            this.logisticProcessor.Setup(m => m.CancelBatchAsync(It.IsAny<int>()));
            await this.controller.CancelBatchAsync(ticketIdTest).ConfigureAwait(false);
            this.logisticProcessor.Verify(x => x.CancelBatchAsync(ticketIdTest), Times.Once);
        }

        /// <summary>
        /// Test of confirm logistic movements.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConfirmedLogisticMovements_ShouldCancelTicketAsync_ToReturnSuccessAsync()
        {
            LogisticMovementsTicketRequest ticketRequest = new LogisticMovementsTicketRequest { TicketId = 3 };
            this.logisticProcessor.Setup(m => m.ConfirmLogisticMovementsAsync(It.IsAny<LogisticMovementsTicketRequest>()));
            await this.controller.ConfirmLogisticMovementsAsync(ticketRequest).ConfigureAwait(false);
            this.logisticProcessor.Verify(x => x.ConfirmLogisticMovementsAsync(ticketRequest), Times.Once);
        }

        /// <summary>
        /// Test of forward logistic movements.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ForwardLogisticMovements_ToReturnSuccessAsync()
        {
            LogisticMovementsTicketRequest ticketRequest = new LogisticMovementsTicketRequest { TicketId = 3, Movements = new List<int> { 281032 } };
            this.logisticProcessor.Setup(m => m.ConfirmLogisticMovementsAsync(It.IsAny<LogisticMovementsTicketRequest>()));
            await this.controller.ForwardLogisticMovementsAsync(ticketRequest).ConfigureAwait(false);
            this.logisticProcessor.Verify(x => x.ForwardLogisticMovementsAsync(ticketRequest), Times.Once);
        }
    }
}
