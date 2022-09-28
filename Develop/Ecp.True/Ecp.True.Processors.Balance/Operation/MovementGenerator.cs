// <copyright file="MovementGenerator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation.Input;
    using Ecp.True.Processors.Balance.Operation.Interfaces;

    /// <summary>
    /// Movement Generator Base.
    /// </summary>
    public abstract class MovementGenerator : IMovementGenerator
    {
        /// <summary>
        /// Generates the asynchronous.
        /// </summary>
        /// <param name="movementInput">The movement input.</param>
        /// <returns>The task.</returns>
        public abstract Task<IEnumerable<Movement>> GenerateAsync(MovementInput movementInput);

        /// <summary>Do Generate Base.</summary>
        /// <returns>movement object.</returns>
        /// <param name="ticket">The ticket.</param>
        /// <param name="source">The source input.</param>
        /// <param name="destination">The destination input.</param>
        /// <param name="calculationDate">The calculationDate.</param>
        protected Movement BuildMovement(Ticket ticket, IOutput source, IOutput destination, DateTime calculationDate)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            return new Movement
            {
                MessageTypeId = (int)MessageType.SpecialMovement,
                SystemTypeId = (int)SystemType.TRUE,
                SourceSystemId = (int)SourceSystem.TRUE,
                EventType = EventType.Insert.ToString(),
                MovementId = Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture),
                TicketId = ticket.TicketId,
                OperationalDate = calculationDate.Date,
                GrossStandardVolume = null,
                MeasurementUnit = Constants.Barrels,
                ScenarioId = ScenarioType.OPERATIONAL,
                Observations = string.Empty,
                Classification = string.Empty,
                SegmentId = ticket.CategoryElementId,
                MovementSource = source == null ? null : new MovementSource
                {
                    SourceNodeId = source.NodeId,
                    SourceProductId = source.ProductId,
                },
                MovementDestination = destination == null ? null : new MovementDestination
                {
                    DestinationNodeId = destination.NodeId,
                    DestinationProductId = destination.ProductId,
                },
            };
        }
    }
}
