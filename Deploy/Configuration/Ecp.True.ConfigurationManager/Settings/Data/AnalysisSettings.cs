// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Settings
{
    using Ecp.True.ConfigurationManager.Console.Settings;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Analysis Settings.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Console.Settings.SettingsBase" />
    public class AnalysisSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisSettings"/> class.
        /// </summary>
        public AnalysisSettings()
        {
            this.ClientSecret = "#AnalysisSettingsClientSecret#";
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
        /// Gets or sets the name of the analysis server audit model.
        /// </summary>
        /// <value>
        /// The name of the analysis server audit model.
        /// </value>
        public string AuditModelName { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Analysis.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.TenantId = input.GetStringValueOrDefault(nameof(this.TenantId), this.TenantId, this.Key);
            this.ClientId = input.GetStringValueOrDefault(nameof(this.ClientId), this.ClientId, this.Key);
            this.ClientSecret = input.GetStringValueOrDefault(nameof(this.ClientSecret), this.ClientSecret, this.Key);
            this.ServerName = input.GetStringValueOrDefault(nameof(this.ServerName), this.ServerName, this.Key);
            this.ModelName = input.GetStringValueOrDefault(nameof(this.ModelName), this.ModelName, this.Key);
            this.AuditModelName = input.GetStringValueOrDefault(nameof(this.AuditModelName), this.AuditModelName, this.Key);
            this.Region = input.GetStringValueOrDefault(nameof(this.Region), this.Region, this.Key);
        }
    }
}
