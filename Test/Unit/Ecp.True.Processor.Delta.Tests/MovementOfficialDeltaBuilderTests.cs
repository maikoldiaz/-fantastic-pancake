// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOfficialDeltaBuilderTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Builders;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using static Ecp.True.Entities.Enumeration.MovementType;
    using static Ecp.True.Entities.Enumeration.OriginType;

    /// <summary>
    /// The BuildResultExecutorTests.
    /// </summary>
    [TestClass]
    public class MovementOfficialDeltaBuilderTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<MovementOfficialDeltaBuilder>> mockLogger;

        /// <summary>
        /// The result build executor.
        /// </summary>
        private IOfficialDeltaBuilder movementOfficialDeltaBuilder;

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
            this.mockLogger = new Mock<ITrueLogger<MovementOfficialDeltaBuilder>>();
            this.movementOfficialDeltaBuilder = new MovementOfficialDeltaBuilder(this.mockLogger.Object);
            var officialErrorMovement = new List<OfficialErrorMovement>
            {
                new OfficialErrorMovement { Origin = OFICIAL, MovementId = "testId", Description = "test1", MovementTransactionId = 1 },
                new OfficialErrorMovement { Origin = OFICIAL, MovementId = "officialMovementId", Description = "test2", MovementTransactionId = 2 },
                new OfficialErrorMovement { Origin = CONSOLIDADO, MovementId = "consolidadoMovementId", Description = "test3", MovementTransactionId = 3 },
            };
            var consolidatedMovement = new List<ConsolidatedMovement>
            {
                new ConsolidatedMovement { ConsolidatedMovementId = 1, MovementTypeId = "test", SourceNodeId = 1, DestinationNodeId = 1, SourceProductId = "testSourceProductId", DestinationProductId = "testDestinationProductId" },
            };
            var deltaNodes = new List<DeltaNode>
            {
                new DeltaNode { DeltaNodeId = 1, NodeId = 1 },
                new DeltaNode { DeltaNodeId = 2, NodeId = 2 },
            };
            var officialResultMovement = new List<OfficialResultMovement>
            {
                new OfficialResultMovement { Origin = OFICIAL, MovementTransactionId = 1, NetStandardVolume = 34.66M, Sign = true, MovementId = "test1", OwnerId = "1", OfficialDelta = 31.20M },
                new OfficialResultMovement { Origin = CONSOLIDADO, MovementTransactionId = 1, NetStandardVolume = 31.66M, Sign = true, MovementId = "test2", OwnerId = "2", OfficialDelta = 33.20M },
                new OfficialResultMovement { Origin = OFICIAL, MovementTransactionId = 1, NetStandardVolume = 32.66M, Sign = false, MovementId = "test3", OwnerId = "3", OfficialDelta = 34.20M },
                new OfficialResultMovement { Origin = OFICIAL, MovementTransactionId = 2, NetStandardVolume = 33.66M, Sign = false, MovementId = "test4", OwnerId = "4", OfficialDelta = 35.20M },
            };
            var pendingOfficialMovement = new List<PendingOfficialMovement>
            {
                new PendingOfficialMovement { MovementTransactionId = 1, MovementId = "1", SourceNodeId = 1, SourceProductId = "1", SourceProductTypeId = 1, DestinationNodeId = 1, DestinationProductId = "1", DestinationProductTypeId = 1, OwnerId = 1, MovementTypeID = 1, SegmentId = 1, MeasurementUnit = 1, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 04), OperationalDate = new DateTime(2020, 07, 07) },
                new PendingOfficialMovement { MovementTransactionId = 3, MovementId = "1", SourceProductId = "1", SourceProductTypeId = 1, DestinationNodeId = 1, DestinationProductId = "1", DestinationProductTypeId = 1, OwnerId = 1, MovementTypeID = 1, SegmentId = 1, MeasurementUnit = 1, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 04), OperationalDate = new DateTime(2020, 07, 07) },
                new PendingOfficialMovement { MovementTransactionId = 4, MovementId = "1", SourceNodeId = 1, SourceProductId = "1", SourceProductTypeId = 1, DestinationProductId = "1", DestinationProductTypeId = 1, OwnerId = 1, MovementTypeID = 1, SegmentId = 1, MeasurementUnit = 1, StartDate = new DateTime(2020, 07, 01), EndDate = new DateTime(2020, 07, 04), OperationalDate = new DateTime(2020, 07, 07) },
                new PendingOfficialMovement { MovementTransactionId = 2, MovementId = "1", SourceNodeId = 2, SourceProductTypeId = 2, DestinationNodeId = 2, DestinationProductTypeId = 2, OwnerId = 2, MovementTypeID = 1, SegmentId = 2, MeasurementUnit = 2, StartDate = new DateTime(2020, 07, 02), EndDate = new DateTime(2020, 07, 05), OperationalDate = new DateTime(2020, 07, 08) },
            };

            var cancellationTypes = new List<Annulation>
            {
                new Annulation { AnnulationMovementTypeId = 1, SourceMovementTypeId = 1 },
                new Annulation { AnnulationMovementTypeId = 2, SourceMovementTypeId = 2 },
            };

            this.deltaData = new OfficialDeltaData() { Ticket = new Ticket { TicketId = 123 }, MovementErrors = officialErrorMovement, ConsolidationMovements = consolidatedMovement, DeltaNodes = deltaNodes, OfficialResultMovements = officialResultMovement, PendingOfficialMovements = pendingOfficialMovement, CancellationTypes = cancellationTypes };
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildErrors_ShouldReturnMovementErrors_WhenOfficialDeltaDataFormedAsync()
        {
            var deltaErrors = await this.movementOfficialDeltaBuilder.BuildErrorsAsync(this.deltaData).ConfigureAwait(false);

            Assert.IsNotNull(deltaErrors);
            Assert.AreEqual(2, deltaErrors.Count());

            Assert.AreEqual(1, deltaErrors.ElementAt(0).DeltaNodeId);
            Assert.AreEqual(1, deltaErrors.ElementAt(0).MovementTransactionId);
            Assert.AreEqual("test1", deltaErrors.ElementAt(0).ErrorMessage);

            Assert.AreEqual(2, deltaErrors.ElementAt(1).DeltaNodeId);
            Assert.AreEqual(2, deltaErrors.ElementAt(1).MovementTransactionId);
            Assert.AreEqual("test2", deltaErrors.ElementAt(1).ErrorMessage);
        }

        [TestMethod]
        public async Task BuildMovements_shouldReturnMovements_WhenOfficialDeltaDataFormedAsync()
        {
            var movements = await this.movementOfficialDeltaBuilder.BuildMovementsAsync(this.deltaData).ConfigureAwait(false);
            Assert.IsNotNull(movements);
            Assert.AreEqual(3, movements.Count());

            // for MovementTransactionId = 1
            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(0).ScenarioId);
            Assert.AreEqual(31.20M, movements.ElementAt(0).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(0).SourceMovementTransactionId);
            Assert.AreEqual(1, movements.ElementAt(0).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(0).SegmentId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(0).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 04), movements.ElementAt(0).Period.EndTime);
            Assert.AreEqual(new DateTime(2020, 07, 07), movements.ElementAt(0).OperationalDate);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialMovementDelta, movements.ElementAt(0).OfficialDeltaMessageTypeId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementTypeId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementSource.SourceNodeId);
            Assert.AreEqual("1", movements.ElementAt(0).MovementSource.SourceProductId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementSource.SourceProductTypeId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementDestination.DestinationNodeId);
            Assert.AreEqual("1", movements.ElementAt(0).MovementDestination.DestinationProductId);
            Assert.AreEqual(1, movements.ElementAt(0).MovementDestination.DestinationProductTypeId);

            // when sign is true
            Assert.AreEqual(1, movements.ElementAt(0).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(31.20M, movements.ElementAt(0).Owners.ElementAt(0).OwnershipValue);

            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(1).ScenarioId);
            Assert.AreEqual(-34.20M, movements.ElementAt(1).NetStandardVolume);
            Assert.AreEqual(1, movements.ElementAt(1).SourceMovementTransactionId);
            Assert.AreEqual(1, movements.ElementAt(1).MeasurementUnit);
            Assert.AreEqual(1, movements.ElementAt(1).SegmentId);
            Assert.AreEqual(new DateTime(2020, 07, 01), movements.ElementAt(1).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 04), movements.ElementAt(1).Period.EndTime);
            Assert.AreEqual(new DateTime(2020, 07, 07), movements.ElementAt(1).OperationalDate);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialMovementDelta, movements.ElementAt(1).OfficialDeltaMessageTypeId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementTypeId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementSource.SourceNodeId);
            Assert.AreEqual("1", movements.ElementAt(1).MovementSource.SourceProductId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementSource.SourceProductTypeId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementDestination.DestinationNodeId);
            Assert.AreEqual("1", movements.ElementAt(1).MovementDestination.DestinationProductId);
            Assert.AreEqual(1, movements.ElementAt(1).MovementDestination.DestinationProductTypeId);

            // when sign is false
            Assert.AreEqual(3, movements.ElementAt(1).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(-34.20M, movements.ElementAt(1).Owners.ElementAt(0).OwnershipValue);

            // for MovementTransactionId = 2
            Assert.AreEqual(ScenarioType.OFFICER, movements.ElementAt(2).ScenarioId);
            Assert.AreEqual(-35.20M, movements.ElementAt(2).NetStandardVolume);
            Assert.AreEqual(2, movements.ElementAt(2).SourceMovementTransactionId);
            Assert.AreEqual(2, movements.ElementAt(2).MeasurementUnit);
            Assert.AreEqual(2, movements.ElementAt(2).SegmentId);
            Assert.AreEqual(new DateTime(2020, 07, 02), movements.ElementAt(2).Period.StartTime);
            Assert.AreEqual(new DateTime(2020, 07, 05), movements.ElementAt(2).Period.EndTime);
            Assert.AreEqual(new DateTime(2020, 07, 08), movements.ElementAt(2).OperationalDate);
            Assert.AreEqual(OfficialDeltaMessageType.OfficialMovementDelta, movements.ElementAt(2).OfficialDeltaMessageTypeId);
            Assert.AreEqual(1, movements.ElementAt(2).MovementTypeId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementSource.SourceNodeId);
            Assert.IsNull(movements.ElementAt(2).MovementSource.SourceProductId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementSource.SourceProductTypeId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementDestination.DestinationNodeId);
            Assert.IsNull(movements.ElementAt(2).MovementDestination.DestinationProductId);
            Assert.AreEqual(2, movements.ElementAt(2).MovementDestination.DestinationProductTypeId);

            // when sign is false
            Assert.AreEqual(4, movements.ElementAt(2).Owners.ElementAt(0).OwnerId);
            Assert.AreEqual(-35.20M, movements.ElementAt(2).Owners.ElementAt(0).OwnershipValue);
        }

        /// <summary>
        /// BuildMovements_ShouldReturnUnidentifiedLoss_WhenOriginalOfficialMovementIsFound.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task BuildMovements_ShouldReturnUnidentifiedLoss_WhenOriginalOfficialMovementIsFoundAsync()
        {
            // Arrange
            var officialMovementTransactionId = 111;
            var consolidatedMovementId = 222;
            var officialDeltaData = new OfficialDeltaData
            {
                OfficialResultMovements = new List<OfficialResultMovement>
                {
                    GetOfficialResultMovement(officialMovementTransactionId, 80.0M, "29"),
                    GetOfficialResultMovement(officialMovementTransactionId, 80.0M, "30"),
                    GetOfficialResultMovement(consolidatedMovementId, 20.0M, "29"),
                    GetOfficialResultMovement(consolidatedMovementId, 20.0M, "30"),
                },
                PendingOfficialMovements = new List<PendingOfficialMovement>
                {
                    GetPendingOfficialMovement(officialMovementTransactionId),
                    GetPendingOfficialMovement(officialMovementTransactionId),
                },
                Ticket = new Ticket { TicketId = 1 },
                CancellationTypes = GetCancellationTypes(),
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    GetConsolidatedMovement(consolidatedMovementId),
                },
            };

            // Act
            var movements = (await this.movementOfficialDeltaBuilder.BuildMovementsAsync(officialDeltaData).ConfigureAwait(false)).ToList();

            // Assert
            Assert.AreEqual(this.deltaData.OfficialResultMovements.Count(), movements.Count);
            var unidentifiedLossCancellations = GetMovementsByType(movements, InputCancellation);
            var toleranceCancellations = GetMovementsByType(movements, OutputCancellation);
            Assert.AreEqual(2, unidentifiedLossCancellations.Count);
            Assert.AreEqual(2, toleranceCancellations.Count);
            toleranceCancellations.ForEach(u => u.Owners.ToList().ForEach(o => Assert.AreEqual(-20.0M, o.OwnershipValue)));
            unidentifiedLossCancellations.ForEach(u => u.Owners.ToList().ForEach(o => Assert.AreEqual(-80.0M, o.OwnershipValue)));
        }

        private static List<Movement> GetMovementsByType(IEnumerable<Movement> movements, MovementType type)
        {
            return movements.Where(m => m.MovementTypeId == (int)type).ToList();
        }

        private static ConsolidatedMovement GetConsolidatedMovement(int consolidatedMovementId)
        {
            return new ConsolidatedMovement
            {
                ConsolidatedMovementId = consolidatedMovementId,
                SourceNodeId = GetPendingOfficialMovement(0).SourceNodeId,
                DestinationNodeId = GetPendingOfficialMovement(0).DestinationNodeId,
                SourceProductId = GetPendingOfficialMovement(0).SourceProductId,
                DestinationProductId = GetPendingOfficialMovement(0).DestinationProductId,
            };
        }

        private static List<Annulation> GetCancellationTypes()
        {
            return new List<Annulation>
            {
                GetAnnulation(UnidentifiedLoss, InputCancellation),
                GetAnnulation(Tolerance, OutputCancellation),
            };
        }

        private static Annulation GetAnnulation(MovementType unidentifiedLoss, MovementType inputCancellation)
        {
            return new Annulation
            {
                SourceMovementTypeId = (int)unidentifiedLoss,
                AnnulationMovementTypeId = (int)inputCancellation,
            };
        }

        private static PendingOfficialMovement GetPendingOfficialMovement(int movementTransactionId)
        {
            return new PendingOfficialMovement
            {
                MovementTransactionId = movementTransactionId,
                MeasurementUnit = 33,
                SegmentId = 1,
                StartDate = new DateTime(2021,8,1),
                EndDate = new DateTime(2021,8,31),
                OperationalDate = new DateTime(2021,8,15),
                MovementTypeID = (int)UnidentifiedLoss,
                SourceNodeId = 1,
            };
        }

        private static OfficialResultMovement GetOfficialResultMovement(int movementTransactionId, decimal officialDelta, string ownerId)
        {
            return new OfficialResultMovement
            {
                Origin = OFICIAL,
                MovementTransactionId = movementTransactionId,
                Sign = false,
                NetStandardVolume = 100.0M,
                OwnerId = ownerId,
                OfficialDelta = officialDelta,
            };
        }
    }
}
