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
// cSpell:ignore presentó,envío,cálculo

namespace Ecp.True.Processors.Delta.Executors
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
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Integration;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The RequestInvocationExecutor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Execution.ExecutorBase" />
    public class RequestInvocationExecutor : ExecutorBase
    {
        private const string ErrorMessage = "Se presentó un error inesperado en el envío de movimientos e inventarios para el cálculo de deltas operativos. Por favor ejecute nuevamente el proceso.";

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
        private readonly IntegrationData integrationData = new IntegrationData(SystemType.OPERATIVEDELTA);

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestInvocationExecutor" /> class.
        /// </summary>
        /// <param name="deltaProxy">The delta proxy.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="saveIntegrationDeltaFile">The save integration file.</param>
        /// <param name="logger">The logger.</param>
        public RequestInvocationExecutor(
             IDeltaProxy deltaProxy,
             IUnitOfWorkFactory unitOfWorkFactory,
             IFailureHandlerFactory failureHandlerFactory,
             IConfigurationHandler configurationHandler,
             ISaveIntegrationDeltaFile saveIntegrationDeltaFile,
             ITrueLogger<RequestInvocationExecutor> logger)
                   : base(logger)
        {
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.deltaProxy = deltaProxy;
            this.configurationHandler = configurationHandler;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.Delta);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.saveIntegrationDeltaFile = saveIntegrationDeltaFile;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 3;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Delta;

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <inheritdoc />
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var deltaData = (DeltaData)input;
            await this.RequestAsync(deltaData).ConfigureAwait(false);

            this.ShouldContinue = !deltaData.HasProcessingErrors;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Populate Delta Response.
        /// </summary>
        /// <param name="deltaData">the delta data.</param>
        /// <param name="deltaResponse">the delta Response.</param>
        private static void PopulateDeltaResponse(DeltaData deltaData, DeltaResponse deltaResponse)
        {
            ArgumentValidators.ThrowIfNull(deltaResponse, nameof(deltaResponse));

            deltaData.ResultMovements = deltaResponse.ResultMovements.ToResponse();
            deltaData.ResultInventories = deltaResponse.ResultInventories.ToResponse();
            deltaData.ErrorMovements = deltaResponse.ErrorMovements.ToResponse();
            deltaData.ErrorInventories = deltaResponse.ErrorInventories.ToResponse();
        }

        /// <summary>
        /// Requests the asynchronous.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private async Task RequestAsync(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            var deltaRequest = this.BuildDeltaRequest(deltaData);
            var ownershipRuleSettings = await this.configurationHandler.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings).ConfigureAwait(false);

            if (ownershipRuleSettings.ShouldStoreResponse)
            {
                await this.SaveDataRequestAsync(deltaData, deltaRequest).ConfigureAwait(false);
            }

            this.deltaProxy.Initialize(ownershipRuleSettings);
            DeltaResponse deltaResponse;
            try
            {
                deltaResponse = await this.deltaProxy.ProcessDeltaAsync(deltaRequest, deltaData.Ticket.TicketId).ConfigureAwait(false);
                PopulateDeltaResponse(deltaData, deltaResponse);
            }
            catch (Exception exception)
            {
                deltaData.HasProcessingErrors = true;
                this.logger.LogError(exception, $"Request to get delta from service failed for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
                await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(deltaData.Ticket.TicketId, ErrorMessage)).ConfigureAwait(false);
                return;
            }

            if (ownershipRuleSettings.ShouldStoreResponse)
            {
                await this.SaveDataResponseAsync(deltaData, deltaResponse).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Saves the data request asynchronous into file registration and azure blob storage.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <param name="deltaRequest">The delta request.</param>
        private async Task SaveDataRequestAsync(DeltaData deltaData, DeltaRequest deltaRequest)
        {
            this.Logger.LogInformation($"Saving operative delta request for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
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
        private async Task SaveDataResponseAsync(DeltaData deltaData, DeltaResponse deltaResponse)
        {
            this.Logger.LogInformation($"Saving operative delta response for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            this.integrationData.Data = deltaResponse.Content;
            this.integrationData.IntegrationType = IntegrationType.RESPONSE;
            _ = await this.saveIntegrationDeltaFile.RegisterIntegrationAsync(this.integrationData).ConfigureAwait(false);
        }

        private DeltaRequest BuildDeltaRequest(DeltaData deltaData)
        {
            return new DeltaRequest
            {
                OriginalMovements = deltaData.OriginalMovements.ToRequest(),
                UpdatedMovements = deltaData.UpdatedMovements.ToRequest(),
                OriginalInventories = deltaData.OriginalInventories.ToRequest(),
                UpdatedInventories = deltaData.UpdatedInventories.ToRequest(),
            };
        }
    }
}
