// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaResultMovement.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaResultMovement.
    /// </summary>
    [DisplayName("resultadoMovimientos")]
    public class OfficialDeltaResultMovement
    {
        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        [JsonProperty("idMovimientoTRUE")]
        public string MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the movement owner identifier.
        /// </summary>
        /// <value>
        /// The movement owner identifier.
        /// </value>
        [JsonProperty("idMovimientoPropietario")]
        public string MovementOwnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OfficialDeltaResultMovement"/> is sign.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sign; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("signo")]
        public string Sign { get; set; }

        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>
        /// The delta.
        /// </value>
        [JsonProperty("deltaOficial")]
        public decimal DeltaOfficial { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        [JsonProperty("origen")]
        public OriginType Origin { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        [JsonProperty("cantidadNeta")]
        public decimal NetStandardVolume { get; set; }
    }
}
