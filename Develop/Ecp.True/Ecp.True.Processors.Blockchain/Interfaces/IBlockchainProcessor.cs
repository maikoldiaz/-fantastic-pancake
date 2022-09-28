// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlockchainProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processor.Blockchain.Events;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// The blockchain processor.
    /// </summary>
    public interface IBlockchainProcessor
    {
        /// <summary>
        /// Gets the paged events asynchronous.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="lastHead">The last head.</param>
        /// <returns>The paged events.</returns>
        Task<EventsPage> GetPagedEventsAsync(int pageSize, ulong? lastHead);

        /// <summary>
        /// Gets the events in range asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The events.
        /// </returns>
        Task<EventsPage> GetEventsInRangeAsync(BlockRangeRequest request);

        /// <summary>
        /// Gets the transaction details asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        Task<BlockTransactionResponse> GetTransactionDetailsAsync(BlockTransactionRequest request);

        /// <summary>
        /// Determines whether [has] [the specified block number].
        /// </summary>
        /// <param name="blockNumber">The block number.</param>
        /// <returns>The task.</returns>
        Task<bool> HasBlockAsync(ulong blockNumber);

        /// <summary>
        /// Tries the get block asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The Task.</returns>
        Task<Block> TryGetBlockAsync(BlockTransactionRequest request);

        /// <summary>
        /// Validates the block range asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The task.</returns>
        Task<bool> ValidateBlockRangeAsync(BlockRangeRequest request);
    }
}
