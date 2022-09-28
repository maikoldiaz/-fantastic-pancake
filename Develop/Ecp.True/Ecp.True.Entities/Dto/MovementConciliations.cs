// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConciliations.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;

    /// <summary>
    /// the MovementConciliations.
    /// </summary>
    public class MovementConciliations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementConciliations" /> class.
        /// </summary>
        /// <param name="conciliatedMovements">conciledMovements.</param>
        /// <param name="noConciliatedMovements">noConciledMovements.</param>
        /// <param name="errorMovements">errorMovements.</param>
        public MovementConciliations(
           IEnumerable<MovementConciliationDto> conciliatedMovements,
           IEnumerable<MovementConciliationDto> noConciliatedMovements,
           IEnumerable<MovementConciliationDto> errorMovements)
        {
            this.ConciliatedMovements = conciliatedMovements;
            this.NoConciliatedMovements = noConciliatedMovements;
            this.ErrorMovements = errorMovements;
        }

        /// <summary>
        /// Gets or sets the Conciled Movements.
        /// </summary>
        /// <value>
        /// The error identifier.
        /// </value>
        public IEnumerable<MovementConciliationDto> ConciliatedMovements { get; set; }

        /// <summary>
        /// Gets or sets the No Conciled Movements.
        /// </summary>
        /// <value>
        /// The error identifier.
        /// </value>
        public IEnumerable<MovementConciliationDto> NoConciliatedMovements { get; set; }

        /// <summary>
        /// Gets or sets the Error Movements.
        /// </summary>
        /// <value>
        /// The error identifier.
        /// </value>
        public IEnumerable<MovementConciliationDto> ErrorMovements { get; set; }
    }
}
