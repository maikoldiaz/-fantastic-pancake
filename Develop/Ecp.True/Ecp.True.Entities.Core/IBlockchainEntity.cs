// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlockchainEntity.cs" company="Microsoft">
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
    /// The temp entity.
    /// </summary>
    public interface IBlockchainEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the Transaction Hash.
        /// </summary>
        /// <value>
        /// The Transaction Hash.
        /// </value>
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the Block Number.
        /// </summary>
        /// <value>
        /// The Block Number.
        /// </value>
        public string BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [blockchain status].
        /// </summary>
        /// <value>
        ///   StatusType.
        /// </value>
        public StatusType BlockchainStatus { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }
    }
}