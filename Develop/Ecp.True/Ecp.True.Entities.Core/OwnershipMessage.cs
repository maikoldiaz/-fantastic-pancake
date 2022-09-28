// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipMessage.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    /// <summary>
    /// The OwnershipMessage.
    /// </summary>
    public class OwnershipMessage
    {
        /// <summary>
        /// Gets or sets the BLOB path.
        /// </summary>
        /// <value>
        /// The BLOB path.
        /// </value>
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is movement; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Gets or sets the ownership nodeId.
        /// </summary>
        /// <value>
        /// The ownership nodeId.
        /// </value>
        public int? OwnershipNodeId { get; set; }
    }
}
