// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConfiguration.cs" company="Microsoft">
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
    /// The Node Configuration.
    /// </summary>
    [DisplayName("ConfiguracionesNodo")]
    public class NodeConfiguration
    {
        /// <summary>
        /// Gets or sets the node ownership rule identifier.
        /// </summary>
        /// <value>
        /// The node ownership rule identifier.
        /// </value>
        [JsonProperty("estrategiaPropiedadNodo")]
        public int? NodeOwnershipRuleId { get; set; }

        /// <summary>
        /// Gets or sets the node product ownership rule identifier.
        /// </summary>
        /// <value>
        /// The node product ownership rule identifier.
        /// </value>
        [JsonProperty("estrategiaPropiedadNodoProducto")]
        public int? NodeProductOwnershipRuleId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [JsonProperty("idNodo")]
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the node order.
        /// </summary>
        /// <value>
        /// The node order.
        /// </value>
        [JsonProperty("ordenNodo")]
        public int? NodeOrder { get; set; }

        /// <summary>
        /// Gets or sets the product order.
        /// </summary>
        /// <value>
        /// The prouct order.
        /// </value>
        [JsonProperty("ordenProducto")]
        public int ProductOrder { get; set; }

        /// <summary>
        /// Gets or sets the PI.
        /// </summary>
        /// <value>
        /// The PI.
        /// </value>
        [JsonProperty("pi")]
        public decimal IdentifiedLoss { get; set; }

        /// <summary>
        /// Gets or sets the PNI.
        /// </summary>
        /// <value>
        /// The PNI.
        /// </value>
        [JsonProperty("pni")]
        public decimal UnidentifiedLoss { get; set; }

        /// <summary>
        /// Gets or sets the INTERFASE.
        /// </summary>
        /// <value>
        /// The INTERFASE.
        /// </value>
        [JsonProperty("interfase")]
        public decimal Interface { get; set; }

        /// <summary>
        /// Gets or sets the TOLERANCIA.
        /// </summary>
        /// <value>
        /// The TOLERANCIA.
        /// </value>
        [JsonProperty("tolerancia")]
        public decimal Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the INVENTARIOFINAL.
        /// </summary>
        /// <value>
        /// The INVENTARIOFINAL.
        /// </value>
        [JsonProperty("inventarioFinal")]
        public decimal FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        [JsonIgnore]
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [JsonIgnore]
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        /// <value>
        /// The rule identifier.
        /// </value>
        [JsonIgnore]
        public int? RuleId { get; set; }

        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        /// <value>
        /// The rule.
        /// </value>
        [JsonIgnore]
        public string Rule { get; set; }

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