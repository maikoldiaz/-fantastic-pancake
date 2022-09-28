// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceCalculationService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation;
    using Ecp.True.Processors.Balance.Operation.Input;
    using Ecp.True.Processors.Balance.Operation.Interfaces;

    /// <summary>
    /// Interface Calculator.
    /// </summary>
    public class InterfaceCalculationService : CalculationService
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly IInterfaceCalculator interfaceCalculator;

        /// <summary>
        /// The movement generators.
        /// </summary>
        private readonly IInterfaceMovementGenerator interfacemovementGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceCalculationService"/> class.
        /// </summary>
        /// <param name="interfaceCalculator">The interface calculator.</param>
        /// <param name="interfacemovementGenerator">The movement generators.</param>
        public InterfaceCalculationService(IInterfaceCalculator interfaceCalculator, IInterfaceMovementGenerator interfacemovementGenerator)
        {
            this.interfaceCalculator = interfaceCalculator;
            this.interfacemovementGenerator = interfacemovementGenerator;
        }

        /// <inheritdoc/>
        public override MovementCalculationStep Type => MovementCalculationStep.Interface;

        /// <inheritdoc/>
        public override async Task<CalculationOutput> ProcessAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(balanceInputInfo, nameof(balanceInputInfo));
            var (interfaceInfos, movements) = await this.ProcessInterfacesAsync(balanceInputInfo, ticket, balanceInputInfo.CalculationDate).ConfigureAwait(false);
            var unbalances = this.GetUnbalances(ticket, balanceInputInfo.CalculationDate, interfaceInfos, unitOfWork);
            return new CalculationOutput(movements, unbalances);
        }

        private static IEnumerable<InterfaceInfo> NeutralizeExtraDecimals(IEnumerable<InterfaceInfo> interfaces)
        {
            var interfaceList = interfaces.ToList();

            // Logic to neutralize extra decimals
            var sourceInterfaceValuesAbs = Math.Abs(interfaceList.Where(x => x.Interface > 0).Sum(y => y.Interface));
            var destinationInterfaceValuesAbs = Math.Abs(interfaceList.Where(x => x.Interface < 0).Sum(y => y.Interface));

            if (sourceInterfaceValuesAbs > destinationInterfaceValuesAbs)
            {
                var interfaceItem = interfaceList.FirstOrDefault(x => x.Interface < 0);
                if (interfaceItem != null)
                {
                    interfaceItem.Interface = interfaceItem.Interface - (sourceInterfaceValuesAbs - destinationInterfaceValuesAbs);
                }
            }

            if (destinationInterfaceValuesAbs > sourceInterfaceValuesAbs)
            {
                var interfaceItem = interfaceList.FirstOrDefault(x => x.Interface > 0);
                if (interfaceItem != null)
                {
                    interfaceItem.Interface = interfaceItem.Interface + (destinationInterfaceValuesAbs - sourceInterfaceValuesAbs);
                }
            }

            return interfaceList;
        }

        /// <summary>
        /// Adds the unbalance.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="calculationDate">The calculation date.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private IEnumerable<Unbalance> GetUnbalances(Ticket ticket, DateTime calculationDate, IEnumerable<InterfaceInfo> interfaces, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var unbalances = new List<Unbalance>();
            interfaces.ForEach(x =>
            {
                var unbalance = new Unbalance
                {
                    NodeId = x.NodeId,
                    ProductId = x.ProductId,
                    TicketId = ticket.TicketId,
                    InitialInventory = x.InitialInventory,
                    Inputs = x.Inputs,
                    Outputs = x.Outputs,
                    FinalInventory = x.FinalInventory,
                    IdentifiedLosses = x.IdentifiedLosses,
                    InterfaceUnbalance = x.Unbalance,
                    Interface = x.Interface,
                    CalculationDate = calculationDate,
                };
                unbalances.Add(unbalance);
            });

            return unbalances;
        }

        private async Task<(IEnumerable<InterfaceInfo> interfaceInfos, IEnumerable<Movement> movements)> ProcessInterfacesAsync(
           BalanceInputInfo balanceInputInfo, Ticket ticket, DateTime calculationDate)
        {
            var calculationInputs = balanceInputInfo.InterfaceNodes.Select(x => BuildCalculationInput(balanceInputInfo, x, ticket));
            var movements = new List<Movement>();
            var interfacesList = new List<InterfaceInfo>();
            foreach (var calculationInput in calculationInputs)
            {
                var interfaces = (IEnumerable<InterfaceInfo>)await this.interfaceCalculator.CalculateAsync(calculationInput).ConfigureAwait(false);
                interfaces = NeutralizeExtraDecimals(interfaces);

                var generatedMovements = await this.interfacemovementGenerator
                .GenerateAsync(new MovementInput(interfaces, ticket, calculationDate)).ConfigureAwait(false);
                movements.AddRange(generatedMovements);
                interfacesList.AddRange(interfaces);
            }

            return (interfacesList, movements);
        }
    }
}
