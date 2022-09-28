// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestratorBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The functions base type.
    /// </summary>
    public class OrchestratorBase : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger logger;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestratorBase"/> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        protected OrchestratorBase(
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            ITrueLogger logger)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.configurationHandler = configurationHandler;
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Calling the activity.
        /// </summary>
        /// <typeparam name="T">the return type.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="durableOrchestrationContext">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns>The task.</returns>
        protected Task<T> CallActivityAsync<T>(string activity, IDurableOrchestrationContext durableOrchestrationContext, T input)
        {
            ArgumentValidators.ThrowIfNull(durableOrchestrationContext, nameof(durableOrchestrationContext));
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            if (!durableOrchestrationContext.IsReplaying)
            {
                this.logger.LogInformation($"{activity}", $"Calling activity {activity} from Orchestrator.");
            }

            return durableOrchestrationContext.CallActivityAsync<T>(activity, input);
        }

        /// <summary>
        /// Calling the activity.
        /// </summary>
        /// <typeparam name="T">the return type.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="durableOrchestrationContext">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns>The task.</returns>
        protected Task ExecuteActivityAsync<T>(string activity, IDurableOrchestrationContext durableOrchestrationContext, T input)
            where T : class
        {
            ArgumentValidators.ThrowIfNull(durableOrchestrationContext, nameof(durableOrchestrationContext));
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            if (!durableOrchestrationContext.IsReplaying)
            {
                this.logger.LogInformation($"{activity}", $"Calling activity {activity} from Orchestrator.");
            }

            return durableOrchestrationContext.CallActivityAsync(activity, input);
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            if (this.azureClientFactory.IsReady)
            {
                return;
            }

            var analysisSettings = await this.configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);
            var storageSettings = await this.configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await this.configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings));
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>The task.</returns>
        protected Task InitializeAsync()
        {
            return this.TryInitializeAsync();
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <param name="orchestratorMetaData">The orchestrator meta data.</param>
        /// <returns>The task.</returns>
        protected async Task InitializeAsync(OrchestratorMetaData orchestratorMetaData)
        {
            ArgumentValidators.ThrowIfNull(orchestratorMetaData, nameof(orchestratorMetaData));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(
                orchestratorMetaData.ChaosValue,
                string.Join(".", orchestratorMetaData.Caller, orchestratorMetaData.Orchestrator, orchestratorMetaData.Activity),
                orchestratorMetaData.ReplyTo);
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <typeparam name="T">the return type.</typeparam>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="orchestratorFunctionName">THe orchestrator function name.</param>
        /// <param name="instanceId">The instance identifier.</param>
        /// <param name="input">The input.</param>
        /// <returns>The task.</returns>
        protected async Task TryStartAsync<T>(IDurableOrchestrationClient durableOrchestrationClient, string orchestratorFunctionName, string instanceId, T input)
        {
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));
            var processingStatus = await durableOrchestrationClient.GetStatusAsync(instanceId).ConfigureAwait(false);

            if (processingStatus != null && (processingStatus.RuntimeStatus == OrchestrationRuntimeStatus.Pending || processingStatus.RuntimeStatus == OrchestrationRuntimeStatus.Running)
                && processingStatus.CustomStatus.ToString() != Constants.CustomFailureStatus)
            {
                this.logger.LogInformation(instanceId, $"Orchestration with instance id {instanceId} is {processingStatus.RuntimeStatus}");
                return;
            }

            if (processingStatus != null)
            {
                await durableOrchestrationClient.PurgeInstanceHistoryAsync(instanceId).ConfigureAwait(false);
            }

            // Calling the Orchestrator
            await durableOrchestrationClient.StartNewAsync(orchestratorFunctionName, instanceId, input).ConfigureAwait(false);
        }

        /// <summary>
        /// Purges the orchestration data asynchronous.
        /// </summary>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="key">The key.</param>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        protected async Task PurgeOrchestrationDataAsync(IDurableOrchestrationClient durableOrchestrationClient, string key, string message)
        {
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));
            var filter = new OrchestrationStatusQueryCondition
            {
                CreatedTimeTo = DateTime.UtcNow.AddMinutes(-30),
            };

            var queryResult = await durableOrchestrationClient.ListInstancesAsync(filter, System.Threading.CancellationToken.None).ConfigureAwait(false);
            foreach (var instance in queryResult.DurableOrchestrationState)
            {
                if (instance.LastUpdatedTime < DateTime.UtcNow.AddDays(-7))
                {
                    this.logger.LogInformation(key, $"{message} : {instance.InstanceId} and name : {instance.Name}");
                    await durableOrchestrationClient.PurgeInstanceHistoryAsync(instance.InstanceId).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Does the handle failure asynchronous.
        /// </summary>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="failureHandler">The failure handler.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        /// <returns>The task.</returns>
        protected async Task DoHandleFailureAsync(IDurableOrchestrationClient durableOrchestrationClient, IUnitOfWork unitOfWork, IFailureHandler failureHandler, string errorMessage)
        {
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentValidators.ThrowIfNull(failureHandler, nameof(failureHandler));
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();
            var tickets = await ticketRepository.GetAllAsync(t => t.Status == StatusType.PROCESSING).ConfigureAwait(false);

            foreach (var ticket in tickets)
            {
                var processingStatus = await durableOrchestrationClient.GetStatusAsync(ticket.TicketId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

                if (processingStatus != null && (processingStatus.RuntimeStatus == OrchestrationRuntimeStatus.Failed || processingStatus.CustomStatus.ToString() == Constants.CustomFailureStatus))
                {
                    this.logger.LogInformation($"Orchestration failed for ticket {ticket.TicketId}");
                    await failureHandler.HandleFailureAsync(unitOfWork, new FailureInfo(ticket.TicketId, errorMessage)).ConfigureAwait(false);
                }
            }
        }
    }
}
