// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaInventoryResultMovementBuilderTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Builders;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OfficialDeltaInventoryResultMovementBuilderTests.
    /// </summary>
    [TestClass]
    public class OfficialDeltaInventoryResultMovementBuilderTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OfficialDeltaInventoryResultMovementBuilder>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IOfficialDeltaBuilder inventoryOfficialDeltaBuilder;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<OfficialDeltaInventoryResultMovementBuilder>>();
            this.inventoryOfficialDeltaBuilder = new OfficialDeltaInventoryResultMovementBuilder(this.mockLogger.Object);
            var officialErrorMovement = new List<OfficialErrorInventory>
            {
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.DELTAOFICIAL, InventoryId = "test1", Description = "test1", InventoryProductId = 1 },
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.DELTAOFICIAL, InventoryId = "test2", Description = "test2", InventoryProductId = 2 },
            };
            var consolidationInventories = new List<ConsolidatedInventoryProduct>
            {
                new ConsolidatedInventoryProduct { ConsolidatedInventoryProductId = 1, NodeId = 2 },
            };
            var deltaNodes = new List<DeltaNode>
            {
                new DeltaNode { DeltaNodeId = 1, NodeId = 1 },
                new DeltaNode { DeltaNodeId = 2, NodeId = 2 },
            };
            var officialResultInventories = new List<OfficialResultInventory>
            {
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.DELTAOFICIAL, TransactionId = 1, NetStandardVolume = 34.66M, Sign = true, InventoryProductUniqueId = "unique1", OwnerId = "1", OfficialDelta = 36.88M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, TransactionId = 1, NetStandardVolume = 33.66M, Sign = true, InventoryProductUniqueId = "unique2", OwnerId = "2", OfficialDelta = 31.88M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.DELTAOFICIAL, TransactionId = 1, NetStandardVolume = 32.66M, Sign = false, InventoryProductUniqueId = "unique3", OwnerId = "3", OfficialDelta = 32.88M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.DELTAOFICIAL, TransactionId = 2, NetStandardVolume = 31.66M, Sign = false, InventoryProductUniqueId = "unique4", OwnerId = "4", OfficialDelta = 33.88M },
            };

            var pendingOfficialInventories = new List<OfficialDeltaInventory>
            {
                new OfficialDeltaInventory { MovementTransactionId = 1, SourceNodeId = 1, SourceProductId = "1", DestinationNodeId = 2, DestinationProductId = "1", OwnerId = 1, SegmentId = 1, MeasurementUnit = 31, OperationalDate = new DateTime(2020, 07, 07) },
                new OfficialDeltaInventory { MovementTransactionId = 2, SourceProductId = "1", DestinationNodeId = 1, DestinationProductId = "1", OwnerId = 1, SegmentId = 2, MeasurementUnit = 31,OperationalDate = new DateTime(2020, 07, 07) },
            };
            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123, StartDate = new DateTime(2020, 07, 06) }, InventoryErrors = officialErrorMovement, ConsolidationInventories = consolidationInventories, DeltaNodes = deltaNodes, OfficialResultInventories = officialResultInventories, OfficialDeltaInventories = pendingOfficialInventories };
        }

        /// <summary>
        /// Builds the movements should return movements when official delta data formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildMovements_shouldReturnMovements_WhenOfficialDeltaDataFormedAsync()
        {
            var movements = await this.inventoryOfficialDeltaBuilder.BuildMovementsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(movements);
            Assert.AreEqual(3, movements.Count());

            // for MovementTransactionId = 1
            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(0).ScenarioId);
            Assert.AreEqual(36.88M, movements.ElementAt(0).NetStandardVolume);
            Assert.AreEqual(31, movements.ElementAt(0).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(0).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialInventoryDelta, movements.ElementAt(0).OfficialDeltaMessageTypeId);
            Assert.AreEqual(187, movements.ElementAt(0).MovementTypeId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementDestination.DestinationNodeId);
            Assert.AreEqual("1", movements.ElementAt(0).MovementDestination.DestinationProductId);

            // when sign is true
            Assert.AreEqual(1, movements.ElementAt(0).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(36.88M, movements.ElementAt(0).Owners.ElementAt(0).OwnershipValue);

            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(1).ScenarioId);
            Assert.AreEqual(32.88M, movements.ElementAt(1).NetStandardVolume);
            Assert.AreEqual(31, movements.ElementAt(1).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(1).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialInventoryDelta, movements.ElementAt(1).OfficialDeltaMessageTypeId);
            Assert.AreEqual(187, movements.ElementAt(1).MovementTypeId);

            // when sign is false
            Assert.AreEqual(2, movements.ElementAt(1).MovementSource.SourceNodeId);
            Assert.AreEqual("1", movements.ElementAt(1).MovementSource.SourceProductId);
            Assert.AreEqual(3, movements.ElementAt(1).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(32.88M, movements.ElementAt(1).Owners.ElementAt(0).OwnershipValue);

            // for MovementTransactionId = 2
            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(2).ScenarioId);
            Assert.AreEqual(33.88M, movements.ElementAt(2).NetStandardVolume);
            Assert.AreEqual(31, movements.ElementAt(2).MeasurementUnit);
            Assert.AreEqual(2, movements.ElementAt(2).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialInventoryDelta, movements.ElementAt(2).OfficialDeltaMessageTypeId);
            Assert.AreEqual(187, movements.ElementAt(2).MovementTypeId);

            // when sign is false
            Assert.AreEqual(4, movements.ElementAt(2).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(33.88M, movements.ElementAt(2).Owners.ElementAt(0).OwnershipValue);
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildErrors_ShouldReturnMovementErrors_WhenOfficialDeltaDataFormedAsync()
        {
            var deltaErrors = await this.inventoryOfficialDeltaBuilder.BuildErrorsAsync(this.deltaData).ConfigureAwait(false);

            Assert.IsNotNull(deltaErrors);
            Assert.AreEqual(3, deltaErrors.Count());

            Assert.AreEqual(1, deltaErrors.ElementAt(0).DeltaNodeId);
            Assert.AreEqual(1, deltaErrors.ElementAt(0).MovementTransactionId);
            Assert.AreEqual("test1", deltaErrors.ElementAt(0).ErrorMessage);

            Assert.AreEqual(2, deltaErrors.ElementAt(1).DeltaNodeId);
            Assert.AreEqual(1, deltaErrors.ElementAt(1).MovementTransactionId);
            Assert.AreEqual("test1", deltaErrors.ElementAt(1).ErrorMessage);

            Assert.AreEqual(1, deltaErrors.ElementAt(2).DeltaNodeId);
            Assert.AreEqual(2, deltaErrors.ElementAt(2).MovementTransactionId);
            Assert.AreEqual("test2", deltaErrors.ElementAt(2).ErrorMessage);
        }
    }
}
