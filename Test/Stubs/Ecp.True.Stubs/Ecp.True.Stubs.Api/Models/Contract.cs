// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contract.cs" company="Microsoft">
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
    using Newtonsoft.Json;

    /// <summary>
    /// The Contract class.
    /// </summary>
    public class Contract
    {
        /// <summary>
        /// Gets a value indicating whether contract is final.
        /// </summary>
        /// <value>
        /// The value indicating whether contract is final or not.
        /// </value>
        [JsonProperty("finalizado")]
        public bool IsFinal => false;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("idContrato")]
        public int ContractId { get; set; }

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
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the buyer owner identifier.
        /// </summary>
        /// <value>
        /// The buyer owner.
        /// </value>
        [JsonProperty("idPropietarioComprador")]
        public int BuyerOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the seller owner identifier.
        /// </summary>
        /// <value>
        /// The seller owner identifier.
        /// </value>
        [JsonProperty("idPropietarioVendedor")]
        public int SellerOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the contract unit.
        /// </summary>
        /// <value>
        /// The contract unit.
        /// </value>
        [JsonIgnore]
        public string ContractUnit { get; set; }

        /// <summary>
        /// Gets the response contract unit.
        /// </summary>
        /// <value>
        /// The response contract unit.
        /// </value>
        [JsonProperty("unidad")]
        public string ResponseContractUnit
        {
            get
            {
                return this.ContractUnit == "%" ? "PORCENTAJE" : "VOLUMEN";
            }
        }

        /// <summary>
        /// Gets or sets the contract value.
        /// </summary>
        /// <value>
        /// The contract value.
        /// </value>
        [JsonProperty("valor")]
        public decimal? ContractValue { get; set; }
    }
}
