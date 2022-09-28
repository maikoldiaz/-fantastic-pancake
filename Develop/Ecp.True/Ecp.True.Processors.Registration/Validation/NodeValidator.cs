// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The Node Validator.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class NodeValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IRepositoryFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeValidator{T}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public NodeValidator(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            var errors = new List<ErrorInfo>();
            var nodeCount = await this.factory.CreateRepository<Node>().GetCountAsync(x => x.NodeId == inventoryProduct.NodeId).ConfigureAwait(false);

            if (nodeCount == 0)
            {
                errors.Add(new ErrorInfo(Registration.Constants.NodeValidationFailed));
                return new ValidationResult(errors);
            }

            if (!await this.ValidateInventoryProductMappingAsync(inventoryProduct.NodeId, inventoryProduct).ConfigureAwait(false))
            {
                errors.Add(new ErrorInfo(Registration.Constants.InventoryProductNotFound));
                return new ValidationResult(errors);
            }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var errorResponse = new List<ErrorInfo>();

            if (!await this.ValidateNodeStatusForManualMovAsync(movement).ConfigureAwait(false))
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.ErrorNodeApproved));
                return new ValidationResult(errorResponse);
            }

            if (!await this.ValidateNodeConnectionAsync(movement).ConfigureAwait(false))
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.NodeConnectionConnectionNotFound));
                return new ValidationResult(errorResponse);
            }

            if (!await this.ValidateProductMappingAsync(movement.MovementSource).ConfigureAwait(false))
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.MovementSourceProductNotFound));
                return new ValidationResult(errorResponse);
            }

            if (!await this.ValidateProductMappingAsync(movement.MovementDestination).ConfigureAwait(false))
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.MovementDestinationProductNotFound));
                return new ValidationResult(errorResponse);
            }

            if (ValidateIdentifiedLoss(movement))
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.MovementUnIdentifiedLossNodeRequired));
                return new ValidationResult(errorResponse);
            }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateEventAsync(Event eventObj)
        {
            ArgumentValidators.ThrowIfNull(eventObj, nameof(eventObj));

            var errors = new List<ErrorInfo>();
            var sourceNode = await this.factory.CreateRepository<Node>().GetByIdAsync(eventObj.SourceNodeId).ConfigureAwait(false);

            if (sourceNode == null)
            {
                errors.Add(new ErrorInfo(Registration.Constants.EventSourceNodeValidationFailed));
                return new ValidationResult(errors);
            }

            var destinationNode = await this.factory.CreateRepository<Node>().GetByIdAsync(eventObj.DestinationNodeId).ConfigureAwait(false);

            if (destinationNode == null)
            {
                errors.Add(new ErrorInfo(Registration.Constants.EventDestinationNodeValidationFailed));
                return new ValidationResult(errors);
            }

            if (!await this.ValidateNodeConnectionInEventAsync(eventObj).ConfigureAwait(false))
            {
                errors.Add(new ErrorInfo(Registration.Constants.NodeConnectionConnectionNotFound));
                return new ValidationResult(errors);
            }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateContractAsync(Contract contractObj)
        {
            ArgumentValidators.ThrowIfNull(contractObj, nameof(contractObj));

            var errors = new List<ErrorInfo>();
            if (contractObj.SourceSystem.Equals(SystemType.EXCEL.ToString("G"), StringComparison.Ordinal))
            {
                var sourceNode = await this.factory.CreateRepository<Node>().GetByIdAsync(contractObj.SourceNodeId).ConfigureAwait(false);

                if (sourceNode == null)
                {
                    errors.Add(new ErrorInfo(Registration.Constants.EventSourceNodeValidationFailed));
                    return new ValidationResult(errors);
                }
            }

            var destinationNode = await this.factory.CreateRepository<Node>().GetByIdAsync(contractObj.DestinationNodeId).ConfigureAwait(false);

            if (destinationNode == null)
            {
                errors.Add(new ErrorInfo(Registration.Constants.EventDestinationNodeValidationFailed));
                return new ValidationResult(errors);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validate Identified Loss.
        /// </summary>
        /// <param name="movement">Movement.</param>
        /// <returns>Identify Loss movement.</returns>
        private static bool ValidateIdentifiedLoss(Movement movement)
        {
            return
                movement.Classification.EqualsIgnoreCase("PerdidaIdentificada") &&
                movement.MovementSource == null &&
                movement.MovementDestination == null;
        }

        /// <summary>
        /// Validates Product Mappings asynchronously.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The result.
        /// </returns>
        private Task<bool> ValidateProductMappingAsync(MovementSource source)
        {
            if (source == null)
            {
                return Task.FromResult(true);
            }

            return this.DoValidateProductMappingAsync(source.SourceProductId, source.SourceNodeId);
        }

        /// <summary>
        /// Validates Product Mappings asynchronously.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <returns>
        /// The result.
        /// </returns>
        private Task<bool> ValidateProductMappingAsync(MovementDestination destination)
        {
            if (destination == null)
            {
                return Task.FromResult(true);
            }

            return this.DoValidateProductMappingAsync(destination.DestinationProductId, destination.DestinationNodeId);
        }

        private async Task<bool> DoValidateProductMappingAsync(string productId, int? nodeId)
        {
            if (!(!string.IsNullOrWhiteSpace(productId) && nodeId.HasValue))
            {
                return true;
            }

            var repo = this.factory.CreateRepository<NodeStorageLocation>();
            var nodeStorageLocationsCount = await repo.GetCountAsync(x => x.NodeId == nodeId && x.Products.Any(y => y.ProductId == productId)).ConfigureAwait(false);
            return nodeStorageLocationsCount > 0;
        }

        private async Task<bool> ValidateInventoryProductMappingAsync(int nodeId, InventoryProduct inventoryProduct)
        {
            var repo = this.factory.CreateRepository<NodeStorageLocation>();
            var nodeStorageLocationsCount = await repo.GetCountAsync(x => x.NodeId == nodeId && x.Products.Any(y => y.ProductId == inventoryProduct.ProductId)).ConfigureAwait(false);
            return nodeStorageLocationsCount > 0;
        }

        /// <summary>
        /// Validate Connection in nodes.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <returns>
        /// Connection exists between nodes or not.
        /// </returns>
        private async Task<bool> ValidateNodeConnectionAsync(Movement movement)
        {
            if (movement.MovementSource == null || movement.MovementDestination == null)
            {
                return true;
            }

            var nodeConnectionCount = await this.factory.CreateRepository<NodeConnection>()
                .GetCountAsync(
                        x => x.SourceNodeId == movement.MovementSource.SourceNodeId.GetValueOrDefault() &&
                        x.DestinationNodeId == movement.MovementDestination.DestinationNodeId.GetValueOrDefault() &&
                        !x.IsDeleted && x.IsActive == true)
                    .ConfigureAwait(false);

            return nodeConnectionCount > 0;
        }

        /// <summary>
        /// Validate Connection in nodes.
        /// </summary>
        /// <param name="eventObject">The eventObject.</param>
        /// <returns>
        /// Connection exists between nodes or not.
        /// </returns>
        private async Task<bool> ValidateNodeConnectionInEventAsync(Event eventObject)
        {
            var nodeConnection = await this.factory.CreateRepository<NodeConnection>()
                .SingleOrDefaultAsync(
                        x => x.SourceNodeId == eventObject.SourceNodeId &&
                        x.DestinationNodeId == eventObject.DestinationNodeId &&
                        !x.IsDeleted && x.IsActive.GetValueOrDefault())
                    .ConfigureAwait(false);

            return nodeConnection != null;
        }

        /// <summary>
        /// Validate status node of movement manual official.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <returns>
        /// Connection exists between nodes or not.
        /// </returns>
        private async Task<bool> ValidateNodeStatusForManualMovAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            switch (movement.SourceSystemId)
            {
                case Constants.ManualMovOfficial:
                    return await this.ValidateNodeStatusManMovOffAsync(movement).ConfigureAwait(false);
                case Constants.ManualInvOfficial:
                    return await this.ValidateNodeStatusManInvOffAsync(movement).ConfigureAwait(false);
                default:
                    return true;
            }
        }

        /// <summary>
        /// Validate status node of movement manual official.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <returns>
        /// Connection exists between nodes or not.
        /// </returns>
        private async Task<bool> ValidateNodeStatusManMovOffAsync(Movement movement)
        {
            var nodes = await this.GetNodeApprovedAsync(movement).ConfigureAwait(false);
            return !nodes.Any(node => (movement.Period.StartTime < node?.LastApprovedDate && movement.Period.EndTime > node?.LastApprovedDate));
        }

        /// <summary>
        /// Validate status node of inventory manual official.
        /// </summary>
        /// <param name="movement">The inventory.</param>
        /// <returns>
        /// Connection exists between nodes or not.
        /// </returns>
        private async Task<bool> ValidateNodeStatusManInvOffAsync(Movement movement)
        {
            if (movement.Period.StartTime == null)
            {
                return false;
            }

            var nodes = await this.GetNodeApprovedAsync(movement).ConfigureAwait(false);
            var currentPeriod = movement.Period.StartTime ?? DateTime.Now;

            return !nodes.Any(node => ((currentPeriod.Month == node.LastApprovedDate?.Month ||
                                        currentPeriod.AddMonths(1).Month == node.LastApprovedDate?.Month) &&
                                        currentPeriod.Year == node.LastApprovedDate?.Year));
        }

        /// <summary>
        /// Get Nodes Approved or Submit for approval.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <returns>
        /// Connection exists between nodes or not.
        /// </returns>
        private async Task<IEnumerable<DeltaNode>> GetNodeApprovedAsync(Movement movement)
        {
            var sourceNodeId = movement.MovementSource?.SourceNodeId;
            var destinationNodeId = movement.MovementDestination?.DestinationNodeId;
            var deltaRepository = this.factory.CreateRepository<DeltaNode>();
            var nodesDelta = await deltaRepository.GetAllAsync(x => (x.NodeId == sourceNodeId || x.NodeId == destinationNodeId) &&
                             (x.Status == OwnershipNodeStatusType.APPROVED || x.Status == OwnershipNodeStatusType.SUBMITFORAPPROVAL)).ConfigureAwait(false);
            var deltaHistoryRepository = this.factory.CreateRepository<DeltaNodeApprovalHistory>();
            var nodeHistory = await deltaHistoryRepository.GetAllAsync(x => nodesDelta.Any(n => n.NodeId == x.NodeId)).ConfigureAwait(false);
            return nodeHistory.OrderByDescending(o => o.CreatedDate).GroupBy(g => g.NodeId).Select(s => new DeltaNode { NodeId = s.First().NodeId, LastApprovedDate = s.First().CreatedDate });
        }
    }
}
