// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Event = Ecp.True.Entities.Registration.Event;

    /// <summary>
    /// The OwnershipExtensions.
    /// </summary>
    public static class OwnershipExtensions
    {
        /// <summary>
        /// Adjusts the ownership volume for decimal round off.
        /// </summary>
        /// <param name="requests">The requests.</param>
        public static void AdjustOwnershipVolumeForDecimalRoundOff(this OwnershipRuleRequest requests)
        {
            ArgumentValidators.ThrowIfNull(requests, nameof(requests));

            foreach (var movement in requests.PreviousMovementsOperationalData.GroupBy(x => x.MovementId))
            {
                var lastOwnership = movement.AsEnumerable().Last();
                lastOwnership.OwnershipVolume = lastOwnership.NetStandardVolume - movement.Sum(x => x.OwnershipVolume) + lastOwnership.OwnershipVolume;
            }

            foreach (var inventory in requests.PreviousInventoryOperationalData.GroupBy(x => x.InventoryId))
            {
                var lastOwnership = inventory.AsEnumerable().Last();
                lastOwnership.OwnershipVolume = lastOwnership.NetStandardVolume - inventory.Sum(x => x.OwnershipVolume) + lastOwnership.OwnershipVolume;
            }
        }

        /// <summary>Gets the new movements..</summary>
        /// <param name="movement">The source movement.</param>
        /// <param name="existingmovement">The input new movement.</param>
        /// <param name="ticket">The ownership ticket.</param>
        /// <param name="inputMovement">The input movement event.</param>
        /// <param name="outputMovement">The output movement event.</param>
        /// <returns>The new movements.</returns>
        public static IEnumerable<Movement> ToMovementsForCollaborationAgreements(
            this Movement movement,
            NewMovement existingmovement,
            Ticket ticket,
            MovementEvent inputMovement,
            MovementEvent outputMovement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            ArgumentValidators.ThrowIfNull(existingmovement, nameof(existingmovement));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var newMovements = new List<Movement>();
            var newDebtorMovement = new Movement
            {
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = existingmovement.NodeId,
                    DestinationProductId = existingmovement.ProductId,
                    DestinationProductTypeId = movement.MovementDestination.DestinationProductTypeId,
                },
                MovementTypeId = (int)MovementType.InputCollaborationAgreement,
                NetStandardVolume = existingmovement.OwnershipVolume,
                MeasurementUnit = movement.MeasurementUnit,
                SegmentId = ticket.CategoryElementId,
                OperationalDate = ticket.StartDate,
                SourceMovementId = movement.MovementTransactionId,
                OwnershipTicketId = ticket.TicketId,
                MovementEvent = inputMovement,
                Ownerships = new List<Ownership> { GetDebtorOwnership(existingmovement, ticket) },
            };

            newDebtorMovement.PopulateDefaultValues();

            var newCreditorMovement = new Movement
            {
                MovementSource = new MovementSource
                {
                    SourceNodeId = existingmovement.NodeId,
                    SourceProductId = existingmovement.ProductId,
                    SourceProductTypeId = movement.MovementDestination.DestinationProductTypeId,
                },
                MovementTypeId = (int)MovementType.OutputCollaborationAgreement,
                NetStandardVolume = existingmovement.OwnershipVolume,
                MeasurementUnit = movement.MeasurementUnit,
                SegmentId = ticket.CategoryElementId,
                OperationalDate = ticket.StartDate,
                SourceMovementId = movement.MovementTransactionId,
                OwnershipTicketId = ticket.TicketId,
                MovementEvent = outputMovement,
                Ownerships = new List<Ownership> { GetCreditorOwnership(existingmovement, ticket) },
            };

            newCreditorMovement.PopulateDefaultValues();

            newMovements.Add(newDebtorMovement);
            newMovements.Add(newCreditorMovement);

            return newMovements;
        }

        /// <summary>Gets the new movements..</summary>
        /// <param name="existingmovement">The existing movement.</param>
        /// <param name="ticket">The ownership ticket.</param>
        /// <returns>The new movements.</returns>
        public static IEnumerable<Movement> ToMovementsForLoansPayments(
            this NewMovement existingmovement,
            Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(existingmovement, nameof(existingmovement));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var newMovements = new List<Movement>();

            var newDebtorMovement = new Movement
            {
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = existingmovement.NodeId,
                    DestinationProductId = existingmovement.ProductId,
                },
                MovementTypeId = (int)MovementType.InputEvacuation,
                NetStandardVolume = existingmovement.OwnershipVolume,
                SegmentId = ticket.CategoryElementId,
                MeasurementUnit = 31,
                OperationalDate = ticket.StartDate,
                OwnershipTicketId = ticket.TicketId,
                BlockchainStatus = StatusType.PROCESSING,
                Ownerships = new List<Ownership> { GetDebtorOwnership(existingmovement, ticket) },
            };

            newDebtorMovement.PopulateDefaultValues();

            var newCreditorMovement = new Movement
            {
                MovementSource = new MovementSource
                {
                    SourceNodeId = existingmovement.NodeId,
                    SourceProductId = existingmovement.ProductId,
                },
                MovementTypeId = (int)MovementType.OutputEvacuation,
                NetStandardVolume = existingmovement.OwnershipVolume,
                SegmentId = ticket.CategoryElementId,
                MeasurementUnit = 31,
                OperationalDate = ticket.StartDate,
                OwnershipTicketId = ticket.TicketId,
                BlockchainStatus = StatusType.PROCESSING,
                Ownerships = new List<Ownership> { GetCreditorOwnership(existingmovement, ticket) },
            };

            newCreditorMovement.PopulateDefaultValues();

            newMovements.Add(newDebtorMovement);
            newMovements.Add(newCreditorMovement);
            return newMovements;
        }

        /// <summary>Gets the event.</summary>
        /// <param name="existingEvent">The source event.</param>
        /// <param name="newMovement">The new movement.</param>
        /// <param name="ticketId">The ownership ticket id.</param>
        /// <returns>The event.</returns>
        public static MovementEvent ToEventForCollaborationAgreements(this Event existingEvent, NewMovement newMovement, int ticketId)
        {
            ArgumentValidators.ThrowIfNull(existingEvent, nameof(existingEvent));
            ArgumentValidators.ThrowIfNull(newMovement, nameof(newMovement));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            var newEvent = new MovementEvent
            {
                EventTypeId = existingEvent.EventTypeId,
                SourceNodeId = existingEvent.SourceNodeId,
                DestinationNodeId = existingEvent.DestinationNodeId,
                SourceProductId = existingEvent.SourceProductId,
                DestinationProductId = existingEvent.DestinationProductId,
                StartDate = existingEvent.StartDate,
                EndDate = existingEvent.EndDate,
                Owner1Id = existingEvent.Owner1Id,
                Owner2Id = existingEvent.Owner2Id,
                Volume = newMovement.OwnershipVolume,
                MeasurementUnit = existingEvent.MeasurementUnit,
                IsDeleted = false,
            };
            return newEvent;
        }

        /// <summary>
        /// Converts input evacuation to input cancellation.
        /// </summary>
        /// <param name="cancellationMovement">The cancellation movement.</param>
        public static void ToInputCancellation(this CancellationMovementDetail cancellationMovement)
        {
            ArgumentValidators.ThrowIfNull(cancellationMovement, nameof(cancellationMovement));

            cancellationMovement.SourceNodeId = cancellationMovement.DestinationNodeId;
            cancellationMovement.SourceProductId = cancellationMovement.DestinationProductId;
            cancellationMovement.DestinationNodeId = null;
            cancellationMovement.DestinationProductId = null;
            cancellationMovement.MovementTypeId = (int)MovementType.InputCancellation;
            cancellationMovement.MovementType = MovementType.InputCancellation.ToString();
        }

        /// <summary>
        /// Converts input evacuation to output cancellation.
        /// </summary>
        /// <param name="cancellationMovement">The cancellation movement.</param>
        public static void ToOutputCancellation(this CancellationMovementDetail cancellationMovement)
        {
            ArgumentValidators.ThrowIfNull(cancellationMovement, nameof(cancellationMovement));

            cancellationMovement.DestinationNodeId = cancellationMovement.SourceNodeId;
            cancellationMovement.DestinationProductId = cancellationMovement.SourceProductId;
            cancellationMovement.SourceNodeId = null;
            cancellationMovement.SourceProductId = null;
            cancellationMovement.MovementTypeId = (int)MovementType.OutputCancellation;
            cancellationMovement.MovementType = MovementType.OutputCancellation.ToString();
        }

        /// <summary>
        /// Converts cancellation movement detail to cancellation movement with ownership.
        /// </summary>
        /// <param name="cancellationMovementDetail">The cancellation movement.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The Movement.</returns>
        public static Movement ToCancellationMovementWithOwnership(this CancellationMovementDetail cancellationMovementDetail, Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(cancellationMovementDetail, nameof(cancellationMovementDetail));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            return new Movement
            {
                NetStandardVolume = cancellationMovementDetail.NetVolume,
                MessageTypeId = cancellationMovementDetail.MessageTypeId,
                MovementTypeId = cancellationMovementDetail.MovementTypeId,
                SegmentId = ticket.CategoryElementId,
                MeasurementUnit = cancellationMovementDetail.Unit,
                TicketId = ticket.TicketId,
                OwnershipTicketId = ticket.TicketId,
                OperationalDate = ticket.StartDate,
                Classification = string.Empty,
                SystemTypeId = Convert.ToInt32(SystemType.TRUE, CultureInfo.InvariantCulture),
                MovementId = DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture),
                EventType = EventType.Insert.ToString(),
                Ownerships = new List<Ownership>
                {
                    new Ownership
                    {
                        OwnershipPercentage = cancellationMovementDetail.OwnershipPercentage ?? 0M,
                        OwnershipVolume = cancellationMovementDetail.OwnershipVolume ?? 0M,
                        OwnerId = cancellationMovementDetail.OwnerId ?? 0,
                        ExecutionDate = DateTime.UtcNow.ToTrue(),
                        MessageTypeId = MessageType.SpecialMovement,
                        TicketId = ticket.TicketId,
                        AppliedRule = Constants.Evacuation,
                        RuleVersion = cancellationMovementDetail.RuleVersion,
                    },
                },
                MovementSource = cancellationMovementDetail.SourceNodeId.HasValue && cancellationMovementDetail.SourceProductId != null
                ? new MovementSource
                {
                    SourceNodeId = cancellationMovementDetail.SourceNodeId,
                    SourceProductId = cancellationMovementDetail.SourceProductId,
                    SourceProductTypeId = cancellationMovementDetail.ProductType,
                }
                : null,
                MovementDestination = cancellationMovementDetail.DestinationNodeId.HasValue && cancellationMovementDetail.DestinationProductId != null
                ? new MovementDestination
                {
                    DestinationNodeId = cancellationMovementDetail.DestinationNodeId,
                    DestinationProductId = cancellationMovementDetail.DestinationProductId,
                    DestinationProductTypeId = cancellationMovementDetail.ProductType,
                }
                : null,
            };
        }

        /// <summary>
        /// Converts cancellation movement to movement operational data.
        /// </summary>
        /// <param name="cancellationMovement">The cancellation movement.</param>
        /// <param name="ticketId">The ticket Id.</param>
        /// <returns>The object of MovementOperationalData.</returns>
        public static MovementOperationalData ToMovementOperationalData(this CancellationMovementDetail cancellationMovement, int ticketId)
        {
            ArgumentValidators.ThrowIfNull(cancellationMovement, nameof(cancellationMovement));

            var cancellationMovementData = new MovementOperationalData
            {
                Ticket = ticketId,
                MovementTransactionId = cancellationMovement.MovementTransactionId,
                MovementTypeId = cancellationMovement.MovementTypeId.ToString(CultureInfo.InvariantCulture),
                SourceNodeId = cancellationMovement.SourceNodeId,
                DestinationNodeId = cancellationMovement.DestinationNodeId,
                SourceProductId = cancellationMovement.SourceProductId,
                DestinationProductId = cancellationMovement.DestinationProductId,
                NetVolume = cancellationMovement.NetVolume,
                OwnershipValue = cancellationMovement.OwnershipVolume,
                OwnershipUnit = cancellationMovement.Unit.HasValue ? cancellationMovement.Unit.ToString() : null,
                OwnerId = cancellationMovement.OwnerId,
                OperationalDate = cancellationMovement.OperationalDate,
                MessageTypeId = (MessageType)Enum.Parse(typeof(MessageType), cancellationMovement.MessageTypeId.ToString(CultureInfo.InvariantCulture)),
            };

            return cancellationMovementData;
        }

        /// <summary>
        /// Converts cancellation movement to movement operational data.
        /// </summary>
        /// <param name="ownershipRuleData">The ownership rule data.</param>
        /// <returns>The object of MovementOperationalData.</returns>
        public static IEnumerable<int> GetOwnershipNodes(this OwnershipRuleData ownershipRuleData)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            var inventoryNodes = ownershipRuleData.OwnershipRuleRequest.InventoryOperationalData.Select(x => x.NodeId).Distinct();
            var previousInventoryNodes = ownershipRuleData.OwnershipRuleRequest.PreviousInventoryOperationalData.Select(x => x.NodeId).Distinct();
            var movementNodes = ownershipRuleData.OwnershipRuleRequest.MovementsOperationalData.Select(x => x.SourceNodeId.GetValueOrDefault())
                .Union(ownershipRuleData.OwnershipRuleRequest.MovementsOperationalData.Select(x => x.DestinationNodeId.GetValueOrDefault())).Where(x => x != 0).Distinct();
            var nodesList = inventoryNodes.Union(previousInventoryNodes).Union(movementNodes);
            return nodesList;
        }

        private static Ownership GetDebtorOwnership(NewMovement existingmovement, Ticket ticket)
        {
            return new Ownership
            {
                OwnerId = existingmovement.DebtorOwnerId,
                OwnershipPercentage = 100.00M,
                OwnershipVolume = existingmovement.OwnershipVolume,
                AppliedRule = existingmovement.AppliedRule,
                RuleVersion = existingmovement.RuleVersion,
                TicketId = ticket.TicketId,
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.SpecialMovement,
            };
        }

        private static Ownership GetCreditorOwnership(NewMovement existingmovement, Ticket ticket)
        {
            return new Ownership
            {
                OwnerId = existingmovement.CreditorOwnerId,
                OwnershipPercentage = 100.00M,
                OwnershipVolume = existingmovement.OwnershipVolume,
                AppliedRule = existingmovement.AppliedRule,
                RuleVersion = existingmovement.RuleVersion,
                TicketId = ticket.TicketId,
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.SpecialMovement,
            };
        }

        private static void PopulateDefaultValues(this Movement movement)
        {
            movement.SourceSystemId = (int)SourceSystem.FICO;
            movement.Classification = string.Empty;
            movement.SystemTypeId = Convert.ToInt32(SystemType.TRUE, CultureInfo.InvariantCulture);
            movement.MovementId = DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);
            movement.MessageTypeId = Convert.ToInt32(MessageType.SpecialMovement, CultureInfo.InvariantCulture);
            movement.EventType = EventType.Insert.ToString();
            movement.ScenarioId = ScenarioType.OPERATIONAL;
        }
    }
}
