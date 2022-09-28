// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculationOutput.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation.Output
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The calculation output.
    /// </summary>
    public class CalculationOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationOutput" /> class.
        /// </summary>
        public CalculationOutput()
        {
            this.Movements = new List<Movement>();
            this.Unbalances = new List<UnbalanceComment>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationOutput" /> class.
        /// </summary>
        /// <param name="movements">The movements.</param>
        public CalculationOutput(IEnumerable<Movement> movements)
        {
            this.Movements = movements;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationOutput" /> class.
        /// </summary>
        /// <param name="unbalances">The unbalances.</param>
        /// <param name="unbalanceList">The unbalance list.</param>
        public CalculationOutput(IEnumerable<UnbalanceComment> unbalances, IEnumerable<Unbalance> unbalanceList)
        {
            this.Unbalances = unbalances;
            this.UnbalanceList = unbalanceList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationOutput" /> class.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="unbalanceList">The unbalance list.</param>
        public CalculationOutput(IEnumerable<Movement> movements, IEnumerable<Unbalance> unbalanceList)
        {
            this.Movements = movements;
            this.UnbalanceList = unbalanceList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationOutput" /> class.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="unbalances">The unbalance list.</param>
        public CalculationOutput(IEnumerable<Movement> movements, IEnumerable<UnbalanceComment> unbalances)
        {
            this.Movements = movements;
            this.Unbalances = unbalances;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationOutput" /> class.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="unbalances">The unbalance comments.</param>
        /// <param name="unbalanceList">The unbalance list.</param>
        public CalculationOutput(IEnumerable<Movement> movements, IEnumerable<UnbalanceComment> unbalances, IEnumerable<Unbalance> unbalanceList)
        {
            this.Movements = movements;
            this.Unbalances = unbalances;
            this.UnbalanceList = unbalanceList;
        }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The collection of movements.
        /// </value>
        public IEnumerable<Movement> Movements { get; } = new List<Movement>();

        /// <summary>
        /// Gets the Unbalances.
        /// </summary>
        /// <value>
        /// The collection of Unbalances.
        /// </value>
        public IEnumerable<UnbalanceComment> Unbalances { get; } = new List<UnbalanceComment>();

        /// <summary>
        /// Gets the Unbalances.
        /// </summary>
        /// <value>
        /// The collection of Unbalances.
        /// </value>
        public IEnumerable<Unbalance> UnbalanceList { get; } = new List<Unbalance>();
    }
}
