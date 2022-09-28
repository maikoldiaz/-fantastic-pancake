// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipErrorMovement.cs" company="Microsoft">
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
    /// The Movement Error class.
    /// </summary>
    [DisplayName("MovimientosErrores")]
    public class OwnershipErrorMovement
    {
        /// <summary>
        /// Gets or sets the response source node id.
        /// </summary>
        /// <value>
        /// Return the response source node id.
        /// </value>
        [JsonProperty("IdNodo")]
        public string ResponseSourceNodeId { get; set; }

        /// <summary>
        /// Gets the node id.
        /// </summary>
        /// <value>
        /// Return the node id.
        /// </value>
        [JsonIgnore]
        public int SourceNodeId => Convert.ToInt32(this.ResponseSourceNodeId, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets the movement id.
        /// </summary>
        /// <value>
        /// Return the movement id.
        /// </value>
        [JsonProperty("IdMovimiento")]
        public string ResponseMovementId { get; set; }

        /// <summary>
        /// Gets the movement id.
        /// </summary>
        /// <value>
        /// Return the movement id.
        /// </value>
        [JsonIgnore]
        public int MovementId => Convert.ToInt32(this.ResponseMovementId, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets the ticket id.
        /// </summary>
        /// <value>
        /// Return the ticket id.
        /// </value>
        [JsonProperty("IdTiquete")]
        public string Ticket { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// Return the error description.
        /// </value>
        [JsonProperty("DescriptionError")]
        public string ErrorDescription { get; set; }

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
