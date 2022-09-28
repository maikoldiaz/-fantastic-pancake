// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRelationshipProcessor.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// Node relationship processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.INodeRelationshipProcessor" />
    public class NodeRelationshipProcessor : ProcessorBase, INodeRelationshipProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRelationshipProcessor" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        public NodeRelationshipProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Creates the node relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns> The task.</returns>
        public async Task CreateNodeRelationshipAsync(OperativeNodeRelationship nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));
            var existsRelationship = await this.OperativeTransferRelationshipExistsAsync(nodeRelationship).ConfigureAwait(false);
            if (existsRelationship != null)
            {
                throw new InvalidDataException(EntityConstants.OperativeTransferRelationshipExists);
            }

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OperativeNodeRelationship>();
                nodeRelationship.IsDeleted = false;
                nodeRelationship.LoadDate = DateTime.Now;
                repository.Insert(nodeRelationship);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the node relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationshipId">The node relationship identifier.</param>
        /// <returns>
        /// Node relationship entity.
        /// </returns>
        public async Task<OperativeNodeRelationship> GetNodeRelationshipAsync(int nodeRelationshipId)
        {
            var repository = this.CreateRepository<OperativeNodeRelationship>();
            return await repository.SingleOrDefaultAsync(x => x.OperativeNodeRelationshipId == nodeRelationshipId && !x.IsDeleted.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the node connection asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns> The task.</returns>
        public async Task UpdateNodeRelationshipAsync(OperativeNodeRelationship nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OperativeNodeRelationship>();
                var existingNodeRelationship = await repository.GetByIdAsync(nodeRelationship.OperativeNodeRelationshipId).ConfigureAwait(false);
                if (existingNodeRelationship == null)
                {
                    throw new KeyNotFoundException(EntityConstants.NodeRelationshipNotFound);
                }

                existingNodeRelationship.CopyFrom(nodeRelationship);
                repository.Update(existingNodeRelationship);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationshipWithOwnership">The node relationship with ownership.</param>
        /// <returns> The task.</returns>
        public async Task CreateLogisticTransferRelationshipAsync(OperativeNodeRelationshipWithOwnership nodeRelationshipWithOwnership)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationshipWithOwnership, nameof(nodeRelationshipWithOwnership));
            var existsRelationship = await this.LogisticTransferRelationshipExistsAsync(nodeRelationshipWithOwnership).ConfigureAwait(false);
            if (existsRelationship != null)
            {
                throw new InvalidDataException(EntityConstants.OperativeTransferRelationshipExists);
            }

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OperativeNodeRelationshipWithOwnership>();
                nodeRelationshipWithOwnership.IsDeleted = false;
                nodeRelationshipWithOwnership.LoadDate = DateTime.Now;
                nodeRelationshipWithOwnership.SourceSystem = EntityConstants.TrueMessage;
                repository.Insert(nodeRelationshipWithOwnership);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Deletes the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <exception cref="KeyNotFoundException">The exception.</exception>
        /// <returns> The task.</returns>
        public async Task DeleteLogisticTransferRelationshipAsync(OperativeNodeRelationshipWithOwnership nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OperativeNodeRelationshipWithOwnership>();
                var existingNodeRelationship = await repository.GetByIdAsync(nodeRelationship.OperativeNodeRelationshipWithOwnershipId).ConfigureAwait(false);
                if (existingNodeRelationship == null)
                {
                    throw new KeyNotFoundException(EntityConstants.NodeRelationshipNotFound);
                }

                existingNodeRelationship.CopyFrom(nodeRelationship);
                repository.Update(existingNodeRelationship);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Validates the node relationship.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>Validation results.</returns>
        public async Task<OperativeNodeRelationship> OperativeTransferRelationshipExistsAsync(OperativeNodeRelationship nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));

            var repository = this.CreateRepository<OperativeNodeRelationship>();
            return await repository.FirstOrDefaultAsync(r => ProductsDetailsAreSame(nodeRelationship, r) &&
                                                                  AllTypesAreSame(nodeRelationship, r) &&
                                                                  TransferPointAndNodesAreSame(nodeRelationship, r)
                                                                  && r.IsDeleted == false).ConfigureAwait(false);
        }

        /// <summary>
        /// Logistics the transfer point exists async.
        /// </summary>
        /// <param name="nodeRelationshipWithOwnership">The node relationship with ownership.</param>
        /// <returns>The Task.</returns>
        public async Task<OperativeNodeRelationshipWithOwnership> LogisticTransferRelationshipExistsAsync(OperativeNodeRelationshipWithOwnership nodeRelationshipWithOwnership)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationshipWithOwnership, nameof(nodeRelationshipWithOwnership));

            var repository = this.CreateRepository<OperativeNodeRelationshipWithOwnership>();
            return await repository.FirstOrDefaultAsync(r => ProductsAndTransferPointAreSame(r, nodeRelationshipWithOwnership) &&
                                                                                     r.LogisticDestinationCenter == nodeRelationshipWithOwnership.LogisticDestinationCenter &&
                                                                                     r.LogisticSourceCenter == nodeRelationshipWithOwnership.LogisticSourceCenter &&
                                                                                     !r.IsDeleted).ConfigureAwait(false);
        }

        private static bool ProductsAndTransferPointAreSame(OperativeNodeRelationshipWithOwnership nodeRelationshipWithOwnership, OperativeNodeRelationshipWithOwnership relationshipWithOwnership)
        {
            if (relationshipWithOwnership.SourceProduct == nodeRelationshipWithOwnership.SourceProduct &&
                relationshipWithOwnership.DestinationProduct == nodeRelationshipWithOwnership.DestinationProduct &&
                relationshipWithOwnership.TransferPoint == nodeRelationshipWithOwnership.TransferPoint)
            {
                return true;
            }

            return false;
        }

        private static bool AllTypesAreSame(OperativeNodeRelationship nodeRelationship, OperativeNodeRelationship relationship)
        {
            if (relationship.SourceNodeType == nodeRelationship.SourceNodeType &&
                    relationship.DestinationNodeType == nodeRelationship.DestinationNodeType &&
                     relationship.MovementType == nodeRelationship.MovementType)
            {
                return true;
            }

            return false;
        }

        private static bool ProductsDetailsAreSame(OperativeNodeRelationship nodeRelationship, OperativeNodeRelationship relationship)
        {
            if (nodeRelationship.SourceProduct == relationship.SourceProduct &&
                nodeRelationship.SourceProductType == relationship.SourceProductType)
            {
                return true;
            }

            return false;
        }

        private static bool TransferPointAndNodesAreSame(OperativeNodeRelationship nodeRelationship, OperativeNodeRelationship relationship)
        {
            if (relationship.TransferPoint == nodeRelationship.TransferPoint &&
                relationship.SourceNode == nodeRelationship.SourceNode &&
                relationship.DestinationNode == nodeRelationship.DestinationNode)
            {
                return true;
            }

            return false;
        }
    }
}
