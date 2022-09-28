// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsTicketValidationResult.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The logistics ticket validation result.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class LogisticsTicketValidationResult : Entity
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string NodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the name of the segment.
        /// </summary>
        /// <value>
        /// The name of the segment.
        /// </value>
        public string SegmentName { get; set; }
    }
}
