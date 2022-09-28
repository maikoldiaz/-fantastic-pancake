// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceBlockchainService.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;

    /// <summary>
    /// The BlockchainNodeProductCalculationService.
    /// </summary>
    public class UnbalanceBlockchainService : BlockchainService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public UnbalanceBlockchainService(
               ITrueLogger<UnbalanceBlockchainService> logger,
               IAzureClientFactory azureclientFactory,
               IUnitOfWorkFactory unitOfWorkFactory,
               ITelemetry telemetry)
               : base(azureclientFactory, logger, telemetry)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override ServiceType Type => ServiceType.Unbalance;

        /// <summary>
        ///  Registers the asynchronous.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The task.</returns>
        /// <inheritdoc />
        public override async Task RegisterAsync(int entityId)
        {
            var repo = this.unitOfWork.CreateRepository<Unbalance>();
            var unbalance = await repo.SingleOrDefaultAsync(u => u.UnbalanceId == entityId && u.BlockchainStatus == StatusType.PROCESSING, "Node").ConfigureAwait(false);
            if (unbalance == null)
            {
                return;
            }

            var result = await this.WriteToBlockchainAsync(unbalance).ConfigureAwait(false);
            await this.QueueAsync(result, entityId, this.Type).ConfigureAwait(false);
        }

        private Task<OffchainMessage> WriteToBlockchainAsync(Unbalance unbalance)
        {
            ArgumentValidators.ThrowIfNull(unbalance, nameof(unbalance));
            var key = $"{unbalance.CalculationDate.Date.ToString("d", CultureInfo.InvariantCulture)}-{unbalance.NodeId}-{unbalance.ProductId}";

            var values = string.Join(
                                    ",",
                                    unbalance.InitialInventory.ToBlockChainNumberString(),
                                    unbalance.FinalInventory.ToBlockChainNumberString(),
                                    unbalance.Inputs.ToBlockChainNumberString(),
                                    unbalance.Outputs.ToBlockChainNumberString(),
                                    unbalance.IdentifiedLosses.ToBlockChainNumberString(),
                                    unbalance.Interface.ToBlockChainNumberString(),
                                    unbalance.Tolerance.ToBlockChainNumberString(),
                                    unbalance.UnidentifiedLosses.ToBlockChainNumberString(),
                                    unbalance.UnbalanceAmount.ToBlockChainNumberString());

            var parameters = new Dictionary<string, object>
                {
                    { "dateNodeProductKey", key },
                    { "calculatedValues", values },
                    { "ticketId", unbalance.TicketId },
                };

            return this.WriteAsync("Register", parameters, ContractNames.UnbalancesFactory);
        }
    }
}
