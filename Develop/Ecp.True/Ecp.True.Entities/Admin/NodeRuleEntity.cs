// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRuleEntity.cs" company="Microsoft">
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
    /// Node Rule.
    /// </summary>
    /// <seealso cref="Entity" />
    public class NodeRuleEntity : VersionableEntity
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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
    }
}
