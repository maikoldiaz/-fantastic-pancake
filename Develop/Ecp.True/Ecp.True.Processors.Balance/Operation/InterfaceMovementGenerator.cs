// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceMovementGenerator.cs" company="Microsoft">
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
    /// Interface Movement Generator.
    /// </summary>
    public class InterfaceMovementGenerator : MovementGenerator, IInterfaceMovementGenerator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<InterfaceMovementGenerator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceMovementGenerator" /> class.
        /// </summary>
        /// <param name="balanceCommunicator">The balance communicator.</param>
        /// <param name="logger">The logger.</param>
        public InterfaceMovementGenerator(ITrueLogger<InterfaceMovementGenerator> logger)
        {
            this.logger = logger;
        }

        /// <summary>Generates the asynchronous.</summary>
        /// <param name="movementInput">The movementInput.</param>
        /// <returns>The task.</returns>
        public override Task<IEnumerable<Movement>> GenerateAsync(MovementInput movementInput)
        {
            ArgumentValidators.ThrowIfNull(movementInput, nameof(movementInput));
            var interfaces = (IEnumerable<InterfaceInfo>)movementInput.Outputs;
            if (Math.Round(interfaces.Select(x => x.Interface).Sum(), 12) != 0.0M)
            {
                this.logger.LogInformation($"Sum of all the interface amount in not zero for ticket {movementInput.Ticket.TicketId}");
                return Task.FromResult<IEnumerable<Movement>>(null);
            }

            int i = 0;
            int j = 0;
            var sourceInterfaces = interfaces.Where(x => x.Interface > 0).OrderByDescending(x => x.Interface).ToList();
            var destinationInterfaces = interfaces.Where(x => x.Interface < 0).OrderBy(x => x.Interface).ToList();

            var sourceInterfaceValues = sourceInterfaces.Select(x => x.Interface).ToArray();
            var destinationInterfaceValues = destinationInterfaces.Select(x => x.Interface).ToArray();

            var movements = new List<Movement>();
            while (i < sourceInterfaceValues.Length)
            {
                var interfaceAmount = Math.Min(Math.Abs(sourceInterfaceValues[i]), Math.Abs(destinationInterfaceValues[j]));
                var movement = this.DoGenerate(sourceInterfaces[i], destinationInterfaces[j], interfaceAmount, movementInput.Ticket, movementInput.CalculationDate);
                movements.Add(movement);

                sourceInterfaceValues[i] = sourceInterfaceValues[i] - interfaceAmount;
                destinationInterfaceValues[j] = destinationInterfaceValues[j] + interfaceAmount;
                if (destinationInterfaceValues[j] == 0.0M)
                {
                    j += 1;
                }

                if (sourceInterfaceValues[i] == 0.0M)
                {
                    i += 1;
                }
            }

            return Task.FromResult(movements.AsEnumerable());
        }

        /// <summary>Do Generate.</summary>
        /// <returns>movement object.</returns>
        /// <param name="sourceInterface">The source interface.</param>
        /// <param name="destinationInterface">The destination interface.</param>
        /// <param name="interfaceAmount">The interface amount.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="calculationDate">The calculationDate.</param>
        protected Movement DoGenerate(InterfaceInfo sourceInterface, InterfaceInfo destinationInterface, decimal interfaceAmount, Ticket ticket, DateTime calculationDate)
        {
            ArgumentValidators.ThrowIfNull(sourceInterface, nameof(sourceInterface));
            ArgumentValidators.ThrowIfNull(destinationInterface, nameof(destinationInterface));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var movement = this.BuildMovement(ticket, sourceInterface, destinationInterface, calculationDate);
            movement.NetStandardVolume = interfaceAmount;
            movement.MovementTypeId = (int)MovementType.Interface;
            movement.VariableTypeId = VariableType.Interface;
            return movement;
        }
    }
}