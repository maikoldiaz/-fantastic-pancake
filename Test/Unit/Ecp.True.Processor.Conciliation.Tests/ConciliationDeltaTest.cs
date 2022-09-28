// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationDeltaTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Conciliation.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Conciliation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The ConciliationDeltaTest class.
    /// </summary>
    [TestClass]
    public class ConciliationDeltaTest
    {
        /// <summary>
        /// The conciliation delta.
        /// </summary>
        private ConciliationDelta conciliationDelta;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.conciliationDelta = new ConciliationDelta();
        }

        /// <summary>
        /// The Calculate Delta Conciliation Result Movement.
        /// </summary>
        [TestMethod]
        public void CalculateDeltaConciliation_ResultMovement()
        {
            // give
            var segmentMovements = this.GetSegmentMovements();
            var oherSegmentMovements = this.GetOtherSegmentMovements();

            // when
            var result = this.conciliationDelta.CalculateDeltaConciliation(segmentMovements, oherSegmentMovements);

            // then
            Assert.AreEqual(5, result.ConciliatedMovements.ToList().Count);
            Assert.AreEqual(5, result.NoConciliatedMovements.ToList().Count);
            Assert.AreEqual(2, result.ErrorMovements.ToList().Count);
        }

        /// <summary>
        /// The Calculate Delta Conciliation for segmentMovements null.
        /// </summary>
        [TestMethod]
        public void CalculateDeltaConciliationsegmentMovementsNull_ResultMovement()
        {
            // give
            var segmentMovements = this.GetSegmentMovements();
            List<MovementConciliationDto> oherSegmentMovements = new List<MovementConciliationDto>();

            // when
            var result = this.conciliationDelta.CalculateDeltaConciliation(segmentMovements, oherSegmentMovements);

            // then
            Assert.AreEqual(0, result.ConciliatedMovements.ToList().Count);
            Assert.AreEqual(5, result.NoConciliatedMovements.ToList().Count);
            Assert.AreEqual(1, result.ErrorMovements.ToList().Count);
        }

        /// <summary>
        /// The Calculate Delta Conciliation for segmentMovements null.
        /// </summary>
        [TestMethod]
        public void CalculateDeltaConciliationoherSegmentMovementsNull_ResultMovement()
        {
            // give
            var segmentMovements = new List<MovementConciliationDto>();
            var oherSegmentMovements = this.GetOtherSegmentMovements();

            // when
            var result = this.conciliationDelta.CalculateDeltaConciliation(segmentMovements, oherSegmentMovements);

            // then
            Assert.AreEqual(0, result.ConciliatedMovements.ToList().Count);
            Assert.AreEqual(3, result.NoConciliatedMovements.ToList().Count);
            Assert.AreEqual(1, result.ErrorMovements.ToList().Count);
        }

        private IEnumerable<MovementConciliationDto> GetSegmentMovements()
        {
            return new List<MovementConciliationDto>
            {
                new MovementConciliationDto()
                {
                    SourceNodeId = 111,
                    DestinationNodeId = 111,
                    SourceProductId = "111",
                    DestinationProductId = "111",
                    OwnerId = 111,
                    MeasurementUnit = 111,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 111,
                    DestinationNodeId = 111,
                    SourceProductId = "111",
                    DestinationProductId = "111",
                    OwnerId = 111,
                    MeasurementUnit = 111,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 222,
                    DestinationNodeId = 222,
                    SourceProductId = "222",
                    DestinationProductId = "222",
                    OwnerId = 222,
                    MeasurementUnit = 222,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 222,
                    DestinationNodeId = 222,
                    SourceProductId = "222",
                    DestinationProductId = "222",
                    OwnerId = 222,
                    MeasurementUnit = 222,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 333,
                    DestinationNodeId = 333,
                    SourceProductId = "333",
                    DestinationProductId = "333",
                    OwnerId = 333,
                    MeasurementUnit = 333,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                },
                new MovementConciliationDto()
                {
                     SourceNodeId = 777,
                     DestinationNodeId = 777,
                     SourceProductId = "777",
                     DestinationProductId = "777",
                     OwnerId = 777,
                     MeasurementUnit = 777,
                     MovementTypeId = 2,
                     OwnershipVolume = 2,
                     SegmentId = 2,
                     OwnershipPercentage = 2,
                     NetStandardVolume = 100,
                },
            };
        }

        private IEnumerable<MovementConciliationDto> GetOtherSegmentMovements()
        {
            return new List<MovementConciliationDto>
            {
                new MovementConciliationDto()
                {
                    SourceNodeId = 555,
                    DestinationNodeId = 555,
                    SourceProductId = "555",
                    DestinationProductId = "555",
                    OwnerId = 555,
                    MeasurementUnit = 555,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 222,
                    DestinationNodeId = 222,
                    SourceProductId = "222",
                    DestinationProductId = "222",
                    OwnerId = 222,
                    MeasurementUnit = 222,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 666,
                    DestinationNodeId = 666,
                    SourceProductId = "666",
                    DestinationProductId = "666",
                    OwnerId = 666,
                    MeasurementUnit = 666,
                    MovementTypeId = 2,
                    OwnershipVolume = 2,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
                new MovementConciliationDto()
                {
                    SourceNodeId = 888,
                    DestinationNodeId = 888,
                    SourceProductId = "888",
                    DestinationProductId = "888",
                    OwnerId = 888,
                    MeasurementUnit = 888,
                    MovementTypeId = 2,
                    OwnershipVolume = 300,
                    SegmentId = 2,
                    OwnershipPercentage = 2,
                    NetStandardVolume = 100,
                },
            };
        }
    }
}
