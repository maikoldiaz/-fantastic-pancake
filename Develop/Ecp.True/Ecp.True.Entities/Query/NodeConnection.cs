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

namespace Ecp.True.Entities.Query
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Node Connections.
    /// </summary>
    [DisplayName("ConfiguracionesConexiones")]
    public class NodeConnection : QueryEntity
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [JsonProperty("idNodoOrigen")]
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [JsonProperty("idNodoDestino")]
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the prioritization.
        /// </summary>
        /// <value>
        /// The prioritization.
        /// </value>
        [JsonProperty("priorizacion")]
        public int? Prioritization { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule Id.
        /// </summary>
        /// <value>
        /// The ownership rule Id.
        /// </value>
        [JsonProperty("idEstrategia")]
        public int? NodeConnectionProductRuleId { get; set; }
    }
}