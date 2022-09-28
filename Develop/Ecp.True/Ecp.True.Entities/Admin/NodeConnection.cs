// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnection.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Host.Api.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.DataAccess.Sql.Tests")]

namespace Ecp.True.Entities.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Newtonsoft.Json;

    /// <summary>
    /// The node connection entity.
    /// </summary>
    /// <seealso cref="Entity" />
    public class NodeConnection : EditableEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NodeConnection" /> class.
        /// </summary>
        public NodeConnection()
        {
            this.Products = new List<NodeConnectionProduct>();
            this.OffchainNodeConnections = new List<OffchainNodeConnection>();
        }

        /// <summary>
        /// Gets or sets the node connection identifier.
        /// </summary>
        /// <value>
        /// The node connection identifier.
        /// </value>
        public int NodeConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeConnectionSourceNodeRequiredValidation)]
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeConnectionDestinationNodeRequiredValidation)]
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [StringLength(300, ErrorMessage = Entities.Constants.NodeConnectionDescriptionLengthValidation)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeConnectionStatusRequiredValidation)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the control limit.
        /// </summary>
        /// <value>
        /// The control limit.
        /// </value>
        public decimal? ControlLimit { get; set; }

        /// <summary>
        /// Gets or sets the is transfer.
        /// </summary>
        /// <value>
        /// The is transfer.
        /// </value>
        public bool? IsTransfer { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.SourceNode != null && this.DestinationNode != null ? $"{this.SourceNode.Name}-{this.DestinationNode.Name}" : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public virtual Node DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the algorithm list identifier.
        /// </summary>
        /// <value>
        /// The algorithm list identifier.
        /// </value>
        [RequiredIf("IsTransfer", true, ErrorMessage = Entities.Constants.AlgorithmNotDefined)]
        public int? AlgorithmId { get; set; }

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>
        /// The algorithm.
        /// </value>
        public virtual Algorithm Algorithm { get; set; }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        [JsonProperty]
        public ICollection<NodeConnectionProduct> Products { get; private set; }

        /// <summary>
        /// Gets the offchain node connections.
        /// </summary>
        /// <value>
        /// The offchain node connections.
        /// </value>
        public ICollection<OffchainNodeConnection> OffchainNodeConnections { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has valid ownership.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has valid ownership; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool HasValidOwnership
        {
            get
            {
                if (this.Products == null)
                {
                    return true;
                }

                return this.Products.Where(p => p.Owners != null && p.Owners.Count > 0).All(p => p.TotalOwnerShipValue == 100);
            }
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="nodeConnection">The node connection.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var nodeConnection = (NodeConnection)entity;

            this.SourceNodeId = nodeConnection.SourceNodeId ?? this.SourceNodeId;
            this.DestinationNodeId = nodeConnection.DestinationNodeId ?? this.DestinationNodeId;
            this.Description = nodeConnection.Description ?? this.Description;
            this.IsActive = nodeConnection.IsActive ?? this.IsActive;
            this.ControlLimit = nodeConnection.ControlLimit ?? this.ControlLimit;
            this.IsTransfer = nodeConnection.IsTransfer ?? this.IsTransfer;
            this.AlgorithmId = nodeConnection.AlgorithmId ?? nodeConnection.AlgorithmId;
            this.RowVersion = nodeConnection.RowVersion;

            if (this.Products != null)
            {
                this.Products.Merge(nodeConnection.Products, o => o.ProductId);
            }
        }

        /// <summary>
        /// Adds the products.
        /// </summary>
        /// <param name="products">The products.</param>
        public void AddProducts(ICollection<NodeConnectionProduct> products)
        {
            this.Products = products;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            this.Products = new List<NodeConnectionProduct>();
            this.OffchainNodeConnections = new List<OffchainNodeConnection>();
        }
    }
}
