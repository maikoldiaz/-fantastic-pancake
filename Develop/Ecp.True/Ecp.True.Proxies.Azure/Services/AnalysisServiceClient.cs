// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisServiceClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Analysis Service Client.
    /// </summary>
    /// <seealso cref="IAnalysisServiceClient" />
    [ExcludeFromCodeCoverage]
    public class AnalysisServiceClient : IAnalysisServiceClient
    {
        /// <summary>
        /// The resource endpoint.
        /// </summary>
        private const string ResourceEndpoint = "https://*.asazure.windows.net/.default";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<AnalysisServiceClient> logger;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly ITokenProvider tokenProvider;

        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The audit report log message.
        /// </summary>
        private readonly string auditReportLogMessage = "Received AAS refresh request for audit reports.";

        /// <summary>
        /// The analysis settings.
        /// </summary>
        private AnalysisSettings analysisSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisServiceClient" /> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tokenProvider">The token provider.</param>
        public AnalysisServiceClient(IHttpClientFactory httpClientFactory, ITrueLogger<AnalysisServiceClient> logger, ITokenProvider tokenProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Initializes the specified analysis settings.
        /// </summary>
        /// <param name="analysisSettings">The analysis settings.</param>
        public void Initialize(AnalysisSettings analysisSettings)
        {
            ArgumentValidators.ThrowIfNull(analysisSettings, nameof(analysisSettings));
            this.analysisSettings = analysisSettings;
        }

        /// <summary>
        /// Refreshes the calculation asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>a task.</returns>
        public async Task RefreshCalculationAsync(int ticketId)
        {
            try
            {
                this.logger.LogInformation($"Received AAS refresh request for cutoff with ticketId - {ticketId}.", Constants.AnalysisServiceRefreshTag);
                var objects = new[]
                {
                Constants.ReportHeaderViewName,
                Constants.ReportTemplateViewName,
                Constants.DimDateCalculatedTableName,
                Constants.ProductTableName,
                Constants.KPIDataViewName,
                Constants.KPIPreviousDataViewName,
                Constants.MovementDetailsViewName,
                Constants.MovementsByProductViewName,
                Constants.AttributeDetailsViewName,
                Constants.InventoryDetailsViewName,
                Constants.QualityDetailsViewName,
                Constants.BalanceControlViewName,
                Constants.BackupMovementDetailsViewName,
                };
                await this.RefreshAsync(ObjectType.Cutoff, Constants.FullAnalysisServiceRefreshType, objects).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to refresh the analysis service during {ObjectType.Cutoff} with ticketId - {ticketId}.", Constants.AnalysisServiceRefreshTag);
            }
        }

        /// <summary>
        /// Refreshes the ownership asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>a task.</returns>
        public async Task RefreshOwnershipAsync(int ticketId)
        {
            try
            {
                this.logger.LogInformation($"Received AAS refresh request for ownership calculation with ticketId - {ticketId}.", Constants.AnalysisServiceRefreshTag);
                var objects = new[]
                {
                Constants.ReportHeaderViewName,
                Constants.ReportTemplateViewName,
                Constants.DimDateCalculatedTableName,
                Constants.ProductTableName,
                Constants.KPIDataWithOwnershipViewName,
                Constants.KPIPreviousDataWithOwnershipViewName,
                Constants.MovementDetailsWithOwnershipViewName,
                Constants.MovementDetailsWithOwnershipOtherSegmentViewName,
                Constants.MovementsByProductWithOwnershipViewName,
                Constants.AttributeDetailsWithOwnershipViewName,
                Constants.InventoryDetailsWithOwnershipViewName,
                Constants.QualityDetailsWithOwnershipViewName,
                Constants.BackupMovementDetailsWithOwnerViewName,
                };
                await this.RefreshAsync(ObjectType.OwnershipCalculation, Constants.FullAnalysisServiceRefreshType, objects).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to refresh the analysis service during {ObjectType.OwnershipCalculation} with ticketId - {ticketId}.", Constants.AnalysisServiceRefreshTag);
            }
        }

        /// <summary>
        /// Refreshes the official delta asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>a task.</returns>
        public async Task RefreshOfficialDeltaAsync(int ticketId)
        {
            try
            {
                this.logger.LogInformation($"Received AAS refresh request for official delta calculation with ticketId - {ticketId}.", Constants.AnalysisServiceRefreshTag);

                var objects = new[]
                {
                Constants.ReportHeaderViewName,
                Constants.ReportTemplateViewName,
                Constants.DimDateCalculatedTableName,
                Constants.OfficialDeltaBalanceViewName,
                Constants.OfficialDeltaMovementViewName,
                Constants.OfficialDeltaInventoryViewName,
                };
                await this.RefreshAsync(ObjectType.OfficialDelta, Constants.FullAnalysisServiceRefreshType, objects).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to refresh the analysis service during {ObjectType.OfficialDelta} with ticketId - {ticketId}.", Constants.AnalysisServiceRefreshTag);
            }
        }

        /// <summary>
        /// Refreshes the audit reports asynchronous.
        /// </summary>
        /// <returns>a task.</returns>
        public async Task RefreshAuditReportsAsync()
        {
            try
            {
                this.logger.LogInformation(this.auditReportLogMessage, Constants.AnalysisServiceRefreshTag);
                await this.RefreshAsync(ObjectType.Audit, Constants.FullAnalysisServiceRefreshType).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to refresh the analysis service during {ObjectType.Audit}.", Constants.AnalysisServiceRefreshTag);
            }
        }

        /// <summary>
        /// Refreshes the analysis server asynchronous.
        /// </summary>
        /// <returns>a task.</returns>
        public async Task RefreshSapMappingDetailsAsync()
        {
            try
            {
                this.logger.LogInformation($"Received Analysis server refresh for {ObjectType.TransferPoint}.", Constants.AnalysisServiceRefreshTag);
                var objects = new[]
                {
                    Constants.SapMappingDetailTableName,
                };
                await this.RefreshAsync(ObjectType.TransferPoint, Constants.DataOnlyAnalysisServiceRefreshType, objects).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to refresh the analysis service during {ObjectType.TransferPoint}.", Constants.AnalysisServiceRefreshTag);
            }
        }

        /// <summary>
        /// Calls the refresh asynchronous.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="refreshType">The refresh type.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>A task representing the response message.</returns>
        private async Task RefreshAsync(ObjectType operation, string refreshType, string[] objects = null)
        {
            var httpClient = this.httpClientFactory.CreateClient(Constants.DefaultHttpClient);

            var token = await this.tokenProvider.GetAppTokenAsync(
                                                        this.analysisSettings.TenantId,
                                                        ResourceEndpoint,
                                                        this.analysisSettings.ClientId,
                                                        this.analysisSettings.ClientSecret)
                                                    .ConfigureAwait(false);
            using (var requestMessage = operation == ObjectType.Audit ? new HttpRequestMessage(HttpMethod.Post, this.analysisSettings.AuditRefreshUri) :
                    new HttpRequestMessage(HttpMethod.Post, this.analysisSettings.RefreshUri))
                {
                    requestMessage.Headers.Accept.Clear();
                    requestMessage.Headers.Authorization = token.ToBearer();

                    var refreshInfo = new AnalysisServiceRefreshInfo
                    {
                        Type = refreshType,
                        MaxParallelism = 10,
                    };

                    objects?.ForEach(tableName =>
                    {
                        refreshInfo.Objects.Add(new AnalysisServiceObject { Table = tableName });
                    });

                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(refreshInfo), Encoding.UTF8, "application/json");

                    await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            }
        }
    }
}
