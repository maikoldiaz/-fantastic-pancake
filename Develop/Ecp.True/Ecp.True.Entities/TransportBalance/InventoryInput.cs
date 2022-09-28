// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryInput.cs" company="Microsoft">
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
    /// The Inventory Input.
    /// </summary>
    public class InventoryInput : Entity
    {
        /// <summary>
        /// Gets or sets the inventory number.
        /// </summary>
        /// <value>
        /// The inventory number.
        /// </value>
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the product volume.
        /// </summary>
        /// <value>
        /// The product volume.
        /// </value>
        public decimal? ProductVolume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the uncertainty percentage.
        /// </summary>
        /// <value>
        /// The uncertainty percentage.
        /// </value>
        public decimal? UncertaintyPercentage { get; set; }
    }
}
