// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProduct.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Processors.Api.Tests")]

namespace Ecp.True.Entities.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Newtonsoft.Json;

    /// <summary>
    ///     The product.
    /// </summary>
    public class StorageLocationProduct : EditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProduct"/> class.
        /// </summary>
        public StorageLocationProduct()
        {
            this.Initialize();
        }

        /// <summary>
        /// Gets or sets the storage location product identifier.
        /// </summary>
        /// <value>
        /// The storage location product identifier.
        /// </value>
        public int StorageLocationProductId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Ecp.True.Entities.Constants.ProductMustBeActive)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required(ErrorMessage = Ecp.True.Entities.Constants.ProductIdRequired)]
        public string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the node storage location identifier.
        /// </summary>
        /// <value>
        ///     The node storage location identifier.
        /// </value>
        public int NodeStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the identified losses rule identifier.
        /// </summary>
        /// <value>
        /// The identified losses rule identifier.
        /// </value>
        [ColumnIgnore]
        public int? OwnershipRuleId { get; set; }

        /// <summary>
        /// Gets or sets the uncertainty percentage.
        /// </summary>
        /// <value>
        /// The uncertainty percentage.
        /// </value>
        [ColumnIgnore]
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the node product rule identifier.
        /// </summary>
        /// <value>
        /// The node product rule identifier.
        /// </value>
        public int? NodeProductRuleId { get; set; }

        /// <summary>
        /// Gets the total owner ship value.
        /// </summary>
        /// <value>
        /// The total owner ship value.
        /// </value>
        [JsonIgnore]
        [ColumnIgnore]
        public decimal TotalOwnerShipValue
        {
            get
            {
                return this.Owners != null ? this.Owners.Sum(o => o.OwnershipPercentage).GetValueOrDefault() : 0;
            }
        }

        /// <summary>
        /// Gets or sets the node storage location.
        /// </summary>
        /// <value>
        /// The storage location node.
        /// </value>
        [ColumnIgnore]
        public virtual NodeStorageLocation NodeStorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [ColumnIgnore]
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the OwnershipRule.
        /// </summary>
        /// <value>
        /// The IdentifiedLossesRule.
        /// </value>
        [ColumnIgnore]
        public virtual CategoryElement OwnershipRule { get; set; }

        /// <summary>
        /// Gets or sets the node product rule.
        /// </summary>
        /// <value>
        /// The node product rule.
        /// </value>
        public virtual NodeProductRule NodeProductRule { get; set; }

        /// <summary>
        /// Gets the owners.
        /// </summary>
        /// <value>
        /// The Owners.
        /// </value>
        [JsonProperty]
        [ColumnIgnore]
        public ICollection<StorageLocationProductOwner> Owners { get; private set; }

        /// <summary>
        /// Gets the storage location product variables.
        /// </summary>
        /// <value>
        /// The storage location product variables.
        /// </value>
        public ICollection<StorageLocationProductVariable> StorageLocationProductVariables { get; private set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var product = (StorageLocationProduct)entity;
            this.UncertaintyPercentage = product.UncertaintyPercentage ?? this.UncertaintyPercentage;
            this.ProductId = product.ProductId ?? this.ProductId;
            this.OwnershipRuleId = product.OwnershipRuleId ?? this.OwnershipRuleId;
            this.IsActive = product.IsActive ?? this.IsActive;
            this.RowVersion = product.RowVersion ?? this.RowVersion;

            if (this.Owners != null)
            {
                this.Owners.Merge(product.Owners, p => p.OwnerId);
            }

            if (this.StorageLocationProductVariables != null)
            {
                this.StorageLocationProductVariables.Merge(product.StorageLocationProductVariables, p => p.StorageLocationProductVariableId);
            }
        }

        /// <summary>
        /// Clears the owners.
        /// </summary>
        public void ClearOwners()
        {
            this.Owners = null;
        }

        /// <summary>
        /// Clears the storage location product variables.
        /// </summary>
        public void ClearStorageLocationProductVariables()
        {
            this.StorageLocationProductVariables = new List<StorageLocationProductVariable>();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            this.Owners = new List<StorageLocationProductOwner>();
            this.StorageLocationProductVariables = new List<StorageLocationProductVariable>();
        }
    }
}