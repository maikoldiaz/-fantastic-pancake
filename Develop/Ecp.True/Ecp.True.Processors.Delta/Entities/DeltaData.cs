// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Entities
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using EfCore.Models;

    /// <summary>
    /// The DeltaData.
    /// </summary>
    public class DeltaData : OrchestratorMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaData"/> class.
        /// </summary>
        public DeltaData()
        {
            this.OriginalMovements = new List<OriginalMovement>();
            this.UpdatedMovements = new List<UpdatedMovement>();
            this.OriginalInventories = new List<OriginalInventory>();
            this.UpdatedInventories = new List<UpdatedInventory>();

            this.ResultMovements = new List<ResultMovement>();
            this.ResultInventories = new List<ResultInventory>();
            this.ErrorMovements = new List<ErrorMovement>();
            this.ErrorInventories = new List<ErrorInventory>();

            this.NodeTags = new List<NodeTag>();
            this.Movements = new List<Movement>();
            this.InventoryProducts = new List<InventoryProduct>();
            this.CancellationTypes = new List<Annulation>();
            this.GeneratedMovements = new List<Movement>();
            this.DeltaErrors = new List<DeltaError>();
        }

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
        public IEnumerable<OriginalMovement> OriginalMovements { get; set; }

        /// <summary>
        /// Gets or sets the updated movements.
        /// </summary>
        /// <value>
        /// The actual movements.
        /// </value>
        public IEnumerable<UpdatedMovement> UpdatedMovements { get; set; }

        /// <summary>
        /// Gets or sets the original inventories.
        /// </summary>
        /// <value>
        /// The original inventory.
        /// </value>
        public IEnumerable<OriginalInventory> OriginalInventories { get; set; }

        /// <summary>
        /// Gets or sets the updated inventories.
        /// </summary>
        /// <value>
        /// The actual inventory.
        /// </value>
        public IEnumerable<UpdatedInventory> UpdatedInventories { get; set; }

        /// <summary>
        /// Gets or sets the result movements.
        /// </summary>
        /// <value>
        /// The result movements.
        /// </value>
        public IEnumerable<ResultMovement> ResultMovements { get; set; }

        /// <summary>
        /// Gets or sets the result inventories.
        /// </summary>
        /// <value>
        /// The result inventories.
        /// </value>
        public IEnumerable<ResultInventory> ResultInventories { get; set; }

        /// <summary>
        /// Gets or sets the error movements.
        /// </summary>
        /// <value>
        /// The error movements.
        /// </value>
        public IEnumerable<ErrorMovement> ErrorMovements { get; set; }

        /// <summary>
        /// Gets or sets the error inventories.
        /// </summary>
        /// <value>
        /// The error inventories.
        /// </value>
        public IEnumerable<ErrorInventory> ErrorInventories { get; set; }

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
        /// Gets or sets the list of NodeTags.
        /// </summary>
        /// <value>
        /// The list of inventories.
        /// </value>
        public IEnumerable<NodeTag> NodeTags { get; set; }

        /// <summary>
        /// Gets or sets the list of annulations.
        /// </summary>
        /// <value>
        /// The list of annulation.
        /// </value>
        public IEnumerable<Annulation> CancellationTypes { get; set; }

        /// <summary>
        /// Gets or sets the next cut off date.
        /// </summary>
        /// <value>
        /// The next cut off date.
        /// </value>
        public DateTime NextCutOffDate { get; set; }

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
    }
}
