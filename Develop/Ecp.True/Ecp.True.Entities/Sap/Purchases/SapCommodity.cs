// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapCommodity.cs" company="Microsoft">
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
    /// The SAP PO SapCommodity class.
    /// </summary>
    public class SapCommodity
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required(ErrorMessage = SapConstants.NameRequired)]
        [StringLength(20, ErrorMessage = SapConstants.NameLength)]
        [JsonProperty("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets type.
        /// </summary>
        [JsonProperty("TYPE")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets quantity.
        /// </summary>
        [JsonProperty("QUANTITY")]
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets quantity uom.
        /// </summary>
        [JsonProperty("QUANTITYUOM")]
        public string QuantityUom { get; set; }
    }
}
