// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonRegistrationOrchestrator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Transform.Orchestrator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Transform.Orchestration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Homologate;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The JSON registration orchestrator.
    /// </summary>
    public class JsonRegistrationOrchestrator : RegistrationOrchestrator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<JsonRegistrationOrchestrator> logger;

        /// <summary>
        /// The homologator.
        /// </summary>
        private readonly IHomologator homologator;

        /// <summary>
        /// The json transform processor.
        /// </summary>
        private readonly ITransformProcessor jsonTransformProcessor;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRegistrationOrchestrator" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="homologator">The homologator.</param>
        /// <param name="transformProcessors">The transform processors.</param>
        /// <param name="telemetry">The telemetry.</param>
        public JsonRegistrationOrchestrator(
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            ITrueLogger<JsonRegistrationOrchestrator> logger,
            IHomologator homologator,
            IEnumerable<ITransformProcessor> transformProcessors,
            ITelemetry telemetry)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            this.logger = logger;
            this.homologator = homologator;
            this.telemetry = telemetry;
            this.jsonTransformProcessor = transformProcessors.Single(x => x.InputType == InputType.JSON);
        }

        /// <summary>
        /// Does the register asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName(nameof(JsonRegistrationOrchestrator))]
        public async Task RegisterAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            var registrationData = context.GetInput<RegistrationData>();

            try
            {
                this.ProcessMetadata(registrationData.ChaosValue, string.Join(".", registrationData.Caller, registrationData.Orchestrator), registrationData.ReplyTo);
                var canonical = await this.CallActivityAsync(ActivityNames.JsonCanonicalTransform, context, registrationData).ConfigureAwait(true);
                var homologated = await this.CallActivityAsync(ActivityNames.JsonHomologate, context, canonical).ConfigureAwait(true);
                await this.ExecuteActivityAsync(ActivityNames.JsonComplete, context, homologated).ConfigureAwait(true);

                this.logger.LogInformation(registrationData.InvocationId.ToString(), homologated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.JsonRegistrationOrchestratorFailed, registrationData.InvocationId.ToString());
                this.telemetry.TrackEvent(Constants.Critical, EventName.RegistrationFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Does the transform asynchronous.
        /// </summary>
        /// <param name="registrationData">The registration data.</param>
        /// <returns>The JArray.</returns>
        [FunctionName(ActivityNames.JsonCanonicalTransform)]
        public async Task<RegistrationData> TransformAsync([ActivityTrigger] RegistrationData registrationData)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            registrationData.Activity = ActivityNames.JsonCanonicalTransform;
            await this.InitializeAsync(registrationData).ConfigureAwait(false);
            var result = await this.jsonTransformProcessor.TransformAsync(registrationData.TrueMessage).ConfigureAwait(false);
            result.TrueMessage.PopulateFailedRecords();
            return result;
        }

        /// <summary>
        /// Does the homologate.
        /// </summary>
        /// <param name="registrationData">The registration data.</param>
        /// <returns>the homologated JArray.</returns>
        [FunctionName(ActivityNames.JsonHomologate)]
        public async Task<RegistrationData> HomologateAsync([ActivityTrigger] RegistrationData registrationData)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            await this.InitializeHomologationAsync(registrationData, ActivityNames.JsonHomologate).ConfigureAwait(false);
            var output = registrationData.TrueMessage.IsRetry ? this.homologator.HomologateObject(registrationData.TrueMessage, registrationData.Data.FirstOrDefault())
                : await this.homologator.HomologateAsync(registrationData.TrueMessage, registrationData.Data, registrationData.TrueMessage.ShouldHomologate).ConfigureAwait(false);

            return new RegistrationData
            {
                TrueMessage = registrationData.TrueMessage,
                Data = output,
            };
        }

        /// <summary>
        /// Does the complete asynchronous.
        /// </summary>
        /// <param name="registrationData">The registration data.</param>
        /// <returns>The Task.</returns>
        [FunctionName(ActivityNames.JsonComplete)]
        public async Task CompleteAsync([ActivityTrigger] RegistrationData registrationData)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            registrationData.Activity = ActivityNames.JsonComplete;
            await this.InitializeAsync(registrationData).ConfigureAwait(false);
            await this.jsonTransformProcessor.CompleteAsync(registrationData.Data, registrationData.TrueMessage).ConfigureAwait(false);
        }
    }
}
