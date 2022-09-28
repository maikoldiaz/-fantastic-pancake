// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using Ecp.True.ConfigurationManager;
    using Ecp.True.ConfigurationManager.Console.Settings;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The analytics config.
    /// </summary>
    public class AnalyticsSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsSettings"/> class.
        /// </summary>
        public AnalyticsSettings()
        {
            this.ClientSecret = "#AnalyticsSettingsClientSecret#";
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
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Analytics.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.TenantId = input.GetStringValueOrDefault(nameof(this.TenantId), this.TenantId, this.Key);
            this.ClientId = input.GetStringValueOrDefault(nameof(this.ClientId), this.ClientId, this.Key);
            this.ClientSecret = input.GetStringValueOrDefault(nameof(this.ClientSecret), this.ClientSecret, this.Key);
            this.Scope = input.GetStringValueOrDefault(nameof(this.Scope), this.Scope, this.Key);
        }
    }
}
