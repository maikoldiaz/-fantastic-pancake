// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaggedNodeInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace EfCore.Models
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The Node Category Tagging class.
    /// </summary>
    public class TaggedNodeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedNodeInfo"/> class.
        /// </summary>
        public TaggedNodeInfo()
        {
            this.TaggedNodes = new List<TaggedNode>();
        }

        /// <summary>
        /// Gets or sets the node tagging identifier.
        /// </summary>
        /// <value>
        /// The node tagging identifier.
        /// </value>
        public OperationalType OperationalType { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        /// <value>
        /// The element identifier.
        /// </value>
        public int ElementId { get; set; }

        /// <summary>
        /// Gets or sets the input date.
        /// </summary>
        /// <value>
        /// The input date.
        /// </value>
        public DateTime InputDate { get; set; }

        /// <summary>
        /// Gets the grouped nodes.
        /// </summary>
        /// <value>
        /// The grouped nodes.
        /// </value>
        public IEnumerable<TaggedNode> TaggedNodes { get; }
    }
}
