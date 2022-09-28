// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionProductRuleEntity.cs" company="Microsoft">
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
    /// Node Connection Product Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class NodeConnectionProductRuleEntity : VersionableEntity
    {
        /// <summary>
        /// Gets or sets the node connection product identifier.
        /// </summary>
        /// <value>
        /// The node connection product identifier.
        /// </value>
        public int NodeConnectionProductId { get; set; }

        /// <summary>
        /// Gets or sets the source operator.
        /// </summary>
        /// <value>
        /// The source operator.
        /// </value>
        public string SourceOperator { get; set; }

        /// <summary>
        /// Gets or sets the destination operator.
        /// </summary>
        /// <value>
        /// The destination operator.
        /// </value>
        public string DestinationOperator { get; set; }

        /// <summary>
        /// Gets or sets a value source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets a value destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets a value product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the rule name.
        /// </summary>
        /// <value>
        /// The rule name.
        /// </value>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets the rule id.
        /// </summary>
        /// <value>
        /// The rule id.
        /// </value>
        public int? RuleId { get; set; }
    }
}
