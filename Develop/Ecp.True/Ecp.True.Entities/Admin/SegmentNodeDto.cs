// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentNodeDto.cs" company="Microsoft">
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
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// Rule entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SegmentNodeDto : Entity
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        public DateTime OperationDate { get; set; }
    }
}
