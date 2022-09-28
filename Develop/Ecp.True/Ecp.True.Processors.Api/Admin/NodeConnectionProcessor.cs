// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProcessor.cs" company="Microsoft">
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;
    using GraphicalNetwork = Ecp.True.Entities.Query.GraphicalNetwork;
    using GraphicalNode = Ecp.True.Entities.Query.GraphicalNode;
    using GraphicalNodeConnection = Ecp.True.Entities.Query.GraphicalNodeConnection;

    /// <summary>
    /// The node connection processor.
    /// </summary>
    public class NodeConnectionProcessor : ProcessorBase, INodeConnectionProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionProcessor"/> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public NodeConnectionProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory repositoryFactory,
            IAzureClientFactory azureClientFactory)
            : base(repositoryFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.azureClientFactory = azureClientFactory;
        }

        /// <inheritdoc/>
        public async Task CreateNodeConnectionAsync(NodeConnection nodeConnection)
        {
            ArgumentValidators.ThrowIfNull(nodeConnection, nameof(nodeConnection));
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeConnection>();

            var offchainConnection = await this.CreateNodeConnectionAsync(nodeConnection, repository, unitOfWork).ConfigureAwait(false);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.SendMessageToServiceBusAsync(offchainConnection.Id, offchainConnection.NodeConnectionId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NodeConnectionInfo>> CreateNodeConnectionListAsync(
            IEnumerable<NodeConnection> nodeConnectionList)
        {
            var nodeConnections = nodeConnectionList.ThrowIfNullOrEmpty(nameof(nodeConnectionList));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeConnection>();

            var info = new List<NodeConnectionInfo>();

            foreach (var nodeConnection in nodeConnections)
            {
                await this.AddNodeConnectionAsync(nodeConnection, info, repository, unitOfWork).ConfigureAwait(false);
            }

            info.ForEach(i =>
            {
                if (i.OffchainConnection != null)
                {
                    this.SendMessageToServiceBusAsync(i.OffchainConnection.Id, i.OffchainConnection.NodeConnectionId)
                        .ConfigureAwait(false);
                }
            });

            return info;
        }

        /// <inheritdoc/>
        public async Task UpdateNodeConnectionAsync(NodeConnection nodeConnection)
        {
            ArgumentValidators.ThrowIfNull(nodeConnection, nameof(nodeConnection));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeConnection>();
            var existing = await DoGetNodeConnectionAsync(
                nodeConnection.SourceNodeId,
                nodeConnection.DestinationNodeId,
                repository,
                "DestinationNode",
                "SourceNode").ConfigureAwait(false);
            if (existing == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeConnectionDoesNotExists);
            }

            existing.CopyFrom(nodeConnection);
            repository.Update(existing);

            var offchainConnection = this.BuildOffchainNodeConnection(existing);
            unitOfWork.CreateRepository<OffchainNodeConnection>().Insert(offchainConnection);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.SendMessageToServiceBusAsync(offchainConnection.Id, offchainConnection.NodeConnectionId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteNodeConnectionAsync(int sourceNodeId, int destinationNodeId)
        {
            var hasMovement = await this.RepositoryFactory
                .MovementRepository
                .HasActiveMovementForConnectionAsync(sourceNodeId, destinationNodeId)
                .ConfigureAwait(false);
            if (hasMovement)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeConnectionDeleteConflict);
            }

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeConnection>();
            var existing = await DoGetNodeConnectionAsync(sourceNodeId, destinationNodeId, repository).ConfigureAwait(false);
            if (existing == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeConnectionDoesNotExists);
            }

            existing.IsDeleted = true;
            repository.Update(existing);

            var offchainConnection = this.BuildOffchainNodeConnection(existing);
            unitOfWork.CreateRepository<OffchainNodeConnection>().Insert(offchainConnection);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.SendMessageToServiceBusAsync(offchainConnection.Id, offchainConnection.NodeConnectionId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<NodeConnection> GetNodeConnectionAsync(int sourceNodeId, int destinationNodeId)
        {
            var repository = this.CreateRepository<NodeConnection>();
            return DoGetNodeConnectionAsync(sourceNodeId, destinationNodeId, repository, nameof(NodeConnection.SourceNode), nameof(NodeConnection.DestinationNode));
        }

        /// <inheritdoc/>
        public async Task UpdateNodeConnectionProductAsync(NodeConnectionProduct product)
        {
            ArgumentValidators.ThrowIfNull(product, nameof(product));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeConnectionProduct>();
            var existingProduct = await repository.GetByIdAsync(product.NodeConnectionProductId).ConfigureAwait(false);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeConnectionProductDoesNotExists);
            }

            existingProduct.CopyFrom(product);
            repository.Update(existingProduct);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task SaveNodeConnectionProductOwnersAsync(UpdateNodeConnectionProductOwners productOwners)
        {
            ArgumentValidators.ThrowIfNull(productOwners, nameof(productOwners));

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<NodeConnectionProduct>();
            var existingProduct = await repository.SingleOrDefaultAsync(p => p.NodeConnectionProductId == productOwners.ProductId, "Owners").ConfigureAwait(false);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeConnectionProductDoesNotExists);
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
        public async Task<IEnumerable<Node>> GetDestinationNodesBySourceNodeIdAsync(int sourceNodeId)
        {
            var nodeConnectionRepository = this.CreateRepository<NodeConnection>();
            var result = await nodeConnectionRepository.GetAllAsync(
                x => x.SourceNodeId == sourceNodeId && !x.IsDeleted,
                nameof(NodeConnection.DestinationNode)).ConfigureAwait(false);

            return result.Select(x => x.DestinationNode);
        }

        /// <inheritdoc/>
        public async Task<GraphicalNetwork> GetGraphicalNetworkAsync(int elementId, int nodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ElementId", elementId },
                { "@NodeId", nodeId },
            };
            var graphicalNodes = await this.CreateRepository<GraphicalNode>().ExecuteQueryAsync(
                Repositories.Constants.GraphicalNodeProcedureName, parameters).ConfigureAwait(false);

            parameters["@NodeId"] = 0;
            var graphicalNodeConnections = await this.CreateRepository<GraphicalNodeConnection>().ExecuteQueryAsync(
                Repositories.Constants.GraphicalNodeConnectionProcedureName, parameters).ConfigureAwait(false);

            UpdateNodes(graphicalNodes, graphicalNodeConnections);

            // All nodes selected
            if (nodeId == 0)
            {
                var listOfNodes = graphicalNodes.Select(x => x.NodeId).ToList();

                // Both source node id and destination node id should present in the nodes collections
                var filteredgraphicalNodeConnections = graphicalNodeConnections.Where(x => listOfNodes.Contains(x.SourceNodeId) && listOfNodes.Contains(x.DestinationNodeId)).ToList();
                return new GraphicalNetwork(graphicalNodes, filteredgraphicalNodeConnections);
            }

            // Single node selected then node connections will be null
            return new GraphicalNetwork(graphicalNodes, null);
        }

        /// <inheritdoc/>
        public async Task<GraphicalNetwork> GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(int sourceNodeId)
        {
            var result = await this.GetGraphicalNetworkDetailsAsync(
                "@SourceNodeId",
                sourceNodeId,
                Repositories.Constants.GraphicalDestinationNodesDetailsProcedureName,
                Repositories.Constants.GraphicalDestinationNodeConnectionsDetailsProcedureName).ConfigureAwait(false);

            // Getting the self connection
            var selfNodeConnection = result.GraphicalNodeConnections.Where(
                x => x.SourceNodeId == sourceNodeId && x.DestinationNodeId == sourceNodeId).AsEnumerable();

            // Removing those node connections where current node id is destination node id
            var nodeConnections = result.GraphicalNodeConnections.Where(
                x => x.DestinationNodeId != sourceNodeId).AsEnumerable();

            result.GraphicalNodeConnections = nodeConnections.Union(selfNodeConnection);
            return result;
        }

        /// <inheritdoc/>
        public async Task<GraphicalNetwork> GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(int destinationNodeId)
        {
            var result = await this.GetGraphicalNetworkDetailsAsync(
                "@DestinationNodeId",
                destinationNodeId,
                Repositories.Constants.GraphicalSourceNodesDetailsProcedureName,
                Repositories.Constants.GraphicalSourceNodeConnectionsDetailsProcedureName).ConfigureAwait(false);

            // Getting the self connection
            var selfNodeConnection = result.GraphicalNodeConnections.Where(
                x => x.SourceNodeId == destinationNodeId && x.DestinationNodeId == destinationNodeId).AsEnumerable();

            // Removing those node connections where current node id is source node id
            var nodeConnections = result.GraphicalNodeConnections.Where(
                x => x.SourceNodeId != destinationNodeId).AsEnumerable();

            result.GraphicalNodeConnections = nodeConnections.Union(selfNodeConnection);
            return result;
        }

        /// <inheritdoc/>
        public Task<IEnumerable<NodeConnectionProductRule>> GetProductRulesAsync()
        {
            return this.CreateRepository<NodeConnectionProductRule>().GetAllAsync(x => x.IsActive);
        }

        private static void UpdateNodes(IEnumerable<GraphicalNode> graphicalNodes, IEnumerable<GraphicalNodeConnection> graphicalNodeConnections)
        {
            var inputConnections = graphicalNodeConnections.GroupBy(a => a.DestinationNodeId).ToDictionary(x => x.Key, x => x.Count());
            var outputConnections = graphicalNodeConnections.GroupBy(a => a.SourceNodeId).ToDictionary(x => x.Key, x => x.Count());

            foreach (var item in graphicalNodes)
            {
                var key = item.NodeId;
                item.InputConnections = inputConnections.ContainsKey(key) ? inputConnections[item.NodeId] : 0;
                item.OutputConnections = outputConnections.ContainsKey(key) ? outputConnections[item.NodeId] : 0;
                if (!string.IsNullOrWhiteSpace(item.NodeTypeIcon) && item.NodeTypeIcon.Contains("fill:", StringComparison.OrdinalIgnoreCase))
                {
                    var index = item.NodeTypeIcon.IndexOf("fill:", StringComparison.OrdinalIgnoreCase);
                    var fillString = item.NodeTypeIcon.Substring(index, 12);
                    item.SegmentColor = string.IsNullOrWhiteSpace(item.SegmentColor) ? Entities.Constants.GreenColorCode : item.SegmentColor;
                    item.NodeTypeIcon = item.NodeTypeIcon.Replace(fillString, $"fill:{item.SegmentColor}", StringComparison.OrdinalIgnoreCase).ReplaceTitleTag();
                }
            }
        }

        private static Task<NodeConnection> DoGetNodeConnectionAsync(int? sourceId, int? destId, IRepository<NodeConnection> repository, params string[] include)
        {
            return repository.SingleOrDefaultAsync(x => x.SourceNodeId == sourceId && x.DestinationNodeId == destId && !x.IsDeleted, include);
        }

        private async Task<GraphicalNetwork> GetGraphicalNetworkDetailsAsync(string parameterType, int nodeId, string nodeProcedureName, string nodeConnectionsProcedureName)
        {
            var parameters = new Dictionary<string, object>
            {
                { parameterType, nodeId },
            };
            var graphicalNodes = await this.CreateRepository<GraphicalNode>().ExecuteQueryAsync(
                nodeProcedureName, parameters).ConfigureAwait(false);

            var graphicalNodeConnections = await this.CreateRepository<GraphicalNodeConnection>().ExecuteQueryAsync(
                nodeConnectionsProcedureName, parameters).ConfigureAwait(false);

            UpdateNodes(graphicalNodes, graphicalNodeConnections);

            var listoffNodes = graphicalNodes.Select(x => x.NodeId).ToList();

            // Both source node id and destination node id should present in the nodes collections
            var filteredgraphicalNodeConnections = graphicalNodeConnections.Where(
                x => listoffNodes.Contains(x.SourceNodeId) && listoffNodes.Contains(x.DestinationNodeId)).AsEnumerable();
            return new GraphicalNetwork(graphicalNodes, filteredgraphicalNodeConnections);
        }

        private async Task<OffchainNodeConnection> CreateNodeConnectionAsync(NodeConnection nodeConnection, IRepository<NodeConnection> repository, IUnitOfWork unitOfWork)
        {
            var existing = await DoGetNodeConnectionAsync(
                nodeConnection.SourceNodeId,
                nodeConnection.DestinationNodeId,
                repository,
                "DestinationNode",
                "SourceNode").ConfigureAwait(false);

            OffchainNodeConnection offchainConnection;
            if (existing == null)
            {
                // Add source node products to node connection products
                var products = await this.GetNodeProductsAsync(nodeConnection.SourceNodeId.Value).ConfigureAwait(false);
                nodeConnection.AddProducts(products.ToArray());

                repository.Insert(nodeConnection);

                offchainConnection = this.BuildOffchainNodeConnection(nodeConnection);
                nodeConnection.OffchainNodeConnections.Add(offchainConnection);
            }
            else if (existing.IsActive == true)
            {
                throw new KeyNotFoundException(Entities.Constants.NodeConnectionAlreadyExists);
            }
            else
            {
                existing.IsActive = true;
                repository.Update(existing);

                offchainConnection = this.BuildOffchainNodeConnection(existing);
                unitOfWork.CreateRepository<OffchainNodeConnection>().Insert(offchainConnection);
            }

            return offchainConnection;
        }

        private async Task<IEnumerable<NodeConnectionProduct>> GetNodeProductsAsync(int sourceNodeId)
        {
            var repo = this.CreateRepository<NodeStorageLocation>();
            var storageLocations = await repo.GetAllAsync(s => s.NodeId == sourceNodeId && s.IsActive.Value, "Products").ConfigureAwait(false);

            return storageLocations
                .SelectMany(s => s.Products)
                .Select(n => new NodeConnectionProduct { ProductId = n.ProductId })
                .Distinct(ExpressionEqualityComparer.Create<NodeConnectionProduct, string>(p => p.ProductId));
        }

        /// <summary>
        /// Sends the message to service bus asynchronous.
        /// </summary>
        /// <param name="nodeConnectionId">The node connection identifier.</param>
        private Task SendMessageToServiceBusAsync(int id, int nodeConnectionId)
        {
            var adminQueueClient = this.azureClientFactory.GetQueueClient(QueueConstants.BlockchainNodeConnectionQueue);
            return adminQueueClient.QueueSessionMessageAsync(id, nodeConnectionId.ToString(CultureInfo.InvariantCulture));
        }

        private OffchainNodeConnection BuildOffchainNodeConnection(NodeConnection connection)
        {
            return new OffchainNodeConnection
            {
                NodeConnectionId = connection.NodeConnectionId,
                IsActive = connection.IsActive.GetValueOrDefault(),
                SourceNodeId = connection.SourceNodeId.GetValueOrDefault(),
                DestinationNodeId = connection.DestinationNodeId.GetValueOrDefault(),
                IsDeleted = connection.IsDeleted,
                BlockchainStatus = StatusType.PROCESSING,
            };
        }

        private async Task<IList<NodeConnectionInfo>> AddNodeConnectionAsync(
            NodeConnection nodeConnection, IList<NodeConnectionInfo> info, IRepository<NodeConnection> repository, IUnitOfWork unitOfWork)
        {
            var nodeConnectionInfo = new NodeConnectionInfo();
            try
            {
                var offchainConnection = await this
                    .CreateNodeConnectionAsync(nodeConnection, repository, unitOfWork).ConfigureAwait(false);

                nodeConnectionInfo.OffchainConnection = offchainConnection;
                nodeConnectionInfo.Status = EntityInfoCreationStatus.Created;
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (KeyNotFoundException)
            {
                nodeConnectionInfo.Status = EntityInfoCreationStatus.Duplicated;
            }

            nodeConnectionInfo.NodeConnection = nodeConnection;
            info.Add(nodeConnectionInfo);

            return info;
        }
    }
}