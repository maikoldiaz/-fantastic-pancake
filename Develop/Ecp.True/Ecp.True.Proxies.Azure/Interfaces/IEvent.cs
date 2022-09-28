// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEvent.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Interfaces
{
    using Nethereum.ABI.FunctionEncoding.Attributes;

    /// <summary>
    /// The event base contract.
    /// </summary>
    /// <seealso cref="Nethereum.ABI.FunctionEncoding.Attributes.IEventDTO" />
    public interface IEvent : IEventDTO
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash.
        /// </summary>
        /// <value>
        /// The transaction hash.
        /// </value>
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }
    }
}
