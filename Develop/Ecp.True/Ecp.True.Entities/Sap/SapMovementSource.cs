// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovementSource.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The SAP PO Movement Source class.
    /// </summary>
    public class SapMovementSource
    {
        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.SourceNodeIdRequired)]
        [StringLength(150, ErrorMessage = SapConstants.SourceNodeIdLengthExceeded)]
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source storage location identifier.
        /// </summary>
        /// <value>
        /// The source storage location identifier.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.SourceStorageLocationIdLengthExceeded)]
        public string SourceStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.MovementSourceProductIdRequired)]
        [StringLength(150, ErrorMessage = SapConstants.SourceProductIdLengthExceeded)]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.SourceProductTypeIdLengthExceeded)]
        public string SourceProductTypeId { get; set; }
    }
}
