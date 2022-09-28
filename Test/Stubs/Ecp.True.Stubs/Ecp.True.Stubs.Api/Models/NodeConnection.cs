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

namespace Ecp.True.Stubs.Api.Models
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Node Connections.
    /// </summary>
    [DisplayName("ConfiguracionesConexiones")]
    public class NodeConnection
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

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        [JsonIgnore]
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        [JsonIgnore]
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [JsonIgnore]
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonIgnore]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        [JsonIgnore]
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        [JsonIgnore]
        public decimal? OwnershipPercentage { get; set; }
    }
}