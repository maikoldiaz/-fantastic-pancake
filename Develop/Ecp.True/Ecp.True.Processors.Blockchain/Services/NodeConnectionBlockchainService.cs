// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionBlockchainService.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;

    /// <summary>
    /// The BlockchainNodeConnectionService.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Blockchain.Registration.BlockchainService" />
    public class NodeConnectionBlockchainService : BlockchainService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public NodeConnectionBlockchainService(
            ITrueLogger<NodeConnectionBlockchainService> logger,
            IAzureClientFactory azureclientFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ITelemetry telemetry)
            : base(azureclientFactory, logger, telemetry)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override ServiceType Type => ServiceType.NodeConnection;

        /// <inheritdoc/>
        public override async Task RegisterAsync(int entityId)
        {
            var repository = this.unitOfWork.CreateRepository<OffchainNodeConnection>();
            var connection = await repository.SingleOrDefaultAsync(a => a.Id == entityId && a.BlockchainStatus == StatusType.PROCESSING).ConfigureAwait(false);

            if (connection == null)
            {
                return;
            }

            var result = await this.WriteToBlockchainAsync(connection).ConfigureAwait(false);
            await this.QueueAsync(result, entityId, this.Type).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes the blockchain movement asynchronous.
        /// </summary>
        /// <param name="connection">The node connection.</param>
        /// <returns>
        /// Returns a task.
        /// </returns>
        private Task<OffchainMessage> WriteToBlockchainAsync(OffchainNodeConnection connection)
        {
            ArgumentValidators.ThrowIfNull(connection, nameof(connection));
            var parameters = new Dictionary<string, object>
                {
                    { "nodeConnectionId", connection.NodeConnectionId.ToString(CultureInfo.InvariantCulture) },
                    { "isActive", connection.IsActive },
                    { "isDeleted", connection.IsDeleted },
                    { "sourceNodeId", connection.SourceNodeId },
                    { "destinationNodeId", connection.DestinationNodeId },
                };

            return this.WriteAsync("Register", parameters, ContractNames.NodeConnectionsFactory);
        }
    }
}
