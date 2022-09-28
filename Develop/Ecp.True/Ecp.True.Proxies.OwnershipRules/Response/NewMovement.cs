// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewMovement.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Response
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The MovementResult class.
    /// </summary>
    [DisplayName("MovimientosNuevos")]
    public class NewMovement
    {
        /// <summary>
        /// Gets or sets the agreement type.
        /// </summary>
        /// <value>
        /// Return the agreement type.
        /// </value>
        [JsonProperty("TipoAcuerdo")]
        public string AgreementType { get; set; }

        /// <summary>
        /// Gets or sets the log event identifier.
        /// </summary>
        /// <value>
        /// The log event identifier.
        /// </value>
        [JsonProperty("IdEvento")]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [JsonProperty("IdNodo")]
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("IdProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets movement id.
        /// </summary>
        /// <value>
        /// Return the movement id.
        /// </value>
        [JsonProperty("IdMovimiento")]
        public int MovementId { get; set; }

        /// <summary>
        /// Gets or sets the creditor owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("IdPropietarioAcreedor")]
        public int CreditorOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the creditor owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("IdPropietarioDeudor")]
        public int DebtorOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        [JsonProperty("VolumenPropiedad")]
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the Applied Rule.
        /// </summary>
        /// <value>
        /// The AppliedRule.
        /// </value>
        [JsonProperty("ReglaAplicada")]
        public string AppliedRule { get; set; }

        /// <summary>
        /// Gets or sets the Rule Version.
        /// </summary>
        /// <value>
        /// The RuleVersion.
        /// </value>
        [JsonProperty("VersionRegla")]
        public string RuleVersion { get; set; }
    }
}
