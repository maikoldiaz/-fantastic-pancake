// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaResultMovement.cs" company="Microsoft">
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
    /// The DeltaResultMovement.
    /// </summary>
    [DisplayName("resultadoMovimientos")]
    public class DeltaResultMovement
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [JsonProperty("idMovimiento")]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        [JsonProperty("idMovimientoTRUE")]
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DeltaResultMovement"/> is sign.
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
        [JsonProperty("delta")]
        public decimal Delta { get; set; }
    }
}
