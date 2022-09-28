// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisSettings.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using System;

    /// <summary>
    /// The Analysis Service settings.
    /// </summary>
    public class AnalysisSettings
    {
        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the name of the analysis server.
        /// </summary>
        /// <value>
        /// The name of the analysis server.
        /// </value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the analysis server model.
        /// </summary>
        /// <value>
        /// The name of the analysis server model.
        /// </value>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        /// Gets the refresh URI.
        /// </summary>
        /// <value>
        /// The refresh URI.
        /// </value>
        public Uri RefreshUri => new Uri($"https://{this.Region}.asazure.windows.net/servers/{this.ServerName}/models/{this.ModelName}/refreshes");

        /// <summary>
        /// Gets or sets the name of the audit server model.
        /// </summary>
        /// <value>
        /// The name of the audit server model.
        /// </value>
        public string AuditModelName { get; set; }

        /// <summary>
        /// Gets the audit refresh URI.
        /// </summary>
        /// <value>
        /// The audit refresh URI.
        /// </value>
        public Uri AuditRefreshUri => new Uri($"https://{this.Region}.asazure.windows.net/servers/{this.ServerName}/models/{this.AuditModelName}/refreshes");
    }
}
