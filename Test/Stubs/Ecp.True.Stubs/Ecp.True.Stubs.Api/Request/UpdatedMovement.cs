// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdatedMovement.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Request
{
    using System.ComponentModel;

    using Newtonsoft.Json;

    /// <summary>
    /// The UpdatedMovement.
    /// </summary>
    [DisplayName("movimientosActualizados")]
    public class UpdatedMovement
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
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        [JsonProperty("valor")]
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [JsonProperty("accion")]
        public string EventType { get; set; }
    }
}
