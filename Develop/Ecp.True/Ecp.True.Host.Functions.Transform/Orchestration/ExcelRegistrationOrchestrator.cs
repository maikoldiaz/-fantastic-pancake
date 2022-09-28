// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelRegistrationOrchestrator.cs" company="Microsoft">
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
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel registration orchestrator.
    /// </summary>
    public class ExcelRegistrationOrchestrator : RegistrationOrchestrator
    {
        /// <summary>
        /// The homologated message.
        /// </summary>
        private readonly string homologatedMessage = "Homologation completed";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ExcelRegistrationOrchestrator> logger;

        /// <summary>
        /// The homologator.
        /// </summary>
        private readonly IHomologator homologator;

        /// <summary>
        /// The excel transform processor.
        /// </summary>
        private readonly ITransformProcessor excelTransformProcessor;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelRegistrationOrchestrator" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="homologator">The homologator.</param>
        /// <param name="transformProcessors">The transform processors.</param>
        /// <param name="telemetry">The telemetry.</param>
        public ExcelRegistrationOrchestrator(
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            ITrueLogger<ExcelRegistrationOrchestrator> logger,
            IHomologator homologator,
            IEnumerable<ITransformProcessor> transformProcessors,
            ITelemetry telemetry)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            this.logger = logger;
            this.homologator = homologator;
            this.telemetry = telemetry;
            this.excelTransformProcessor = transformProcessors.Single(x => x.InputType == InputType.EXCEL);
        }

        /// <summary>
        /// Does the register asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName(nameof(ExcelRegistrationOrchestrator))]
        public async Task RegisterAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            var registrationData = context.GetInput<RegistrationData>();

            try
            {
                this.ProcessMetadata(registrationData.ChaosValue, string.Join(".", registrationData.Caller, registrationData.Orchestrator), registrationData.ReplyTo);
                var canonical = await this.CallActivityAsync(ActivityNames.ExcelCanonicalTransform, context, registrationData).ConfigureAwait(true);
                var homologated = await this.CallActivityAsync(ActivityNames.ExcelHomologate, context, canonical).ConfigureAwait(true);
                await this.ExecuteActivityAsync(ActivityNames.ExcelComplete, context, homologated).ConfigureAwait(true);

                this.logger.LogInformation(registrationData.InvocationId.ToString(), homologated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.ExcelRegistrationOrchestratorFailed, registrationData.InvocationId.ToString());
                this.telemetry.TrackEvent(Constants.Critical, EventName.RegistrationFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Does the transform asynchronous.
        /// </summary>
        /// <param name="registrationData">The message.</param>
        /// <returns>The JArray.</returns>
        [FunctionName(ActivityNames.ExcelCanonicalTransform)]
        public async Task<RegistrationData> TransformAsync([ActivityTrigger] RegistrationData registrationData)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            registrationData.Activity = ActivityNames.ExcelCanonicalTransform;
            await this.InitializeAsync(registrationData).ConfigureAwait(false);
            var result = await this.excelTransformProcessor.TransformAsync(registrationData.TrueMessage).ConfigureAwait(false);
            result.TrueMessage.PopulateFailedRecords();
            return result;
        }

        /// <summary>
        /// Does the homologate.
        /// </summary>
        /// <param name="registrationData">The message.</param>
        /// <returns>the homologated JArray.</returns>
        [FunctionName(ActivityNames.ExcelHomologate)]
        public async Task<RegistrationData> HomologateAsync([ActivityTrigger] RegistrationData registrationData)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            await this.InitializeHomologationAsync(registrationData, ActivityNames.ExcelHomologate).ConfigureAwait(false);
            var output = new JArray();
            await this.HomologateDataAsync(registrationData.Data, "Movements", registrationData.TrueMessage, output).ConfigureAwait(true);
            await this.HomologateDataAsync(registrationData.Data, "Inventory", registrationData.TrueMessage, output).ConfigureAwait(true);
            await this.HomologateDataAsync(registrationData.Data, "Events", registrationData.TrueMessage, output).ConfigureAwait(true);
            await this.HomologateDataAsync(registrationData.Data, "Contracts", registrationData.TrueMessage, output).ConfigureAwait(true);

            return new RegistrationData
            {
                Data = output,
                TrueMessage = registrationData.TrueMessage,
            };
        }

        /// <summary>
        /// Does the complete asynchronous.
        /// </summary>
        /// <param name="registrationData">The registration data.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.ExcelComplete)]
        public async Task CompleteAsync([ActivityTrigger] RegistrationData registrationData)
        {
            ArgumentValidators.ThrowIfNull(registrationData, nameof(registrationData));
            registrationData.Activity = ActivityNames.ExcelComplete;
            await this.InitializeAsync(registrationData).ConfigureAwait(false);
            await this.excelTransformProcessor.CompleteAsync(registrationData.Data, registrationData.TrueMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Homologate array.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="type">The type.</param>
        private async Task HomologateDataAsync(JArray output, string type, TrueMessage message, JArray resultOutput)
        {
            var objData = new JObject();
            var transformedData = output.OfType<JObject>().SingleOrDefault(o => o.ContainsKey(type))?.Value<JArray>(type);
            if (transformedData != null)
            {
                var homologatedData = await this.homologator.HomologateAsync(message, transformedData, true).ConfigureAwait(false);
                objData.Add(new JProperty(type, homologatedData));
                this.logger.LogInformation(this.homologatedMessage, message.MessageId);
                resultOutput.Add(objData);
            }
        }
    }
}
