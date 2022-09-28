// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResultInventory.cs" company="Microsoft">
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
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Ecp.True.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The MovementResult class.
    /// </summary>
    [DisplayName("ResultadoInventarios")]
    public class OwnershipResultInventory
    {
        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        [JsonProperty("VolumenPropiedad")]
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the response inventory id.
        /// </summary>
        /// <value>
        /// Return the response inventory id.
        /// </value>
        [JsonProperty("IdInventario")]
        public string ResponseInventoryId { get; set; }

        /// <summary>
        /// Gets the inventory id.
        /// </summary>
        /// <value>
        /// Return the inventory id.
        /// </value>
        [JsonIgnore]
        public int InventoryId => Convert.ToInt32(this.ResponseInventoryId, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("IdPropietario")]
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        [JsonProperty("PorcentajePropiedad")]
        public decimal OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the applied rule.
        /// </summary>
        /// <value>
        /// The applied rule.
        /// </value>
        [JsonProperty("ReglaAplicada")]
        public string AppliedRule { get; set; }

        /// <summary>
        /// Gets or sets the rule version.
        /// </summary>
        /// <value>
        /// The rule version.
        /// </value>
        [JsonProperty("VersionRegla")]
        public int RuleVersion { get; set; }

        /// <summary>
        /// Gets or sets the response ticket id.
        /// </summary>
        /// <value>
        /// Return the response ticket id.
        /// </value>
        [JsonProperty("IdTiquete")]
        public string ResponseTicket { get; set; }

        /// <summary>
        /// Gets the ticket id.
        /// </summary>
        /// <value>
        /// Return the ticket id.
        /// </value>
        [JsonIgnore]
        public int Ticket => Convert.ToInt32(this.ResponseTicket, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets the execution date.
        /// </summary>
        /// <value>
        /// Return the execution date.
        /// </value>
        [JsonIgnore]
        public DateTime ExecutionDate => DateTime.UtcNow.ToTrue();
    }
}
