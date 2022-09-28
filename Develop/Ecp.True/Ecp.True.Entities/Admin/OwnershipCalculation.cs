// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculation.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System.Collections.Generic;

    /// <summary>
    /// OwnershipCalculation Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipCalculation : OwnershipCalculationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipCalculation"/> class.
        /// </summary>
        public OwnershipCalculation()
        {
            this.OwnershipCalculationResults = new List<OwnershipCalculationResult>();
        }

        /// <summary>
        /// Gets or sets the ownership calculation identifier.
        /// </summary>
        /// <value>
        /// The ownership calculation identifier.
        /// </value>
        public int OwnershipCalculationId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets the ownership calculation results.
        /// </summary>
        /// <value>
        /// The ownership calculation result.
        /// </value>
        public virtual ICollection<OwnershipCalculationResult> OwnershipCalculationResults { get; }
    }
}