// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestInvocationExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Integration;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;

    /// <summary>
    /// The RequestInvocationExecutor class.
    /// </summary>
    public class RequestInvocationExecutor : ExecutorBase
    {
        /// <summary>
        /// The ownership rule proxy.
        /// </summary>
        private readonly IOwnershipRuleProxy ownershipRuleProxy;

        /// <summary>
        /// The ownership calculation service.
        /// </summary>
        private readonly IOwnershipCalculationService ownershipCalculationService;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The save integration file.
        /// </summary>
        private readonly ISaveIntegrationOwnershipFile saveIntegrationOwnershipFile;

        /// <summary>
        /// The integration data.
        /// </summary>
        private readonly IntegrationData integrationData = new IntegrationData(SystemType.OWNERSHIP);

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestInvocationExecutor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipRuleProxy">The ownership rule proxy.</param>
        /// <param name="ownershipCalculationService">The ownership calculation service.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="saveIntegrationOwnershipFile">The save integration file.</param>
        public RequestInvocationExecutor(
            ITrueLogger<RequestInvocationExecutor> logger,
            IOwnershipRuleProxy ownershipRuleProxy,
            IOwnershipCalculationService ownershipCalculationService,
            IConfigurationHandler configurationHandler,
            ISaveIntegrationOwnershipFile saveIntegrationOwnershipFile)
            : base(logger)
        {
            this.ownershipRuleProxy = ownershipRuleProxy;
            this.ownershipCalculationService = ownershipCalculationService;
            this.configurationHandler = configurationHandler;
            this.saveIntegrationOwnershipFile = saveIntegrationOwnershipFile;
        }

        /// <inheritdoc/>
        public override int Order => 3;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Ownership;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var ownershipRuleData = (OwnershipRuleData)input;
            var ownershipRuleSettings = await this.configurationHandler.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings).ConfigureAwait(false);

            this.ownershipRuleProxy.Initialize(ownershipRuleSettings);
            try
            {
                ownershipRuleData.OwnershipRuleResponse = await this.ownershipRuleProxy.ProcessOwnershipAsync(
                                                                        ownershipRuleData.OwnershipRuleRequest,
                                                                        ownershipRuleData.TicketId)
                                                                        .ConfigureAwait(false);
                if (ownershipRuleSettings.ShouldStoreResponse)
                {
                    await this.SaveDataRequestResponseAsync(ownershipRuleData).ConfigureAwait(false);
                }

                await this.ownershipCalculationService.AddOwnershipNodesAsync(ownershipRuleData.GetOwnershipNodes(), ownershipRuleData.TicketId).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, $"{ownershipRuleData.TicketId}", $"Error occurred while executing the RequestInvocationExecutor: {exception.Message}");
                ownershipRuleData.Errors = new List<ErrorInfo> { new ErrorInfo(exception.Message) };
                await this.SaveDataRequestAsync(ownershipRuleData).ConfigureAwait(false);
            }

            this.ShouldContinue = !ownershipRuleData.Errors.Any();
            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }

        private async Task SaveDataRequestResponseAsync(OwnershipRuleData ownershipRuleData)
        {
            await this.SaveDataRequestAsync(ownershipRuleData).ConfigureAwait(false);
            await this.SaveDataResponseAsync(ownershipRuleData).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the data request asynchronous into file registration and azure blob storage.
        /// </summary>
        /// <param name="ownershipRuleData">The ownership data.</param>
        private async Task SaveDataRequestAsync(OwnershipRuleData ownershipRuleData)
        {
            this.Logger.LogInformation($"Saving official delta request for ticket {ownershipRuleData.TicketId}", ownershipRuleData.TicketId);

            this.integrationData.Id = ownershipRuleData.TicketId;
            this.integrationData.Data = ownershipRuleData.OwnershipRuleRequest.RawRequest;
            this.integrationData.IntegrationType = IntegrationType.REQUEST;

            this.integrationData.PreviousUploadId =
                await this.saveIntegrationOwnershipFile.RegisterIntegrationAsync(this.integrationData).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the data response asynchronous into file registration and azure blob storage.
        /// </summary>
        /// <param name="ownershipRuleData">The ownership data.</param>
        private async Task SaveDataResponseAsync(OwnershipRuleData ownershipRuleData)
        {
            this.Logger.LogInformation($"Saving official delta response for ticket {ownershipRuleData.TicketId}", ownershipRuleData.TicketId);

            this.integrationData.Data = ownershipRuleData.OwnershipRuleResponse.ResponseContent;
            this.integrationData.IntegrationType = IntegrationType.RESPONSE;
            _ = await this.saveIntegrationOwnershipFile.RegisterIntegrationAsync(this.integrationData).ConfigureAwait(false);
        }
    }
}
