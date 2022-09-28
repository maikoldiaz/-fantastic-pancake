// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProduct.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The NodeConnectionProduct.
    /// </summary>
    public class NodeConnectionProduct : EditableEntity
    {
        /// <summary>
        /// Gets or sets the node connection product identifier.
        /// </summary>
        /// <value>
        /// The node connection product identifier.
        /// </value>
        public int NodeConnectionProductId { get; set; }

        /// <summary>
        /// Gets or sets the node connection identifier.
        /// </summary>
        /// <value>
        /// The node connection identifier.
        /// </value>
        public int NodeConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required(ErrorMessage = Ecp.True.Entities.Constants.NodeConnectionProductRequired)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the uncertainty percentage.
        /// </summary>
        /// <value>
        /// The uncertainty percentage.
        /// </value>
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName
        {
            get
            {
                return this.Product != null ? this.Product.Name : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        [Range(1, 1000000, ErrorMessage = Entities.Constants.InvalidPriority)]
        public int? Priority
        {
            get { return this.InputPriority; }
            set { this.InputPriority = value ?? 1; }
        }

        /// <summary>
        /// Gets or sets the node connection product rule identifier.
        /// </summary>
        /// <value>
        /// The node connection product rule identifier.
        /// </value>
        public int? NodeConnectionProductRuleId { get; set; }

        /// <summary>
        /// Gets the total owner ship value.
        /// </summary>
        /// <value>
        /// The total owner ship value.
        /// </value>
        [JsonIgnore]
        public decimal TotalOwnerShipValue
        {
            get
            {
                return this.Owners != null ? this.Owners.Sum(o => o.OwnershipPercentage).GetValueOrDefault() : 0;
            }
        }

        /// <summary>
        /// Gets or sets the node connection.
        /// </summary>
        /// <value>
        /// The node connection.
        /// </value>
        [ColumnIgnore]
        public virtual NodeConnection NodeConnection { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [ColumnIgnore]
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the node connection product rule.
        /// </summary>
        /// <value>
        /// The node connection product rule.
        /// </value>
        public virtual NodeConnectionProductRule NodeConnectionProductRule { get; set; }

        /// <summary>
        /// Gets the node connection product owners.
        /// </summary>
        /// <value>
        /// The node connection product owners.
        /// </value>
        [JsonProperty]
        public ICollection<NodeConnectionProductOwner> Owners { get; private set; }

        /// <summary>
        /// Gets or sets the input priority.
        /// </summary>
        /// <value>
        /// The input priority.
        /// </value>
        private int? InputPriority { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var product = (NodeConnectionProduct)entity;
            this.UncertaintyPercentage = product.UncertaintyPercentage ?? this.UncertaintyPercentage;
            this.ProductId = product.ProductId ?? this.ProductId;
            this.IsDeleted = product.IsDeleted;
            this.Priority = product.Priority ?? 1;
            this.NodeConnectionProductRuleId = product.NodeConnectionProductRuleId ?? this.NodeConnectionProductRuleId;
            this.RowVersion = product.RowVersion;

            if (this.Owners != null)
            {
                this.Owners.Merge(product.Owners, p => p.OwnerId);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            this.Owners = new List<NodeConnectionProductOwner>();
        }
    }
}
