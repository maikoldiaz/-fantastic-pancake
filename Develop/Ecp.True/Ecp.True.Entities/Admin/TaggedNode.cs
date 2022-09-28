// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaggedNode.cs" company="Microsoft">
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
    /// Node Tag Type.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class TaggedNode : Entity
    {
        /// <summary>
        /// Gets or sets the node tag identifier.
        /// </summary>
        /// <value>
        /// The node tag identifier.
        /// </value>
        public int NodeTagId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }
    }
}
