// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Api.Services.Interfaces;
    using Ecp.True.Processors.Core;

    /// <summary>
    /// The node cost center processor.
    /// </summary>
    public class NodeCostCenterProcessor : ProcessorBase, INodeCostCenterProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly INodeCostCenterValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCostCenterProcessor"/> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="validator">The NodeCostCenter validator.</param>
        public NodeCostCenterProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory repositoryFactory,
            INodeCostCenterValidator validator)
        : base(repositoryFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NodeCostCenterInfo>> CreateNodeCostCentersAsync(IEnumerable<NodeCostCenter> nodeCostCenters)
        {
            var nodeCostCenterList = nodeCostCenters.ThrowIfNullOrEmpty(nameof(nodeCostCenters));

            nodeCostCenterList = nodeCostCenterList.Distinct().ToList();

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeCostCenter>();
            var results = new List<NodeCostCenterInfo>();

            foreach (NodeCostCenter nodeCostCenter in nodeCostCenterList)
            {
                ArgumentValidators.ThrowIfNull(nodeCostCenter, nameof(nodeCostCenter));
                NodeCostCenter existing = await GetExistingNodeCostCenterAsync(repository, nodeCostCenter).ConfigureAwait(false);

                if (existing is null)
                {
                    nodeCostCenter.IsActive = true;
                    repository.Insert(nodeCostCenter);
                    results.Add(new NodeCostCenterInfo { NodeCostCenter = nodeCostCenter, Status = NodeCostCenterInfo.CreationStatus.Created });
                }
                else
                {
                    results.Add(new NodeCostCenterInfo { NodeCostCenter = existing, Status = NodeCostCenterInfo.CreationStatus.Duplicated });
                }
            }

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            return results;
        }

        /// <inheritdoc/>
        public async Task<NodeCostCenter> UpdateNodeCostCenterAsync(NodeCostCenter nodeCostCenter)
        {
            ArgumentValidators.ThrowIfNull(nodeCostCenter, nameof(nodeCostCenter));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeCostCenter>();

            var existing = await GetUniqueNodeCostCenterAsync(repository, nodeCostCenter).ConfigureAwait(false);

            if (nodeCostCenter.Equals(existing))
            {
                string statusMessage = existing.IsActive.GetValueOrDefault() ? Entities.Constants.NodeCostCenterAlreadyExistsActive : Entities.Constants.NodeCostCenterAlreadyExistsInactive;

                throw new KeyNotFoundException(statusMessage);
            }
            else if (existing is null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeCostCenterDoesNotExists);
            }
            else
            {
                existing.CopyFrom(nodeCostCenter);
                repository.Update(existing);
            }

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            return existing;
        }

        /// <inheritdoc/>
        public async Task DeleteNodeCostCenterAsync(int nodeCostCenterId)
        {
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeCostCenter>();

            var existing = await repository.SingleOrDefaultAsync(
                n => n.NodeCostCenterId == nodeCostCenterId
                && !n.IsDeleted).ConfigureAwait(false);

            if (existing is null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeCostCenterDoesNotExists);
            }

            var validation = await this.validator.ValidateForDeletionAsync(existing).ConfigureAwait(false);
            if (!validation.IsSuccess)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeCostCenterHasMovements);
            }
            else
            {
                existing.IsActive = false;
                existing.IsDeleted = true;
                repository.Update(existing);
            }

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private static async Task<NodeCostCenter> GetUniqueNodeCostCenterAsync(IRepository<NodeCostCenter> repository, NodeCostCenter nodeCostCenter)
        {
            int sourceNodeId = nodeCostCenter.SourceNodeId.GetValueOrDefault();
            int? destinationNodeId = nodeCostCenter.DestinationNodeId;
            int movementTypeId = nodeCostCenter.MovementTypeId.GetValueOrDefault();
            int nodeCostCenterId = nodeCostCenter.NodeCostCenterId;

            return await repository.SingleOrDefaultAsync(
                x => x.NodeCostCenterId == nodeCostCenterId
                && x.SourceNodeId == sourceNodeId
                && x.DestinationNodeId == destinationNodeId
                && x.MovementTypeId == movementTypeId).ConfigureAwait(false);
        }

        private static async Task<NodeCostCenter> GetExistingNodeCostCenterAsync(IRepository<NodeCostCenter> repository, NodeCostCenter nodeCostCenter)
        {
            int sourceNodeId = nodeCostCenter.SourceNodeId.GetValueOrDefault();
            int? destinationNodeId = nodeCostCenter.DestinationNodeId;
            int movementTypeId = nodeCostCenter.MovementTypeId.GetValueOrDefault();

            return await repository.SingleOrDefaultAsync(
                x => x.SourceNodeId == sourceNodeId
                && x.DestinationNodeId == destinationNodeId
                && x.MovementTypeId == movementTypeId
                && !x.IsDeleted).ConfigureAwait(false);
        }
    }
}