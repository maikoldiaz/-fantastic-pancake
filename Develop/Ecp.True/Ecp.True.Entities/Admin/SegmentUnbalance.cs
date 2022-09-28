// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentUnbalance.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// SegmentUnbalance Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SegmentUnbalance : Entity, ITicketEntity
    {
        /// <summary>
        /// Gets or sets the segment unbalance identifier.
        /// </summary>
        /// <value>
        /// The segment unbalance identifier.
        /// </value>
        public int SegmentUnbalanceId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket identifier.
        /// </summary>
        /// <value>
        /// The ownership ticket identifier.
        /// </value>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory volume.
        /// </summary>
        /// <value>
        /// The initial inventory volume.
        /// </value>
        public decimal? InitialInventoryVolume { get; set; }

        /// <summary>
        /// Gets or sets the final inventory volume.
        /// </summary>
        /// <value>
        /// The final inventory volume.
        /// </value>
        public decimal? FinalInventoryVolume { get; set; }

        /// <summary>
        /// Gets or sets the input volume.
        /// </summary>
        /// <value>
        /// The input volume.
        /// </value>
        public decimal? InputVolume { get; set; }

        /// <summary>
        /// Gets or sets the output volume.
        /// </summary>
        /// <value>
        /// The output volume.
        /// </value>
        public decimal? OutputVolume { get; set; }

        /// <summary>
        /// Gets or sets the identified losses volume.
        /// </summary>
        /// <value>
        /// The identified losses volume.
        /// </value>
        public decimal? IdentifiedLossesVolume { get; set; }

        /// <summary>
        /// Gets or sets the unbalance volume.
        /// </summary>
        /// <value>
        /// The unbalance volume.
        /// </value>
        public decimal? UnbalanceVolume { get; set; }

        /// <summary>
        /// Gets or sets the interface volume.
        /// </summary>
        /// <value>
        /// The interface volume.
        /// </value>
        public decimal? InterfaceVolume { get; set; }

        /// <summary>
        /// Gets or sets the tolerance volume.
        /// </summary>
        /// <value>
        /// The tolerance volume.
        /// </value>
        public decimal? ToleranceVolume { get; set; }

        /// <summary>
        /// Gets or sets the unidentified losses volume.
        /// </summary>
        /// <value>
        /// The unidentified losses volume.
        /// </value>
        public decimal? UnidentifiedLossesVolume { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public virtual CategoryElement Segment { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the unbalance ticket.
        /// </summary>
        /// <value>
        /// The unbalance ticket.
        /// </value>
        public virtual Ticket UnbalanceTicket { get; set; }
    }
}
