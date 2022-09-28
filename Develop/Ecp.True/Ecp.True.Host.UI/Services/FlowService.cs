// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Services
{
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.UI.Auth.Interfaces;
    using Ecp.True.Host.UI.Services.Interfaces;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The report service.
    /// </summary>
    public class FlowService : IFlowService
    {
        private readonly string[] scopes;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The Auth provider.
        /// </summary>
        private readonly IAuthProvider authProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowService" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="authProvider">The Auth provider.</param>
        public FlowService(IConfigurationHandler configurationHandler, IConfiguration configuration, IAuthProvider authProvider)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            this.configurationHandler = configurationHandler;
            this.authProvider = authProvider;
            this.scopes = new[] { configuration["FlowApi:FlowScope"] };
        }

        /// <summary>
        /// Gets flow configuration.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>The flow config details.</returns>
        public async Task<FlowSettings> GetFlowConfigurationAsync(string key)
        {
            var flowConfig = await this.configurationHandler.GetConfigurationAsync<FlowSettings>(ConfigurationConstants.FlowSettings).ConfigureAwait(false);
            flowConfig.AccessToken = await this.authProvider.GetUserAccessTokenAsync(this.scopes).ConfigureAwait(false);
            return flowConfig;
        }
    }
}
