// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.UI.Services.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.PowerBI.Api;
    using Microsoft.PowerBI.Api.Models;
    using Microsoft.Rest;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The report service.
    /// </summary>
    public class ReportService : IReportService
    {
        /// <summary>
        /// The report client factory.
        /// </summary>
        private readonly IReportClientFactory reportClientFactory;

        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ReportService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService" /> class.
        /// </summary>
        /// <param name="reportClientFactory">The report client factory.</param>
        /// <param name="httpClientFactory">The HTTP client proxy.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public ReportService(IReportClientFactory reportClientFactory, IHttpClientFactory httpClientFactory, IConfigurationHandler configurationHandler, ITrueLogger<ReportService> logger)
        {
            this.reportClientFactory = reportClientFactory;
            this.httpClientFactory = httpClientFactory;
            this.configurationHandler = configurationHandler;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ReportDetails> GetReportDetailsAsync(string key)
        {
            try
            {
                this.logger.LogInformation($"Report {key} requested", Constants.PowerBiTag);
                var reportConfig = await this.configurationHandler.GetConfigurationAsync<ReportSettings>(ConfigurationConstants.ReportSettings).ConfigureAwait(false);
                var reportDetails = reportConfig.Reports[key];
                var oauthEndpoint = new Uri($"https://login.microsoftonline.com/{reportConfig.TenantId}/oauth2/v2.0/token");
                var httpClient = this.httpClientFactory.CreateClient(Constants.DefaultHttpClient);
                using (var content = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("client_id", reportConfig.ClientId),
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_secret", reportConfig.ClientSecret),
                        new KeyValuePair<string, string>("scope", reportConfig.Scope),
                        }))
                {
                    var result = await httpClient.PostAsync(
                        oauthEndpoint,
                        content).ConfigureAwait(false);
                    if (result.IsSuccessStatusCode)
                    {
                        var tokenString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var accessToken = JToken.Parse(tokenString).Value<string>("access_token");
                        var embedToken = await this.GenerateEmbedTokenAsync(reportConfig.PrincipalId, reportDetails, accessToken).ConfigureAwait(false);
                        reportDetails.Token = embedToken.Token;
                    }
                }

                return reportDetails;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error in accessing report {key}", Constants.PowerBiTag);
                throw;
            }
        }

        private async Task<EmbedToken> GenerateEmbedTokenAsync(string principalId, ReportDetails reportDetails, string accessToken)
        {
            var tokenCredentials = new TokenCredentials(accessToken);
            using (var client = this.reportClientFactory.GetClient("https://api.powerbi.com", tokenCredentials))
            {
                var generateTokenRequest = !reportDetails.IsAnalysis
                    ? new GenerateTokenRequest(accessLevel: "view")
                    : new GenerateTokenRequest(accessLevel: "view", new EffectiveIdentity(principalId, new[] { reportDetails.DataSetId }));
                return await client.Reports.GenerateTokenInGroupAsync(new Guid(reportDetails.GroupId), new Guid(reportDetails.ReportId), generateTokenRequest).ConfigureAwait(false);
            }
        }
    }
}
