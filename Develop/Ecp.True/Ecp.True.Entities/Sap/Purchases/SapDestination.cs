// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapDestination.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap.Purchases
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Destination class.
    /// </summary>
    public class SapDestination
    {
        /// <summary>
        /// Gets or sets destination location.
        /// </summary>
        [Required(ErrorMessage = SapConstants.DestinationLocationRequired)]
        [JsonProperty("DESTINATIONLOCATION")]
        public string DestinationLocation { get; set; }

        /// <summary>
        /// Gets or sets destination node.
        /// </summary>
        [Required(ErrorMessage = SapConstants.DestinationNodeRequired)]
        [JsonProperty("DESTINATIONNODE")]
        public string DestinationNode { get; set; }
    }
}
