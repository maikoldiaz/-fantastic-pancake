// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeError.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// OwnershipNodeError Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipNodeError : Entity
    {
        /// <summary>
        /// Gets or sets the ownership node error identifier.
        /// </summary>
        /// <value>
        /// The ownership node error identifier.
        /// </value>
        public int OwnershipNodeErrorId { get; set; }

        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int OwnershipNodeId { get; set; }

        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory product identifier.
        /// </value>
        public int? InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets the inventory product.
        /// </summary>
        /// <value>
        /// The inventory product.
        /// </value>
        public virtual InventoryProduct InventoryProduct { get; set; }

        /// <summary>
        /// Gets or sets the movement.
        /// </summary>
        /// <value>
        /// The movement.
        /// </value>
        public virtual Movement Movement { get; set; }

        /// <summary>
        /// Gets or sets the ownership node.
        /// </summary>
        /// <value>
        /// The ownership node.
        /// </value>
        public virtual OwnershipNode OwnershipNode { get; set; }
    }
}
