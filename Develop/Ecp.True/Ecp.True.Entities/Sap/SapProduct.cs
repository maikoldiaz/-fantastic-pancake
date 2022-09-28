// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapProduct.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Product class.
    /// </summary>
    public class SapProduct
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.ProductIdRequired)]
        [StringLength(150, ErrorMessage = SapConstants.ProductIdLengthExceeded)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.ProductTypeLengthExceeded)]
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the product volume.
        /// </summary>
        /// <value>
        /// The product volume.
        /// </value>
        [Required(ErrorMessage = SapConstants.ProductsVolumeRequired)]
        [JsonProperty("NetStandardQuantity")]
        public decimal? ProductVolume { get; set; }

        /// <summary>
        /// Gets or sets the GrossStandardQuantity.
        /// </summary>
        /// <value>
        /// The GrossStandardQuantity.
        /// </value>
        public decimal? GrossStandardQuantity { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [Required(ErrorMessage = SapConstants.MeasurementUnitRequired)]
        [StringLength(50, ErrorMessage = SapConstants.MeasurementUnitLengthExceeded)]
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the batch Id.
        /// </summary>
        /// <value>
        /// The batch Id.
        /// </value>
        [StringLength(25, ErrorMessage = SapConstants.BatchIdLengthExceeded)]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IEnumerable<SapAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        public IEnumerable<SapOwner> Owners { get; set; }
    }
}
