// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaErrorMovement.cs" company="Microsoft">
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
    /// The DeltaErrorMovement.
    /// </summary>
    [DisplayName("movimientosErrores")]
    public class OfficialDeltaErrorMovement
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("descriptionError")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        [JsonProperty("origen")]
        public OriginType Origin { get; set; }
    }
}
