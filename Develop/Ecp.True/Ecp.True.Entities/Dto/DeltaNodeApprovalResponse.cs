// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeApprovalResponse.cs" company="Microsoft">
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
    /// <summary>
    /// The delta node approval request.
    /// </summary>
    public class DeltaNodeApprovalResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approver exist.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is approver exist; otherwise, <c>false</c>.
        /// </value>
        public bool IsApproverExist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid official delta node.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid official delta node; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidOfficialDeltaNode { get; set; }
    }
}