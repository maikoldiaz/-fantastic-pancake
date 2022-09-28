// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementInput.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Operation.Input
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// The movement input.
    /// </summary>
    public class MovementInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementInput"/> class.
        /// </summary>
        /// <param name="outputs">The outputs.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="calculationDate">The calculationDate.</param>
        public MovementInput(IEnumerable<IOutput> outputs, Ticket ticket, DateTime calculationDate)
        {
            this.Outputs = outputs;
            this.Ticket = ticket;
            this.CalculationDate = calculationDate;
        }

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        public IEnumerable<IOutput> Outputs { get; }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The Ticket.
        /// </value>
        public Ticket Ticket { get; }

        /// <summary>
        /// Gets or sets the Execution time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime CalculationDate { get; set; }
    }
}
