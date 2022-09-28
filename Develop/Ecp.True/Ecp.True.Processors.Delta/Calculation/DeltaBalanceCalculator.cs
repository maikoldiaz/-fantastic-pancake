// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaBalanceCalculator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Calculation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;

    /// <summary>
    /// Interface Calculator.
    /// </summary>
    public class DeltaBalanceCalculator : IDeltaBalanceCalculator
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly ITrueLogger<DeltaBalanceCalculator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaBalanceCalculator"/> class.
        /// </summary>
        /// <param name="unbalanceCalculator">The unbalance calculator.</param>
        /// <param name="logger">The logger.</param>
        public DeltaBalanceCalculator(ITrueLogger<DeltaBalanceCalculator> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public IEnumerable<DeltaBalance> Calculate(
            Ticket ticket,
            ConcurrentDictionary<string, List<Movement>> movementMap,
            ConcurrentDictionary<string, List<ConsolidatedMovement>> consolidatedMovementMap,
            ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>> consolidatedInventoryMap)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            ArgumentValidators.ThrowIfNull(movementMap, nameof(movementMap));
            ArgumentValidators.ThrowIfNull(consolidatedMovementMap, nameof(consolidatedMovementMap));
            ArgumentValidators.ThrowIfNull(consolidatedInventoryMap, nameof(consolidatedInventoryMap));

            this.logger.LogInformation($"Starting official delta pre-calculation for Ticket: {ticket.TicketId}", $"{ticket.TicketId}");
            var deltaBalanceList = new List<DeltaBalance>();
            var uniqueKeysList = movementMap.Keys.Union(consolidatedMovementMap.Keys).Union(consolidatedInventoryMap.Keys);
            uniqueKeysList.ForEach(uniqueKey =>
            {
                consolidatedInventoryMap.TryGetValue(uniqueKey, out List<ConsolidatedInventoryProduct> consolidatedInventories);
                consolidatedInventories ??= new List<ConsolidatedInventoryProduct>();

                consolidatedMovementMap.TryGetValue(uniqueKey, out List<ConsolidatedMovement> consolidatedMovements);
                consolidatedMovements ??= new List<ConsolidatedMovement>();

                movementMap.TryGetValue(uniqueKey, out List<Movement> movements);
                movements ??= new List<Movement>();

                var initialInventory = GetInitialInventory(uniqueKey, consolidatedInventories, ticket);
                var finalInventory = GetFinalInventory(uniqueKey, consolidatedInventories, ticket);
                var input = GetInputs(uniqueKey, consolidatedMovements, ticket);
                var output = GetOutputs(uniqueKey, consolidatedMovements, ticket);
                var deltaInitialInventory = GetDeltaInitialInventory(uniqueKey, movements, ticket);
                var deltaFinalInventory = GetDeltaFinalInventory(uniqueKey, movements, ticket);
                var deltaInput = GetDeltaInputs(uniqueKey, movements, ticket);
                var deltaOutput = GetDeltaOutputs(uniqueKey, movements, ticket);
                var control = initialInventory + deltaInitialInventory + input + deltaInput - output - deltaOutput - finalInventory - deltaFinalInventory;
                deltaBalanceList.Add(new DeltaBalance
                {
                    NodeId = Convert.ToInt32(IdHelper.GetNodeId(uniqueKey), CultureInfo.InvariantCulture),
                    ProductId = IdHelper.GetProductId(uniqueKey),
                    ElementOwnerId = Convert.ToInt32(IdHelper.GetOwnerId(uniqueKey), CultureInfo.InvariantCulture),
                    MeasurementUnit = !string.IsNullOrWhiteSpace(IdHelper.GetMeasurementUnit(uniqueKey)) ? IdHelper.GetMeasurementUnit(uniqueKey) : null,
                    InitialInventory = initialInventory,
                    FinalInventory = finalInventory,
                    Input = input,
                    Output = output,
                    DeltaInitialInventory = deltaInitialInventory,
                    DeltaFinalInventory = deltaFinalInventory,
                    DeltaInput = deltaInput,
                    DeltaOutput = deltaOutput,
                    Control = control,
                    StartDate = ticket.StartDate,
                    EndDate = ticket.EndDate,
                    SegmentId = ticket.CategoryElementId,
                });
            });

            this.logger.LogInformation($"Finished official delta pre-calculation for Ticket: {ticket.TicketId}", $"{ticket.TicketId}");
            return deltaBalanceList;
        }

        /// <summary>Gets the initial inventory.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="consolidatedInventories">The consolidated inventories.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// The initial inventory.
        /// </returns>
        private static decimal GetInitialInventory(string uniqueKey, List<ConsolidatedInventoryProduct> consolidatedInventories, Ticket ticket)
        {
            // Calculates initial inventory with inventory records for the selected node on the start day minus 1 of the period. They come from the consolidated operational information of inventories.
            var inventories = consolidatedInventories.Where(x => x.InventoryDate.Date == ticket.StartDate.AddDays(-1).Date);

            return inventories.SelectMany(x => x.ConsolidatedOwners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipVolume);
        }

        /// <summary>Gets the final inventory.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="consolidatedInventories">The consolidated inventories.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The final inventory.</returns>
        private static decimal GetFinalInventory(string uniqueKey, List<ConsolidatedInventoryProduct> consolidatedInventories, Ticket ticket)
        {
            // Calculates final inventory with inventory records for the single node selected on the end day of the period. They come from the consolidated operational information of inventories.
            var inventories = consolidatedInventories.Where(x => x.InventoryDate.Date == ticket.EndDate.Date);

            return inventories.SelectMany(x => x.ConsolidatedOwners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipVolume);
        }

        /// <summary>Gets the inputs.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="consolidatedMovements">The consolidated movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// The inputs.
        /// </returns>
        private static decimal GetInputs(string uniqueKey, List<ConsolidatedMovement> consolidatedMovements, Ticket ticket)
        {
            // Calculates inputs with movements whose destination node is the only node selected for each of the days of the period. They come from consolidated operational information on movements.
            var movements = consolidatedMovements.Where(x => x.StartDate.Date == ticket.StartDate.Date
                            && x.EndDate.Date == ticket.EndDate.Date && x.DestinationNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) == IdHelper.GetNodeId(uniqueKey)
                            && x.DestinationProductId == IdHelper.GetProductId(uniqueKey));

            return movements.SelectMany(x => x.ConsolidatedOwners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipVolume);
        }

        /// <summary>Gets the outputs.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="consolidatedMovements">The consolidated movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The outputs.</returns>
        private static decimal GetOutputs(string uniqueKey, List<ConsolidatedMovement> consolidatedMovements, Ticket ticket)
        {
            // Calculates outputs with movements whose origin node is the only node selected for each of the days of the period. They come from consolidated operational information on movements.
            var movements = consolidatedMovements.Where(x => x.StartDate.Date == ticket.StartDate.Date
                            && x.EndDate.Date == ticket.EndDate.Date && x.SourceNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) == IdHelper.GetNodeId(uniqueKey)
                            && x.SourceProductId == IdHelper.GetProductId(uniqueKey));

            return movements.SelectMany(x => x.ConsolidatedOwners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipVolume);
        }

        /// <summary>Gets the delta initial inventory.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The delta initial inventory.</returns>
        private static decimal GetDeltaInitialInventory(string uniqueKey, List<Movement> movements, Ticket ticket)
        {
            // Calculates delta initial inventory with Delta inventory type movement records for the node,
            // with less than 1 day of the selected initial period i.e. the operational date is equal to the initial date of the period minus one day.
            var initialInvMovements = movements.Where(x => x.OperationalDate.Date == ticket.StartDate.AddDays(-1).Date && ((x.MovementSource?.SourceNodeId != null
                        && x.MovementDestination?.DestinationNodeId == null) || (x.MovementSource?.SourceNodeId == null
                        && x.MovementDestination?.DestinationNodeId != null)));
            return GetDeltaInventory(uniqueKey, initialInvMovements);
        }

        /// <summary>Gets the delta final inventory.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The delta final inventory.</returns>
        private static decimal GetDeltaFinalInventory(string uniqueKey, List<Movement> movements, Ticket ticket)
        {
            // Calculates delta final inventory with records of movements of the Delta type of inventory for the selected node on the day of the end of the period.
            var finalInvMovements = movements.Where(x => x.OperationalDate.Date == ticket.EndDate.Date && ((x.MovementSource?.SourceNodeId != null
                        && x.MovementDestination?.DestinationNodeId == null) || (x.MovementSource?.SourceNodeId == null
                        && x.MovementDestination?.DestinationNodeId != null)));
            return GetDeltaInventory(uniqueKey, finalInvMovements);
        }

        /// <summary>Gets the delta inputs.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The delta inputs.</returns>
        private static decimal GetDeltaInputs(string uniqueKey, List<Movement> movements, Ticket ticket)
        {
            // Calculates delta input with movements whose destination node is the single node selected for each of the days of the period, whose origin is FICO (official delta),
            // delta classification and manual movements of the official scenario (source system identifier equal to the system identifier "ManualMovOficial"),
            // which have an official delta ticket associated, the start and end dates are equal to the start and end dates of the period
            // They come from the operational information of movements.
            var filteredMovements = movements.Where(x => x.Period.StartTime.GetValueOrDefault().Date == ticket.StartDate.Date &&
                                   x.Period.EndTime.GetValueOrDefault().Date == ticket.EndDate.Date
                                   && x.MovementDestination?.DestinationNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) == IdHelper.GetNodeId(uniqueKey)
                                   && x.MovementDestination?.DestinationProductId == IdHelper.GetProductId(uniqueKey)
                                   && (x.OfficialDeltaMessageTypeId == True.Entities.Enumeration.OfficialDeltaMessageType.ConsolidatedMovementDelta
                                   || x.OfficialDeltaMessageTypeId == True.Entities.Enumeration.OfficialDeltaMessageType.OfficialMovementDelta
                                   || x.SourceSystemId == Constants.ManualMovOfficial));

            return filteredMovements.SelectMany(x => x.Owners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipValue.GetValueOrDefault());
        }

        /// <summary>Gets the delta outputs.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The delta outputs.</returns>
        private static decimal GetDeltaOutputs(string uniqueKey, List<Movement> movements, Ticket ticket)
        {
            // Calculates delta output with movements whose source node is the single node selected for each of the days of the period, whose origin is FICO (official delta),
            // delta classification and manual movements of the official scenario (source system identifier equal to the system identifier "ManualMovOficial"),
            // which have an official delta ticket associated, the start and end dates are equal to the start and end dates of the period
            // They come from the operational information of movements.
            var filteredMovements = movements.Where(x => x.Period.StartTime.GetValueOrDefault().Date == ticket.StartDate.Date &&
                                   x.Period.EndTime.GetValueOrDefault().Date == ticket.EndDate.Date
                                   && x.MovementSource?.SourceNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) == IdHelper.GetNodeId(uniqueKey)
                                   && x.MovementSource?.SourceProductId == IdHelper.GetProductId(uniqueKey)
                                   && (x.OfficialDeltaMessageTypeId == True.Entities.Enumeration.OfficialDeltaMessageType.ConsolidatedMovementDelta
                                   || x.OfficialDeltaMessageTypeId == True.Entities.Enumeration.OfficialDeltaMessageType.OfficialMovementDelta
                                   || x.SourceSystemId == Constants.ManualMovOfficial));

            return filteredMovements.SelectMany(x => x.Owners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipValue.GetValueOrDefault());
        }

        /// <summary>Gets the delta inventory.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>The delta inventory.</returns>
        private static decimal GetDeltaInventory(string uniqueKey, IEnumerable<Movement> movements)
        {
            // They come from the information resulting from the official inventory deltas calculated by FICO
            // and also includes the manual movements of the official scenario (source system identifier equal to the system identifier "ManualInvOficial"),
            // which have an official delta ticket associated.
            return GetDeltaInvManual(uniqueKey, movements) + GetInvDeltas(uniqueKey, movements);
        }

        private static decimal GetInvDeltas(string uniqueKey, IEnumerable<Movement> movements)
        {
            var inventoryDeltas = movements.Where(x =>
                                  x.OfficialDeltaMessageTypeId == True.Entities.Enumeration.OfficialDeltaMessageType.ConsolidatedInventoryDelta
                                  || x.OfficialDeltaMessageTypeId == True.Entities.Enumeration.OfficialDeltaMessageType.OfficialInventoryDelta);

            return GetDeltaInvSource(uniqueKey, inventoryDeltas) + GetDeltaInvDest(uniqueKey, inventoryDeltas);
        }

        /// <summary>Gets the delta inv manual.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>The delta inventory manual.</returns>
        private static decimal GetDeltaInvManual(string uniqueKey, IEnumerable<Movement> movements)
        {
            // Use official manual inv movements that have only the source or destination node and where the node is equal to the node selected to generate the report.
            var manualInvMovements = movements.Where(x =>
                        x.SourceSystemId == Constants.ManualInvOfficial);

            return GetDeltaInvSource(uniqueKey, manualInvMovements) + GetDeltaInvDest(uniqueKey, manualInvMovements);
        }

        /// <summary>Gets the delta inv source manual.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="manualInvMovements">The manual inv movements.</param>
        /// <returns>The source manual delta inventory.</returns>
        private static decimal GetDeltaInvSource(string uniqueKey, IEnumerable<Movement> manualInvMovements)
        {
            // If the movement has a source node, the net quantity used for the calculation must be multiplied by minus -1.
            var manualSourceMovements = manualInvMovements.Where(x => x.MovementSource?.SourceNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) == IdHelper.GetNodeId(uniqueKey));

            return manualSourceMovements.SelectMany(x => x.Owners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => -1 * z.OwnershipValue.GetValueOrDefault());
        }

        /// <summary>Gets the delta inv dest manual.</summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="manualInvMovements">The manual inv movements.</param>
        /// <returns>The destination manual delta inventory.</returns>
        private static decimal GetDeltaInvDest(string uniqueKey, IEnumerable<Movement> manualInvMovements)
        {
            // If the movement has a destination node, the net quantity must be used unchanged.
            var manualDestinationMovements = manualInvMovements.Where(x => x.MovementDestination?.DestinationNodeId.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)
                                        == IdHelper.GetNodeId(uniqueKey));

            return manualDestinationMovements.SelectMany(x => x.Owners)
                .Where(y => y.OwnerId.ToString(CultureInfo.InvariantCulture) == IdHelper.GetOwnerId(uniqueKey)).Sum(z => z.OwnershipValue.GetValueOrDefault());
        }
    }
}
