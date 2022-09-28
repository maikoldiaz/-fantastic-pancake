// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreviousMovementOperationalData.cs" company="Microsoft">
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
    /// The Previous Movements Operational Data.
    /// </summary>
    [DisplayName("PropiedadMovimientosEntrada")]
    public class PreviousMovementOperationalData
    {
        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        [JsonProperty("volumenPropiedad")]
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [JsonProperty("idMovimiento")]
        public int MovementId { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        [JsonIgnore]
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        [JsonIgnore]
        public string Owner { get; set; }
    }
}