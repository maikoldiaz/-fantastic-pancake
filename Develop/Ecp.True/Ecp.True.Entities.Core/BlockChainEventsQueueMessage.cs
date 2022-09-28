// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockChainEventsQueueMessage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    /// <summary>
    /// The Queue Message.
    /// </summary>
    public class BlockChainEventsQueueMessage
    {
        /// <summary>
        /// Gets or sets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        public string BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets the log event identifier.
        /// </summary>
        /// <value>
        /// The log event identifier.
        /// </value>
        public string LogEventId { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract.
        /// </summary>
        /// <value>
        /// The type of the contract.
        /// </value>
        public ContractType ContractType { get; set; }

        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int? OwnershipNodeId { get; set; }
    }
}
