// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedOwner.cs" company="Microsoft">
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

    /// <summary>
    /// The Consolidated Owner.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ConsolidatedOwner : Entity
    {
        /// <summary>
        /// Gets or sets the consolidated owner identifier.
        /// </summary>
        /// <value>
        /// The consolidated owner identifier.
        /// </value>
        public int ConsolidatedOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        public int? ConsolidatedMovementId { get; set; }

        /// <summary>
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        public int? ConsolidatedInventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual CategoryElement Owner { get; set; }

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
