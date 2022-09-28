// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainEntity.cs" company="Microsoft">
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
    /// The blockchain entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    /// <seealso cref="Ecp.True.Entities.Core.IBlockchainEntity" />
    public class BlockchainEntity : Entity, IBlockchainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainEntity"/> class.
        /// </summary>
        protected BlockchainEntity()
        {
        }

        /// <inheritdoc/>
        public string TransactionHash { get; set; }

        /// <inheritdoc/>
        public string BlockNumber { get; set; }

        /// <inheritdoc/>
        public StatusType BlockchainStatus { get; set; }

        /// <inheritdoc/>
        public int RetryCount { get; set; }
    }
}
