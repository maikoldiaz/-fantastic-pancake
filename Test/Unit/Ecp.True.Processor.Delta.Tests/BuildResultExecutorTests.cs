// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildResultExecutorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Executors;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The BuildResultExecutorTests.
    /// </summary>
    [TestClass]
    public class BuildResultExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<BuildResultExecutor>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IExecutor buildResultExecutor;

        /// <summary>
        /// The delta data.
        /// </summary>
        private DeltaData deltaData;

        /// <summary>
        /// The delta request.
        /// </summary>
        private DeltaRequest deltaRequest;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IDeltaStrategyFactory> strategyFactory;

        /// <summary>
        /// Informations the build executor should return order.
        /// </summary>
        [TestMethod]
        public void BuildResultExecutor_ShouldReturnOrder()
        {
            Assert.AreEqual(6, this.buildResultExecutor.Order);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.strategyFactory = new Mock<IDeltaStrategyFactory>();
            this.mockLogger = new Mock<ITrueLogger<BuildResultExecutor>>();
            this.buildResultExecutor = new BuildResultExecutor(this.mockLogger.Object, this.strategyFactory.Object);
            this.deltaData = new DeltaData() { Ticket = new Ticket { TicketId = 123 } };
            this.deltaRequest = new DeltaRequest();
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldFormatData_WhenRequestFormedAsync()
        {
            var originalMovement = new DeltaOriginalMovement
            {
                MovementId = "Mov1",
                MovementTransactionId = 1,
                NetStandardVolume = 123.33M,
            };

            var updatedMovement = new DeltaUpdatedMovement
            {
                MovementId = "Mov1",
                MovementTransactionId = 1,
                NetStandardVolume = 123.33M,
                EventType = "Insert",
            };

            var originalInventory = new DeltaOriginalInventory
            {
                InventoryProductUniqueId = "Inv",
                InventoryProductId = 12,
                ProductVolume = 123.33M,
            };

            var updatedInventory = new DeltaUpdatedInventory
            {
                InventoryProductUniqueId = "Inv",
                InventoryProductId = 12,
                ProductVolume = 123.33M,
                EventType = "Insert",
            };
            var nodeTagList = new List<NodeTag>();
            nodeTagList.Add(
                new NodeTag
                {
                    NodeId = 5,
                });
            this.deltaRequest.OriginalMovements = new List<DeltaOriginalMovement> { originalMovement };
            this.deltaRequest.UpdatedMovements = new List<DeltaUpdatedMovement> { updatedMovement };
            this.deltaRequest.OriginalInventories = new List<DeltaOriginalInventory> { originalInventory };
            this.deltaRequest.UpdatedInventories = new List<DeltaUpdatedInventory> { updatedInventory };
            this.deltaData.NodeTags = nodeTagList;
            var movements = new List<Movement> { new Movement { MovementDestination = new MovementDestination { DestinationNodeId = 6 }, MovementSource = new MovementSource { SourceNodeId = 5 } } };

            this.strategyFactory.Setup(a => a.MovementDeltaStrategy.Build(It.IsAny<DeltaData>())).Returns(movements);
            this.strategyFactory.Setup(a => a.InventoryDeltaStrategy.Build(It.IsAny<DeltaData>())).Returns(movements);

            await this.buildResultExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);
            this.strategyFactory.Verify(a => a.MovementDeltaStrategy.Build(It.IsAny<DeltaData>()), Times.Exactly(1));
            this.strategyFactory.Verify(a => a.InventoryDeltaStrategy.Build(It.IsAny<DeltaData>()), Times.Exactly(1));
            Assert.IsNotNull(this.deltaData.GeneratedMovements);
            Assert.IsTrue(this.deltaData.GeneratedMovements.Any());
            Assert.IsFalse(this.deltaData.DeltaErrors.Any());
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnDeltaError_WhenValidationFailedAsync()
        {
            var originalMovement = new DeltaOriginalMovement
            {
                MovementId = "Mov1",
                MovementTransactionId = 1,
                NetStandardVolume = 123.33M,
            };

            var updatedMovement = new DeltaUpdatedMovement
            {
                MovementId = "Mov1",
                MovementTransactionId = 1,
                NetStandardVolume = 123.33M,
                EventType = "Insert",
            };

            var originalInventory = new DeltaOriginalInventory
            {
                InventoryProductUniqueId = "Inv",
                InventoryProductId = 12,
                ProductVolume = 123.33M,
            };

            var updatedInventory = new DeltaUpdatedInventory
            {
                InventoryProductUniqueId = "Inv",
                InventoryProductId = 12,
                ProductVolume = 123.33M,
                EventType = "Insert",
            };

            this.deltaRequest.OriginalMovements = new List<DeltaOriginalMovement> { originalMovement };
            this.deltaRequest.UpdatedMovements = new List<DeltaUpdatedMovement> { updatedMovement };
            this.deltaRequest.OriginalInventories = new List<DeltaOriginalInventory> { originalInventory };
            this.deltaRequest.UpdatedInventories = new List<DeltaUpdatedInventory> { updatedInventory };
            var movements = new List<Movement> { new Movement { MovementDestination = new MovementDestination { DestinationNodeId = 6 }, MovementSource = new MovementSource { SourceNodeId = 5 } } };

            this.strategyFactory.Setup(a => a.MovementDeltaStrategy.Build(It.IsAny<DeltaData>())).Returns(movements);
            this.strategyFactory.Setup(a => a.InventoryDeltaStrategy.Build(It.IsAny<DeltaData>())).Returns(movements);

            await this.buildResultExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);

            this.strategyFactory.Verify(a => a.MovementDeltaStrategy.Build(It.IsAny<DeltaData>()), Times.Exactly(1));
            this.strategyFactory.Verify(a => a.InventoryDeltaStrategy.Build(It.IsAny<DeltaData>()), Times.Exactly(1));
            Assert.IsNotNull(this.deltaData.DeltaErrors);
            Assert.IsFalse(this.deltaData.GeneratedMovements.Any());
            Assert.IsTrue(this.deltaData.DeltaErrors.Any());
        }

        /// <summary>
        /// Builds the result executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void BuildResultExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(6, this.buildResultExecutor.Order);
        }

        /// <summary>
        /// Builds the result executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void BuildResultExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.buildResultExecutor.ProcessType);
        }
    }
}
