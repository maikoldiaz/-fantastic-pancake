// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesFilter.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;

    /// <summary>
    /// The Nodes Filter DTO.
    /// </summary>
    public class NodesFilter
    {
        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the node types.
        /// </summary>
        /// <value>
        /// The node types.
        /// </value>
        public IEnumerable<int> NodeTypeIds { get; set; }

        /// <summary>
        /// Gets or sets the operators.
        /// </summary>
        /// <value>
        /// The operators.
        /// </value>
        public IEnumerable<int> OperatorIds { get; set; }
    }
}
