// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Entities.OfficialDelta
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The official delta data entity.
    /// </summary>
    public class OfficialDeltaData : OrchestratorMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaData"/> class.
        /// </summary>
        public OfficialDeltaData()
        {
            this.PendingOfficialInventories = new List<PendingOfficialInventory>();
            this.PendingOfficialMovements = new List<PendingOfficialMovement>();
            this.ConsolidationMovements = new List<ConsolidatedMovement>();
            this.ConsolidationInventories = new List<ConsolidatedInventoryProduct>();

            this.OfficialResultMovements = new List<OfficialResultMovement>();
            this.OfficialResultInventories = new List<OfficialResultInventory>();
            this.MovementErrors = new List<OfficialErrorMovement>();
            this.InventoryErrors = new List<OfficialErrorInventory>();

            this.Movements = new List<Movement>();
            this.InventoryProducts = new List<InventoryProduct>();
            this.CancellationTypes = new List<Annulation>();
            this.GeneratedMovements = new List<Movement>();
            this.DeltaErrors = new List<DeltaError>();
            this.DeltaNodes = new List<DeltaNode>();
            this.MovementTransactionIds = new List<MovementsToDelete>();
            this.OfficialDeltaMovements = new List<OfficialDeltaMovement>();
            this.OfficialDeltaInventories = new List<OfficialDeltaInventory>();
        }

        /// <summary>
        /// Gets or sets the PendingOfficialMovements.
        /// </summary>
        /// <value>
        /// The PendingOfficialMovements.
        /// </value>
        public IEnumerable<PendingOfficialMovement> PendingOfficialMovements { get; set; }

        /// <summary>
        /// Gets or sets the PendingOfficialInventories.
        /// </summary>
        /// <value>
        /// The PendingOfficialInventories.
        /// </value>
        public IEnumerable<PendingOfficialInventory> PendingOfficialInventories { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket id.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the original movements.
        /// </summary>
        /// <value>
        /// The original movements.
        /// </value>
        public IEnumerable<ConsolidatedMovement> ConsolidationMovements { get; set; }

        /// <summary>
        /// Gets or sets the original inventories.
        /// </summary>
        /// <value>
        /// The original inventory.
        /// </value>
        public IEnumerable<ConsolidatedInventoryProduct> ConsolidationInventories { get; set; }

        /// <summary>
        /// Gets or sets the result movements.
        /// </summary>
        /// <value>
        /// The result movements.
        /// </value>
        public IEnumerable<OfficialResultMovement> OfficialResultMovements { get; set; }

        /// <summary>
        /// Gets or sets the result inventories.
        /// </summary>
        /// <value>
        /// The result inventories.
        /// </value>
        public IEnumerable<OfficialResultInventory> OfficialResultInventories { get; set; }

        /// <summary>
        /// Gets or sets the error movements.
        /// </summary>
        /// <value>
        /// The error movements.
        /// </value>
        public IEnumerable<OfficialErrorMovement> MovementErrors { get; set; }

        /// <summary>
        /// Gets or sets the error inventories.
        /// </summary>
        /// <value>
        /// The error inventories.
        /// </value>
        public IEnumerable<OfficialErrorInventory> InventoryErrors { get; set; }

        /// <summary>
        /// Gets or sets the list of movements.
        /// </summary>
        /// <value>
        /// The list of movements.
        /// </value>
        public IEnumerable<Movement> Movements { get; set; }

        /// <summary>
        /// Gets or sets the list of inventories.
        /// </summary>
        /// <value>
        /// The list of inventories.
        /// </value>
        public IEnumerable<InventoryProduct> InventoryProducts { get; set; }

        /// <summary>
        /// Gets or sets the list of annulations.
        /// </summary>
        /// <value>
        /// The list of annulation.
        /// </value>
        public IEnumerable<Annulation> CancellationTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has processing errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has processing errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasProcessingErrors { get; set; }

        /// <summary>
        /// Gets or sets the GeneratedMovements.
        /// </summary>
        /// <value>
        /// The GeneratedMovements.
        /// </value>
        public IEnumerable<Movement> GeneratedMovements { get; set; }

        /// <summary>
        /// Gets or sets the GeneratedMovements.
        /// </summary>
        /// <value>
        /// The GeneratedMovements.
        /// </value>
        public IEnumerable<DeltaError> DeltaErrors { get; set; }

        /// <summary>
        /// Gets or sets the DeltaNodes.
        /// </summary>
        /// <value>
        /// The DeltaNodes.
        /// </value>
        public IEnumerable<DeltaNode> DeltaNodes { get; set; }

        /// <summary>
        /// Gets or sets the MovementTransactionIds.
        /// </summary>
        /// <value>
        /// The MovementTransactionIds.
        /// </value>
        public IEnumerable<MovementsToDelete> MovementTransactionIds { get; set; }

        /// <summary>
        /// Gets or sets the official delta movements.
        /// </summary>
        /// <value>
        /// The official delta movements.
        /// </value>
        public IEnumerable<OfficialDeltaMovement> OfficialDeltaMovements { get; set; }

        /// <summary>
        /// Gets or sets the official delta inventories.
        /// </summary>
        /// <value>
        /// The official delta inventories.
        /// </value>
        public IEnumerable<OfficialDeltaInventory> OfficialDeltaInventories { get; set; }
    }
}
