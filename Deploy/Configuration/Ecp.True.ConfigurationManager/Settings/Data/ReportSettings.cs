// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Console.Settings
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.ConfigurationManager.Console.Entities;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The report settings.
    /// </summary>
    public class ReportSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSettings"/> class.
        /// </summary>
        public ReportSettings()
        {
            this.ClientSecret = "#PowerBIAppSecret#";
            this.Scope = "https://analysis.windows.net/powerbi/api/.default";
            this.Reports = new Dictionary<string, ReportDetails>();
        }

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
        /// Gets or sets the principal identifier.
        /// </summary>
        /// <value>
        /// The principal identifier.
        /// </value>
        public string PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        /// Gets the reports.
        /// </summary>
        /// <value>
        /// The reports.
        /// </value>
        public IDictionary<string, ReportDetails> Reports { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Report.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.TenantId = input.GetStringValueOrDefault(nameof(this.TenantId), this.TenantId, this.Key);
            this.PrincipalId = input.GetStringValueOrDefault(nameof(this.PrincipalId), this.PrincipalId, this.Key);
            this.ClientId = input.GetStringValueOrDefault(nameof(this.ClientId), this.ClientId, this.Key);
            this.ClientSecret = input.GetStringValueOrDefault(nameof(this.ClientSecret), this.ClientSecret, this.Key);
            this.Scope = input.GetStringValueOrDefault(nameof(this.Scope), this.Scope, this.Key);

            if (input.ShouldIgnore(this.Key, nameof(this.Reports)))
            {
                return;
            }

            // Copy reports
            var reports = input.ExistingSetting.GetValue(nameof(this.Reports));

            if (this.Reports.Count == 0 && reports != null)
            {
                this.Reports = reports.ToObject<IDictionary<string, ReportDetails>>();
            }

            var allKeys = this.Reports.Keys.ToArray();
            foreach (var key in allKeys)
            {
                var existingReport = reports?.SelectToken(key);
                if (existingReport != null)
                {
                    this.Reports[key].CopyFrom(existingReport);
                }
            }
        }
    }
}
