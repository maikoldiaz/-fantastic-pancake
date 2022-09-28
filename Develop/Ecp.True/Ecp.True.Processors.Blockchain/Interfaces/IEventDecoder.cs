// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventDecoder.cs" company="Microsoft">
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
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The event decoder.
    /// </summary>
    public interface IEventDecoder
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; }

        /// <summary>
        /// Decodes the event asynchronous.
        /// </summary>
        /// <param name="blockchainEvent">The blockchain event.</param>
        /// <returns>The decoded event JSON.</returns>
        Task<JObject> DecodeAsync(IBlockchainEvent blockchainEvent);
    }
}
