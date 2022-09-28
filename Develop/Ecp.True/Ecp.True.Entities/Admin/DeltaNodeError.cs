// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeError.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The delta error.
    /// </summary>
    public class DeltaNodeError : Entity
    {
        /// <summary>
        /// Gets or sets the delta node error.
        /// </summary>
        /// <value>
        /// The delta node error.
        /// </value>
        public int DeltaNodeErrorId { get; set; }

        /// <summary>
        /// Gets or sets the delta error identifier.
        /// </summary>
        /// <value>
        /// The delta error identifier.
        /// </value>
        public int DeltaNodeId { get; set; }

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
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        public int? ConsolidatedMovementId { get; set; }

        /// <summary>
        /// Gets or sets the consolidated inventory product identifier.
        /// </summary>
        /// <value>
        /// The consolidated inventory product identifier.
        /// </value>
        public int? ConsolidatedInventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the movement.
        /// </summary>
        /// <value>
        /// The movement.
        /// </value>
        public virtual DeltaNode DeltaNode { get; set; }

        /// <summary>
        /// Gets or sets the movement.
        /// </summary>
        /// <value>
        /// The movement.
        /// </value>
        public virtual Movement Movement { get; set; }

        /// <summary>
        /// Gets or sets the inventory product.
        /// </summary>
        /// <value>
        /// The inventory product.
        /// </value>
        public virtual InventoryProduct InventoryProduct { get; set; }

        /// <summary>
        /// Gets or sets the consolidated movement.
        /// </summary>
        /// <value>
        /// The consolidated movement.
        /// </value>
        public virtual ConsolidatedMovement ConsolidatedMovement { get; set; }

        /// <summary>
        /// Gets or sets the consolidated inventory product.
        /// </summary>
        /// <value>
        /// The consolidated inventory product.
        /// </value>
        public virtual ConsolidatedInventoryProduct ConsolidatedInventoryProduct { get; set; }
    }
}
