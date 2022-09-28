// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryOfficialDeltaBuilderTests.cs" company="Microsoft">
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
    /// The BuildResultExecutorTests.
    /// </summary>
    [TestClass]
    public class InventoryOfficialDeltaBuilderTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<InventoryOfficialDeltaBuilder>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IOfficialDeltaBuilder inventoryOfficialDeltaBuilder;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<InventoryOfficialDeltaBuilder>>();
            this.inventoryOfficialDeltaBuilder = new InventoryOfficialDeltaBuilder(this.mockLogger.Object);
            var officialErrorInventory = new List<OfficialErrorInventory>
            {
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, InventoryProductId = 1, Description = "test1", InventoryId = "inventoryId1" },
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, InventoryProductId = 2, Description = "test2", InventoryId = "inventoryId2" },
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, InventoryProductId = 3, Description = "test3", InventoryId = "inventoryId3" },
            };
            var consolidationInventories = new List<ConsolidatedInventoryProduct>
            {
                new ConsolidatedInventoryProduct { ConsolidatedInventoryProductId = 1, NodeId = 2 },
            };
            var deltaNodes = new List<DeltaNode>
            {
                new DeltaNode { DeltaNodeId = 1, NodeId = 1 },
                new DeltaNode { DeltaNodeId = 2, NodeId = 1 },
            };
            var officialResultInventories = new List<OfficialResultInventory>
            {
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, TransactionId = 1, NetStandardVolume = 34.66M, Sign = true, InventoryProductUniqueId = "unique1", OwnerId = "1", OfficialDelta = 36.88M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, TransactionId = 1, NetStandardVolume = 33.66M, Sign = true, InventoryProductUniqueId = "unique2", OwnerId = "2", OfficialDelta = 31.88M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, TransactionId = 1, NetStandardVolume = 32.66M, Sign = false, InventoryProductUniqueId = "unique3", OwnerId = "3", OfficialDelta = 32.88M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, TransactionId = 2, NetStandardVolume = 31.66M, Sign = false, InventoryProductUniqueId = "unique4", OwnerId = "4", OfficialDelta = 33.88M },
            };

            var pendingOfficialInventories = new List<PendingOfficialInventory>
            {
                new PendingOfficialInventory { InventoryProductID = 1, NodeId = 1, InventoryDate = new DateTime(2020, 07, 06), OwnerId = 1, MeasurementUnit = 1, SegmentId = 1, ProductID = "1" },
                new PendingOfficialInventory { InventoryProductID = 2, NodeId = 1, InventoryDate = new DateTime(2020, 07, 07), OwnerId = 2, MeasurementUnit = 2, SegmentId = 2, ProductID = "2" },
            };

            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123, StartDate = new DateTime(2020, 07, 06) }, InventoryErrors = officialErrorInventory, ConsolidationInventories = consolidationInventories, DeltaNodes = deltaNodes, OfficialResultInventories = officialResultInventories, PendingOfficialInventories = pendingOfficialInventories };
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildErrors_ShouldReturnErrors_WhenOfficialDeltaDataFormedAsync()
        {
            var deltaErrors = await this.inventoryOfficialDeltaBuilder.BuildErrorsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(deltaErrors);
            Assert.AreEqual(2, deltaErrors.Count());

            Assert.AreEqual(1, deltaErrors.ElementAt(0).DeltaNodeId);
            Assert.AreEqual(1, deltaErrors.ElementAt(0).InventoryProductId);
            Assert.AreEqual("test1", deltaErrors.ElementAt(0).ErrorMessage);

            Assert.AreEqual(1, deltaErrors.ElementAt(1).DeltaNodeId);
            Assert.AreEqual(2, deltaErrors.ElementAt(1).InventoryProductId);
            Assert.AreEqual("test2", deltaErrors.ElementAt(1).ErrorMessage);
        }

        [TestMethod]
        public async Task BuildMovements_shouldReturnMovements_WhenOfficialDeltaDataFormedAsync()
        {
            var movements = await this.inventoryOfficialDeltaBuilder.BuildMovementsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(movements);
            Assert.AreEqual(3, movements.Count());

            // For InventoryProductId = 1
            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(0).ScenarioId);
            Assert.AreEqual(36.88M, movements.ElementAt(0).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(0).SourceInventoryProductId);
            Assert.AreEqual(1, movements.ElementAt(0).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(0).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialInventoryDelta, movements.ElementAt(0).OfficialDeltaMessageTypeId);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(0).OperationalDate);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(0).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(0).Period.EndTime);
            Assert.AreEqual(187, movements.ElementAt(0).MovementTypeId);

            // When the sign is true
            Assert.IsNull(movements.ElementAt(0).MovementSource);
            Assert.AreEqual(1, movements.ElementAt(0).MovementDestination.DestinationNodeId);
            Assert.AreEqual("1",movements.ElementAt(0).MovementDestination.DestinationProductId);
            Assert.AreEqual(1, movements.ElementAt(0).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(36.88M, movements.ElementAt(0).Owners.ElementAt(0).OwnershipValue);

            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(1).ScenarioId);
            Assert.AreEqual(32.88M, movements.ElementAt(1).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(1).SourceInventoryProductId);
            Assert.AreEqual(1, movements.ElementAt(1).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(1).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialInventoryDelta, movements.ElementAt(1).OfficialDeltaMessageTypeId);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(1).OperationalDate);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(1).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(1).Period.EndTime);
            Assert.AreEqual(187, movements.ElementAt(1).MovementTypeId);

            // When the sign is false
            Assert.IsNull(movements.ElementAt(1).MovementDestination);
            Assert.AreEqual(1, movements.ElementAt(1).MovementSource.SourceNodeId);
            Assert.AreEqual("1", movements.ElementAt(1).MovementSource.SourceProductId);
            Assert.AreEqual(3, movements.ElementAt(1).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(32.88M, movements.ElementAt(1).Owners.ElementAt(0).OwnershipValue);

            // For InventoryProductId = 2
            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(2).ScenarioId);
            Assert.AreEqual(33.88M, movements.ElementAt(2).NetStandardVolume);
            Assert.AreEqual(2, movements.ElementAt(2).SourceInventoryProductId);
            Assert.AreEqual(2, movements.ElementAt(2).MeasurementUnit);
            Assert.AreEqual(2, movements.ElementAt(2).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialInventoryDelta, movements.ElementAt(2).OfficialDeltaMessageTypeId);
            Assert.AreEqual(new DateTime(2020, 07, 07), movements.ElementAt(2).OperationalDate);
            Assert.AreEqual(new DateTime(2020, 07, 07), movements.ElementAt(2).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 07), movements.ElementAt(2).Period.EndTime);
            Assert.AreEqual(187, movements.ElementAt(2).MovementTypeId);

            // When the sign is false
            Assert.IsNull(movements.ElementAt(2).MovementDestination);
            Assert.AreEqual(1, movements.ElementAt(2).MovementSource.SourceNodeId);
            Assert.AreEqual("2", movements.ElementAt(2).MovementSource.SourceProductId);
            Assert.AreEqual(4, movements.ElementAt(2).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(33.88M, movements.ElementAt(2).Owners.ElementAt(0).OwnershipValue);
        }
    }
}
