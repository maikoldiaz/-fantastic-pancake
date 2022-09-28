// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnidentifiedLossMovementGenerator.cs" company="Microsoft">
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
    /// UnIdentified Loss Movement Generator.
    /// </summary>
    public class UnidentifiedLossMovementGenerator : MovementGenerator, IUnidentifiedLossMovementGenerator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<UnidentifiedLossMovementGenerator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnidentifiedLossMovementGenerator" /> class.
        /// </summary>
        /// <param name="balanceCommunicator">The balance communicator.</param>
        /// <param name="logger">The logger.</param>
        public UnidentifiedLossMovementGenerator(ITrueLogger<UnidentifiedLossMovementGenerator> logger)
        {
            this.logger = logger;
        }

        /// <summary>Generates the asynchronous.</summary>
        /// <param name="movementInput">The movementInput.</param>
        /// <returns>The task.</returns>
        public override Task<IEnumerable<Movement>> GenerateAsync(MovementInput movementInput)
        {
            ArgumentValidators.ThrowIfNull(movementInput, nameof(movementInput));
            var unidentifiedLosses = (IEnumerable<UnIdentifiedLossInfo>)movementInput.Outputs;
            this.logger.LogInformation($"Generating unidentified loss movements for ticket {movementInput.Ticket.TicketId}", $"{movementInput.Ticket.TicketId}");

            var movements = new List<Movement>();
            foreach (var item in unidentifiedLosses.Where(x => x.UnIdentifiedLoss != 0))
            {
                var movement = this.DoGenerate(item, movementInput.Ticket, movementInput.CalculationDate);
                movements.Add(movement);
            }

            return Task.FromResult(movements.AsEnumerable());
        }

        /// <summary>Do Generate.</summary>
        /// <returns>movement object.</returns>
        /// <param name="unidentifiedLossInfo">The interfaceInfo.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="calculationDate">The calculationDate.</param>
        protected Movement DoGenerate(UnIdentifiedLossInfo unidentifiedLossInfo, Ticket ticket, DateTime calculationDate)
        {
            ArgumentValidators.ThrowIfNull(unidentifiedLossInfo, nameof(unidentifiedLossInfo));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var movement = unidentifiedLossInfo.UnIdentifiedLoss > 0 ?
                this.BuildMovement(ticket, unidentifiedLossInfo, null, calculationDate) : this.BuildMovement(ticket, null, unidentifiedLossInfo, calculationDate);
            movement.NetStandardVolume = Math.Abs(unidentifiedLossInfo.UnIdentifiedLoss);
            movement.MovementTypeId = (int)MovementType.UnidentifiedLoss;
            movement.VariableTypeId = VariableType.UnidentifiedLosses;
            return movement;
        }
    }
}
