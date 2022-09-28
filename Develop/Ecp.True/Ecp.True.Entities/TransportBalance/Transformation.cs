// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transformation.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.TransportBalance
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// Transformation Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Transformation : EditableEntity
    {
        /// <summary>
        /// Gets or sets the transformation identifier.
        /// </summary>
        /// <value>
        /// The transformation identifier.
        /// </value>
        public int TransformationId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the origin source node identifier.
        /// </summary>
        /// <value>
        /// The origin source node identifier.
        /// </value>
        public int OriginSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the origin destination node identifier.
        /// </summary>
        /// <value>
        /// The origin destination node identifier.
        /// </value>
        public int? OriginDestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the origin source product identifier.
        /// </summary>
        /// <value>
        /// The origin source product identifier.
        /// </value>
        public string OriginSourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the origin destination product identifier.
        /// </summary>
        /// <value>
        /// The origin destination product identifier.
        /// </value>
        public string OriginDestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the origin measurement identifier.
        /// </summary>
        /// <value>
        /// The origin measurement identifier.
        /// </value>
        public int OriginMeasurementId { get; set; }

        /// <summary>
        /// Gets or sets the destination source node identifier.
        /// </summary>
        /// <value>
        /// The destination source node identifier.
        /// </value>
        public int DestinationSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination destination node identifier.
        /// </summary>
        /// <value>
        /// The destination destination node identifier.
        /// </value>
        public int? DestinationDestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination source product identifier.
        /// </summary>
        /// <value>
        /// The destination source product identifier.
        /// </value>
        public string DestinationSourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination destination product identifier.
        /// </summary>
        /// <value>
        /// The destination destination product identifier.
        /// </value>
        public string DestinationDestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination measurement identifier.
        /// </summary>
        /// <value>
        /// The destination measurement identifier.
        /// </value>
        public int DestinationMeasurementId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is movement; otherwise, <c>false</c>.
        /// </value>
        public bool IsMovement
        {
            get
            {
                return this.MessageTypeId == (int)MessageType.Movement;
            }
        }

        /// <summary>
        /// Gets or sets the destination destination node.
        /// </summary>
        /// <value>
        /// The destination destination node.
        /// </value>
        public virtual Node DestinationDestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination destination product.
        /// </summary>
        /// <value>
        /// The destination destination product.
        /// </value>
        public virtual Product DestinationDestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination measurement.
        /// </summary>
        /// <value>
        /// The destination measurement.
        /// </value>
        public virtual CategoryElement DestinationMeasurement { get; set; }

        /// <summary>
        /// Gets or sets the destination source node.
        /// </summary>
        /// <value>
        /// The destination source node.
        /// </value>
        public virtual Node DestinationSourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination source product.
        /// </summary>
        /// <value>
        /// The destination source product.
        /// </value>
        public virtual Product DestinationSourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the origin destination node.
        /// </summary>
        /// <value>
        /// The origin destination node.
        /// </value>
        public virtual Node OriginDestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the origin destination product.
        /// </summary>
        /// <value>
        /// The origin destination product.
        /// </value>
        public virtual Product OriginDestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the origin measurement.
        /// </summary>
        /// <value>
        /// The origin measurement.
        /// </value>
        public virtual CategoryElement OriginMeasurement { get; set; }

        /// <summary>
        /// Gets or sets the origin source node.
        /// </summary>
        /// <value>
        /// The origin source node.
        /// </value>
        public virtual Node OriginSourceNode { get; set; }

        /// <summary>
        /// Gets or sets the origin source product.
        /// </summary>
        /// <value>
        /// The origin source product.
        /// </value>
        public virtual Product OriginSourceProduct { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var transformation = (Transformation)entity;
            this.OriginSourceNodeId = transformation.OriginSourceNodeId;
            this.OriginDestinationNodeId = transformation.OriginDestinationNodeId;
            this.OriginSourceProductId = transformation.OriginSourceProductId;
            this.OriginDestinationProductId = transformation.OriginDestinationProductId;
            this.OriginMeasurementId = transformation.OriginMeasurementId;
            this.DestinationSourceNodeId = transformation.DestinationSourceNodeId;
            this.DestinationDestinationNodeId = transformation.DestinationDestinationNodeId;
            this.DestinationSourceProductId = transformation.DestinationSourceProductId;
            this.DestinationDestinationProductId = transformation.DestinationDestinationProductId;
            this.DestinationMeasurementId = transformation.DestinationMeasurementId;

            this.RowVersion = transformation.RowVersion;
        }
    }
}