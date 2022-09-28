// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainServiceProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Blockchain.Interfaces;

    /// <summary>
    /// The blockchain service provider.
    /// </summary>
    public class BlockchainServiceProvider : IBlockchainServiceProvider
    {
        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The services.
        /// </summary>
        private readonly IBlockchainService[] services;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainServiceProvider" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="services">The services.</param>
        public BlockchainServiceProvider(IConfigurationHandler configurationHandler, IBlockchainService[] services)
        {
            this.configurationHandler = configurationHandler;
            this.services = services;
        }

        /// <inheritdoc/>
        public async Task<IBlockchainService> GetBlockchainServiceAsync(ServiceType type)
        {
            var configuration = await this.configurationHandler.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings).ConfigureAwait(false);

            var service = this.services.Single(s => s.Type == type);
            service.Initialize(configuration);

            return service;
        }
    }
}
