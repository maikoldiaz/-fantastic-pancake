// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleBulkUpdateRequest.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The Nodes Filter DTO.
    /// </summary>
    public class OwnershipRuleBulkUpdateRequest
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
        public IEnumerable<BulkUpdateEntity> Ids { get; set; }

        /// <summary>
        /// Gets or sets the VariableTypeIds.
        /// </summary>
        /// <value>
        /// The VariableTypeIds.
        /// </value>
        public IEnumerable<int> VariableTypeIds { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule ID.
        /// </summary>
        /// <value>
        /// The ownership rule ID.
        /// </value>
        public int OwnershipRuleId { get; set; }
    }
}
