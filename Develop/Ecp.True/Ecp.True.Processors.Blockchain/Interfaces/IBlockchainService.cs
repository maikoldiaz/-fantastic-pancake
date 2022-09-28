// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlockchainService.cs" company="Microsoft">
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
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The blockchain base interface.
    /// </summary>
    public interface IBlockchainService
    {
        /// <summary>
        /// Gets the service type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        ServiceType Type { get; }

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>Returns the completed task.</returns>
        Task RegisterAsync(int entityId);

        /// <summary>
        /// Initializes the specified blockchain configuration.
        /// </summary>
        /// <param name="blockchainConfiguration">The blockchain configuration.</param>
        void Initialize(BlockchainSettings blockchainConfiguration);
    }
}
