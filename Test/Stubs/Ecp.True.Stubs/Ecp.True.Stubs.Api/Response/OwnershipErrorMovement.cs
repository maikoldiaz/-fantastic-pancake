// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipErrorMovement.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Response
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Movement Error class.
    /// </summary>
    [DisplayName("MovimientosErrores")]
    public class OwnershipErrorMovement
    {
        /// <summary>
        /// Gets or sets the inventory id.
        /// </summary>
        /// <value>
        /// Return the inventory id.
        /// </value>
        [JsonProperty("IdInventario")]
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// Return the node id.
        /// </value>
        [JsonProperty("IdNodo")]
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the movement id.
        /// </summary>
        /// <value>
        /// Return the movement id.
        /// </value>
        [JsonProperty("IdMovimiento")]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the ticket id.
        /// </summary>
        /// <value>
        /// Return the ticket id.
        /// </value>
        [JsonProperty("IdTiquete")]
        public string Ticket { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// Return the error description.
        /// </value>
        [JsonProperty("DescriptionError")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// Return the execution date.
        /// </value>
        [JsonProperty("FechaEjecucion")]
        public DateTime ExecutionDate { get; set; }
    }
}
