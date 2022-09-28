// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceToleranceMovementGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation.Input;
    using Ecp.True.Processors.Balance.Operation.Interfaces;

    /// <summary>
    /// Balance Tolerance Movement Generator.
    /// </summary>
    public class BalanceToleranceMovementGenerator : MovementGenerator, IBalanceToleranceMovementGenerator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<BalanceToleranceMovementGenerator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceToleranceMovementGenerator" /> class.
        /// </summary>
        /// <param name="balanceCommunicator">The balance communicator.</param>
        /// <param name="logger">The logger.</param>
        public BalanceToleranceMovementGenerator(ITrueLogger<BalanceToleranceMovementGenerator> logger)
        {
            this.logger = logger;
        }

        /// <summary>Generates the asynchronous.</summary>
        /// <param name="movementInput">The movementInput.</param>
        /// <returns>The task.</returns>
        public override Task<IEnumerable<Movement>> GenerateAsync(MovementInput movementInput)
        {
            ArgumentValidators.ThrowIfNull(movementInput, nameof(movementInput));
            var balanceTolerances = (IEnumerable<BalanceTolerance>)movementInput.Outputs;

            this.logger.LogInformation($"Generating balance tolerance movements for ticket {movementInput.Ticket.TicketId}", $"{movementInput.Ticket.TicketId}");
            var movements = new List<Movement>();
            foreach (var item in balanceTolerances.Where(x => x.Tolerance != 0.0M))
            {
                var movement = this.DoGenerate(item, movementInput.Ticket, movementInput.CalculationDate);
                movements.Add(movement);
            }

            return Task.FromResult(movements.AsEnumerable());
        }

        /// <summary>Do Generate.</summary>
        /// <returns>movement object.</returns>
        /// <param name="balanceTolerance">The balanceTolerance.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="calculationDate">The calculationDate.</param>
        protected Movement DoGenerate(BalanceTolerance balanceTolerance, Ticket ticket, DateTime calculationDate)
        {
            ArgumentValidators.ThrowIfNull(balanceTolerance, nameof(balanceTolerance));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var movement = balanceTolerance.Unbalance > 0 ? this.BuildMovement(ticket, balanceTolerance, null, calculationDate) : this.BuildMovement(ticket, null, balanceTolerance, calculationDate);
            movement.NetStandardVolume = Math.Abs(balanceTolerance.Tolerance);
            movement.MovementTypeId = (int)MovementType.Tolerance;
            movement.VariableTypeId = VariableType.BalanceTolerance;
            return movement;
        }
    }
}
