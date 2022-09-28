// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeProductRuleEntity.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Node Product Rule Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class NodeProductRuleEntity : VersionableEntity
    {
        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        /// <value>
        /// The type of the node.
        /// </value>
        public string NodeType { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the storage location.
        /// </summary>
        /// <value>
        /// The storage location.
        /// </value>
        public string StorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        /// <value>
        /// The rule identifier.
        /// </value>
        public int? RuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        /// <value>
        /// The name of the rule.
        /// </value>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets the storage location product identifier.
        /// </summary>
        /// <value>
        /// The storage location product identifier.
        /// </value>
        public int StorageLocationProductId { get; set; }
    }
}
