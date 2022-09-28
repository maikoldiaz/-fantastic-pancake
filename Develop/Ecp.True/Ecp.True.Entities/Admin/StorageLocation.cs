// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocation.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Storage Location.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class StorageLocation : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocation"/> class.
        /// </summary>
        public StorageLocation()
        {
            this.NodeStorageLocations = new List<NodeStorageLocation>();
            this.StorageLocationProductMappings = new List<StorageLocationProductMapping>();
        }

        /// <summary>
        /// Gets or sets the storage location identifier.
        /// </summary>
        /// <value>
        /// The storage location identifier.
        /// </value>
        [MaxLength(20, ErrorMessage = Constants.Max20Characters)]
        public string StorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength(150, ErrorMessage = Constants.Max150Characters)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the logistic center identifier.
        /// </summary>
        /// <value>
        /// The logistic center identifier.
        /// </value>
        public string LogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the logistic center.
        /// </summary>
        /// <value>
        /// The logistic center.
        /// </value>
        public virtual LogisticCenter LogisticCenter { get; set; }

        /// <summary>
        /// Gets the node storage locations.
        /// </summary>
        /// <value>
        /// The node storage locations.
        /// </value>
        public virtual ICollection<NodeStorageLocation> NodeStorageLocations { get; }

        /// <summary>
        /// Gets the storage location product mapping.
        /// </summary>
        /// <value>
        /// The storage location product mapping.
        /// </value>
        public virtual ICollection<StorageLocationProductMapping> StorageLocationProductMappings { get; }
    }
}
