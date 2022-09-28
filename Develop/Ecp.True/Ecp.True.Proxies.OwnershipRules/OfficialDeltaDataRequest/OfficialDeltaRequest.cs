// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaRequest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaRequest.
    /// </summary>
    public class OfficialDeltaRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaRequest"/> class.
        /// </summary>
        public OfficialDeltaRequest()
        {
            this.CancellationTypes = new List<OfficialDeltaCancellationTypes>();
            this.PendingOfficialInventories = new List<OfficialDeltaPendingOfficialInventory>();
            this.ConsolidationInventories = new List<OfficialDeltaConsolidatedInventoryProduct>();
            this.PendingOfficialMovements = new List<OfficialDeltaPendingOfficialMovement>();
            this.ConsolidationMovements = new List<OfficialDeltaConsolidatedMovement>();
            this.OfficialDeltaMovements = new List<OfficialDeltaMovement>();
            this.OfficialDeltaInventories = new List<OfficialDeltaInventory>();
        }

        /// <summary>
        /// Gets or sets the cancellation types.
        /// </summary>
        /// <value>
        /// The cancellation types.
        /// </value>
        [JsonProperty("configuracionTiposMovimientos")]
        public IEnumerable<OfficialDeltaCancellationTypes> CancellationTypes { get; set; }

        /// <summary>
        /// Gets or sets the pending official inventories.
        /// </summary>
        /// <value>
        /// The pending official inventories.
        /// </value>
        [JsonProperty("balanceOficialInventarios")]
        public IEnumerable<OfficialDeltaPendingOfficialInventory> PendingOfficialInventories { get; set; }

        /// <summary>
        /// Gets or sets the consolidation inventories.
        /// </summary>
        /// <value>
        /// The consolidation inventories.
        /// </value>
        [JsonProperty("consolidadosInventarios")]
        public IEnumerable<OfficialDeltaConsolidatedInventoryProduct> ConsolidationInventories { get; set; }

        /// <summary>
        /// Gets or sets the pending official movements.
        /// </summary>
        /// <value>
        /// The pending official movements.
        /// </value>
        [JsonProperty("balanceOficialMovimientos")]
        public IEnumerable<OfficialDeltaPendingOfficialMovement> PendingOfficialMovements { get; set; }

        /// <summary>
        /// Gets or sets the consolidation movements.
        /// </summary>
        /// <value>
        /// The consolidation movements.
        /// </value>
        [JsonProperty("consolidadosMovimientos")]
        public IEnumerable<OfficialDeltaConsolidatedMovement> ConsolidationMovements { get; set; }

        /// <summary>
        /// Gets or sets the official delta movements.
        /// </summary>
        /// <value>
        /// The official delta movements.
        /// </value>
        [JsonProperty("deltasOficialesMovimientos")]
        public IEnumerable<OfficialDeltaMovement> OfficialDeltaMovements { get; set; }

        /// <summary>
        /// Gets or sets the official delta inventories.
        /// </summary>
        /// <value>
        /// The official delta inventories.
        /// </value>
        [JsonProperty("deltasOficialesInventarios")]
        public IEnumerable<OfficialDeltaInventory> OfficialDeltaInventories { get; set; }
    }
}
