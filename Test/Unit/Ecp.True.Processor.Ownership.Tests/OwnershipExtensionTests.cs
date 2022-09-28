// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipExtensionTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The ValidationExecutorTests.
    /// </summary>
    [TestClass]
    public class OwnershipExtensionTests
    {
        [TestMethod]
        public void ToAdjustOwnershipVolumeForDecimalRoundOff()
        {
            var owner1 = new PreviousMovementOperationalData { MovementId = 1, NetStandardVolume = Convert.ToDecimal(10.57), OwnershipPercentage = 50, OwnershipVolume = Convert.ToDecimal(5.29) };
            var owner2 = new PreviousMovementOperationalData { MovementId = 1, NetStandardVolume = Convert.ToDecimal(10.57), OwnershipPercentage = 50, OwnershipVolume = Convert.ToDecimal(5.29) };

            var owner1Inv = new PreviousInventoryOperationalData { InventoryId = 1, NetStandardVolume = Convert.ToDecimal(10.57), OwnershipPercentage = Convert.ToDecimal(60.7), OwnershipVolume = Convert.ToDecimal(6.42) };
            var owner2Inv = new PreviousInventoryOperationalData { InventoryId = 1, NetStandardVolume = Convert.ToDecimal(10.57), OwnershipPercentage = Convert.ToDecimal(39.6), OwnershipVolume = Convert.ToDecimal(4.15) };

            var request = new OwnershipRuleRequest { PreviousMovementsOperationalData = new List<PreviousMovementOperationalData> { owner1, owner2 }, PreviousInventoryOperationalData = new List<PreviousInventoryOperationalData> { owner1Inv, owner2Inv } };

            request.AdjustOwnershipVolumeForDecimalRoundOff();

            Assert.AreEqual(request.PreviousMovementsOperationalData.Last().OwnershipVolume, Convert.ToDecimal(5.28));
            Assert.AreEqual(request.PreviousMovementsOperationalData.First().OwnershipVolume + request.PreviousMovementsOperationalData.Last().OwnershipVolume, Convert.ToDecimal(10.57));
        }

        /// <summary>
        /// Should Return List of Movement for collaboration and agreements.
        /// </summary>
        [TestMethod]
        public void ToMovementsForCollaborationAgreements_ShouldReturnListOfMovement()
        {
            // Arrange
            var movement = new Movement
            {
                MovementTransactionId = 1,
                MessageTypeId = 1,
                SystemTypeId = 3,
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 20,
                    DestinationProductId = "30",
                    DestinationProductTypeId = 40,
                },
                MeasurementUnit = 31,
                SegmentId = 1042,
                OperationalDate = DateTime.UtcNow,
                GrossStandardVolume = 4000,
                NetStandardVolume = 9000,
            };

            var newMovement = new NewMovement
            {
                AgreementType = Constants.Collaboration,
                EventId = 1,
                MovementId = 120,
                CreditorOwnerId = 29,
                DebtorOwnerId = 30,
                OwnershipVolume = 2000,
                AppliedRule = "6",
                RuleVersion = "1",
                NodeId = 20,
                ProductId = "30",
            };
            var movementEvent = new MovementEvent
            {
                MovementEventId = 1,
                EventTypeId = 45,
                SourceNodeId = 878,
                DestinationNodeId = 879,
                SourceProductId = "10000002318",
                DestinationProductId = "10000002372",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                Owner1Id = 29,
                Owner2Id = 30,
                Volume = 9000,
                MeasurementUnit = "31",
                IsDeleted = false,
            };

            var ticket = new Ticket
            {
                TicketId = 12375,
            };

            // Act
            var result = movement.ToMovementsForCollaborationAgreements(newMovement, ticket, movementEvent, movementEvent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToList().Count);
            Assert.AreEqual(newMovement.NodeId, result.ToList()[0].MovementDestination.DestinationNodeId);
            Assert.AreEqual(newMovement.ProductId, result.ToList()[0].MovementDestination.DestinationProductId);
            Assert.AreEqual(newMovement.NodeId, result.ToList()[1].MovementSource.SourceNodeId);
            Assert.AreEqual(newMovement.ProductId, result.ToList()[1].MovementSource.SourceProductId);
        }

        /// <summary>
        /// Should Return List of Movement for loans and payments.
        /// </summary>
        [TestMethod]
        public void ToMovementsForLoansPayments_ShouldReturnListOfMovement()
        {
            // Arrange
            var newMovement = new NewMovement
            {
                AgreementType = Constants.Collaboration,
                EventId = 1,
                MovementId = 120,
                CreditorOwnerId = 29,
                DebtorOwnerId = 30,
                OwnershipVolume = 2000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            var ticket = new Ticket
            {
                TicketId = 12375,
            };

            // Act
            var result = newMovement.ToMovementsForLoansPayments(ticket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToList().Count);
        }

        /// <summary>
        /// Should Return Movement Event for collaboration and agreements.
        /// </summary>
        [TestMethod]
        public void ToEventForCollaborationAgreements_ShouldReturnMovementEvent()
        {
            // Arrange
            var event1 = new Entities.Registration.Event
            {
                EventId = 1,
                EventTypeId = 45,
                SourceNodeId = 878,
                DestinationNodeId = 879,
                SourceProductId = "10000002318",
                DestinationProductId = "10000002372",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                Owner1Id = 29,
                Owner2Id = 30,
                Volume = 9000,
                MeasurementUnit = "31",
                IsDeleted = false,
            };
            var newMovement = new NewMovement
            {
                AgreementType = Constants.Collaboration,
                EventId = 1,
                MovementId = 120,
                CreditorOwnerId = 29,
                DebtorOwnerId = 30,
                OwnershipVolume = 2000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            // Act
            var result = event1.ToEventForCollaborationAgreements(newMovement, 12375);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(event1.EventTypeId, result.EventTypeId);
            Assert.AreEqual(event1.DestinationNodeId, result.DestinationNodeId);
            Assert.AreEqual(event1.DestinationProductId, result.DestinationProductId);
            Assert.AreEqual(event1.Owner1Id, result.Owner1Id);
            Assert.AreEqual(event1.Owner2Id, result.Owner2Id);
        }

        /// <summary>
        /// Should return cancellation movement with ownership for cancellation movement details.
        /// </summary>
        [TestMethod]
        public void ToCancellationMovementWithOwnership_ShouldReturnCancellationMovement()
        {
            // Arrange
            var cancellationMovementDetail = new CancellationMovementDetail
            {
                SourceNodeId = 878,
                SourceProductId = "10000002318",
                DestinationNodeId = null,
                DestinationProductId = null,
                ProductType = 1111,
                NetVolume = 10000,
                Unit = 1,
                MessageTypeId = 10,
                MovementTypeId = 155,
                OwnershipPercentage = 100,
                OwnershipVolume = 10000,
                OwnerId = 12345,
                RuleVersion = "1",
            };

            var ticket = new Ticket
            {
                TicketId = 123456,
                CategoryElementId = 100,
                StartDate = DateTime.UtcNow.ToTrue(),
            };

            // Act
            var result = cancellationMovementDetail.ToCancellationMovementWithOwnership(ticket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cancellationMovementDetail.OwnerId, result.Ownerships.First().OwnerId);
            Assert.AreEqual(ticket.TicketId, result.Ownerships.First().TicketId);
            Assert.AreEqual(Constants.Evacuation, result.Ownerships.First().AppliedRule);

            Assert.AreEqual(cancellationMovementDetail.SourceNodeId, result.MovementSource.SourceNodeId);
            Assert.IsNull(result.MovementDestination);

            Assert.AreEqual(EventType.Insert.ToString(), result.EventType);
            Assert.AreEqual(ticket.CategoryElementId, result.SegmentId);
            Assert.AreEqual(ticket.TicketId, result.OwnershipTicketId);
            Assert.AreEqual(ticket.StartDate, result.OperationalDate);
            Assert.AreEqual(cancellationMovementDetail.NetVolume, result.NetStandardVolume);
            Assert.AreEqual(cancellationMovementDetail.MovementTypeId, result.MovementTypeId);
        }

        public void ToInputCancellationMovement_ShouldReturnInputCancellationMovement()
        {
            var cancellationMovement = new CancellationMovementDetail
            {
                DestinationNodeId = 12345,
                DestinationProductId = "11111",
            };

            cancellationMovement.ToInputCancellation();

            Assert.AreEqual(12345, cancellationMovement.SourceNodeId);
            Assert.AreEqual("11111", cancellationMovement.SourceProductId);
            Assert.AreEqual(MovementType.InputCancellation, cancellationMovement.MovementTypeId);
            Assert.AreEqual(MovementType.InputCancellation.ToString(), cancellationMovement.MovementType);
        }

        public void ToOutputCancellationMovement_ShouldReturnOutputCancellationMovement()
        {
            var cancellationMovement = new CancellationMovementDetail
            {
                DestinationNodeId = 12345,
                SourceProductId = "11111",
            };

            cancellationMovement.ToInputCancellation();

            Assert.AreEqual(12345, cancellationMovement.DestinationNodeId);
            Assert.AreEqual("11111", cancellationMovement.DestinationProductId);
            Assert.AreEqual(MovementType.OutputCancellation, cancellationMovement.MovementTypeId);
            Assert.AreEqual(MovementType.OutputCancellation.ToString(), cancellationMovement.MovementType);
        }

        /// <summary>
        /// Should return ownership nodes.
        /// </summary>
        [TestMethod]
        public void GetOwnershipNodes_ShouldReturnOwnershipNodes()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                OwnershipRuleRequest = new OwnershipRuleRequest
                {
                    InventoryOperationalData = new List<InventoryOperationalData>
                    {
                        new InventoryOperationalData
                        {
                            NodeId = 1,
                        },
                    },
                    PreviousInventoryOperationalData = new List<PreviousInventoryOperationalData>
                    {
                        new PreviousInventoryOperationalData
                        {
                            NodeId = 2,
                        },
                    },
                    MovementsOperationalData = new List<MovementOperationalData>
                    {
                        new MovementOperationalData
                        {
                            SourceNodeId = 3,
                            DestinationNodeId = 4,
                        },
                    },
                },
            };

            var ownershipNodes = ownershipRuleData.GetOwnershipNodes();

            Assert.AreEqual(4, ownershipNodes.Count());
            Assert.AreEqual(1, ownershipNodes.ElementAt(0));
            Assert.AreEqual(2, ownershipNodes.ElementAt(1));
            Assert.AreEqual(3, ownershipNodes.ElementAt(2));
            Assert.AreEqual(4, ownershipNodes.ElementAt(3));
        }
    }
}
