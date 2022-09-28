// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementDeltaBuilderTests.cs" company="Microsoft">
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
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The BuildResultExecutorTests.
    /// </summary>
    [TestClass]
    public class ConsolidatedMovementDeltaBuilderTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ConsolidatedMovementDeltaBuilder>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IOfficialDeltaBuilder consolidatedMovementDeltaBuilder;

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
            this.mockLogger = new Mock<ITrueLogger<ConsolidatedMovementDeltaBuilder>>();
            this.consolidatedMovementDeltaBuilder = new ConsolidatedMovementDeltaBuilder(this.mockLogger.Object);
            var officialErrorMovement = new List<OfficialErrorMovement>
            {
                new OfficialErrorMovement { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, MovementId = "testId", Description = "test1", MovementTransactionId = 1 },
                new OfficialErrorMovement { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, MovementId = "consolidadoMovementId", Description = "test2", MovementTransactionId = 2 },
                new OfficialErrorMovement { Origin = True.Entities.Enumeration.OriginType.OFICIAL, MovementId = "officialMovementId", Description = "test3", MovementTransactionId = 3 },
            };
            var consolidatedMovement = new List<ConsolidatedMovement>
            {
                new ConsolidatedMovement { ConsolidatedMovementId = 1, MovementTypeId = "1", SourceNodeId = 1, DestinationNodeId = 1, SourceProductId = "testSourceProductId", DestinationProductId = "testDestinationProductId", SegmentId = 1, MeasurementUnit = "1", StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 05) },
                new ConsolidatedMovement { ConsolidatedMovementId = 3, MovementTypeId = "3", SourceProductId = "testSourceProductId3", DestinationProductId = "testDestinationProductId3", SegmentId = 2, MeasurementUnit = "1", StartDate = new DateTime(2020, 07, 02), EndDate = new DateTime(2020, 07, 06) },
                new ConsolidatedMovement { ConsolidatedMovementId = 1, MovementTypeId = "1", SourceNodeId = 2, DestinationNodeId = 2, SourceProductId = "testSourceProductId2", DestinationProductId = "testDestinationProductId2", SegmentId = 3, MeasurementUnit = "1", StartDate = new DateTime(2020, 07, 03), EndDate = new DateTime(2020, 07, 07) },
                new ConsolidatedMovement { ConsolidatedMovementId = 2, MovementTypeId = "2", SourceNodeId = 2, DestinationNodeId = 2, SourceProductId = "testSourceProductId4", DestinationProductId = "testDestinationProductId4", SegmentId = 4, MeasurementUnit = "1", StartDate = new DateTime(2020, 07, 04), EndDate = new DateTime(2020, 07, 08) },
            };
            var deltaNodes = new List<DeltaNode>
            {
                new DeltaNode { DeltaNodeId = 1, NodeId = 1 },
                new DeltaNode { DeltaNodeId = 2, NodeId = 2 },
            };
            var officialResultMovement = new List<OfficialResultMovement>
            {
                new OfficialResultMovement { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, MovementTransactionId = 1, NetStandardVolume = 34.66M, Sign = true, MovementId = "test", OwnerId = "1", OfficialDelta = 31.20M },
                new OfficialResultMovement { Origin = True.Entities.Enumeration.OriginType.OFICIAL, MovementTransactionId = 1, NetStandardVolume = 34.16M, Sign = true, MovementId = "test1", OwnerId = "2", OfficialDelta = 31.10M },
                new OfficialResultMovement { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, MovementTransactionId = 1, NetStandardVolume = 31.66M, Sign = false, MovementId = "test2", OwnerId = "3", OfficialDelta = 33.20M },
                new OfficialResultMovement { Origin = True.Entities.Enumeration.OriginType.CONSOLIDADO, MovementTransactionId = 2, NetStandardVolume = 30.66M, Sign = false, MovementId = "test3", OwnerId = "4", OfficialDelta = 31.30M },
            };
            var cancellationTypes = new List<Annulation>
            {
                new Annulation { AnnulationMovementTypeId = 1, SourceMovementTypeId = 1 },
                new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 2 },
            };

            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123 }, MovementErrors = officialErrorMovement, ConsolidationMovements = consolidatedMovement, DeltaNodes = deltaNodes, OfficialResultMovements = officialResultMovement, CancellationTypes = cancellationTypes };
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildErrors_ShouldReturnMovementErrors_WhenOfficialDeltaDataFormedAsync()
        {
            var deltaErrors = await this.consolidatedMovementDeltaBuilder.BuildErrorsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(deltaErrors);
            Assert.AreEqual(2, deltaErrors.Count());

            Assert.AreEqual(1, deltaErrors.ElementAt(0).ConsolidatedMovementId);
            Assert.AreEqual(1, deltaErrors.ElementAt(0).DeltaNodeId);
            Assert.AreEqual("test1", deltaErrors.ElementAt(0).ErrorMessage);

            Assert.AreEqual(2, deltaErrors.ElementAt(1).ConsolidatedMovementId);
            Assert.AreEqual(2, deltaErrors.ElementAt(1).DeltaNodeId);
            Assert.AreEqual("test2", deltaErrors.ElementAt(1).ErrorMessage);
        }

        /// <summary>
        /// Builds the movements should return movements when official delta data formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildMovements_shouldReturnMovements_WhenOfficialDeltaDataFormedAsync()
        {
            var movements = await this.consolidatedMovementDeltaBuilder.BuildMovementsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(movements);
            Assert.AreEqual(3, movements.Count());

            // for MovementTransactionId = 1
            Assert.AreEqual(31.20M, movements.ElementAt(0).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(0).ConsolidatedMovementTransactionId);
            Assert.AreEqual(1, movements.ElementAt(0).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(0).SegmentId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(0).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(0).Period.EndTime);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(0).OperationalDate);
            Assert.AreEqual(OfficialDeltaMessageType.ConsolidatedMovementDelta, movements.ElementAt(0).OfficialDeltaMessageTypeId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementTypeId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementSource.SourceNodeId);
            Assert.AreEqual("testSourceProductId", movements.ElementAt(0).MovementSource.SourceProductId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementDestination.DestinationNodeId);
            Assert.AreEqual("testDestinationProductId", movements.ElementAt(0).MovementDestination.DestinationProductId);

            // when sign is true
            Assert.AreEqual(1, movements.ElementAt(0).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(31.20M, movements.ElementAt(0).Owners.ElementAt(0).OwnershipValue);

            Assert.AreEqual(-33.20M, movements.ElementAt(1).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(1).ConsolidatedMovementTransactionId);
            Assert.AreEqual(1, movements.ElementAt(1).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(1).SegmentId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(1).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(1).Period.EndTime);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(1).OperationalDate);
            Assert.AreEqual(OfficialDeltaMessageType.ConsolidatedMovementDelta, movements.ElementAt(1).OfficialDeltaMessageTypeId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementTypeId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementSource.SourceNodeId);
            Assert.AreEqual("testSourceProductId", movements.ElementAt(1).MovementSource.SourceProductId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementDestination.DestinationNodeId);
            Assert.AreEqual("testDestinationProductId", movements.ElementAt(1).MovementDestination.DestinationProductId);

            // when sign is false
            Assert.AreEqual(3, movements.ElementAt(1).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(-33.20M, movements.ElementAt(1).Owners.ElementAt(0).OwnershipValue);

            // for MovementTransactionId = 2
            Assert.AreEqual(-31.30M, movements.ElementAt(2).NetStandardVolume);
            Assert.AreEqual(2, movements.ElementAt(2).ConsolidatedMovementTransactionId);
            Assert.AreEqual(1, movements.ElementAt(2).MeasurementUnit);
            Assert.AreEqual(4, movements.ElementAt(2).SegmentId);
            Assert.AreEqual(new DateTime(2020, 07, 04), movements.ElementAt(2).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 08), movements.ElementAt(2).Period.EndTime);
            Assert.AreEqual(new DateTime(2020, 07, 04), movements.ElementAt(2).OperationalDate);
            Assert.AreEqual(OfficialDeltaMessageType.ConsolidatedMovementDelta, movements.ElementAt(2).OfficialDeltaMessageTypeId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementTypeId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementSource.SourceNodeId);
            Assert.AreEqual("testSourceProductId4", movements.ElementAt(2).MovementSource.SourceProductId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementDestination.DestinationNodeId);
            Assert.AreEqual("testDestinationProductId4", movements.ElementAt(2).MovementDestination.DestinationProductId);

            // when sign is false
            Assert.AreEqual(4, movements.ElementAt(2).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(-31.30M, movements.ElementAt(2).Owners.ElementAt(0).OwnershipValue);
        }
    }
}
