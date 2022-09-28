// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationData.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The ConsolidationData.
    /// </summary>
    /// <seealso cref="OrchestratorMetaData" />
    public class ConsolidationData : OrchestratorMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidationData"/> class.
        /// </summary>
        public ConsolidationData()
        {
            this.Batches = new List<ConsolidationBatch>();
        }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket id.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the batches.
        /// </summary>
        /// <value>
        /// The batches.
        /// </value>
        public IEnumerable<ConsolidationBatch> Batches { get; set; }
    }
}
