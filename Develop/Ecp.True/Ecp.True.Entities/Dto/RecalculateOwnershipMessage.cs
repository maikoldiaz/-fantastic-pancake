// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculateOwnershipMessage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Recalculate ownership message DTO.
    /// </summary>
    [DataContract]
    public class RecalculateOwnershipMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculateOwnershipMessage" /> class.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <param name="hasDeletedMovementOwnerships">The deleted movement ownership flag.</param>
        public RecalculateOwnershipMessage(int ownershipNodeId, bool hasDeletedMovementOwnerships)
        {
            this.OwnershipNodeId = ownershipNodeId;
            this.HasDeletedMovementOwnerships = hasDeletedMovementOwnerships;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculateOwnershipMessage"/> class.
        /// </summary>
        public RecalculateOwnershipMessage()
        {
        }

        /// <summary>
        /// Gets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        [DataMember]
        public int OwnershipNodeId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether there are deleted movement ownerships.
        /// </summary>
        /// <value>
        /// true or false.
        /// </value>
        [DataMember]
        public bool HasDeletedMovementOwnerships { get; private set; }
    }
}
