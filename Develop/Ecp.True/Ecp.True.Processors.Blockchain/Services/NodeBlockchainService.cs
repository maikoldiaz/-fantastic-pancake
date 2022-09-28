// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeBlockchainService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;

    /// <summary>
    /// The BlockchainNodeService.
    /// </summary>
    public class NodeBlockchainService : BlockchainService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public NodeBlockchainService(
            ITrueLogger<NodeBlockchainService> logger,
            IAzureClientFactory azureclientFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ITelemetry telemetry)
            : base(azureclientFactory, logger, telemetry)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override ServiceType Type => ServiceType.Node;

        /// <inheritdoc/>
        public override async Task RegisterAsync(int entityId)
        {
            var repository = this.unitOfWork.CreateRepository<OffchainNode>();
            var node = await repository.SingleOrDefaultAsync(a => a.Id == entityId && a.BlockchainStatus == StatusType.PROCESSING).ConfigureAwait(false);
            if (node == null)
            {
                return;
            }

            var result = await this.WriteToBlockchainAsync(node).ConfigureAwait(false);
            await this.QueueAsync(result, entityId, this.Type).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes the blockchain movement asynchronous.
        /// </summary>
        /// <param name="node">The node.</param>
        private async Task<OffchainMessage> WriteToBlockchainAsync(OffchainNode node)
        {
            ArgumentValidators.ThrowIfNull(node, nameof(node));
            var hasNode = await this.HasNodeAsync(node.NodeId).ConfigureAwait(false);
            if (node.NodeStateTypeId != (int)NodeState.CreatedNode && !hasNode)
            {
                // For backward compatibility in environments where blockchain is not deleted
                // we will create the node if it is not there.
                // This block will never execute in PROD
                await this.CreateNodeAsync(node).ConfigureAwait(false);
            }

            var parameters = new Dictionary<string, object>
            {
                { "nodeId", node.NodeId.ToString(CultureInfo.InvariantCulture) },
                { "name", node.Name },
                { "state", node.NodeStateTypeId },
                { "lastUpdateDate", node.LastUpdateDate.Ticks },
                { "isActive", node.IsActive },
            };

            return await this.WriteAsync("Register", parameters, ContractNames.NodesFactory).ConfigureAwait(false);
        }

        private Task<bool> HasNodeAsync(int nodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "nodeId", nodeId.ToString(CultureInfo.InvariantCulture) },
            };

            return this.GetBlockchainDataAsync<bool>("HasNode", parameters, ContractNames.NodesFactory);
        }

        private Task CreateNodeAsync(OffchainNode node)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "nodeId", node.NodeId.ToString(CultureInfo.InvariantCulture) },
                    { "name", node.Name },
                    { "state", (int)NodeState.CreatedNode },
                    { "lastUpdateDate", node.LastUpdateDate.Ticks },
                    { "isActive", node.IsActive },
                };

            return this.WriteAsync("Register", parameters, ContractNames.NodesFactory);
        }
    }
}
