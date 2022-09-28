// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using OfficialDeltaInventory = Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest.OfficialDeltaInventory;
    using OfficialDeltaMovement = Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest.OfficialDeltaMovement;
    using OriginType = Ecp.True.Entities.Enumeration.OriginType;

    /// <summary>
    /// The DeltaExtensions.
    /// </summary>
    public static class DeltaExtensions
    {
        /// <summary>
        /// Populates the result movements.
        /// </summary>
        /// <param name="deltaResultMovements">The delta result movements.</param>
        /// <returns>The ResultMovement.</returns>
        public static IEnumerable<ResultMovement> ToResponse(this IEnumerable<DeltaResultMovement> deltaResultMovements)
        {
            ArgumentValidators.ThrowIfNull(deltaResultMovements, nameof(deltaResultMovements));

            var filteredDeltaResultMovements = deltaResultMovements.Where(a => a.Sign == Constants.Positive || a.Sign == Constants.Negative);
            return filteredDeltaResultMovements.Select(x => new ResultMovement
            {
                MovementId = x.MovementId,
                MovementTransactionId = x.MovementTransactionId,
                Sign = x.Sign.EqualsIgnoreCase(Constants.Positive),
                Delta = x.Delta,
            });
        }

        /// <summary>
        /// Populates the result inventories.
        /// </summary>
        /// <param name="deltaResultInventories">The delta result inventories.</param>
        /// <returns>The ResultInventory.</returns>
        public static IEnumerable<ResultInventory> ToResponse(this IEnumerable<DeltaResultInventory> deltaResultInventories)
        {
            ArgumentValidators.ThrowIfNull(deltaResultInventories, nameof(deltaResultInventories));

            var filteredDeltaResultInventories = deltaResultInventories.Where(a => a.Sign == Constants.Positive || a.Sign == Constants.Negative);
            return filteredDeltaResultInventories.Select(x => new ResultInventory
            {
                InventoryProductUniqueId = x.InventoryProductUniqueId,
                InventoryProductId = x.InventoryTransactionId,
                Sign = x.Sign.EqualsIgnoreCase(Constants.Positive),
                Delta = x.Delta,
            });
        }

        /// <summary>
        /// Populates the error movements.
        /// </summary>
        /// <param name="deltaErrorMovements">The delta error movements.</param>
        /// <returns>The ErrorMovement.</returns>
        public static IEnumerable<ErrorMovement> ToResponse(this IEnumerable<DeltaErrorMovement> deltaErrorMovements)
        {
            ArgumentValidators.ThrowIfNull(deltaErrorMovements, nameof(deltaErrorMovements));

            return deltaErrorMovements.Select(x => new ErrorMovement
            {
                MovementId = x.MovementId,
                MovementTransactionId = x.MovementTransactionId,
                Description = x.Description,
            });
        }

        /// <summary>
        /// Populates the error inventories.
        /// </summary>
        /// <param name="deltaErrorInventories">The delta error inventories.</param>
        /// <returns>The ErrorInventory.</returns>
        public static IEnumerable<ErrorInventory> ToResponse(this IEnumerable<DeltaErrorInventory> deltaErrorInventories)
        {
            ArgumentValidators.ThrowIfNull(deltaErrorInventories, nameof(deltaErrorInventories));

            return deltaErrorInventories.Select(x => new ErrorInventory
            {
                InventoryId = x.InventoryProductUniqueId,
                InventoryProductId = x.InventoryTransactionId,
                Description = x.Description,
            });
        }

        /// <summary>
        /// Populates the result inventories.
        /// </summary>
        /// <param name="deltaResultInventories">The delta result inventories.</param>
        /// <returns>The ResultInventory.</returns>
        public static IEnumerable<OfficialResultInventory> ToResponse(this IEnumerable<OfficialDeltaResultInventory> deltaResultInventories)
        {
            ArgumentValidators.ThrowIfNull(deltaResultInventories, nameof(deltaResultInventories));

            return deltaResultInventories.Where(a => a.Sign == Constants.Positive || a.Sign == Constants.Negative).Select(x => new OfficialResultInventory
            {
                TransactionId = x.Origin == OriginType.DELTAOFICIAL ?
                                              Convert.ToInt32(x.MovementTransactionId, CultureInfo.InvariantCulture)
                                            : Convert.ToInt32(x.InventoryTransactionId, CultureInfo.InvariantCulture),
                Sign = x.Sign.EqualsIgnoreCase(Constants.Positive),
                OfficialDelta = x.DeltaOfficial,
                Origin = x.Origin,
                NetStandardVolume = x.NetStandardVolume,
                OwnerId = x.Origin == OriginType.DELTAOFICIAL ? x.MovementOwnerId : x.InventoryProductOwnerId,
            });
        }

        /// <summary>
        /// Populates the error movements.
        /// </summary>
        /// <param name="deltaErrorMovements">The delta error movements.</param>
        /// <returns>The ErrorMovement.</returns>
        public static IEnumerable<OfficialErrorMovement> ToResponse(this IEnumerable<OfficialDeltaErrorMovement> deltaErrorMovements)
        {
            ArgumentValidators.ThrowIfNull(deltaErrorMovements, nameof(deltaErrorMovements));

            return deltaErrorMovements.Select(x => new OfficialErrorMovement
            {
                MovementTransactionId = Convert.ToInt32(x.MovementTransactionId, CultureInfo.InvariantCulture),
                Description = x.Description,
                Origin = x.Origin,
            });
        }

        /// <summary>
        /// Populates the error inventories.
        /// </summary>
        /// <param name="deltaErrorInventories">The delta error inventories.</param>
        /// <returns>The ErrorInventory.</returns>
        public static IEnumerable<OfficialErrorInventory> ToResponse(this IEnumerable<OfficialDeltaErrorInventory> deltaErrorInventories)
        {
            ArgumentValidators.ThrowIfNull(deltaErrorInventories, nameof(deltaErrorInventories));

            return deltaErrorInventories.Select(x => new OfficialErrorInventory
            {
                InventoryProductId = Convert.ToInt32(x.InventoryTransactionId, CultureInfo.InvariantCulture),
                Description = x.Description,
                Origin = x.Origin,
            });
        }

        /// <summary>
        /// Populates the original movements.
        /// </summary>
        /// <param name="originalMovements">The original movements.</param>
        /// <returns>The collection of OriginalMovement.</returns>
        public static IEnumerable<DeltaOriginalMovement> ToRequest(this IEnumerable<OriginalMovement> originalMovements)
        {
            ArgumentValidators.ThrowIfNull(originalMovements, nameof(originalMovements));

            return originalMovements.Select(x => new DeltaOriginalMovement
            {
                MovementId = x.MovementId,
                MovementTransactionId = x.MovementTransactionId,
                NetStandardVolume = x.NetStandardVolume,
            });
        }

        /// <summary>
        /// Populates the updated movements.
        /// </summary>
        /// <param name="updatedMovements">The updated movements.</param>
        /// <returns>The collection of UpdatedMovement.</returns>
        public static IEnumerable<DeltaUpdatedMovement> ToRequest(this IEnumerable<UpdatedMovement> updatedMovements)
        {
            ArgumentValidators.ThrowIfNull(updatedMovements, nameof(updatedMovements));

            return updatedMovements.Select(x => new DeltaUpdatedMovement
            {
                MovementId = x.MovementId,
                MovementTransactionId = x.MovementTransactionId,
                NetStandardVolume = x.NetStandardVolume,
                EventType = x.EventType.ToUpperInvariant(),
            });
        }

        /// <summary>
        /// Populates the original inventories.
        /// </summary>
        /// <param name="originalInventories">The original inventories.</param>
        /// <returns>The collection of OriginalInventory.</returns>
        public static IEnumerable<DeltaOriginalInventory> ToRequest(this IEnumerable<OriginalInventory> originalInventories)
        {
            ArgumentValidators.ThrowIfNull(originalInventories, nameof(originalInventories));

            return originalInventories.Select(x => new DeltaOriginalInventory
            {
                InventoryProductUniqueId = x.InventoryProductUniqueId,
                InventoryProductId = x.InventoryProductId,
                ProductVolume = x.ProductVolume,
            });
        }

        /// <summary>
        /// Populates the updated inventories.
        /// </summary>
        /// <param name="updatedInventories">The updated inventories.</param>
        /// <returns>The collection of UpdatedInventory.</returns>
        public static IEnumerable<DeltaUpdatedInventory> ToRequest(this IEnumerable<UpdatedInventory> updatedInventories)
        {
            ArgumentValidators.ThrowIfNull(updatedInventories, nameof(updatedInventories));

            return updatedInventories.Select(x => new DeltaUpdatedInventory
            {
                InventoryProductUniqueId = x.InventoryProductUniqueId,
                InventoryProductId = x.InventoryProductId,
                ProductVolume = x.ProductVolume,
                EventType = x.EventType.ToUpperInvariant(),
            });
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="cancellationTypes">The cancellation types.</param>
        /// <returns>The collection of OfficialDeltaCancellationTypes.</returns>
        public static IEnumerable<OfficialDeltaCancellationTypes> ToRequest(this IEnumerable<Annulation> cancellationTypes)
        {
            ArgumentValidators.ThrowIfNull(cancellationTypes, nameof(cancellationTypes));

            return cancellationTypes.Select(x => new OfficialDeltaCancellationTypes
            {
                SourceMovementTypeId = x.SourceMovementTypeId.ToString(CultureInfo.InvariantCulture),
                AnnulationMovementTypeId = x.AnnulationMovementTypeId.ToString(CultureInfo.InvariantCulture),
            });
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="pendingOfficialInventories">The pending official inventories.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The collection of OfficialDeltaPendingOfficialInventory.</returns>
        public static IEnumerable<OfficialDeltaPendingOfficialInventory> ToRequest(this IEnumerable<PendingOfficialInventory> pendingOfficialInventories, Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(pendingOfficialInventories, nameof(pendingOfficialInventories));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            return pendingOfficialInventories.Select(x => new OfficialDeltaPendingOfficialInventory
            {
                InventoryProductID = x.InventoryProductID.ToString(CultureInfo.InvariantCulture),
                InventoryProductOwnerId = x.OwnerId.ToString(CultureInfo.InvariantCulture),
                InventoryDate = x.InventoryDate.Date == ticket.StartDate.Date ?
                x.InventoryDate.AddDays(-1).ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture)
                : x.InventoryDate.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture),
                NodeId = x.NodeId.ToString(CultureInfo.InvariantCulture),
                ProductID = x.ProductID,
                OwnerId = x.OwnerId.ToString(CultureInfo.InvariantCulture),
                OwnerShipValue = x.OwnerShipValue.ToTrueDecimal().GetValueOrDefault(),
            });
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="consolidatedInventories">The consolidated inventories.</param>
        /// <returns>The collection of OfficialDeltaConsolidatedInventoryProduct.</returns>
        public static IEnumerable<OfficialDeltaConsolidatedInventoryProduct> ToRequest(this IEnumerable<ConsolidatedInventoryProduct> consolidatedInventories)
        {
            ArgumentValidators.ThrowIfNull(consolidatedInventories, nameof(consolidatedInventories));

            var officialDeltaConsolidatedInventoryProduct = new List<OfficialDeltaConsolidatedInventoryProduct>();
            foreach (var inventoryProduct in consolidatedInventories)
            {
                foreach (var owner in inventoryProduct.ConsolidatedOwners)
                {
                    var officialConsolidatedInventory = new OfficialDeltaConsolidatedInventoryProduct();
                    officialConsolidatedInventory.ConsolidatedInventoryProductId = inventoryProduct.ConsolidatedInventoryProductId.ToString(CultureInfo.InvariantCulture);
                    officialConsolidatedInventory.InventoryProductOwnerId = owner.OwnerId.ToString(CultureInfo.InvariantCulture);
                    officialConsolidatedInventory.InventoryDate = inventoryProduct.InventoryDate.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture);
                    officialConsolidatedInventory.NodeId = inventoryProduct.NodeId.ToString(CultureInfo.InvariantCulture);
                    officialConsolidatedInventory.ProductId = inventoryProduct.ProductId;
                    officialConsolidatedInventory.OwnerId = owner.OwnerId.ToString(CultureInfo.InvariantCulture);
                    officialConsolidatedInventory.OwnershipVolume = owner.OwnershipVolume.ToTrueDecimal().GetValueOrDefault();
                    officialDeltaConsolidatedInventoryProduct.Add(officialConsolidatedInventory);
                }
            }

            return officialDeltaConsolidatedInventoryProduct;
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="pendingOfficialMovement">The pending official movement.</param>
        /// <returns>The collection of OfficialDeltaPendingOfficialMovement.</returns>
        public static IEnumerable<OfficialDeltaPendingOfficialMovement> ToRequest(this IEnumerable<PendingOfficialMovement> pendingOfficialMovement)
        {
            ArgumentValidators.ThrowIfNull(pendingOfficialMovement, nameof(pendingOfficialMovement));

            return pendingOfficialMovement.Select(x => new OfficialDeltaPendingOfficialMovement
            {
                MovementTransactionId = x.MovementTransactionId.ToString(CultureInfo.InvariantCulture),
                MovementOwnerId = x.OwnerId.ToString(CultureInfo.InvariantCulture),
                SourceNodeId = x.SourceNodeId.HasValue ? x.SourceNodeId.Value.ToString(CultureInfo.InvariantCulture) : null,
                DestinationNodeId = x.DestinationNodeId.HasValue ? x.DestinationNodeId.Value.ToString(CultureInfo.InvariantCulture) : null,
                SourceProductId = x.SourceProductId,
                DestinationProductId = x.DestinationProductId,
                MovementTypeId = x.MovementTypeID.ToString(CultureInfo.InvariantCulture),
                OwnerId = x.OwnerId.ToString(CultureInfo.InvariantCulture),
                OwnerShipValue = x.OwnerShipValue.ToTrueDecimal().GetValueOrDefault(),
            });
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="consolidatedMovement">The consolidated movement.</param>
        /// <returns>The collection of OfficialDeltaConsolidatedMovement.</returns>
        public static IEnumerable<OfficialDeltaConsolidatedMovement> ToRequest(this IEnumerable<ConsolidatedMovement> consolidatedMovement)
        {
            ArgumentValidators.ThrowIfNull(consolidatedMovement, nameof(consolidatedMovement));
            var consolidatedMovementList = consolidatedMovement.ToList();

            ArgumentValidators.ThrowIfNull(consolidatedMovement, nameof(consolidatedMovement));

            var officialDeltaConsolidatedMovement = new List<OfficialDeltaConsolidatedMovement>();

            foreach (var movement in consolidatedMovementList)
            {
                officialDeltaConsolidatedMovement.AddRange(GetOfficialDeltaConsolidatedMovements(movement));
            }

            return officialDeltaConsolidatedMovement;
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="officialDeltaMovements">The official delta movements.</param>
        /// <returns>The collection of OfficialDeltaConsolidatedMovement.</returns>
        public static IEnumerable<OfficialDeltaMovement> ToRequest(this IEnumerable<True.Entities.Query.OfficialDeltaMovement> officialDeltaMovements)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaMovements, nameof(officialDeltaMovements));
            return officialDeltaMovements.Select(x => new OfficialDeltaMovement
            {
                MovementTransactionId = x.MovementTransactionId.ToString(CultureInfo.InvariantCulture),
                MovementOwnerId = x.MovementOwnerId.ToString(CultureInfo.InvariantCulture),
                SourceNodeId = x.SourceNodeId.HasValue ? x.SourceNodeId.ToString() : string.Empty,
                DestinationNodeId = x.DestinationNodeId.HasValue ? x.DestinationNodeId.ToString() : string.Empty,
                SourceProductId = x.SourceProductId,
                DestinationProductId = x.DestinationProductId,
                MovementTypeId = x.MovementTypeId.ToString(CultureInfo.InvariantCulture),
                OwnerId = x.OwnerId.ToString(CultureInfo.InvariantCulture),
                OwnershipVolume = (double)x.OwnershipVolume,
            });
        }

        /// <summary>
        /// Converts to request.
        /// </summary>
        /// <param name="officialDeltaInventories">The official delta inventories.</param>
        /// <returns>The collection of OfficialDeltaConsolidatedMovement.</returns>
        public static IEnumerable<OfficialDeltaInventory> ToRequest(this IEnumerable<True.Entities.Query.OfficialDeltaInventory> officialDeltaInventories)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaInventories, nameof(officialDeltaInventories));
            return officialDeltaInventories.Select(x => new OfficialDeltaInventory
            {
                MovementTransactionId = x.MovementTransactionId.ToString(CultureInfo.InvariantCulture),
                MovementOwnerId = x.MovementOwnerId.ToString(CultureInfo.InvariantCulture),
                OperationalDate = x.OperationalDate.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture),
                NodeId = x.NodeId.ToString(CultureInfo.InvariantCulture),
                ProductId = x.ProductId,
                OwnerId = x.OwnerId.ToString(CultureInfo.InvariantCulture),
                OwnershipVolume = (double)x.OwnershipVolume,
            });
        }

        /// <summary>
        /// Get all node List.
        /// </summary>
        /// <param name="officialDeltaData">the officialDeltaData.</param>
        /// <returns>node list.</returns>
        public static IEnumerable<int> GetPendingOfficialMovementNodes(this OfficialDeltaData officialDeltaData)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));
            var inventoryNodeIds = officialDeltaData.PendingOfficialInventories.Select(x => x.NodeId);

            var sourceNodeIds = officialDeltaData.PendingOfficialMovements.Where(y => y.SourceNodeId != null &&
            y.SourceNodeSegmentId == y.SegmentId).Select(x => x.SourceNodeId.GetValueOrDefault());

            var destinationNodeIds = officialDeltaData.PendingOfficialMovements.Where(y => y.DestinationNodeId != null &&
            y.DestinationNodeSegmentID == y.SegmentId).Select(x => x.DestinationNodeId.GetValueOrDefault());
            return inventoryNodeIds.Union(sourceNodeIds).Union(destinationNodeIds).Distinct();
        }

        /// <summary>
        /// Adds a new official result with the volume difference between the unidentified loss and the official result.
        /// </summary>
        /// <param name="convertedResults">The collection to add a movement to.</param>
        /// <param name="unidentifiedLossVolume">The unidentified loss.</param>
        /// <param name="consolidatedOwner">The original consolidated owner.</param>
        /// <param name="deltaResult">The delta result.</param>
        public static void AddToleranceMovementConsolidate(
            this ICollection<OfficialResultMovement> convertedResults,
            decimal? unidentifiedLossVolume,
            ConsolidatedOwner consolidatedOwner,
            OfficialDeltaResultMovement deltaResult)
        {
            ArgumentValidators.ThrowIfNull(convertedResults, nameof(convertedResults));
            ArgumentValidators.ThrowIfNull(deltaResult, nameof(deltaResult));

            if (consolidatedOwner?.ConsolidatedMovementId is null
                || unidentifiedLossVolume is null)
            {
                return;
            }

            convertedResults.Add(new OfficialResultMovement
            {
                MovementTransactionId = consolidatedOwner.ConsolidatedMovementId.Value,
                Sign = deltaResult.Sign.EqualsIgnoreCase(Constants.Positive),
                OfficialDelta = deltaResult.DeltaOfficial - unidentifiedLossVolume.Value,
                Origin = deltaResult.Origin,
                NetStandardVolume = deltaResult.NetStandardVolume,
                OwnerId = deltaResult.MovementOwnerId,
            });
        }

        private static IEnumerable<OfficialDeltaConsolidatedMovement> GetOfficialDeltaConsolidatedMovements(ConsolidatedMovement movement)
        {
            return movement.ConsolidatedOwners.Select(owner => CreateOfficialDeltaConsolidatedMovement(movement, owner));
        }

        private static OfficialDeltaConsolidatedMovement CreateOfficialDeltaConsolidatedMovement(ConsolidatedMovement movement, ConsolidatedOwner owner)
        {
            return new OfficialDeltaConsolidatedMovement
            {
                ConsolidatedMovementId = movement.ConsolidatedMovementId.ToString(CultureInfo.InvariantCulture),
                MovementOwnerId = owner.OwnerId.ToString(CultureInfo.InvariantCulture),
                SourceNodeId = movement.SourceNodeId?.ToString(CultureInfo.InvariantCulture),
                DestinationNodeId = movement.DestinationNodeId?.ToString(CultureInfo.InvariantCulture),
                SourceProductId = movement.SourceProductId,
                DestinationProductId = movement.DestinationProductId,
                MovementTypeId = movement.MovementTypeId,
                OwnerId = owner.OwnerId.ToString(CultureInfo.InvariantCulture),
                OwnershipVolume = owner.OwnershipVolume.ToTrueDecimal().GetValueOrDefault(),
            };
        }
    }
}
