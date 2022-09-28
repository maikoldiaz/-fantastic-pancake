// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationOrchestrator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Transform.Orchestration
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Homologate;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The registration orchestrator.
    /// </summary>
    public class RegistrationOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationOrchestrator" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="homologator">The homologator.</param>
        /// <param name="transformProcessors">The transform processors.</param>
        protected RegistrationOrchestrator(
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            ITrueLogger<RegistrationOrchestrator> logger)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
        }

        /// <summary>
        /// Initializes the homologation.
        /// </summary>
        /// <param name="registrationData">The activity identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <returns>The task.</returns>
        protected async Task InitializeHomologationAsync(RegistrationData registrationData, string activity)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            registrationData.Activity = activity;
            await this.InitializeAsync(registrationData).ConfigureAwait(false);

            var configurationHandler = this.Resolve<IConfigurationHandler>();
            var refreshInterval = await configurationHandler.GetConfigurationAsync<int>(ConfigurationConstants.HomologationRefreshIntervalInSecs).ConfigureAwait(false);

            var homologationMappings = this.Resolve<IHomologationMapper>();
            await homologationMappings.InitializeAsync(refreshInterval).ConfigureAwait(false);

            var transformationMapper = this.Resolve<ITransformationMapper>();
            await transformationMapper.InitializeAsync(refreshInterval).ConfigureAwait(false);
        }
    }
}
