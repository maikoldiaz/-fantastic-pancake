// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipRule.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The node rule.
    /// </summary>
    public class NodeOwnershipRule : RuleEntity
    {
        /// <summary>Initializes a new instance of the <see cref="NodeOwnershipRule"/> class.</summary>
        public NodeOwnershipRule()
        {
            this.Nodes = new List<Node>();
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public ICollection<Node> Nodes { get; }
    }
}
