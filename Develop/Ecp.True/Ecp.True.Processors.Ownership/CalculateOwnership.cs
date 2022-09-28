// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculateOwnership.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Ownership.Interfaces;

    /// <summary>
    /// The calculate ownership.
    /// </summary>
    public class CalculateOwnership : ICalculateOwnership
    {
        /// <summary>
        /// Calculates the and register asynchronous.
        /// </summary>
        /// <param name="nodeId">The node.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public OwnershipCalculation Calculate(
              int nodeId,
              string productId,
              int measurementUnit,
              DateTime date,
              int ownerId,
              int ticketId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> movements)
        {
            ArgumentValidators.ThrowIfNull(nodeId, nameof(nodeId));
            ArgumentValidators.ThrowIfNull(productId, nameof(productId));

            var nodeIds = new List<int> { nodeId };
            var inputMovementValue = GetMovementOwnership(inputMovements, measurementUnit, ownerId);
            var outputMovementValue = GetMovementOwnership(outputMovements, measurementUnit, ownerId);
            var initialInventoryValue = GetInventoryOwnership(initialInventories, measurementUnit, ownerId);
            var finalInventoryValue = GetInventoryOwnership(finalInventories, measurementUnit, ownerId);
            var interfaceValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.Interface);
            var toleranceValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.BalanceTolerance);
            var unidentifiedValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.UnidentifiedLosses);
            var identifiedLossValue = GetIdentifiedLossOwnership(movements, nodeIds, ownerId, date, productId, measurementUnit);

            var ownershipCalculation = new OwnershipCalculation
            {
                NodeId = nodeId,
                Date = date.Date,
                ProductId = productId,
                MeasurementUnit = measurementUnit,
                OwnershipTicketId = ticketId,
                OwnerId = ownerId,
                InputVolume = inputMovementValue,
                OutputVolume = outputMovementValue,
                InitialInventoryVolume = initialInventoryValue,
                FinalInventoryVolume = finalInventoryValue,
                InterfaceVolume = interfaceValue,
                ToleranceVolume = toleranceValue,
                UnidentifiedLossesVolume = unidentifiedValue,
                IdentifiedLossesVolume = identifiedLossValue,
                UnbalanceVolume = GetUnbalanceVolume(
                    initialInventoryValue,
                    inputMovementValue,
                    outputMovementValue,
                    finalInventoryValue,
                    identifiedLossValue,
                    interfaceValue,
                    toleranceValue,
                    unidentifiedValue),
            };

            return ownershipCalculation;
        }

        /// <summary>
        /// Calculates the and register.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="nodeIds">The node identifiers.</param>
        /// <returns>
        /// The SegmentOwnershipCalculation.
        /// </returns>
        public SegmentOwnershipCalculation CalculateAndRegisterForSegment(
              string productId,
              int measurementUnit,
              DateTime date,
              int ownerId,
              int ticketId,
              int segmentId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> movements,
              IEnumerable<int> nodeIds)
        {
            ArgumentValidators.ThrowIfNull(productId, nameof(productId));

            var inputMovementValue = GetMovementOwnership(inputMovements, measurementUnit, ownerId);
            var outputMovementValue = GetMovementOwnership(outputMovements, measurementUnit, ownerId);
            var initialInventoryValue = GetInventoryOwnership(initialInventories, measurementUnit, ownerId);
            var finalInventoryValue = GetInventoryOwnership(finalInventories, measurementUnit, ownerId);
            var interfaceValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.Interface);
            var toleranceValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.BalanceTolerance);
            var unidentifiedValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.UnidentifiedLosses);
            var identifiedLossValue = GetIdentifiedLossOwnership(movements, nodeIds, ownerId, date, productId, measurementUnit);

            var ownershipCalculation = new SegmentOwnershipCalculation
            {
                Date = date.Date,
                ProductId = productId,
                MeasurementUnit = measurementUnit,
                OwnershipTicketId = ticketId,
                OwnerId = ownerId,
                SegmentId = segmentId,
                InputVolume = inputMovementValue,
                OutputVolume = outputMovementValue,
                InitialInventoryVolume = initialInventoryValue,
                FinalInventoryVolume = finalInventoryValue,
                InterfaceVolume = interfaceValue,
                ToleranceVolume = toleranceValue,
                UnidentifiedLossesVolume = unidentifiedValue,
                IdentifiedLossesVolume = identifiedLossValue,
                UnbalanceVolume = GetUnbalanceVolume(
                    initialInventoryValue,
                    inputMovementValue,
                    outputMovementValue,
                    finalInventoryValue,
                    identifiedLossValue,
                    interfaceValue,
                    toleranceValue,
                    unidentifiedValue),
            };

            return ownershipCalculation;
        }

        /// <summary>
        /// Calculates the and register.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="systemId">The system identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="nodeIds">The node identifiers.</param>
        /// <returns>
        /// The SystemOwnershipCalculation.
        /// </returns>
        public SystemOwnershipCalculation CalculateAndRegisterForSystem(
              string productId,
              int measurementUnit,
              DateTime date,
              int ownerId,
              int ticketId,
              int segmentId,
              int systemId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> movements,
              IEnumerable<int> nodeIds)
        {
            ArgumentValidators.ThrowIfNull(productId, nameof(productId));

            var inputMovementValue = GetMovementOwnership(inputMovements, measurementUnit, ownerId);
            var outputMovementValue = GetMovementOwnership(outputMovements, measurementUnit, ownerId);
            var initialInventoryValue = GetInventoryOwnership(initialInventories, measurementUnit, ownerId);
            var finalInventoryValue = GetInventoryOwnership(finalInventories, measurementUnit, ownerId);
            var interfaceValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.Interface);
            var toleranceValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.BalanceTolerance);
            var unidentifiedValue = GetOwnership(movements, ownerId, date, productId, measurementUnit, nodeIds, VariableType.UnidentifiedLosses);
            var identifiedLossValue = GetIdentifiedLossOwnership(movements, nodeIds, ownerId, date, productId, measurementUnit);

            var ownershipCalculation = new SystemOwnershipCalculation
            {
                Date = date.Date,
                ProductId = productId,
                MeasurementUnit = measurementUnit,
                OwnershipTicketId = ticketId,
                OwnerId = ownerId,
                SegmentId = segmentId,
                SystemId = systemId,
                InputVolume = inputMovementValue,
                OutputVolume = outputMovementValue,
                InitialInventoryVolume = initialInventoryValue,
                FinalInventoryVolume = finalInventoryValue,
                InterfaceVolume = interfaceValue,
                ToleranceVolume = toleranceValue,
                UnidentifiedLossesVolume = unidentifiedValue,
                IdentifiedLossesVolume = identifiedLossValue,
                UnbalanceVolume = GetUnbalanceVolume(
                    initialInventoryValue,
                    inputMovementValue,
                    outputMovementValue,
                    finalInventoryValue,
                    identifiedLossValue,
                    interfaceValue,
                    toleranceValue,
                    unidentifiedValue),
            };

            return ownershipCalculation;
        }

        /// <inheritdoc/>
        public IEnumerable<OwnershipCalculation> CalculatePercentageAndRegister(IEnumerable<OwnershipCalculation> resultOwnershipCalculation)
        {
            ArgumentValidators.ThrowIfNull(resultOwnershipCalculation, nameof(resultOwnershipCalculation));
            var finalOwnershipCalculations = new List<OwnershipCalculation>();
            if (resultOwnershipCalculation.Any())
            {
                var totalNetInputVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InputVolume.GetValueOrDefault()));
                var totalNetOutputVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.OutputVolume.GetValueOrDefault()));
                var totalNetInitialInventoryVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InitialInventoryVolume.GetValueOrDefault()));
                var totalNetFinalInventoryVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.FinalInventoryVolume.GetValueOrDefault()));
                var totalNetInterfaceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InterfaceVolume.GetValueOrDefault()));
                var totalNetToleranceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.ToleranceVolume.GetValueOrDefault()));
                var totalNetUnidentifiedLossesVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.UnidentifiedLossesVolume.GetValueOrDefault()));
                var totalNetIdentifiedLossesVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.IdentifiedLossesVolume.GetValueOrDefault()));
                var totalNetUnbalanceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.UnbalanceVolume.GetValueOrDefault()));

                resultOwnershipCalculation.ForEach(ownershipCalculation =>
                {
                    ownershipCalculation.InputPercentage = totalNetInputVolume != 0 ? Math.Abs(decimal.Round(ownershipCalculation.InputVolume.Value / totalNetInputVolume * 100, 2)) : 0;
                    ownershipCalculation.OutputPercentage = totalNetOutputVolume != 0 ?
                                                            Math.Abs(decimal.Round(ownershipCalculation.OutputVolume.Value / totalNetOutputVolume * 100, 2)) : 0;
                    ownershipCalculation.InitialInventoryPercentage = totalNetInitialInventoryVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.InitialInventoryVolume.Value / totalNetInitialInventoryVolume * 100, 2)) : 0;
                    ownershipCalculation.FinalInventoryPercentage = totalNetFinalInventoryVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.FinalInventoryVolume.Value / totalNetFinalInventoryVolume * 100, 2)) : 0;
                    ownershipCalculation.InterfacePercentage = totalNetInterfaceVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.InterfaceVolume.Value / totalNetInterfaceVolume * 100, 2)) : 0;
                    ownershipCalculation.TolerancePercentage = totalNetToleranceVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.ToleranceVolume.Value / totalNetToleranceVolume * 100, 2)) : 0;
                    ownershipCalculation.UnidentifiedLossesPercentage = totalNetUnidentifiedLossesVolume != 0 ?
                                                                        Math.Abs(decimal.Round(ownershipCalculation.UnidentifiedLossesVolume.Value / totalNetUnidentifiedLossesVolume * 100, 2))
                                                                        : 0;
                    ownershipCalculation.IdentifiedLossesPercentage = totalNetIdentifiedLossesVolume != 0 ?
                                                                        Math.Abs(decimal.Round(ownershipCalculation.IdentifiedLossesVolume.Value / totalNetIdentifiedLossesVolume * 100, 2)) : 0;
                    ownershipCalculation.UnbalancePercentage = totalNetUnbalanceVolume != 0 ?
                                                                        Math.Abs(decimal.Round(ownershipCalculation.UnbalanceVolume.Value / totalNetUnbalanceVolume * 100, 2)) : 0;

                    finalOwnershipCalculations.Add(ownershipCalculation);
                });
            }

            return finalOwnershipCalculations;
        }

        /// <inheritdoc/>
        public IEnumerable<SegmentOwnershipCalculation> CalculatePercentageAndRegisterForSegment(IEnumerable<SegmentOwnershipCalculation> resultOwnershipCalculation)
        {
            ArgumentValidators.ThrowIfNull(resultOwnershipCalculation, nameof(resultOwnershipCalculation));
            var finalSegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>();
            if (resultOwnershipCalculation.Any())
            {
                var totalNetInputVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InputVolume.GetValueOrDefault()));
                var totalNetOutputVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.OutputVolume.GetValueOrDefault()));
                var totalNetInitialInventoryVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InitialInventoryVolume.GetValueOrDefault()));
                var totalNetFinalInventoryVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.FinalInventoryVolume.GetValueOrDefault()));
                var totalNetInterfaceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InterfaceVolume.GetValueOrDefault()));
                var totalNetToleranceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.ToleranceVolume.GetValueOrDefault()));
                var totalNetUnidentifiedLossesVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.UnidentifiedLossesVolume.GetValueOrDefault()));
                var totalNetIdentifiedLossesVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.IdentifiedLossesVolume.GetValueOrDefault()));
                var totalNetUnbalanceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.UnbalanceVolume.GetValueOrDefault()));

                resultOwnershipCalculation.ForEach(ownershipCalculation =>
                {
                    ownershipCalculation.InputPercentage = totalNetInputVolume != 0 ? Math.Abs(decimal.Round(ownershipCalculation.InputVolume.Value / totalNetInputVolume * 100, 2)) : 0;
                    ownershipCalculation.OutputPercentage = totalNetOutputVolume != 0 ?
                    Math.Abs(decimal.Round(ownershipCalculation.OutputVolume.Value / totalNetOutputVolume * 100, 2)) : 0;
                    ownershipCalculation.InitialInventoryPercentage = totalNetInitialInventoryVolume != 0 ?
                                                                       Math.Abs(decimal.Round(ownershipCalculation.InitialInventoryVolume.Value / totalNetInitialInventoryVolume * 100, 2)) : 0;
                    ownershipCalculation.FinalInventoryPercentage = totalNetFinalInventoryVolume != 0 ?
                                                                       Math.Abs(decimal.Round(ownershipCalculation.FinalInventoryVolume.Value / totalNetFinalInventoryVolume * 100, 2)) : 0;
                    ownershipCalculation.InterfacePercentage = totalNetInterfaceVolume != 0 ?
                                                                       Math.Abs(decimal.Round(ownershipCalculation.InterfaceVolume.Value / totalNetInterfaceVolume * 100, 2)) : 0;
                    ownershipCalculation.TolerancePercentage = totalNetToleranceVolume != 0 ?
                                                                       Math.Abs(decimal.Round(ownershipCalculation.ToleranceVolume.Value / totalNetToleranceVolume * 100, 2)) : 0;
                    ownershipCalculation.UnidentifiedLossesPercentage = totalNetUnidentifiedLossesVolume != 0 ?
                                                                         Math.Abs(decimal.Round(
                                                                         ownershipCalculation.UnidentifiedLossesVolume.Value / totalNetUnidentifiedLossesVolume * 100, 2)) : 0;
                    ownershipCalculation.IdentifiedLossesPercentage = totalNetIdentifiedLossesVolume != 0 ?
                                                                         Math.Abs(decimal.Round(ownershipCalculation.IdentifiedLossesVolume.Value / totalNetIdentifiedLossesVolume * 100, 2)) : 0;
                    ownershipCalculation.UnbalancePercentage = totalNetUnbalanceVolume != 0 ?
                                                                         Math.Abs(decimal.Round(ownershipCalculation.UnbalanceVolume.Value / totalNetUnbalanceVolume * 100, 2)) : 0;
                    finalSegmentOwnershipCalculations.Add(ownershipCalculation);
                });
            }

            return finalSegmentOwnershipCalculations;
        }

        /// <inheritdoc/>
        public IEnumerable<SystemOwnershipCalculation> CalculatePercentageAndRegisterForSystem(
            IEnumerable<SystemOwnershipCalculation> resultOwnershipCalculation)
        {
            ArgumentValidators.ThrowIfNull(resultOwnershipCalculation, nameof(resultOwnershipCalculation));
            var finalSystemOwnershipCalculations = new List<SystemOwnershipCalculation>();
            if (resultOwnershipCalculation.Any())
            {
                var totalNetInputVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InputVolume.GetValueOrDefault()));
                var totalNetOutputVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.OutputVolume.GetValueOrDefault()));
                var totalNetInitialInventoryVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InitialInventoryVolume.GetValueOrDefault()));
                var totalNetFinalInventoryVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.FinalInventoryVolume.GetValueOrDefault()));
                var totalNetInterfaceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.InterfaceVolume.GetValueOrDefault()));
                var totalNetToleranceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.ToleranceVolume.GetValueOrDefault()));
                var totalNetUnidentifiedLossesVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.UnidentifiedLossesVolume.GetValueOrDefault()));
                var totalNetIdentifiedLossesVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.IdentifiedLossesVolume.GetValueOrDefault()));
                var totalNetUnbalanceVolume = resultOwnershipCalculation.Sum(x => Math.Abs(x.UnbalanceVolume.GetValueOrDefault()));

                resultOwnershipCalculation.ForEach(ownershipCalculation =>
                {
                    ownershipCalculation.InputPercentage = totalNetInputVolume != 0 ? Math.Abs(decimal.Round(ownershipCalculation.InputVolume.Value / totalNetInputVolume * 100, 2)) : 0;
                    ownershipCalculation.OutputPercentage = totalNetOutputVolume != 0 ?
                    Math.Abs(decimal.Round(ownershipCalculation.OutputVolume.Value / totalNetOutputVolume * 100, 2)) : 0;
                    ownershipCalculation.InitialInventoryPercentage = totalNetInitialInventoryVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.InitialInventoryVolume.Value / totalNetInitialInventoryVolume * 100, 2)) : 0;
                    ownershipCalculation.FinalInventoryPercentage = totalNetFinalInventoryVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.FinalInventoryVolume.Value / totalNetFinalInventoryVolume * 100, 2)) : 0;
                    ownershipCalculation.InterfacePercentage = totalNetInterfaceVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.InterfaceVolume.Value / totalNetInterfaceVolume * 100, 2)) : 0;
                    ownershipCalculation.TolerancePercentage = totalNetToleranceVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.ToleranceVolume.Value / totalNetToleranceVolume * 100, 2)) : 0;
                    ownershipCalculation.UnidentifiedLossesPercentage = totalNetUnidentifiedLossesVolume != 0 ?
                                                                      Math.Abs(decimal.Round(
                                                                      ownershipCalculation.UnidentifiedLossesVolume.Value / totalNetUnidentifiedLossesVolume * 100, 2)) : 0;
                    ownershipCalculation.IdentifiedLossesPercentage = totalNetIdentifiedLossesVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.IdentifiedLossesVolume.Value / totalNetIdentifiedLossesVolume * 100, 2)) : 0;
                    ownershipCalculation.UnbalancePercentage = totalNetUnbalanceVolume != 0 ?
                                                                      Math.Abs(decimal.Round(ownershipCalculation.UnbalanceVolume.Value / totalNetUnbalanceVolume * 100, 2)) : 0;
                    finalSystemOwnershipCalculations.Add(ownershipCalculation);
                });
            }

            return finalSystemOwnershipCalculations;
        }

        private static decimal? GetUnbalanceVolume(
            decimal initialInventory,
            decimal inputMovement,
            decimal outputMovement,
            decimal finalInventory,
            decimal identifiedLoss,
            decimal interfaceValue,
            decimal tolerance,
            decimal unidentified)
        {
            return initialInventory + inputMovement - outputMovement - finalInventory - identifiedLoss + interfaceValue + tolerance + unidentified;
        }

        private static decimal GetMovementOwnership(IEnumerable<Movement> movements, int measurementUnit, int ownerId)
        {
            var movementOwnerships = new List<Ownership>();
            movements.Where(x => x.MeasurementUnit == measurementUnit).ForEach(
                a => movementOwnerships.AddRange(a.Ownerships.Where(x => x.OwnerId == ownerId)));
            return movementOwnerships.Sum(o => o.OwnershipVolume);
        }

        private static decimal GetInventoryOwnership(IEnumerable<InventoryProduct> inventoryProducts, int measurementUnit, int ownerId)
        {
            var inventoryOwnerships = new List<Ownership>();
            inventoryProducts.Where(x => x.MeasurementUnit == measurementUnit).ForEach(
                a => inventoryOwnerships.AddRange(a.Ownerships.Where(x => x.OwnerId == ownerId)));
            return inventoryOwnerships.Sum(o => o.OwnershipVolume);
        }

        private static decimal GetOwnership(IEnumerable<Movement> movements, int ownerId, DateTime date, string productId, int measurementUnit, IEnumerable<int> nodeIds, VariableType variableType)
        {
            // Get all the interface movements based upon the operational date
            movements = movements.Where(x => x.VariableTypeId == variableType && x.OperationalDate.Date == date.Date);

            // Get all the movements whose destination node and destination product matches
            var destinationNodeMovements = movements.Where(x => nodeIds.Any(y => y == x.MovementDestination?.DestinationNodeId));
            var destinationMovementProducts = destinationNodeMovements.Where(x => x.MovementDestination?.DestinationProductId == productId && x.MeasurementUnit == measurementUnit);
            var destinationMovementsOwnerships = new List<Ownership>();
            destinationMovementProducts.ForEach(a => destinationMovementsOwnerships.AddRange(a.Ownerships));

            // Filtering the destination owners
            var filteredDestinationOwnerMovements = destinationMovementsOwnerships
                .Where(x => x.OwnerId == ownerId);

            // Sum of specific owner for all destination movements
            var destinationVolume = filteredDestinationOwnerMovements.Sum(o => o.OwnershipVolume);

            // Get all the movements whose source node and source product matches
            var sourceNodeMovements = movements.Where(x => nodeIds.Any(y => y == x.MovementSource?.SourceNodeId));
            var sourceMovementProduct = sourceNodeMovements.Where(x => x.MovementSource?.SourceProductId == productId && x.MeasurementUnit == measurementUnit);
            var sourceMovementsOwnerships = new List<Ownership>();
            sourceMovementProduct.ForEach(a => sourceMovementsOwnerships.AddRange(a.Ownerships));

            // Filtering the source owners
            var filteredSourceOwnerMovements = sourceMovementsOwnerships
                .Where(x => x.OwnerId == ownerId);

            // Sum of specific owner for all source movement
            var sourceVolume = filteredSourceOwnerMovements.Sum(o => o.OwnershipVolume);

            var volume = destinationVolume - sourceVolume;

            return volume;
        }

        private static decimal GetIdentifiedLossOwnership(IEnumerable<Movement> movements, IEnumerable<int> nodeIds, int ownerId, DateTime date, string productId, int measurementUnit)
        {
            // Get all the interface movements based upon the operational date
            movements = movements.Where(x => x.MessageTypeId == (int)MessageType.Loss && x.OperationalDate.Date == date.Date);

            // Get all the movements whose source product and source node matches
            var sourceMovements = movements.Where(x => nodeIds.Any(y => y == x.MovementSource?.SourceNodeId));
            var movementProducts = sourceMovements.Where(x => x.MovementSource?.SourceProductId == productId && x.MeasurementUnit == measurementUnit);
            var movementsOwnerships = new List<Ownership>();
            movementProducts.ForEach(a => movementsOwnerships.AddRange(a.Ownerships));

            // Filtering the owners
            var filteredOwnerMovements = movementsOwnerships
                .Where(x => x.OwnerId == ownerId);

            // Sum of specific owner for all movements
            var volume = filteredOwnerMovements.Sum(o => o.OwnershipVolume);

            return volume;
        }
    }
}
