// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Inventory class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapInventory
    {
        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        [Required(ErrorMessage = SapConstants.SourceSystemNameRequired)]
        [StringLength(25, ErrorMessage = SapConstants.SourceSystemLengthExceeded)]
        [JsonProperty("SourceSystem")]
        public string SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the destination system.
        /// </summary>
        /// <value>
        /// The destination system.
        /// </value>
        [Required(ErrorMessage = SapConstants.DestinationSystemNameRequired)]
        [StringLength(25, ErrorMessage = SapConstants.DestinationSystemLengthExceeded)]
        public string DestinationSystem { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [Required(ErrorMessage = SapConstants.EventTypeRequired)]
        [StringLength(10, ErrorMessage = SapConstants.EventTypeLengthExceededForInventory)]
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.InventoryIdRequired)]
        [StringLength(50, ErrorMessage = SapConstants.InventoryIdLengthExceeded)]
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the inventory date.
        /// </summary>
        /// <value>
        /// The inventroy date.
        /// </value>
        [Required(ErrorMessage = SapConstants.InventoryDateRequired)]
        public DateTime? InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.NodeIdRequired)]
        [StringLength(150, ErrorMessage = SapConstants.NodeIdLengthExceeded)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tank.
        /// </summary>
        /// <value>
        /// The name of the tank.
        /// </value>
        [StringLength(20, ErrorMessage = SapConstants.TankNameLengthExceeded)]
        [JsonProperty("Tank")]
        public string TankName { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        [MustNotBeEmpty(ErrorMessage = SapConstants.ProductsRequired)]
        public IEnumerable<SapProduct> Products { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.ObservationLengthExceeded)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.OperatorLengthExceeded)]
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the uncertainty percentage.
        /// </summary>
        /// <value>
        /// The uncertainty percentage.
        /// </value>
        [JsonProperty("Uncertainty")]
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        [Required(ErrorMessage = SapConstants.SegmentRequired)]
        [StringLength(150, ErrorMessage = SapConstants.SegmentLengthExceeded)]
        public string SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the escenario.
        /// </summary>
        /// <value>
        /// The escenario.
        /// </value>
        [Required(ErrorMessage = SapConstants.ScenarioRequired)]
        [ValidateEnum(typeof(ScenarioType), SapConstants.ScenarioIdValueRangeFailed)]
        public string ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [StringLength(50, ErrorMessage = SapConstants.VersionLengthExceeded)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.SystemLengthExceeded)]
        [JsonProperty("System")]
        public string SystemId { get; set; }
    }
}
