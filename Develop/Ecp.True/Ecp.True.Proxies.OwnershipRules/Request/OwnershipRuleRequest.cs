// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleRequest.cs" company="Microsoft">
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
    using System.ComponentModel;
    using Ecp.True.Entities.Query;
    using Newtonsoft.Json;
    using NodeConnection = Ecp.True.Entities.Query.NodeConnection;

    /// <summary>
    /// The Volume Input class.
    /// </summary>
    [DisplayName("VolInput")]
    public class OwnershipRuleRequest
    {
        /// <summary>
        /// Gets or sets the list of inventory operational data.
        /// </summary>
        /// <value>
        /// Return the list of inventory operational data.
        /// </value>
        [JsonProperty("inventarios")]
        public IEnumerable<InventoryOperationalData> InventoryOperationalData { get; set; }

        /// <summary>
        /// Gets or sets the list of previous inventory operational data.
        /// </summary>
        /// <value>
        /// Return the list of previous inventories operational data.
        /// </value>
        [JsonProperty("invetarioIniciales")]
        public IEnumerable<PreviousInventoryOperationalData> PreviousInventoryOperationalData { get; set; }

        /// <summary>
        /// Gets or sets the list of movements.
        /// </summary>
        /// <value>
        /// Return the list of movements.
        /// </value>
        [JsonProperty("movimientos")]
        public IEnumerable<MovementOperationalData> MovementsOperationalData { get; set; }

        /// <summary>
        /// Gets or sets the list of previous movement operational data.
        /// </summary>
        /// <value>
        /// Return the list of previous movement operational data.
        /// </value>
        [JsonProperty("movimientosEntrada")]
        public IEnumerable<PreviousMovementOperationalData> PreviousMovementsOperationalData { get; set; }

        /// <summary>
        /// Gets or sets the list of node configurations.
        /// </summary>
        /// <value>
        /// Return the list of node configurations.
        /// </value>
        [JsonProperty("configuracionNodo")]
        public IEnumerable<NodeConfiguration> NodeConfigurations { get; set; }

        /// <summary>
        /// Gets or sets the list of node connections.
        /// </summary>
        /// <value>
        /// Return the list of node connections.
        /// </value>
        [JsonProperty("configuracionesConexiones")]
        public IEnumerable<NodeConnection> NodeConnections { get; set; }

        /// <summary>
        /// Gets or sets the list of events.
        /// </summary>
        /// <value>
        /// Return the list of events.
        /// </value>
        [JsonProperty("eventos")]
        public IEnumerable<Event> Events { get; set; }

        /// <summary>
        /// Gets or sets the list of contracts.
        /// </summary>
        /// <value>
        /// Return the list of contracts.
        /// </value>
        [JsonProperty("movimientosComerciales")]
        public IEnumerable<Contract> Contracts { get; set; }

        /// <summary>
        /// Gets or sets the raw request.
        /// </summary>
        /// <value>
        /// The raw request.
        /// </value>
        [JsonIgnore]
        public string RawRequest { get; set; }
    }
}
