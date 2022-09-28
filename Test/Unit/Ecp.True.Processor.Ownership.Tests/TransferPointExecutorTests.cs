// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferPointExecutorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class TransferPointExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<TransferPointExecutor>> mockLogger;

        /// <summary>
        /// The mock analytical ownership calculation service.
        /// </summary>
        private Mock<IAnalyticalOwnershipCalculationService> mockAnalyticalOwnershipCalculationService;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The transfer point executor.
        /// </summary>
        private IExecutor transferPointExecutor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<TransferPointExecutor>>();
            this.mockAnalyticalOwnershipCalculationService = new Mock<IAnalyticalOwnershipCalculationService>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.transferPointExecutor = new TransferPointExecutor(this.mockLogger.Object, this.mockAnalyticalOwnershipCalculationService.Object);
        }

        /// <summary>
        /// Executes the asynchronous transfer point executor asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_TransferPointExecutorAsync()
        {
            var transferPointMovement = new TransferPointMovement
            {
                TicketId = 25281,
            };

            this.mockAnalyticalOwnershipCalculationService.Setup(x => x.GetTransferPointMovementsAsync(It.IsAny<int>())).ReturnsAsync(new List<TransferPointMovement> { transferPointMovement });
            this.mockAnalyticalOwnershipCalculationService.Setup(x => x.GetOwnershipAnalyticalDataAsync(It.IsAny<List<TransferPointMovement>>())).ReturnsAsync(new List<OwnershipAnalytics>());

            await this.transferPointExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockAnalyticalOwnershipCalculationService.Verify(x => x.GetTransferPointMovementsAsync(It.IsAny<int>()), Times.Once);
            this.mockAnalyticalOwnershipCalculationService.Verify(x => x.GetOwnershipAnalyticalDataAsync(It.IsAny<List<TransferPointMovement>>()), Times.Once);
        }

        /// <summary>
        /// Transfers the point executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void TransferPointExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.transferPointExecutor.ProcessType);
        }

        /// <summary>
        /// Transfers the point executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void TransferPointExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(1, this.transferPointExecutor.Order);
        }
    }
}
