// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeProductRule.cs" company="Microsoft">
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
    /// The node product rule.
    /// </summary>
    public class NodeProductRule : RuleEntity
    {
        /// <summary>Initializes a new instance of the <see cref="NodeProductRule"/> class.</summary>
        public NodeProductRule()
        {
            this.StorageLocationProducts = new List<StorageLocationProduct>();
        }

        /// <summary>
        /// Gets the node products.
        /// </summary>
        /// <value>
        /// The node products.
        /// </value>
        public ICollection<StorageLocationProduct> StorageLocationProducts { get; private set; }
    }
}
