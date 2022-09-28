// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logistics.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Ownership
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using EntityConstants = Ecp.True.Entities.Constants;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    /// <summary>
    /// Logistics Generator.
    /// </summary>
    public class Logistics : FunctionBase
    {
        /// <summary>
        /// The ownership calculation service.
        /// </summary>
        private readonly IOwnershipCalculationService ownershipCalculationService;

        /// <summary>
        /// The data generator service.
        /// </summary>
        private readonly IDataGeneratorService dataGeneratorService;

        /// <summary>
        /// The excel service.
        /// </summary>
        private readonly IExcelService excelService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<Logistics> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logistics"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="ownershipCalculationService">The ownership calculation service.</param>
        /// <param name="dataGeneratorService">The data generator service.</param>
        /// <param name="excelService">The excel service.</param>
        public Logistics(
            ITrueLogger<Logistics> logger,
            IServiceProvider serviceProvider,
            IOwnershipCalculationService ownershipCalculationService,
            IDataGeneratorService dataGeneratorService,
            IExcelService excelService)
            : base(serviceProvider)
        {
            this.ownershipCalculationService = ownershipCalculationService;
            this.dataGeneratorService = dataGeneratorService;
            this.excelService = excelService;
            this.logger = logger;
        }

        /// <summary>
        /// Generates the logistics asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("GenerateLogistics")]
        public async Task GenerateOfficialLogisticsAsync(
           [ServiceBusTrigger("%LogisticsQueue%", Connection = "IntegrationServiceBusConnectionString")] QueueMessage message,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            await this.InitializeAsync().ConfigureAwait(false);
            this.logger.LogInformation($"The logistics excel generation is triggered for ticket: {message.TicketId}");
            try
            {
                this.ProcessMetadata(label, FunctionNames.Ownership, replyTo);
                var logisticsInfo = await this.ownershipCalculationService.GetLogisticsDetailsAsync(message.TicketId, message.SystemTypeId.GetValueOrDefault()).ConfigureAwait(false);
                if ((SystemType)message.SystemTypeId == SystemType.SIV && logisticsInfo != null)
                {
                    var data = this.dataGeneratorService.TransformLogisticsData(logisticsInfo);
                    await this.excelService.ExportAndUploadLogisticsExcelAsync(
                        data,
                        SystemType.TRUE.ToString().ToLowerCase(),
                        "logistics",
                        logisticsInfo.Ticket.TicketId.ToString(CultureInfo.InvariantCulture),
                        logisticsInfo.Ticket.CategoryElement.Name,
                        logisticsInfo.Ticket.Owner.Name,
                        "ReporteLogistico").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await this.HandleLogisticsErrorsAsync(ex, message.TicketId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            var azureClientFactory = this.Resolve<IAzureClientFactory>();
            if (azureClientFactory.IsReady)
            {
                return;
            }

            var configurationHandler = this.Resolve<IConfigurationHandler>();

            var analysisSettings = await configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);
            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings));
        }

        /// <summary>
        /// Handles the logistics database errors asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        private async Task HandleLogisticsErrorsAsync(Exception exception, int ticketId)
        {
            try
            {
                var errorMessage = string.Empty;
                var sqlException = exception as SqlException;
                if (sqlException != null)
                {
                    if (exception.Message.EqualsIgnoreCase(SqlConstants.NoSapHomologationForMovementType))
                    {
                        this.logger.LogError(sqlException, $"{EntityConstants.NoSapHomologationFoundForMovementType} Ticket Id: {ticketId}");
                        errorMessage = EntityConstants.NoSapHomologationFoundForMovementType;
                    }
                    else if (exception.Message.EqualsIgnoreCase(SqlConstants.InvalidCombinationToSivMovement))
                    {
                        this.logger.LogError(sqlException, $"{EntityConstants.InvalidCombinationToSivMovement} Ticket Id: {ticketId}");
                        errorMessage = EntityConstants.InvalidCombinationToSivMovement;
                    }
                    else
                    {
                        this.logger.LogError(sqlException, $"Error occurred for ticket Id: {ticketId}. Error: {exception.Message}");
                        errorMessage = exception.Message;
                    }
                }
                else
                {
                    this.logger.LogError(exception, $"Exception while processing ticket Id: {ticketId} for logistics excel generation.");
                    errorMessage = exception.Message;
                }

                await this.ownershipCalculationService.UpdateTicketErrorsAsync(ticketId, errorMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        private Task InitializeAsync()
        {
            return this.TryInitializeAsync();
        }
    }
}
