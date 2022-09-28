// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovementRequestDestination.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Request
{
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Movement Destination class.
    /// </summary>
    public class SapMovementRequestDestination
    {
        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [JsonProperty("destinationNodeId")]
        public string DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination storage location identifier.
        /// </summary>
        /// <value>
        /// The destination storage location identifier.
        /// </value>
        [JsonProperty("destinationStorageLocationId")]
        public string DestinationStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        [JsonProperty("destinationProductId")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product type identifier.
        /// </summary>
        /// <value>
        /// The destination product type identifier.
        /// </value>
        [JsonProperty("destinationProductTypeId")]
        public string DestinationProductTypeId { get; set; }
    }
}
