// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaRequest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Request
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaRequest.
    /// </summary>
    public class DeltaRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaRequest"/> class.
        /// </summary>
        public DeltaRequest()
        {
            this.OriginalMovements = new List<DeltaOriginalMovement>();
            this.UpdatedMovements = new List<DeltaUpdatedMovement>();
            this.OriginalInventories = new List<DeltaOriginalInventory>();
            this.UpdatedInventories = new List<DeltaUpdatedInventory>();
        }

        /// <summary>
        /// Gets or sets the original movements.
        /// </summary>
        /// <value>
        /// The original movements.
        /// </value>
        [JsonProperty("movimientosOriginales")]
        public IEnumerable<DeltaOriginalMovement> OriginalMovements { get; set; }

        /// <summary>
        /// Gets or sets the updated movements.
        /// </summary>
        /// <value>
        /// The actual movements.
        /// </value>
        [JsonProperty("movimientosActualizados")]
        public IEnumerable<DeltaUpdatedMovement> UpdatedMovements { get; set; }

        /// <summary>
        /// Gets or sets the original inventories.
        /// </summary>
        /// <value>
        /// The original inventory.
        /// </value>
        [JsonProperty("inventarioOriginales")]
        public IEnumerable<DeltaOriginalInventory> OriginalInventories { get; set; }

        /// <summary>
        /// Gets or sets the updated inventories.
        /// </summary>
        /// <value>
        /// The actual inventory.
        /// </value>
        [JsonProperty("inventarioActualizados")]
        public IEnumerable<DeltaUpdatedInventory> UpdatedInventories { get; set; }
    }
}
