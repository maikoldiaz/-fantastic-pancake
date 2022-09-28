// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementAggregationServiceTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Services;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static System.Globalization.CultureInfo;
    using static Ecp.True.Entities.Enumeration.MovementType;

    /// <summary>
    /// The movementAggregationStrategyTests test class.
    /// </summary>
    [TestClass]
    public class MovementAggregationServiceTests
    {
        private const decimal TestOwnershipVolume = 10.0M;
        private const decimal TestToleranceOwnerShipValue = 5.0M;
        private MovementAggregationService service;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.service = new MovementAggregationService();
        }

        /// <summary>
        /// ShouldThrowException_WhenTheEnumerableIsNull.
        /// </summary>
        [TestMethod]
        public void ShouldThrowException_WhenTheEnumerableIsNull()
        {
            // Arrange
            // Act
            IEnumerable<OfficialDeltaConsolidatedMovement> Act() => this.service.AggregateTolerancesAndUnidentifiedLosses(null, new OfficialDeltaData());

            // Assert
            var exception = Assert.ThrowsException<ArgumentNullException>(Act);
            Assert.AreEqual("Value cannot be null. (Parameter 'consolidatedMovements')", exception.Message);
        }

        /// <summary>
        /// ShouldThrowException_WhenTheEnumerableIsNull.
        /// </summary>
        [TestMethod]
        public void ShouldThrowException_WhenTheDeltaDataIsNull()
        {
            // Arrange
            // Act
            IEnumerable<OfficialDeltaConsolidatedMovement> Act() => this.service.AggregateTolerancesAndUnidentifiedLosses(new List<OfficialDeltaConsolidatedMovement>() { new OfficialDeltaConsolidatedMovement() }, null);

            // Assert
            var exception = Assert.ThrowsException<ArgumentNullException>(Act);
            Assert.AreEqual("Value cannot be null. (Parameter 'officialDeltaData')", exception.Message);
        }

        /// <summary>
        /// ShouldNotAggregateMovements_IfThoseMovementHaveATolerance.
        /// </summary>
        [TestMethod]
        public void ShouldNotAggregateMovements_IfThoseMovementHaveATolerance()
        {
            // Arrange
            var consolidatedMovements = GetUnidentifiedLossesAndTolerancesMovements();
            var firstMovement = consolidatedMovements.First();
            var officialDeltaTolerance = GetPendingOfficialMovement(firstMovement, TestToleranceOwnerShipValue);

            // Act
            var deltaData = new OfficialDeltaData
            {
                PendingOfficialMovements = new List<PendingOfficialMovement>() { officialDeltaTolerance },
            };
            var aggregatedMovements = this.service.AggregateTolerancesAndUnidentifiedLosses(consolidatedMovements, deltaData);

            // Assert
            Assert.AreEqual(4, aggregatedMovements.Count());
            Assert.AreNotEqual(
                GetUnidentifiedLossesAndTolerances(consolidatedMovements)
                    .Sum(c => c.OwnershipVolume),
                GetFirstUnidentifiedLoss(aggregatedMovements).OwnershipVolume);
            Assert.AreEqual(TestOwnershipVolume, GetFirstUnidentifiedLoss(aggregatedMovements).OwnershipVolume);
        }

        /// <summary>
        /// ShouldNotChangeMovements_WhenNoTolerancesOrUnidentifiedLossArePresentInTheEnumerable.
        /// </summary>
        [TestMethod]
        public void ShouldNotChangeMovements_WhenNoTolerancesOrUnidentifiedLossArePresentInTheEnumerable()
        {
            // Arrange
            var consolidatedMovements = GetNonUnidentifiedLossesAndTolerancesMovements();
            var initialCount = consolidatedMovements.Count();

            // Act
            var aggregatedMovements = this.service.AggregateTolerancesAndUnidentifiedLosses(consolidatedMovements, new OfficialDeltaData());

            // Assert
            Assert.AreEqual(initialCount, aggregatedMovements.Count());
        }

        /// <summary>
        /// Should_WhenNoTolerancesOrUnidentifiedLossArePresentInTheEnumerable.
        /// </summary>
        [TestMethod]
        public void ShouldAggregate_TolerancesAndUnidentifiedLosses()
        {
            // Arrange
            var consolidatedMovements = GetUnidentifiedLossesAndTolerancesMovements();

            // Act
            var aggregatedMovements = this.service.AggregateTolerancesAndUnidentifiedLosses(consolidatedMovements, new OfficialDeltaData());

            // Assert
            Assert.AreEqual(3, aggregatedMovements.Count());
            Assert.AreEqual(20.0M, GetFirstUnidentifiedLoss(aggregatedMovements).OwnershipVolume);
        }

        private static PendingOfficialMovement GetPendingOfficialMovement(OfficialDeltaConsolidatedMovement firstMovement, decimal ownerShipValue)
        {
            var sourceNodeId = GetInt(firstMovement.SourceNodeId);
            var destinationNodeId = GetInt(firstMovement.DestinationNodeId);

            return new PendingOfficialMovement
            {
                SourceNodeId = sourceNodeId,
                DestinationNodeId = destinationNodeId,
                SourceProductId = firstMovement.SourceProductId,
                DestinationProductId = firstMovement.DestinationProductId,
                OwnerId = GetInt(firstMovement.OwnerId).GetValueOrDefault(),
                OwnerShipValue = ownerShipValue,
                MovementTypeID = (int)Tolerance,
            };
        }

        private static int? GetInt(string integer)
        {
            var sourceNodeId = Convert.ToInt32(integer ?? "0", InvariantCulture);

            if (sourceNodeId == 0)
            {
                return null;
            }

            return sourceNodeId;
        }

        private static OfficialDeltaConsolidatedMovement GetFirstUnidentifiedLoss(IEnumerable<OfficialDeltaConsolidatedMovement> aggregatedMovements)
        {
            return aggregatedMovements.First(c => MovementIsOfType(c, UnidentifiedLoss));
        }

        private static IEnumerable<OfficialDeltaConsolidatedMovement> GetUnidentifiedLossesAndTolerances(IEnumerable<OfficialDeltaConsolidatedMovement> consolidatedMovements)
        {
            return consolidatedMovements
                .Where(c =>
                    MovementIsOfType(c, UnidentifiedLoss) ||
                    MovementIsOfType(c, Tolerance));
        }

        private static bool MovementIsOfType(OfficialDeltaConsolidatedMovement m, MovementType movementType)
        {
            return m.MovementTypeId == ((int)movementType).ToString(InvariantCulture);
        }

        private static IEnumerable<OfficialDeltaConsolidatedMovement> GetNonUnidentifiedLossesAndTolerancesMovements()
        {
            yield return GetConsolidatedMovement(DeltaInventory, "1", "2", "3", "4", "5");
            yield return GetConsolidatedMovement(CancellationTransferConciliation, "1", "2", "3", "4", "5");
            yield return GetConsolidatedMovement(InputCancellation, "1", "2", "3", "4", "5");
            yield return GetConsolidatedMovement(Interface, "1", "2", "3", "4", "5");
        }

        private static IEnumerable<OfficialDeltaConsolidatedMovement> GetUnidentifiedLossesAndTolerancesMovements()
        {
            yield return GetConsolidatedMovement(UnidentifiedLoss, "1", "2", "3", "4", "5");
            yield return GetConsolidatedMovement(Tolerance, "1", "2", "3", "4", "5");
            yield return GetConsolidatedMovement(Tolerance, "1", "2", "3", "4", "6");
            yield return GetConsolidatedMovement(InputCancellation, "1", "2", "3", "4", "5");
        }

        private static OfficialDeltaConsolidatedMovement GetConsolidatedMovement(MovementType movementType, string sourceNodeId, string destinationNodeId, string sourceProductId, string destinationProductId, string ownerId)
        {
            return new OfficialDeltaConsolidatedMovement
            {
                MovementTypeId = ((int)movementType).ToString(InvariantCulture),
                SourceNodeId = sourceNodeId,
                DestinationNodeId = destinationNodeId,
                SourceProductId = sourceProductId,
                DestinationProductId = destinationProductId,
                OwnerId = ownerId,
                OwnershipVolume = TestOwnershipVolume,
            };
        }
    }
}