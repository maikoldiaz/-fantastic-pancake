// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlockchainServiceProvider.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The blockchain service provider.
    /// </summary>
    public interface IBlockchainServiceProvider
    {
        /// <summary>
        /// Gets the blockchain service.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The blockchain service.
        /// </returns>
        Task<IBlockchainService> GetBlockchainServiceAsync(ServiceType type);
    }
}
