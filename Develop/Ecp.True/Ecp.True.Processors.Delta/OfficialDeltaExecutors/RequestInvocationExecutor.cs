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

namespace Ecp.True.Processors.Delta.OfficialDeltaExecutors
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Integration;
    using Ecp.True.Processors.Delta.Services;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The RequestInvocationExecutor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Execution.ExecutorBase" />
    public class RequestInvocationExecutor : ExecutorBase
    {
        /// <summary>
        /// The delta proxy.
        /// </summary>
        private readonly IDeltaProxy deltaProxy;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The failure handler.
        /// </summary>
        private readonly IFailureHandler failureHandler;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<RequestInvocationExecutor> logger;

        /// <summary>
        /// The save integration file.
        /// </summary>
        private readonly ISaveIntegrationDeltaFile saveIntegrationDeltaFile;

        /// <summary>
        /// The integration data.
        /// </summary>
        private readonly IntegrationData integrationData = new IntegrationData(SystemType.OFFICIALDELTA);

        /// <summary>
        /// The movement aggregation strategy.
        /// </summary>
        private readonly IMovementAggregationService aggregationService;

        /// <summary>
        /// The official delta response convert service.
        /// </summary>
        private readonly IOfficialDeltaResponseConvertService responseConvertService;

        private bool shouldStoreResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestInvocationExecutor" /> class.
        /// </summary>
        /// <param name="deltaProxy">The delta proxy.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="saveIntegrationDeltaFile">The save integration file.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="aggregationService">The aggregation service.</param>
        /// <param name="responseConvertService">The response convert service.</param>
        public RequestInvocationExecutor(
             IDeltaProxy deltaProxy,
             IUnitOfWorkFactory unitOfWorkFactory,
             IFailureHandlerFactory failureHandlerFactory,
             IConfigurationHandler configurationHandler,
             ISaveIntegrationDeltaFile saveIntegrationDeltaFile,
             ITrueLogger<RequestInvocationExecutor> logger,
             IMovementAggregationService aggregationService,
             IOfficialDeltaResponseConvertService responseConvertService)
                   : base(logger)
        {
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.deltaProxy = deltaProxy;
            this.configurationHandler = configurationHandler;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.OfficialDelta);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.saveIntegrationDeltaFile = saveIntegrationDeltaFile;
            this.logger = logger;
            this.aggregationService = aggregationService;
            this.responseConvertService = responseConvertService;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.OfficialDelta;

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <inheritdoc />
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            await this.InitializeConfigAsync()
                .ConfigureAwait(false);

            var deltaData = (OfficialDeltaData)input;
            this.Logger.LogInformation($"Started {nameof(RequestInvocationExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            await this.SendOfficialAsync(deltaData).ConfigureAwait(false);
            this.Logger.LogInformation($"Completed {nameof(RequestInvocationExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            this.ShouldContinue = !deltaData.HasProcessingErrors;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        private async Task InitializeConfigAsync()
        {
            var ownershipRuleSettings = await this.configurationHandler
                .GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings)
                .ConfigureAwait(false);

            this.deltaProxy.Initialize(ownershipRuleSettings);

            this.shouldStoreResponse = ownershipRuleSettings.ShouldStoreResponse;
        }

        /// <summary>
        /// Populate Delta Response.
        /// </summary>
        /// <param name="deltaData">the delta data.</param>
        /// <param name="deltaResponse">the delta Response.</param>
        private void PopulateDeltaResponse(OfficialDeltaData deltaData, OfficialDeltaResponse deltaResponse)
        {
            ArgumentValidators.ThrowIfNull(deltaResponse, nameof(deltaResponse));

            deltaData.OfficialResultMovements = this.responseConvertService
                .ConvertOfficialDeltaResponse(deltaResponse.ResultMovements, deltaData);
            deltaData.OfficialResultInventories = deltaResponse.ResultInventories.ToResponse();
            deltaData.MovementErrors = deltaResponse.ErrorMovements.ToResponse();
            deltaData.InventoryErrors = deltaResponse.ErrorInventories.ToResponse();
        }

        /// <summary>
        /// Requests the asynchronous.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private async Task SendOfficialAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            var deltaRequest = this.BuildDeltaRequest(deltaData);

            await this.SaveDataRequestAsync(deltaData, deltaRequest).ConfigureAwait(false);

            OfficialDeltaResponse deltaResponse = null;
            try
            {
                this.Logger.LogInformation($"Request official delta from FICO for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

                deltaResponse = await this.deltaProxy.ProcessOfficialDeltaAsync(deltaRequest).ConfigureAwait(false);

                this.Logger.LogInformation($"Received official delta response from FICO for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

                this.PopulateDeltaResponse(deltaData, deltaResponse);
            }
            catch (Exception exception)
            {
                deltaData.HasProcessingErrors = true;
                this.logger.LogError(exception, $"Request to get official delta from service failed for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
                await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(deltaData.Ticket.TicketId, Constants.OfficialDeltaFailureMessage)).ConfigureAwait(false);
                return;
            }

            await this.SaveDataResponseAsync(deltaData, deltaResponse).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the data request asynchronous into file registration and azure blob storage.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <param name="deltaRequest">The delta request.</param>
        private async Task SaveDataRequestAsync(OfficialDeltaData deltaData, OfficialDeltaRequest deltaRequest)
        {
            if (!this.shouldStoreResponse)
            {
                return;
            }

            this.Logger.LogInformation($"Saving official delta request for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            this.integrationData.Id = deltaData.Ticket.TicketId;
            this.integrationData.Data = JObject.FromObject(deltaRequest).ToString();
            this.integrationData.IntegrationType = IntegrationType.REQUEST;

            this.integrationData.PreviousUploadId =
                await this.saveIntegrationDeltaFile.RegisterIntegrationAsync(this.integrationData).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the data response asynchronous into file registration and azure blob storage.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <param name="deltaResponse">The delta response.</param>
        private async Task SaveDataResponseAsync(OfficialDeltaData deltaData, OfficialDeltaResponse deltaResponse)
        {
            if (!this.shouldStoreResponse)
            {
                return;
            }

            this.Logger.LogInformation($"Saving official delta response for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            this.integrationData.Data = deltaResponse.Content;
            this.integrationData.IntegrationType = IntegrationType.RESPONSE;
            _ = await this.saveIntegrationDeltaFile.RegisterIntegrationAsync(this.integrationData).ConfigureAwait(false);
        }

        private OfficialDeltaRequest BuildDeltaRequest(OfficialDeltaData deltaData)
        {
            var officialDeltaRequest = new OfficialDeltaRequest
            {
                CancellationTypes = deltaData.CancellationTypes.ToRequest(),
                PendingOfficialInventories = deltaData.PendingOfficialInventories.ToRequest(deltaData.Ticket),
                PendingOfficialMovements = deltaData.PendingOfficialMovements.ToRequest(),
                ConsolidationInventories = deltaData.ConsolidationInventories.ToRequest(),
                ConsolidationMovements = deltaData.ConsolidationMovements.ToRequest(),
                OfficialDeltaMovements = deltaData.OfficialDeltaMovements.ToRequest(),
                OfficialDeltaInventories = deltaData.OfficialDeltaInventories.ToRequest(),
            };

            officialDeltaRequest.ConsolidationMovements = this.aggregationService
                .AggregateTolerancesAndUnidentifiedLosses(officialDeltaRequest.ConsolidationMovements, deltaData);

            return officialDeltaRequest;
        }
    }
}
