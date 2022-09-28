// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationBatch.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Entities
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The ConsolidationBatch.
    /// </summary>
    public class ConsolidationBatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidationBatch"/> class.
        /// </summary>
        public ConsolidationBatch()
        {
            this.ConsolidationNodes = new List<ConsolidationNodeData>();
        }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket id.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [should process inventory].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [should process inventory]; otherwise, <c>false</c>.
        /// </value>
        public bool ShouldProcessInventory { get; set; }

        /// <summary>
        /// Gets or sets the consolidation nodes data set.
        /// </summary>
        /// <value>
        /// The consolidation nodes data set.
        /// </value>
        public IEnumerable<ConsolidationNodeData> ConsolidationNodes { get; set; }
    }
}
