// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Reporting
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Reporting.Interfaces;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The reporting orchestrator.
    /// </summary>
    public class ReportGenerator : FunctionBase
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IReportGeneratorFactory factory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ReportGenerator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerator" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public ReportGenerator(IReportGeneratorFactory factory, ITrueLogger<ReportGenerator> logger, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.factory = factory;
            this.logger = logger;
        }

        /// <summary>
        /// Generates the before cut off asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName("BeforeCutoff")]
        public Task GenerateBeforeCutOffAsync(
           [ServiceBusTrigger("%BeforeCutoffReportQueue%", Connection = "IntegrationServiceBusConnectionString")] int executionId,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return this.ProcessAsync(label, replyTo, executionId, ReportType.BeforeCutOff);
        }

        /// <summary>
        /// Generates the official initial balance asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName("OfficialInitialBalance")]
        public Task GenerateOfficialInitialBalanceAsync(
           [ServiceBusTrigger("%OfficialInitialBalanceReportQueue%", Connection = "IntegrationServiceBusConnectionString")] int executionId,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return this.ProcessAsync(label, replyTo, executionId, ReportType.OfficialInitialBalance);
        }

        /// <summary>
        /// Generates the before cutoff for non son segments asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName("OperativeBalanceWithOwnership")]
        public Task GenerateOperativeBalanceOwnershipAsync(
           [ServiceBusTrigger("%OperativeBalanceReportQueue%", Connection = "IntegrationServiceBusConnectionString")] int executionId,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return this.ProcessAsync(label, replyTo, executionId, ReportType.OperativeBalance);
        }

        /// <summary>
        /// Generates the before cut off asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName("SendToSap")]
        public Task GenerateSendToSapAsync(
           [ServiceBusTrigger("%SapBalanceReportQueue%", Connection = "IntegrationServiceBusConnectionString")] int executionId,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return this.ProcessAsync(label, replyTo, executionId, ReportType.SapBalance);
        }

        /// <summary>
        /// Generates User Roles and permissions report.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName("GenerateReportUserRoles")]
        public Task GenerateUserRolesAndPermissionsAsync(
           [ServiceBusTrigger("%userrolesandpermissionsreportQueue%", Connection = "IntegrationServiceBusConnectionString")] int executionId,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return this.ProcessAsync(label, replyTo, executionId, ReportType.UserRolesAndPermissions);
        }

        /// <summary>
        /// Purges the report execution asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("PurgeReportExecution")]
        public async Task PurgeReportExecutionAsync(
           [TimerTrigger("%ReportPurgeInterval%")] TimerInfo timer,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            this.logger.LogInformation($"Purge Report Execution Data function triggered with schedule: {timer.Schedule}", Constants.PurgeReportExecution);
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge report executions history job has started with schedule: {timer.Schedule}", Constants.PurgeReportExecution);
                var generator = this.factory.GetReportGenerator(ReportType.BeforeCutOff);
                await generator.PurgeReportHistoryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.PurgeReportExecution);
            }

            this.logger.LogInformation($"The purge report executions history job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.PurgeReportExecution);
        }

        private async Task ProcessAsync(string label, string replyTo, int executionId, ReportType type)
        {
            this.logger.LogInformation($"Processing report type {type}, ExecutionId: {executionId}");

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Report, replyTo);

            var generator = this.factory.GetReportGenerator(type);
            await generator.GenerateAsync(executionId).ConfigureAwait(false);
        }
    }
}
