// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeProcessor.cs" company="Microsoft">
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
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;
    using EfCore.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     The Node Processor.
    /// </summary>
    public class NodeProcessor : ProcessorBase, INodeProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The node order manager.
        /// </summary>
        private readonly INodeOrderManager nodeOrderManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeProcessor" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The Unit of Work Factory.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="repositoryFactory">The factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="nodeOrderManager">The node order manager.</param>
        public NodeProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IBusinessContext businessContext,
            IRepositoryFactory repositoryFactory,
            IAzureClientFactory azureClientFactory,
            IConfigurationHandler configurationHandler,
            INodeOrderManager nodeOrderManager)
            : base(repositoryFactory)
        {
            this.businessContext = businessContext;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.azureClientFactory = azureClientFactory;
            this.configurationHandler = configurationHandler;
            this.nodeOrderManager = nodeOrderManager;
        }

        /// <summary>
        /// Saves the node asynchronous.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public async Task<int> SaveNodeAsync(Node node)
        {
            ArgumentValidators.ThrowIfNull(node, nameof(node));
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Node>();
            ClearOwners(node);

            await this.nodeOrderManager.TryReorderAsync(node, repository).ConfigureAwait(false);
            repository.Insert(node);

            var nodeTagRepository = unitOfWork.CreateRepository<NodeTag>();
            await this.InsertNodeTagsAsync(node, nodeTagRepository).ConfigureAwait(false);

            node.OffchainNodes.Add(this.BuildOffchainNode(node, NodeState.CreatedNode));

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.SendMessageToServiceBusAsync(node.OffchainNodes.First().Id, node.OffchainNodes.First().NodeId).ConfigureAwait(false);

            return node.NodeId;
        }

        /// <summary>Updates the node asynchronous.</summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public async Task UpdateNodeAsync(Node node)
        {
            ArgumentValidators.ThrowIfNull(node, nameof(node));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Node>();

            var existingNode = await repository.SingleOrDefaultAsync(
                x => x.NodeId == node.NodeId,
                "NodeStorageLocations.Products.Owners",
                "NodeStorageLocations.Products.StorageLocationProductVariables").ConfigureAwait(false);
            if (existingNode == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeStorageLocationProductDoesNotExists);
            }

            await this.nodeOrderManager.TryReorderAsync(node, repository).ConfigureAwait(false);
            existingNode.CopyFrom(node);

            await UpdateNodeConnectionProductsAsync(node, unitOfWork).ConfigureAwait(false);
            repository.Update(existingNode);

            var offchainRepo = unitOfWork.CreateRepository<OffchainNode>();
            var offchainNode = this.BuildOffchainNode(existingNode, NodeState.UpdatedNode);
            offchainRepo.Insert(offchainNode);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.SendMessageToServiceBusAsync(offchainNode.Id, offchainNode.NodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the node by identifier asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>
        /// The node.
        /// </returns>
        public Task<Node> GetNodeByIdAsync(int nodeId)
        {
            return this.CreateRepository<Node>().GetByIdAsync(nodeId);
        }

        /// <summary>
        /// Gets the node by name asynchronous.
        /// </summary>
        /// <param name="nodeName">The node name.</param>
        /// <returns>
        /// Returns [true] is the name does not exists otherwise [false].
        /// </returns>
        public Task<Node> GetNodeByNameAsync(string nodeName)
        {
            return this.CreateRepository<Node>().SingleOrDefaultAsync(a => a.Name == nodeName, "LogisticCenter", "LogisticCenter.StorageLocations");
        }

        /// <summary>
        /// Gets the storage location by name asynchronous.
        /// </summary>
        /// <param name="storageName">The storage name.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>
        /// Returns [true] is the name does not exists otherwise [false].
        /// </returns>
        public Task<NodeStorageLocation> GetStorageLocationByNameAsync(string storageName, int nodeId)
        {
            return this.CreateRepository<NodeStorageLocation>().SingleOrDefaultAsync(a => a.Name == storageName && a.NodeId == nodeId);
        }

        /// <summary>Updates the node properties asynchronous.</summary>
        /// <param name="node">The node.</param>
        /// <exception cref="KeyNotFoundException">Key Not Found Exception.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateNodePropertiesAsync(Node node)
        {
            ArgumentValidators.ThrowIfNull(node, nameof(node));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Node>();
            var existingNode = await repository.GetByIdAsync(node.NodeId).ConfigureAwait(false);
            if (existingNode == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeDoesNotExists);
            }

            existingNode.RowVersion = node.RowVersion;
            existingNode.AcceptableBalancePercentage = node.AcceptableBalancePercentage ?? existingNode.AcceptableBalancePercentage;
            existingNode.ControlLimit = node.ControlLimit ?? existingNode.ControlLimit;

            repository.Update(existingNode);

            var offchainRepo = unitOfWork.CreateRepository<OffchainNode>();
            var offchainNode = this.BuildOffchainNode(existingNode, NodeState.UpdatedNode);
            offchainRepo.Insert(offchainNode);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.SendMessageToServiceBusAsync(offchainNode.Id, offchainNode.NodeId).ConfigureAwait(false);
        }

        /// <summary>Updates the storage location product asynchronous.</summary>
        /// <param name="storageLocationProduct">The storage location product.</param>
        /// <exception cref="KeyNotFoundException">KeyNotFoundException.</exception>
        /// <returns>The status.</returns>
        public async Task UpdateStorageLocationProductAsync(StorageLocationProduct storageLocationProduct)
        {
            ArgumentValidators.ThrowIfNull(storageLocationProduct, nameof(storageLocationProduct));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<StorageLocationProduct>();
            var existingProduct = await repository.GetByIdAsync(storageLocationProduct.StorageLocationProductId).ConfigureAwait(false);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeStorageLocationProductDoesNotExists);
            }

            storageLocationProduct.ClearOwners();
            storageLocationProduct.ClearStorageLocationProductVariables();
            existingProduct.CopyFrom(storageLocationProduct);
            existingProduct.RowVersion = storageLocationProduct.RowVersion;

            repository.Update(existingProduct);
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the storage location product owners.
        /// </summary>
        /// <param name="productOwners">The product owners.</param>
        /// <exception cref="KeyNotFoundException">The KeyNotFound Exception.</exception>
        /// <exception cref="InvalidDataException">The InvalidData Exception.</exception>
        /// <returns> The Task.</returns>
        public async Task UpdateStorageLocationProductOwnersAsync(UpdateStorageLocationProductOwners productOwners)
        {
            ArgumentValidators.ThrowIfNull(productOwners, nameof(productOwners));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<StorageLocationProduct>();
            var existingProduct = await repository.SingleOrDefaultAsync(p => p.StorageLocationProductId == productOwners.ProductId, "Owners").ConfigureAwait(false);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeStorageLocationProductDoesNotExists);
            }

            existingProduct.RowVersion = productOwners.RowVersion;
            existingProduct.Owners.Merge(productOwners.Owners, o => o.OwnerId);
            if (existingProduct.TotalOwnerShipValue != 100)
            {
                throw new InvalidDataException(Entities.Constants.ProductOwnerShipTotalValueValidation);
            }

            repository.Update(existingProduct);
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Node>> FilterNodesAsync(NodesFilter nodesFilter)
        {
            var query = this.RepositoryFactory.NodeRepository.FilterNodes(nodesFilter);
            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<Node> GetNodeWithSameOrderAsync(int nodeId, int segmentId, int order)
        {
            return this.RepositoryFactory.NodeRepository.GetNodeWithSameOrderAsync(nodeId, segmentId, order);
        }

        /// <inheritdoc/>
        public async Task<CategoryElement> GetNodeTypeAsync(int nodeId)
        {
            return await this.RepositoryFactory.NodeRepository.GetNodeTypeForNodeAsync(nodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the nodes with the provided strategy.
        /// </summary>
        /// <param name="request">The node ownership rule bulk update request.</param>
        /// <returns>The task.</returns>
        public async Task UpdateNodeOwnershipRulesAsync(OwnershipRuleBulkUpdateRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            if (await ValidateConcurrencyAsync(request, unitOfWork).ConfigureAwait(false))
            {
                throw new DBConcurrencyException(Entities.Constants.RowConcurrencyConflict);
            }

            var parameters = new Dictionary<string, object>
                             {
                                { "@BulkUploadIds", request.Ids.Select(r => r.Id).ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
                                { "@VariableTypeIds", request.VariableTypeIds.ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
                                { "@ruleType", request.OwnershipRuleType.ToString() },
                                { "@ruleId", request.OwnershipRuleId },
                                { "@lastmodifiedBy", this.businessContext.UserId },
                             };
            await this.CreateRepository<OwnershipRuleBulkUpdateRequestDto>().ExecuteAsync(Repositories.Constants.BulkUpdateRulesProcedureName, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<NodeOwnershipRule>> GetNodeOwnershipRulesAsync()
        {
            return this.CreateRepository<NodeOwnershipRule>().GetAllAsync(x => x.IsActive);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<NodeProductRule>> GetNodeProductRulesAsync()
        {
            return this.CreateRepository<NodeProductRule>().GetAllAsync(x => x.IsActive);
        }

        /// <summary>
        /// Builds the node connection product.
        /// </summary>
        /// <param name="storageLocationProduct">The storage location product.</param>
        /// <param name="nodeConnection">The node connection.</param>
        /// <returns>The node connection product.</returns>
        private static NodeConnectionProduct BuildNodeConnectionProduct(StorageLocationProduct storageLocationProduct, NodeConnection nodeConnection)
        {
            var existingProduct = nodeConnection.Products.SingleOrDefault(p => p.ProductId == storageLocationProduct.ProductId);
            return new NodeConnectionProduct
            {
                NodeConnectionId = nodeConnection.NodeConnectionId,
                ProductId = storageLocationProduct.ProductId,
                RowVersion = existingProduct?.RowVersion,
                Priority = existingProduct?.Priority,
            };
        }

        /// <summary>
        /// Updates the node connection products asynchronous.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private static async Task UpdateNodeConnectionProductsAsync(Node node, IUnitOfWork unitOfWork)
        {
            var nodeConnectionsRepository = unitOfWork.CreateRepository<NodeConnection>();
            var nodeConnections = await nodeConnectionsRepository.GetAllAsync(x => x.SourceNodeId == node.NodeId, "Products", "Products.Owners").ConfigureAwait(false);
            foreach (var nodeConnection in nodeConnections)
            {
                var newNodeConnection = new NodeConnection
                {
                    RowVersion = nodeConnection.RowVersion,
                };

                var products = node.NodeStorageLocations
                                    .SelectMany(x => x.Products.Select(y => BuildNodeConnectionProduct(y, nodeConnection)))
                                    .Distinct(ExpressionEqualityComparer.Create<NodeConnectionProduct, string>(p => p.ProductId));
                newNodeConnection.AddProducts(products.ToArray());

                nodeConnection.CopyFrom(newNodeConnection);
                nodeConnectionsRepository.Update(nodeConnection);
            }
        }

        /// <summary>
        /// Clears Owners.
        /// </summary>
        /// <param name="node">The node.</param>
        private static void ClearOwners(Node node)
        {
            var products = node.NodeStorageLocations.SelectMany(x => x.Products);
            foreach (var product in products)
            {
                product.ClearOwners();
            }
        }

        /// <summary>
        /// Creating the repositories and updating the IDs.
        /// </summary>
        /// <param name="request">The node strategy bulk update request.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The task.</returns>
        private static async Task<bool> ValidateConcurrencyAsync(OwnershipRuleBulkUpdateRequest request, IUnitOfWork unitOfWork)
        {
            var requests = request.Ids.ToDictionary(r => r.Id, s => s.RowVersion);
            var store = new Dictionary<int, string>();
            if (request.OwnershipRuleType == OwnershipRuleType.StorageLocationProduct || request.OwnershipRuleType == OwnershipRuleType.StorageLocationProductVariable)
            {
                var storageLocationRepository = unitOfWork.CreateRepository<StorageLocationProduct>();
                var storageLocationItems = await storageLocationRepository.GetAllAsync(x => requests.Keys.Contains(x.StorageLocationProductId)).ConfigureAwait(false);
                storageLocationItems.ForEach(item => store.Add(item.StorageLocationProductId, item.RowVersion.ToBase64()));
            }
            else if (request.OwnershipRuleType == OwnershipRuleType.Node)
            {
                var nodeRepository = unitOfWork.CreateRepository<Node>();
                var nodeItems = await nodeRepository.GetAllAsync(x => requests.Keys.Contains(x.NodeId)).ConfigureAwait(false);
                nodeItems.ForEach(item => store.Add(item.NodeId, item.RowVersion.ToBase64()));
            }
            else
            {
                var nodeProductRepository = unitOfWork.CreateRepository<NodeConnectionProduct>();
                var nodeProductItems = await nodeProductRepository.GetAllAsync(x => requests.Keys.Contains(x.NodeConnectionProductId)).ConfigureAwait(false);
                nodeProductItems.ForEach(item => store.Add(item.NodeConnectionProductId, item.RowVersion.ToBase64()));
            }

            return request.Ids.ToDictionary(r => r.Id, s => s.RowVersion).Except(store).Any();
        }

        /// <summary>
        /// Sends the message to service bus asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The task.</returns>
        private Task SendMessageToServiceBusAsync(int id, int nodeId)
        {
            var adminQueueClient = this.azureClientFactory.GetQueueClient(QueueConstants.BlockchainNodeQueue);
            return adminQueueClient.QueueSessionMessageAsync(id, nodeId.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Inserts the node tag properties.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nodeTagRepository">The node tag repository.</param>
        /// <exception cref="KeyNotFoundException">The KeyNotFoundException.</exception>
        private async Task InsertNodeTagsAsync(Node node, IRepository<NodeTag> nodeTagRepository)
        {
            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);
            var period = systemConfig.NodeTagGracePeriodInMonths.GetValueOrDefault();

            var startDate = DateTime.UtcNow.ToTrue().Date.AddMonths(-period);
            var nodeTypeNodeTag = new NodeTag
            {
                NodeId = node.NodeId,
                ElementId = (int)node.NodeTypeId,
                StartDate = startDate,
                EndDate = DateTime.MaxValue.Date,
            };

            nodeTagRepository.Insert(nodeTypeNodeTag);

            var segmentNodeTag = new NodeTag
            {
                NodeId = node.NodeId,
                ElementId = (int)node.SegmentId,
                StartDate = startDate,
                EndDate = DateTime.MaxValue.Date,
            };

            nodeTagRepository.Insert(segmentNodeTag);

            var operatorNodeTag = new NodeTag
            {
                NodeId = node.NodeId,
                ElementId = (int)node.OperatorId,
                StartDate = startDate,
                EndDate = DateTime.MaxValue.Date,
            };

            nodeTagRepository.Insert(operatorNodeTag);
        }

        private OffchainNode BuildOffchainNode(Node node, NodeState state)
        {
            return new OffchainNode
            {
                NodeId = node.NodeId,
                NodeStateTypeId = (int)state,
                IsActive = node.IsActive.GetValueOrDefault(),
                LastUpdateDate = DateTime.UtcNow.ToTrue(),
                Name = node.Name,
                BlockchainStatus = StatusType.PROCESSING,
            };
        }
    }
}