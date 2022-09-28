// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleBulkUpdateRequestDto.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// SegmentOwnershipCalculation Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipRuleBulkUpdateRequestDto : Entity
    {
        /// <summary>
        /// Gets or sets the ownership rule type.
        /// </summary>
        /// <value>
        /// The ownership rule type.
        /// </value>
        public OwnershipRuleType OwnershipRuleType { get; set; }

        /// <summary>
        /// Gets or sets the IDs.
        /// </summary>
        /// <value>
        /// The IDs.
        /// </value>
        public IEnumerable<int> Ids { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule ID.
        /// </summary>
        /// <value>
        /// The ownership rule ID.
        /// </value>
        public int OwnershipRuleId { get; set; }
    }
}
