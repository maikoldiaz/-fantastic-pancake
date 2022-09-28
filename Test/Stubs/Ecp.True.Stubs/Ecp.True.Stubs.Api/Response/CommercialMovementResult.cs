// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommercialMovementsResult.cs" company="Microsoft">
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
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The ResultCommercialMovements class.
    /// </summary>
    [DisplayName("ResultadoMovimientosComerciales")]
    public class CommercialMovementResult
    {
        /// <summary>
        /// Gets or sets movement id.
        /// </summary>
        /// <value>
        /// Return the movement id.
        /// </value>
        [JsonProperty("IdMovimiento")]
        public int MovementId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movement type.
        /// </summary>
        /// <value>
        /// The name of the movement type.
        /// </value>
        [JsonProperty("TipoMovimiento")]
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("IdContrato")]
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        [JsonProperty("Volumen")]
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("IdPropietario")]
        public int OwnerId { get; set; }

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
