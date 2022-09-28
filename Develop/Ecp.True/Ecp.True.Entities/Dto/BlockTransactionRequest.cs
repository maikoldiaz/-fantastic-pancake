// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockTransactionRequest.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The block transaction request.
    /// </summary>
    public class BlockTransactionRequest
    {
        /// <summary>
        /// Gets or sets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        [Required(ErrorMessage = Constants.BlockNumberRequired)]
        [JsonConverter(typeof(BlockNumberConverter))]
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash.
        /// </summary>
        /// <value>
        /// The transaction hash.
        /// </value>
        [Required(ErrorMessage = Constants.TransactionHashRequired)]
        public string TransactionHash { get; set; }
    }
}
