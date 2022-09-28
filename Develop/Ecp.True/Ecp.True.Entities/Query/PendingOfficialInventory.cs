// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingOfficialInventory.cs" company="Microsoft">
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
    /// The pending official Inventories.
    /// </summary>
    public class PendingOfficialInventory : QueryEntity
    {
        /// <summary>
        /// Gets or sets the movement transactionId.
        /// </summary>
        /// <value>
        /// The movement transactionId.
        /// </value>
        public int InventoryProductID { get; set; }

        /// <summary>
        /// Gets or sets the InventoryProductUniqueId.
        /// </summary>
        /// <value>
        /// TheInventoryProductUniqueId.
        /// </value>
        public string InventoryProductUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the InventoryDate.
        /// </summary>
        /// <value>
        /// InventoryDate.
        /// </value>
        public DateTime InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the NodeId.
        /// </summary>
        /// <value>
        /// NodeId.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the ProductID.
        /// </summary>
        /// <value>
        /// ProductID.
        /// </value>
        public string ProductID { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership Value.
        /// </summary>
        /// <value>
        /// ownership value.
        /// </value>
        public decimal OwnerShipValue { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }
    }
}
