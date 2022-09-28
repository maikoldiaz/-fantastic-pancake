// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockTransactionResponse.cs" company="Microsoft">
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
    using System;
    using Ecp.True.Core;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The block transaction response.
    /// </summary>
    public class BlockTransactionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTransactionResponse"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public BlockTransactionResponse(BlockTransactionRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            this.BlockNumber = request.BlockNumber;
            this.TransactionHash = request.TransactionHash;
        }

        /// <summary>
        /// Gets or sets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets the gas used.
        /// </summary>
        /// <value>
        /// The gas used.
        /// </value>
        public ulong GasUsed { get; set; }

        /// <summary>
        /// Gets or sets the block hash.
        /// </summary>
        /// <value>
        /// The block hash.
        /// </value>
        public string BlockHash { get; set; }

        /// <summary>
        /// Gets or sets the gas limit.
        /// </summary>
        /// <value>
        /// The gas limit.
        /// </value>
        public ulong GasLimit { get; set; }

        /// <summary>
        /// Gets or sets the transaction time.
        /// </summary>
        /// <value>
        /// The transaction time.
        /// </value>
        public DateTime? TransactionTime { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash.
        /// </summary>
        /// <value>
        /// The transaction hash.
        /// </value>
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public int? Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public JToken Content { get; set; }
    }
}
