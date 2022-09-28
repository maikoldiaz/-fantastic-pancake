// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedInventoryDeltaBuilderTests.cs" company="Microsoft">
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
    public class ConsolidatedInventoryDeltaBuilderTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ConsolidatedInventoryDeltaBuilder>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IOfficialDeltaBuilder consolidatedInventoryDeltaBuilder;

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
            this.mockLogger = new Mock<ITrueLogger<ConsolidatedInventoryDeltaBuilder>>();
            this.consolidatedInventoryDeltaBuilder = new ConsolidatedInventoryDeltaBuilder(this.mockLogger.Object);
            var officialErrorInventory = new List<OfficialErrorInventory>
            {
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, InventoryProductId = 1, Description = "test1", InventoryId = "inventory1" },
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, InventoryProductId = 2, Description = "test2", InventoryId = "inventory2" },
                new OfficialErrorInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, InventoryProductId = 3, Description = "test3", InventoryId = "inventory3" },
            };
            var consolidationInventories = new List<ConsolidatedInventoryProduct>
            {
                new ConsolidatedInventoryProduct { ConsolidatedInventoryProductId = 1, NodeId = 1, SegmentId = 1, MeasurementUnit = "1", InventoryDate = new DateTime(2020, 07, 01), ProductId = "producttest1" },
                new ConsolidatedInventoryProduct { ConsolidatedInventoryProductId = 3, NodeId = 2, SegmentId = 2, MeasurementUnit = "2", InventoryDate = new DateTime(2020, 07, 01), ProductId = "producttest2" },
                new ConsolidatedInventoryProduct { ConsolidatedInventoryProductId = 2, NodeId = 2, SegmentId = 4, MeasurementUnit = "4", InventoryDate = new DateTime(2020, 07, 01), ProductId = "producttest3" },
            };
            var deltaNodes = new List<DeltaNode>
            {
                new DeltaNode { DeltaNodeId = 1, NodeId = 1 },
                new DeltaNode { DeltaNodeId = 2, NodeId = 2 },
            };
            var officialResultInventories = new List<OfficialResultInventory>
            {
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, TransactionId = 1, NetStandardVolume = 34.66M, Sign = true, InventoryProductUniqueId = "uniqueid1", OwnerId = "1", OfficialDelta = 31.20M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.OFICIAL, TransactionId = 1, NetStandardVolume = 32.66M, Sign = true, InventoryProductUniqueId = "uniqueid2", OwnerId = "2", OfficialDelta = 32.20M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, TransactionId = 1, NetStandardVolume = 31.66M, Sign = false, InventoryProductUniqueId = "uniqueid3", OwnerId = "3", OfficialDelta = 33.20M },
                new OfficialResultInventory { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, TransactionId = 2, NetStandardVolume = 30.66M, Sign = false, InventoryProductUniqueId = "uniqueid4", OwnerId = "4", OfficialDelta = 34.20M },
            };

            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123 }, InventoryErrors = officialErrorInventory, ConsolidationInventories = consolidationInventories, DeltaNodes = deltaNodes, OfficialResultInventories = officialResultInventories };
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildErrors_ShouldReturnErrors_WhenOfficialDeltaDataFormedAsync()
        {
            var deltaErrors = await this.consolidatedInventoryDeltaBuilder.BuildErrorsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(deltaErrors);
            Assert.AreEqual(2, deltaErrors.Count());

            Assert.AreEqual(1, deltaErrors.ElementAt(0).ConsolidatedInventoryProductId);
            Assert.AreEqual(1, deltaErrors.ElementAt(0).DeltaNodeId);
            Assert.AreEqual("test1", deltaErrors.ElementAt(0).ErrorMessage);

            Assert.AreEqual(2, deltaErrors.ElementAt(1).ConsolidatedInventoryProductId);
            Assert.AreEqual(2, deltaErrors.ElementAt(1).DeltaNodeId);
            Assert.AreEqual("test2", deltaErrors.ElementAt(1).ErrorMessage);
        }

        [TestMethod]
        public async Task BuildMovements_shouldReturnMovements_WhenOfficialDeltaDataFormedAsync()
        {
            var movements = await this.consolidatedInventoryDeltaBuilder.BuildMovementsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(movements);
            Assert.AreEqual(3, movements.Count());

            // for MovementTransactionId = 1
            Assert.AreEqual(31.20M, movements.ElementAt(0).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(0).ConsolidatedInventoryProductId);
            Assert.AreEqual(1, movements.ElementAt(0).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(0).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.ConsolidatedInventoryDelta, movements.ElementAt(0).OfficialDeltaMessageTypeId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(0).OperationalDate);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(0).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(0).Period.EndTime);
            Assert.AreEqual(187, movements.ElementAt(0).MovementTypeId);

            // when sign is true
            Assert.IsNull(movements.ElementAt(0).MovementSource);
            Assert.AreEqual(1, movements.ElementAt(0).MovementDestination.DestinationNodeId);
            Assert.AreEqual("producttest1", movements.ElementAt(0).MovementDestination.DestinationProductId);

            Assert.AreEqual(1, movements.ElementAt(0).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(31.20M, movements.ElementAt(0).Owners.ElementAt(0).OwnershipValue);

            Assert.AreEqual(33.20M, movements.ElementAt(1).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(1).ConsolidatedInventoryProductId);
            Assert.AreEqual(1, movements.ElementAt(1).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(1).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.ConsolidatedInventoryDelta, movements.ElementAt(1).OfficialDeltaMessageTypeId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(1).OperationalDate);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(1).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(1).Period.EndTime);
            Assert.AreEqual(187, movements.ElementAt(1).MovementTypeId);

            // when sign is false
            Assert.IsNull(movements.ElementAt(1).MovementDestination);
            Assert.AreEqual(1, movements.ElementAt(1).MovementSource.SourceNodeId);
            Assert.AreEqual("producttest1", movements.ElementAt(1).MovementSource.SourceProductId);

            Assert.AreEqual(3, movements.ElementAt(1).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(33.20M, movements.ElementAt(1).Owners.ElementAt(0).OwnershipValue);

            // for MovementTransactionId = 2
            Assert.AreEqual(34.20M, movements.ElementAt(2).NetStandardVolume);
            Assert.AreEqual(2, movements.ElementAt(2).ConsolidatedInventoryProductId);
            Assert.AreEqual(4, movements.ElementAt(2).MeasurementUnit);
            Assert.AreEqual(4, movements.ElementAt(2).SegmentId);
            Assert.AreEqual(OfficialDeltaMessageType.ConsolidatedInventoryDelta, movements.ElementAt(2).OfficialDeltaMessageTypeId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(2).OperationalDate);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(2).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(2).Period.EndTime);
            Assert.AreEqual(187, movements.ElementAt(2).MovementTypeId);

            // when sign is false
            Assert.IsNull(movements.ElementAt(2).MovementDestination);
            Assert.AreEqual(2, movements.ElementAt(2).MovementSource.SourceNodeId);
            Assert.AreEqual("producttest3", movements.ElementAt(2).MovementSource.SourceProductId);

            Assert.AreEqual(4, movements.ElementAt(2).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(34.20M, movements.ElementAt(2).Owners.ElementAt(0).OwnershipValue);
        }
    }
}
