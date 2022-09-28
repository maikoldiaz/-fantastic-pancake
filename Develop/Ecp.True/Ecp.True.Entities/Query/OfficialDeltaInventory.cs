// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaInventory.cs" company="Microsoft">
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

    /// <summary>
    /// The Official Delta Inventory.
    /// </summary>
    public class OfficialDeltaInventory : QueryEntity
    {
        /// <summary>
        /// Gets or sets the Movement Transaction Id.
        /// </summary>
        /// <value>
        /// The Movement Transaction Id.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Owner Id.
        /// </summary>
        /// <value>
        /// The Movement Owner Id.
        /// </value>
        public int MovementOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Date.
        /// </summary>
        /// <value>
        /// The Movement Date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the Node Id.
        /// </summary>
        /// <value>
        /// The Node Id.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the Product Id.
        /// </summary>
        /// <value>
        /// The Product Id.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Owner Id.
        /// </summary>
        /// <value>
        /// The Owner Id.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the Ownership Value.
        /// </summary>
        /// <value>
        /// The Ownership Value.
        /// </value>
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the Source Node Id.
        /// </summary>
        /// <value>
        /// The Source Node Id.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Destination Node Id.
        /// </summary>
        /// <value>
        /// The Destination Node Id.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Origin Product Id.
        /// </summary>
        /// <value>
        /// The Origin Product Id.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the Destination Product Id.
        /// </summary>
        /// <value>
        /// The Destination Product Id.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }
    }
}
