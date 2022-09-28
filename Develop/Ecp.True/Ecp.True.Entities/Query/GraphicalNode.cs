// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicalNode.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    /// <summary>
    /// The Graphical Node.
    /// </summary>
    public class GraphicalNode : QueryEntity
    {
        /// <summary>
        ///     Gets or sets the node identifier.
        /// </summary>
        /// <value>
        ///     The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the node.
        /// </summary>
        /// <value>
        ///     The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the acceptable balance percentage.
        /// </summary>
        /// <value>
        /// The Acceptable Balance Percentage.
        /// </value>
        public decimal? AcceptableBalancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the control limit.
        /// </summary>
        /// <value>
        /// The Control Limit.
        /// </value>
        public decimal? ControlLimit { get; set; }

        /// <summary>
        ///     Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        ///     The segment identifier.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        ///     Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        ///     The Operator identifier.
        /// </value>
        public string Operator { get; set; }

        /// <summary>
        ///     Gets or sets the node type.
        /// </summary>
        /// <value>
        ///     The node type.
        /// </value>
        public string NodeType { get; set; }

        /// <summary>
        ///     Gets or sets the segment color.
        /// </summary>
        /// <value>
        ///     The segment color.
        /// </value>
        public string SegmentColor { get; set; }

        /// <summary>
        ///     Gets or sets the node type icon.
        /// </summary>
        /// <value>
        ///     The node type icon.
        /// </value>
        public string NodeTypeIcon { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        ///     Gets or sets the number of input connections.
        /// </summary>
        /// <value>
        ///     The input connections .
        /// </value>
        public int? InputConnections { get; set; }

        /// <summary>
        ///     Gets or sets the number of output connections.
        /// </summary>
        /// <value>
        ///     The output connections .
        /// </value>
        public int? OutputConnections { get; set; }
    }
}