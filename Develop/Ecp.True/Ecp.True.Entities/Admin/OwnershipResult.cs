// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResult.cs" company="Microsoft">
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
    /// The OwnershipResult.
    /// </summary>
    public class OwnershipResult : Entity
    {
        /// <summary>
        /// Gets or sets the ownership result identifier.
        /// </summary>
        /// <value>
        /// The ownership result identifier.
        /// </value>
        public int OwnershipResultId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        public int? InventoryProductId { get; set; }

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
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        public decimal InitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        public decimal FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public decimal Input { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public decimal Output { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the inventory transaction.
        /// </summary>
        /// <value>
        /// The inventory transaction.
        /// </value>
        public virtual InventoryProduct InventoryProduct { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public virtual MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }
    }
}
