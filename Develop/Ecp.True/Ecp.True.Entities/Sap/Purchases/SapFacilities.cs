// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapFacilities.cs" company="Microsoft">
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
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Facilities class.
    /// </summary>
    public class SapFacilities
    {
        /// <summary>
        /// Gets or sets facility.
        /// </summary>
        [JsonProperty("FACILITY")]
        public SapFacility Facility { get; set; }
    }
}
