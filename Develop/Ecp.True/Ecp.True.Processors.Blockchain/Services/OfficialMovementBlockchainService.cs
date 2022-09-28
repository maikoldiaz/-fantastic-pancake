// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialMovementBlockchainService.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The official Blockchain Movement service.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Registration.Interfaces.IBlockchainMovementService" />
    public class OfficialMovementBlockchainService : BlockchainService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OfficialMovementBlockchainService> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialMovementBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public OfficialMovementBlockchainService(
            ITrueLogger<OfficialMovementBlockchainService> logger,
            IAzureClientFactory azureclientFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ITelemetry telemetry)
            : base(azureclientFactory, logger, telemetry)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override ServiceType Type => ServiceType.OfficialMovement;

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="entityId">The deltaNode identifier.</param>
        /// <returns>Returns the completed task.</returns>
        public override async Task RegisterAsync(int entityId)
        {
            this.logger.LogInformation($"DeltaNodeApproval Movement transaction {entityId}.", $"{entityId}");
            var parameters = new Dictionary<string, object>
            {
               { "@DeltaNodeId", entityId },
            };
            //// DB Insertion
            var repository = this.unitOfWork.CreateRepository<OfficialBlockChainMovement>();
            var movementTransactionIds = await repository.ExecuteQueryAsync(Repositories.Constants.UpdateOfficialMovementPendingApproval, parameters).ConfigureAwait(false);
            //// Block chain insertion
            var tasks = movementTransactionIds.Select(officialBlockchainMovement =>
            this.SendToBlockchainAsync(officialBlockchainMovement.MovementTransactionId, QueueConstants.BlockchainMovementQueue));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
