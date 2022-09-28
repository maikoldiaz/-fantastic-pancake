// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationProcessor.cs" company="Microsoft">
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
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Transformation Processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.ITransformationProcessor" />
    public class TransformationProcessor : ProcessorBase, ITransformationProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationProcessor" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unitOfWork Factory.</param>
        /// <param name="repositoryFactory">The repository Factory.</param>
        /// <param name="businessContext">The business Context.</param>
        public TransformationProcessor(IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory)
             : base(repositoryFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <inheritdoc/>
        public async Task CreateTransformationAsync(TransformationDto transformation)
        {
            ArgumentValidators.ThrowIfNull(transformation, nameof(transformation));
            var existsTransformation = await this.ExistsTransformationAsync(transformation).ConfigureAwait(false);
            if (existsTransformation.Any())
            {
                throw new InvalidDataException(EntityConstants.SourceTransformationDuplicate);
            }

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<Transformation>();

                repository.Insert(transformation.ToEntity());
                await UpdateVersionAsync(unitOfWork).ConfigureAwait(false);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task DeleteTransformationAsync(DeleteTransformation transformation)
        {
            ArgumentValidators.ThrowIfNull(transformation, nameof(transformation));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<Transformation>();
                var existingTransformation = await DoGetTransformationAsync(transformation.TransformationId, repository).ConfigureAwait(false);
                if (existingTransformation == null)
                {
                    throw new KeyNotFoundException(EntityConstants.TransformationDoesNotExists);
                }

                existingTransformation.IsDeleted = true;
                existingTransformation.RowVersion = transformation.RowVersion;
                repository.Update(existingTransformation);
                await UpdateVersionAsync(unitOfWork).ConfigureAwait(false);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Transformation>> ExistsTransformationAsync(TransformationDto transformationDto)
        {
            ArgumentValidators.ThrowIfNull(transformationDto, nameof(transformationDto));
            var repository = this.RepositoryFactory.CreateRepository<Transformation>();
            if (transformationDto.MessageTypeId == MessageType.Movement)
            {
                return await repository.GetAllAsync(x => x.MessageTypeId == (int)transformationDto.MessageTypeId && x.IsDeleted == false
                                        && x.OriginMeasurementId == transformationDto.Origin.MeasurementUnitId
                                        && ValidateMovementTransformationExist(transformationDto, x)).ConfigureAwait(false);
            }

            return await repository.GetAllAsync(x => x.MessageTypeId == (int)transformationDto.MessageTypeId && x.IsDeleted == false
                                    && ValidateInventoryTransformationExist(transformationDto, x)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task UpdateTransformationAsync(TransformationDto transformation)
        {
            ArgumentValidators.ThrowIfNull(transformation, nameof(transformation));

            var existsTransformation = await this.ExistsTransformationAsync(transformation).ConfigureAwait(false);
            if (existsTransformation.Any(a => a.TransformationId != transformation.TransformationId))
            {
                throw new InvalidDataException(EntityConstants.SourceTransformationDuplicate);
            }

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<Transformation>();
                var existing = await repository.GetByIdAsync(transformation.TransformationId).ConfigureAwait(false);
                if (existing == null)
                {
                    throw new KeyNotFoundException(EntityConstants.TransformationDoesNotExists);
                }

                existing.CopyFrom(transformation.ToEntity());
                repository.Update(existing);

                await UpdateVersionAsync(unitOfWork).ConfigureAwait(false);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TransformationInfo> GetTransformationInfoAsync(int transformationId)
        {
            var repository = this.RepositoryFactory.CreateRepository<Transformation>();
            var transformation = await DoGetTransformationAsync(transformationId, repository).ConfigureAwait(false);
            if (transformation == null)
            {
                throw new KeyNotFoundException(EntityConstants.TransformationDoesNotExists);
            }

            return await this.BuildTransformationInfoAsync(transformation).ConfigureAwait(false);
        }

        private static async Task UpdateVersionAsync(IUnitOfWork unitOfWork)
        {
            var versionRepository = unitOfWork.CreateRepository<Entities.Admin.Version>();
            var version = await versionRepository.SingleOrDefaultAsync(x => x.Type == nameof(Transformation)).ConfigureAwait(false);
            if (version == null)
            {
                var firstVersion = new Version { Type = nameof(Transformation), Number = 1 };
                versionRepository.Insert(firstVersion);
            }
            else
            {
                version.Number += 1;
                versionRepository.Update(version);
            }
        }

        /// <summary>
        /// Validates the transformation.
        /// </summary>
        /// <param name="transformationId">The transformation identifier.</param>
        /// <param name="repository">The node connetion repository.</param>
        /// <param name="include">The include properties.</param>
        /// <returns>
        /// Returns node connection status.
        /// </returns>
        private static Task<Transformation> DoGetTransformationAsync(int transformationId, IRepository<Transformation> repository, params string[] include)
        {
            return repository.SingleOrDefaultAsync(x => x.TransformationId == transformationId, include);
        }

        private static bool ValidateInventoryTransformationExist(TransformationDto transformationDto, Transformation x)
        {
            return x.OriginSourceNodeId == transformationDto.Origin.SourceNodeId
                            && x.OriginSourceProductId == transformationDto.Origin.SourceProductId
                            && x.OriginMeasurementId == transformationDto.Origin.MeasurementUnitId;
        }

        private static bool ValidateMovementTransformationExist(TransformationDto transformationDto, Transformation x)
        {
            return x.OriginSourceNodeId == transformationDto.Origin.SourceNodeId
                           && x.OriginDestinationNodeId == transformationDto.Origin.DestinationNodeId
                           && x.OriginSourceProductId == transformationDto.Origin.SourceProductId
                           && x.OriginDestinationProductId == transformationDto.Origin.DestinationProductId;
        }

        private async Task<TransformationInfo> BuildTransformationInfoAsync(Transformation transformation)
        {
            ArgumentValidators.ThrowIfNull(transformation, nameof(transformation));

            // Get Nodes
            var originDestinationNodes = transformation.IsMovement ? this.GetDestinationNodesAsync(transformation.OriginSourceNodeId) : Task.FromResult(new List<Node>().AsEnumerable());
            var destinationDestinationNodes = transformation.IsMovement ? this.GetDestinationNodesAsync(transformation.DestinationSourceNodeId) : Task.FromResult(new List<Node>().AsEnumerable());

            // Get Products
            var originSourceProducts = this.GetProductsAsync(transformation.OriginSourceNodeId);
            var destinationSourceProducts = this.GetProductsAsync(transformation.DestinationSourceNodeId);
            var originDestinationProducts = transformation.IsMovement ? this.GetProductsAsync(
                transformation.OriginDestinationNodeId.Value) : Task.FromResult(new List<StorageLocationProduct>().AsEnumerable());
            var destinationDestinationProducts = transformation.IsMovement ? this.GetProductsAsync(transformation.DestinationDestinationNodeId.Value) :
                Task.FromResult(new List<StorageLocationProduct>().AsEnumerable());

            await Task.WhenAll(
                    originDestinationNodes,
                    originSourceProducts,
                    originDestinationProducts,
                    destinationDestinationNodes,
                    destinationSourceProducts,
                    destinationDestinationProducts).ConfigureAwait(false);

            return new TransformationInfo
            {
                Origin = new NodeProductInfo
                {
                    DestinationNodes = await originDestinationNodes.ConfigureAwait(false),
                    SourceProducts = await originSourceProducts.ConfigureAwait(false),
                    DestinationProducts = await originDestinationProducts.ConfigureAwait(false),
                },
                Destination = new NodeProductInfo
                {
                    DestinationNodes = await destinationDestinationNodes.ConfigureAwait(false),
                    SourceProducts = await destinationSourceProducts.ConfigureAwait(false),
                    DestinationProducts = await destinationDestinationProducts.ConfigureAwait(false),
                },
                RowVersion = transformation.RowVersion,
            };
        }

        private async Task<IEnumerable<Node>> GetDestinationNodesAsync(int sourceNodeId)
        {
            var repo = this.CreateRepository<NodeConnection>();
            var result = await repo.GetAllAsync(x => x.SourceNodeId == sourceNodeId && !x.IsDeleted, nameof(NodeConnection.DestinationNode)).ConfigureAwait(false);
            return result.Select(x => new Node { NodeId = x.DestinationNodeId.Value, Name = x.DestinationNode.Name });
        }

        private async Task<IEnumerable<StorageLocationProduct>> GetProductsAsync(int nodeId)
        {
            var repo = this.CreateRepository<NodeStorageLocation>();
            var result = await repo.GetAllAsync(x => x.NodeId == nodeId && x.IsActive.Value, "Products", "Products.Product").ConfigureAwait(false);
            return result.SelectMany(x => x.Products).Select(p => new StorageLocationProduct { Product = new Product { Name = p.Product.Name }, ProductId = p.ProductId });
        }
    }
}