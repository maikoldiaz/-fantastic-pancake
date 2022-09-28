// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaResponseConvertServiceTestTests.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Services;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static System.Globalization.CultureInfo;
    using static Ecp.True.Entities.Enumeration.MovementType;
    using OriginType = Ecp.True.Entities.Enumeration.OriginType;

    /// <summary>
    /// The officialDeltaResponseConvertServiceTestTests test class.
    /// </summary>
    [TestClass]
    public class OfficialDeltaResponseConvertServiceTestTests
    {
        private const int OfficialTransactionId1 = 11;
        private const int OfficialTransactionId2 = 22;
        private readonly OfficialDeltaResponseConvertService sut = new OfficialDeltaResponseConvertService();

        /// <summary>
        /// ShouldThrowArgumentNullException_IfResultsEnumerableIsNull.
        /// </summary>
        [TestMethod]
        public void ShouldThrowArgumentNullException_IfResultsEnumerableIsNull()
        {
            // Arrange
            // Act
            IEnumerable<OfficialResultMovement> Execution() =>
                this.sut.ConvertOfficialDeltaResponse(default, new OfficialDeltaData());

            // Assert
            var exeption = Assert.ThrowsException<ArgumentNullException>(Execution);
            Assert.AreEqual("Value cannot be null. (Parameter 'deltaResults')", exeption.Message);
        }

        /// <summary>
        /// ShouldThrowArgumentNullException_IfResultsEnumerableIsNull.
        /// </summary>
        [TestMethod]
        public void ShouldThrowArgumentNullException_IfDeltaDataEnumerableIsNull()
        {
            // Arrange
            // Act
            IEnumerable<OfficialResultMovement> Execution() => this.sut.ConvertOfficialDeltaResponse(new List<OfficialDeltaResultMovement>(), default);

            // Assert
            var exeption = Assert.ThrowsException<ArgumentNullException>(Execution);
            Assert.AreEqual("Value cannot be null. (Parameter 'deltaData')", exeption.Message);
        }

        /// <summary>
        /// ShouldReturnAsManyResultsAsDeltaResultsWhereGiven.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterUnidentifiedLossAndToleranceDelta_WhenTheDeltaValueIsGreaterThanTheUnidentifiedLossAndAnOfficialDeltaIsSent()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Negative, OfficialTransactionId1, 50.0M, "1"),
                GetDeltaResult(Constants.Positive, OfficialTransactionId2, 10.0M, "1"),
            };

            var consolidatedOwner = GetConsolidatedOwner(1, 100.0M, 1);
            var consolidatedOwner2 = GetConsolidatedOwner(1, 20.0M, 2);

            var consolidatedMovement = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 1, UnidentifiedLoss);

            var consolidatedMovement2 = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 2, Tolerance);

            var deltaData = new OfficialDeltaData
            {
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    consolidatedMovement, consolidatedMovement2,
                },
                PendingOfficialMovements = new List<PendingOfficialMovement>
                {
                    CreateOfficialMovement(50.0M, UnidentifiedLoss, OfficialTransactionId1),
                    CreateOfficialMovement(30.0M, UnidentifiedLoss, OfficialTransactionId2),
                },
            };

            consolidatedOwner.ConsolidatedMovement = consolidatedMovement;

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);
            var negativeUnidentifiedLoss = officialResults.ElementAt(0);
            var positiveTolerance = officialResults.ElementAt(1);

            // Assert
            Assert.AreEqual(2, officialResults.Count());

            Assert.AreEqual(50.0M, negativeUnidentifiedLoss.OfficialDelta);
            Assert.IsFalse(negativeUnidentifiedLoss.Sign);
            Assert.AreEqual(OfficialTransactionId1, negativeUnidentifiedLoss.MovementTransactionId);

            Assert.AreEqual(10.0M, positiveTolerance.OfficialDelta);
            Assert.IsTrue(positiveTolerance.Sign);
            Assert.AreEqual(OfficialTransactionId2, positiveTolerance.MovementTransactionId);
        }

        /// <summary>
        /// ShouldReturnAsManyResultsAsDeltaResultsWhereGiven.
        /// </summary>
        [TestMethod]
        public void ShouldNotRegisterTolerances_WhenDeltaIsLessThanOriginalMovement()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Negative, OfficialTransactionId1, 40.0M, "1"),
            };

            var consolidatedOwner = GetConsolidatedOwner(1, 100.0M, 1);
            var consolidatedOwner2 = GetConsolidatedOwner(1, 20.0M, 2);

            var consolidatedMovement = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 1, UnidentifiedLoss);

            var consolidatedMovement2 = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 2, Tolerance);

            var deltaData = new OfficialDeltaData
            {
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    consolidatedMovement, consolidatedMovement2,
                },
                PendingOfficialMovements = new List<PendingOfficialMovement>
                {
                    CreateOfficialMovement(80.0M, UnidentifiedLoss, OfficialTransactionId1),
                },
            };

            consolidatedOwner.ConsolidatedMovement = consolidatedMovement;

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);
            var negativeUnidentifiedLoss = officialResults.ElementAt(0);

            // Assert
            Assert.AreEqual(1, officialResults.Count());

            Assert.AreEqual(40.0M, negativeUnidentifiedLoss.OfficialDelta);
            Assert.IsFalse(negativeUnidentifiedLoss.Sign);

            Assert.AreEqual(OfficialTransactionId1, negativeUnidentifiedLoss.MovementTransactionId);
        }

        /// <summary>
        /// ShouldReturnAsManyResultsAsDeltaResultsWhereGiven.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterTolerances_WhenDeltaIsGreaterThanOriginalMovement()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Negative, OfficialTransactionId1, 110.0M, "1"),
            };

            var consolidatedOwner1 = GetConsolidatedOwner(1, 100.0M, 1);
            var consolidatedOwner2 = GetConsolidatedOwner(2, 100.0M, 1);
            var consolidatedOwner3 = GetConsolidatedOwner(1, 100.0M, 2);
            var consolidatedOwner4 = GetConsolidatedOwner(2, 20.0M, 2);

            var consolidatedMovement = CreateConsolidatedMovement(consolidatedOwner1, consolidatedOwner2, 1, UnidentifiedLoss);

            var consolidatedMovement2 = CreateConsolidatedMovement(consolidatedOwner3, consolidatedOwner4, 2, Tolerance);

            var deltaData = new OfficialDeltaData
            {
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    consolidatedMovement, consolidatedMovement2,
                },
                PendingOfficialMovements = new List<PendingOfficialMovement>
                {
                    CreateOfficialMovement(10.0M, UnidentifiedLoss, OfficialTransactionId1),
                },
            };

            consolidatedOwner1.ConsolidatedMovement = consolidatedMovement;
            consolidatedOwner2.ConsolidatedMovement = consolidatedMovement;
            consolidatedOwner3.ConsolidatedMovement = consolidatedMovement2;
            consolidatedOwner4.ConsolidatedMovement = consolidatedMovement2;

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);
            var negativeUnidentifiedLoss = officialResults.ElementAt(0);
            var negativeTolerance = officialResults.ElementAt(1);

            // Assert
            Assert.AreEqual(2, officialResults.Count());

            Assert.AreEqual(100.0M, negativeUnidentifiedLoss.OfficialDelta);
            Assert.IsFalse(negativeUnidentifiedLoss.Sign);
            Assert.AreEqual(OfficialTransactionId1, negativeUnidentifiedLoss.MovementTransactionId);

            Assert.AreEqual(10.0M, negativeTolerance.OfficialDelta);
            Assert.IsFalse(negativeTolerance.Sign);
            Assert.AreEqual(2, negativeTolerance.MovementTransactionId);
        }

        /// <summary>
        /// ShouldReturnAsManyResultsAsDeltaResultsWhereGiven.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterPositiveTolerancesAnUnidentifiedLosses_WhenOfficialIsGreaterThanConsolidated()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Positive, OfficialTransactionId1, 20.0M, "1"),
                GetDeltaResult(Constants.Positive, OfficialTransactionId2, 30.0M, "1"),
            };

            var consolidatedOwner = GetConsolidatedOwner(1, 100.0M, 1);
            var consolidatedOwner2 = GetConsolidatedOwner(1, 20.0M, 2);

            var consolidatedMovement = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 1, UnidentifiedLoss);

            var consolidatedMovement2 = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 2, Tolerance);

            var deltaData = new OfficialDeltaData
            {
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    consolidatedMovement, consolidatedMovement2,
                },
                PendingOfficialMovements = new List<PendingOfficialMovement>
                {
                    CreateOfficialMovement(20.0M, UnidentifiedLoss, OfficialTransactionId1),
                    CreateOfficialMovement(30.0M, Tolerance, OfficialTransactionId2),
                },
            };

            consolidatedOwner.ConsolidatedMovement = consolidatedMovement;

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);
            var negativeUnidentifiedLoss = officialResults.ElementAt(0);
            var negativeTolerance = officialResults.ElementAt(1);

            // Assert
            Assert.AreEqual(2, officialResults.Count());

            Assert.AreEqual(20.0M, negativeUnidentifiedLoss.OfficialDelta);
            Assert.IsTrue(negativeUnidentifiedLoss.Sign);
            Assert.AreEqual(OfficialTransactionId1, negativeUnidentifiedLoss.MovementTransactionId);

            Assert.AreEqual(30.0M, negativeTolerance.OfficialDelta);
            Assert.IsTrue(negativeTolerance.Sign);
            Assert.AreEqual(OfficialTransactionId2, negativeTolerance.MovementTransactionId);
        }

        /// <summary>
        /// ShouldReturnAsManyResultsAsDeltaResultsWhereGiven.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterUnidentifiedLossAndTolerance_WhenDeltaIsNegativeGreaterThanOriginalMovementAndNoOfficialMovementIsGiven()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Negative, 1, 120.0M, "1"),
            };

            var consolidatedOwner = GetConsolidatedOwner(1, 100.0M, 1);
            var consolidatedOwner2 = GetConsolidatedOwner(2, 20.0M, 1);
            var consolidatedOwner3 = GetConsolidatedOwner(1, 100.0M, 2);
            var consolidatedOwner4 = GetConsolidatedOwner(2, 20.0M, 2);

            var consolidatedMovement = CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 1, UnidentifiedLoss);

            var consolidatedMovement2 = CreateConsolidatedMovement(consolidatedOwner3, consolidatedOwner4, 2, Tolerance);

            var deltaData = new OfficialDeltaData
            {
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    consolidatedMovement, consolidatedMovement2,
                },
            };

            consolidatedOwner.ConsolidatedMovement = consolidatedMovement;

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);
            var negativeUnidentifiedLoss = officialResults.ElementAt(0);
            var negativeTolerance = officialResults.ElementAt(1);

            // Assert
            Assert.AreEqual(2, officialResults.Count());

            Assert.AreEqual(100.0M, negativeUnidentifiedLoss.OfficialDelta);
            Assert.IsFalse(negativeUnidentifiedLoss.Sign);
            Assert.AreEqual(1, negativeUnidentifiedLoss.MovementTransactionId);

            Assert.AreEqual(20.0M, negativeTolerance.OfficialDelta);
            Assert.IsFalse(negativeTolerance.Sign);
            Assert.AreEqual(2, negativeTolerance.MovementTransactionId);
        }

        /// <summary>
        /// ShouldRegisterMovement_IfTheUnidentifiedLossIsPositive.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterMovement_IfTheUnidentifiedLossIsPositive()
        {
            bool ContainsDeltaMovement(OfficialResultMovement o, List<OfficialDeltaResultMovement> officialDeltaResultMovements)
            {
                var firstOfficialResult = officialDeltaResultMovements.First();
                return o.MovementTransactionId.ToString(InvariantCulture) == firstOfficialResult.MovementTransactionId &&
                       o.Sign &&
                       o.NetStandardVolume == 30.0M &&
                       o.OwnerId == firstOfficialResult.MovementOwnerId;
            }

            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Positive, 1, 30.0M, "1"),
            };

            var deltaData = new OfficialDeltaData();

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);

            // Assert
            Assert.IsTrue(officialResults.Any(o => ContainsDeltaMovement(o, deltaResults)));
        }

        /// <summary>
        /// ShouldNotRegisterDeltaWithZeroVolume.
        /// </summary>
        [TestMethod]
        public void ShouldNotRegisterDeltaWithZeroVolume()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Positive, 1, 0.00M, "1"),
            };
            var deltaData = new OfficialDeltaData();

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);

            // Assert
            Assert.AreEqual(0, officialResults.Count());
        }

        /// <summary>
        /// ShouldCancelTheUnidentifiedLoss_WhenTheUnidentifiedLossValueIsBiggerThanTheUnidentifiedLoss.
        /// </summary>
        [TestMethod]
        public void ShouldRegisterUnidentifiedLossDelta_WhenTheDeltaValueIsLessThanTheUnidentifiedLoss()
        {
            // Arrange
            var deltaResults = new List<OfficialDeltaResultMovement>
            {
                GetDeltaResult(Constants.Negative, 1, 5.0M, "1"),
            };

            var consolidatedOwner = GetConsolidatedOwner(1, 10.0M, 1);
            var consolidatedOwner2 = GetConsolidatedOwner(2, 10.0M, 1);
            var deltaData = new OfficialDeltaData
            {
                ConsolidationMovements = new List<ConsolidatedMovement>
                {
                    CreateConsolidatedMovement(consolidatedOwner, consolidatedOwner2, 1, UnidentifiedLoss),
                },
            };

            // Act
            var officialResults = this.sut.ConvertOfficialDeltaResponse(deltaResults, deltaData);
            var firstResult = officialResults.First();

            // Assert
            Assert.AreEqual(1, officialResults.Count());
            Assert.AreEqual(5.0M, firstResult.NetStandardVolume);
        }

        private static PendingOfficialMovement CreateOfficialMovement(decimal ownerShipValue, MovementType type, int transactionId)
        {
            return new PendingOfficialMovement
            {
                MovementTransactionId = transactionId,
                MovementTypeID = (int)type,
                OwnerId = 1,
                OwnerShipValue = ownerShipValue,
            };
        }

        private static ConsolidatedMovement CreateConsolidatedMovement(ConsolidatedOwner consolidatedOwner, ConsolidatedOwner consolidatedOwner2, int consolidatedMovementId, MovementType type)
        {
            return new ConsolidatedMovement
            {
                ConsolidatedMovementId = consolidatedMovementId,
                ConsolidatedOwners =
                {
                    consolidatedOwner,
                    consolidatedOwner2,
                },
                MovementTypeId = ((int)type).ToString(InvariantCulture),
            };
        }

        private static ConsolidatedOwner GetConsolidatedOwner(int ownerId, decimal ownershipVolume, int consolidatedMovementId)
        {
            return new ConsolidatedOwner()
            {
                ConsolidatedMovementId = consolidatedMovementId,
                OwnerId = ownerId,
                OwnershipVolume = ownershipVolume,
            };
        }

        private static OfficialDeltaResultMovement GetDeltaResult(string sign, int transactionId, decimal deltaOfficial, string ownerId)
        {
            return new OfficialDeltaResultMovement
            {
                Sign = sign,
                MovementTransactionId = transactionId.ToString(InvariantCulture),
                DeltaOfficial = deltaOfficial,
                NetStandardVolume = deltaOfficial,
                Origin = OriginType.DELTAOFICIAL,
                MovementOwnerId = ownerId,
            };
        }
    }
}