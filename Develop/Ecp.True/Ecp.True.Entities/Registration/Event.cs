// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The event.
    /// </summary>
    public class Event : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the event type identifier.
        /// </summary>
        /// <value>
        /// The event type identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.EventTypeIdRequired)]
        public int EventTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.SourceNodeIdRequired)]
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.DestinationNodeIdRequired)]
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [Required(ErrorMessage = Constants.EventSourceProductIdRequired)]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        [Required(ErrorMessage = Constants.EventDestinationProductIdRequired)]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.EventStartDateIsMandatory)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.EventEndDateIsMandatory)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the first owner identifier.
        /// </summary>
        /// <value>
        /// The first owner identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.Owner1Requiredvalidation)]
        public int Owner1Id { get; set; }

        /// <summary>
        /// Gets or sets the second owner identifier.
        /// </summary>
        /// <value>
        /// The second owner identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.Owner2Requiredvalidation)]
        public int Owner2Id { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.EventVolumeRequired)]
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.EventUnitIdRequired)]
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public virtual Node DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public virtual Product DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the first owner.
        /// </summary>
        /// <value>
        /// The first owner.
        /// </value>
        public virtual CategoryElement Owner1 { get; set; }

        /// <summary>
        /// Gets or sets the second owner.
        /// </summary>
        /// <value>
        /// The second owner.
        /// </value>
        public virtual CategoryElement Owner2 { get; set; }

        /// <summary>
        /// Gets or sets the ownership event.
        /// </summary>
        /// <value>
        /// The ownership event.
        /// </value>
        public virtual CategoryElement EventType { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public virtual Product SourceProduct { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var eventObj = (Event)entity;
            this.Volume = eventObj.Volume;
            this.MeasurementUnit = eventObj.MeasurementUnit;
            this.EventTypeId = eventObj.EventTypeId;
            this.Owner1Id = eventObj.Owner1Id;
            this.Owner2Id = eventObj.Owner2Id;
        }
    }
}
