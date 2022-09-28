// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// The balance service.
    /// </summary>
    public class BalanceService : IBalanceService
    {
        /// <summary>
        /// The calculation services.
        /// </summary>
        private readonly IEnumerable<ICalculationService> calculationServices;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceService" /> class.
        /// </summary>
        /// <param name="calculationServices">The calculators.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        public BalanceService(
            IEnumerable<ICalculationService> calculationServices,
            IRepositoryFactory repositoryFactory)
        {
            this.calculationServices = calculationServices;
            this.repositoryFactory = repositoryFactory;
        }

        /// <inheritdoc/>
        public async Task<CalculationOutput> ProcessStepAsync(UnbalanceRequest getUnbalances, MovementCalculationStep step, IUnitOfWork unitOfWork, ITrueLogger logger)
        {
            ArgumentValidators.ThrowIfNull(getUnbalances, nameof(getUnbalances));
            ArgumentValidators.ThrowIfNull(logger, nameof(logger));
            logger.LogInformation($"Balance calculation at {step} calculation step, for ticket with {getUnbalances.Ticket.TicketId}.", $"{getUnbalances.Ticket.TicketId}");
            var balanceInput = await this.GetBalanceInputAsync(getUnbalances).ConfigureAwait(false);
            var calculationOutput = await this.ProcessCalculationAsync(balanceInput, getUnbalances.Ticket, unitOfWork, step).ConfigureAwait(false);
            return calculationOutput;
        }

        /// <summary>
        /// Gets the balance input asynchronous.
        /// </summary>
        /// <param name="getUnbalances">The get unbalances.</param>
        /// <returns>The balance input.</returns>
        public async Task<BalanceInput> GetBalanceInputAsync(UnbalanceRequest getUnbalances)
        {
            ArgumentValidators.ThrowIfNull(getUnbalances, nameof(getUnbalances));
            var balanceInput = new BalanceInput();
            var movementTransactionIds = getUnbalances.TransferPoints.Select(x => x.MovementTransactionId);
            var interfaceParameters = new Dictionary<string, object>
            {
                { "@catElementId", getUnbalances.Ticket.CategoryElementId },
                { "@startDate", getUnbalances.Ticket.StartDate.Date },
                { "@endDate", getUnbalances.Ticket.EndDate.Date },
                { "@isInterface", true },
            };

            await this.GetDataFromRepositoryAsync<NodeInput>(
            (nodes) => balanceInput.InterfaceNodes = nodes,
            Repositories.Constants.GetNodesProcedureName,
            interfaceParameters).ConfigureAwait(false);

            var parameters = new Dictionary<string, object>
            {
                { "@catElementId", getUnbalances.Ticket.CategoryElementId },
                { "@startDate", getUnbalances.Ticket.StartDate.Date },
                { "@endDate", getUnbalances.Ticket.EndDate.Date },
                { "@isInterface", false },
            };

            await this.GetDataFromRepositoryAsync<NodeInput>(
            (nodes) => balanceInput.Nodes = nodes,
            Repositories.Constants.GetNodesProcedureName,
            parameters).ConfigureAwait(false);

            var nodeIds = balanceInput.Nodes.Select(x => x.NodeId);
            var movementParameters = new Dictionary<string, object>
            {
                { "@NodeListType", nodeIds.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
                { "@StartDate", getUnbalances.Ticket.StartDate.Date },
                { "@EndDate", getUnbalances.Ticket.EndDate.Date },
                { "@TicketId", getUnbalances.Ticket.TicketId },
                { "@MovementTransactionIds", movementTransactionIds.ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
            };

            var inventoryParameters = new Dictionary<string, object>
            {
                { "@NodeListType", nodeIds.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
                { "@StartDate", getUnbalances.Ticket.StartDate.AddDays(-1).Date },
                { "@EndDate", getUnbalances.Ticket.EndDate.Date },
                { "@TicketId", getUnbalances.Ticket.TicketId },
                { "@FirstTimeNodes", getUnbalances.FirstTimeNodes.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
            };

            await this.GetDataFromRepositoryAsync<MovementCalculationInput>(
                (mov) => balanceInput.Movements = mov,
                Repositories.Constants.GetMovementsProcedureName,
                movementParameters).ConfigureAwait(false);

            await this.GetDataFromRepositoryAsync<InventoryInput>(
                (inv) => balanceInput.Inventories = inv,
                Repositories.Constants.GetInventoriesProcedureName,
                inventoryParameters).ConfigureAwait(false);

            return balanceInput;
        }

        /// <inheritdoc/>
        public async Task<CalculationOutput> ProcessCalculationAsync(
            BalanceInput balanceInput,
            Ticket ticket,
            IUnitOfWork unitOfWork,
            MovementCalculationStep step)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            ArgumentValidators.ThrowIfNull(balanceInput, nameof(balanceInput));
            var calculationOutputs = new List<CalculationOutput>();
            var calculationDate = ticket.StartDate.Date;
            while (calculationDate <= ticket.EndDate.Date)
            {
                var balanceInputInfo = new BalanceInputInfo
                {
                    CalculationDate = calculationDate,
                    Nodes = balanceInput.Nodes.Where(x => x.CalculationDate == calculationDate),
                    InterfaceNodes = balanceInput.InterfaceNodes.Where(x => x.CalculationDate == calculationDate),
                    InitialInventories = balanceInput.Inventories.Where(x => x.InventoryDate.Date == calculationDate.AddDays(-1).Date),
                    FinalInventories = balanceInput.Inventories.Where(x => x.InventoryDate.Date == calculationDate),
                    Movements = balanceInput.Movements.Where(x => x.OperationalDate.Date == calculationDate),
                };

                if (balanceInputInfo.Nodes.Any())
                {
                    var calculationOutput = await this.calculationServices.Single(x => x.Type == step).ProcessAsync(
                    balanceInputInfo, ticket, unitOfWork).ConfigureAwait(false);
                    calculationOutputs.Add(calculationOutput);
                }

                calculationDate = calculationDate.AddDays(1);
            }

            var movements = calculationOutputs.SelectMany(x => x.Movements).ToList();
            var unbalances = calculationOutputs.SelectMany(x => x.Unbalances).ToList();
            var unbalanceList = calculationOutputs.SelectMany(x => x.UnbalanceList).ToList();
            return new CalculationOutput(movements, unbalances, unbalanceList);
        }

        private async Task GetDataFromRepositoryAsync<T>(Action<IEnumerable<T>> setter, string storedProcedureName, IDictionary<string, object> parameters)
     where T : class, IEntity
        {
            setter(await this.repositoryFactory.CreateRepository<T>()
                            .ExecuteQueryAsync(storedProcedureName, parameters).ConfigureAwait(false));
        }
    }
}
