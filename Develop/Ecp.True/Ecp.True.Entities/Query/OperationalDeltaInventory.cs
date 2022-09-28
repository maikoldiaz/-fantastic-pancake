// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalDeltaInventory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The category.
    /// </summary>
    public class OperationalDeltaInventory : QueryEntity
    {
        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the inventory date.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public DateTime InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public EventType Action { get; set; }
    }
}
