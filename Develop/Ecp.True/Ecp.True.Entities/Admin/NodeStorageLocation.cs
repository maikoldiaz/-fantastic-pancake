// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeStorageLocation.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;

    /// <summary>
    ///     The storage location.
    /// </summary>
    public class NodeStorageLocation : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeStorageLocation" /> class.
        /// </summary>
        public NodeStorageLocation()
        {
            this.Products = new List<StorageLocationProduct>();
            this.Sources = new List<MovementSource>();
            this.Destinations = new List<MovementDestination>();
        }

        /// <summary>
        /// Gets or sets the node storage location identifier.
        /// </summary>
        /// <value>
        /// The node storage location identifier.
        /// </value>
        public int NodeStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeStorageLocationNameRequired)]
        [MaxLength(150, ErrorMessage = Entities.Constants.StorageLocationNameMax150Characters)]
        [RegularExpression(Entities.Constants.AllowNumbersAndLettersWithSpecialCharactersWithSpaceRegex, ErrorMessage = Entities.Constants.StorageLocationNameSpecialCharactersMessage)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [MaxLength(1000, ErrorMessage = Entities.Constants.StorageLocationMax1000Characters)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the storage location type identifier.
        /// </summary>
        /// <value>
        /// The storage location type identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.TypeRequired)]
        public int? StorageLocationTypeId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeStorageLocationMustBeActive)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [send to SAP].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [send to SAP]; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeStorageLocationSendToSapRequired)]
        public bool? SendToSap { get; set; }

        /// <summary>
        /// Gets or sets the storage location identifier.
        /// </summary>
        /// <value>
        /// The storage location identifier.
        /// </value>
        [RequiredIf("SendToSap", true, ErrorMessage = Entities.Constants.SapCodeRequired)]
        public string StorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        [ColumnIgnore]
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the logistic center.
        /// </summary>
        /// <value>
        /// The logistic center.
        /// </value>
        [ColumnIgnore]
        public virtual StorageLocation StorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the type of the storage location.
        /// </summary>
        /// <value>
        /// The type of the storage location.
        /// </value>
        [ColumnIgnore]
        public virtual CategoryElement StorageLocationType { get; set; }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        [MustNotBeEmptyIf("IsActive", true, ErrorMessage = Entities.Constants.StoreShouldHaveAtLeastOneProduct)]
        [ColumnIgnore]
        public virtual ICollection<StorageLocationProduct> Products { get; }

        /// <summary>
        /// Gets the source movements.
        /// </summary>
        /// <value>
        /// The Source Movements.
        /// </value>
        [ColumnIgnore]
        public virtual ICollection<MovementSource> Sources { get; }

        /// <summary>
        /// Gets the destination movements.
        /// </summary>
        /// <value>
        /// The destination Movements.
        /// </value>
        [ColumnIgnore]
        public virtual ICollection<MovementDestination> Destinations { get; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="NodeStorageLocation">The nodeStorageLocation.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var location = (NodeStorageLocation)entity;

            this.Description = location.Description ?? this.Description;
            this.IsActive = location.IsActive ?? this.IsActive;
            this.Name = location.Name ?? this.Name;
            this.SendToSap = location.SendToSap ?? this.SendToSap;
            this.StorageLocationTypeId = location.StorageLocationTypeId ?? this.StorageLocationTypeId;
            this.StorageLocationId = location.StorageLocationId;
            if (this.Products != null)
            {
                this.Products.Merge(location.Products, o => o.ProductId);
            }
        }
    }
}