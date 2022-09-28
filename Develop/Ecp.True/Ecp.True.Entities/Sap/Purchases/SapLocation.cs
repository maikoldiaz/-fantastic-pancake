// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLocation.cs" company="Microsoft">
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
    /// The SAP PO Location class.
    /// </summary>
    public class SapLocation
    {
        /// <summary>
        /// Gets or sets destination.
        /// </summary>
        [Required(ErrorMessage = SapConstants.DestinationRequired)]
        [JsonProperty("DESTINATION")]
        public SapDestination Destination { get; set; }
    }
}
