// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapOwner.cs" company="Microsoft">
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
    /// The SAP PO owner class.
    /// </summary>
    public class SapOwner
    {
        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.OwnerIdRequired)]
        [StringLength(150, ErrorMessage = SapConstants.OwnerIdLengthExceeded)]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        [Required(ErrorMessage = SapConstants.OwnershipValueRequired)]
        public decimal? OwnershipValue { get; set; }

        /// <summary>
        /// Gets or sets the owner ship value unit.
        /// </summary>
        /// <value>
        /// The owner ship value unit.
        /// </value>
        [Required(ErrorMessage = SapConstants.OwnershipValueUnitRequired)]
        [StringLength(50, ErrorMessage = SapConstants.OwnershipValueUnitLengthExceeded)]
        public string OwnershipValueUnit { get; set; }
    }
}
