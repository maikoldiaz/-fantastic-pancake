// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeData.cs" company="Microsoft">
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
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// OwnershipNode Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipNodeData : Entity
    {
        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int OwnershipNodeId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the ownership status id.
        /// </summary>
        /// <value>
        /// The node ownership status id.
        /// </value>
        public int OwnershipStatusId { get; set; }

        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketTypeId { get; set; }

        /// <summary>Gets or sets the segment.</summary>
        /// <value>The segment.</value>
        public string Segment { get; set; }

        /// <summary>Gets or sets the name of the category.</summary>
        /// <value>The name of the category.</value>
        public string CategoryName { get; set; }

        /// <summary>Gets or sets the ticket start date.</summary>
        /// <value>The ticket start date.</value>
        public DateTime TicketStartDate { get; set; }

        /// <summary>Gets or sets the ticket final date.</summary>
        /// <value>The ticket final date.</value>
        public DateTime TicketFinalDate { get; set; }

        /// <summary>Gets or sets the cutoff execution date.</summary>
        /// <value>The cutoff execution date.</value>
        public DateTime CutoffExecutionDate { get; set; }

        /// <summary>Gets or sets the created by.</summary>
        /// <value>The created by.</value>
        public override string CreatedBy { get; set; }

        /// <summary>Gets or sets the name of the owner.</summary>
        /// <value>The name of the owner.</value>
        public string OwnerName { get; set; }

        /// <summary>Gets or sets the error message.</summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }

        /// <summary>Gets or sets the BLOB path.</summary>
        /// <value>The BLOB path.</value>
        public string BlobPath { get; set; }

        /// <summary>Gets or sets the name of the node.</summary>
        /// <value>The name of the node.</value>
        public string NodeName { get; set; }

        /// <summary>Gets or sets the state.</summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the transfer point.
        /// </summary>
        /// <value>
        /// The transfer point.
        /// </value>
        public bool IsTransferPoint { get; set; }
    }
}
