// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipApprovalRequest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core.Attributes;

    /// <summary>
    /// Node Ownership Approval Request DTO.
    /// </summary>
    public class NodeOwnershipApprovalRequest
    {
        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        [Required(ErrorMessage = Constants.OwnershipNodeIdMandatory)]
        public int OwnershipNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Approver Alias.
        /// </summary>
        /// <value>
        /// The Approver Alias.
        /// </value>
        [Required(ErrorMessage = Constants.ApproverAliasMandatory)]
        public string ApproverAlias { get; set; }

        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        /// <value>
        /// The Comment.
        /// </value>
        [RequiredIf("Status", "REJECTED", ErrorMessage = Constants.RejectedStatusCommentNeeded)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        [Required(ErrorMessage = Constants.StatusMandatory)]
        public string Status { get; set; }
    }
}
