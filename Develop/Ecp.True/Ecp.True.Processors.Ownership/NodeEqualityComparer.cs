// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeEqualityComparer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The NodeEqualityComparer.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{Ecp.True.Entities.Admin.Node}" />
    public class NodeEqualityComparer : IEqualityComparer<Node>
    {
        /// <inheritdoc/>
        public bool Equals(Node x, Node y)
        {
            return x?.NodeId == y?.NodeId;
        }

        /// <inheritdoc/>
        public int GetHashCode(Node obj)
        {
            ArgumentValidators.ThrowIfNull(obj, nameof(obj));
            return obj.NodeId.GetHashCode();
        }
    }
}
