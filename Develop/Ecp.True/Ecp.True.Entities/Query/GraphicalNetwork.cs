// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicalNetwork.cs" company="Microsoft">
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
    using System.Collections.Generic;

    /// <summary>
    /// The Graphical Network.
    /// </summary>
    public class GraphicalNetwork : QueryEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicalNetwork"/> class.
        /// </summary>
        /// <param name="graphicalNodes">The graphical nodes.</param>
        /// <param name="graphicalNodeConnections">The graphical node connections.</param>
        public GraphicalNetwork(IEnumerable<GraphicalNode> graphicalNodes, IEnumerable<GraphicalNodeConnection> graphicalNodeConnections)
        {
            this.GraphicalNodes = graphicalNodes;
            this.GraphicalNodeConnections = graphicalNodeConnections;
        }

        /// <summary>
        ///     Gets or sets the GraphicalNodes.
        /// </summary>
        /// <value>
        ///     The GraphicalNodes.
        /// </value>
        public IEnumerable<GraphicalNode> GraphicalNodes { get; set; }

        /// <summary>
        ///     Gets or sets the GraphicalNodes.
        /// </summary>
        /// <value>
        ///     The GraphicalNodes.
        /// </value>
        public IEnumerable<GraphicalNodeConnection> GraphicalNodeConnections { get; set; }
    }
}