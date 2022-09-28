// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleRefreshHistory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The ownership rule refresh history.
    /// </summary>
    public class OwnershipRuleRefreshHistory : Entity
    {
        /// <summary>
        /// Gets or sets the ownership rule refresh history identifier.
        /// </summary>
        /// <value>
        /// The ownership rule refresh history identifier.
        /// </value>
        public int OwnershipRuleRefreshHistoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OwnershipRuleRefreshHistory"/> is status.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets a requested by.
        /// </summary>
        /// <value>
        ///   Requested by.
        /// </value>
        public string RequestedBy { get; set; }
    }
}
