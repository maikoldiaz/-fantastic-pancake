// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailabilitySettings.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.ConfigurationManager.Console.Settings;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Availability settings.
    /// </summary>
    public class AvailabilitySettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilitySettings"/> class.
        /// </summary>
        public AvailabilitySettings()
        {
            this.ClientSecret = "#AvailabilityClientSecret#";
            this.ResourceGroups = new List<ResourceGroups>();
            this.ResourceId = "https://management.core.windows.net";
            this.ResourceDetailPath = "https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/resources?api-version=2019-10-01";
            this.AvailabilityPath = "https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ResourceHealth/availabilityStatuses?api-version=2020-05-01";
            this.MetricPath = "https://management.azure.com/{0}/providers/microsoft.insights/metrics?api-version=2018-01-01";
        }

        /// <summary>
        /// Gets or sets client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets client Secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets tenant Id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets resource Id.
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource detail path.
        /// </summary>
        /// <value>
        /// The resource detail path.
        /// </value>
        public string ResourceDetailPath { get; set; }

        /// <summary>
        /// Gets or sets the availability path.
        /// </summary>
        /// <value>
        /// The availability path.
        /// </value>
        public string AvailabilityPath { get; set; }

        /// <summary>
        /// Gets or sets the metric path.
        /// </summary>
        /// <value>
        /// The metric path.
        /// </value>
        public string MetricPath { get; set; }

        /// <summary>
        /// Gets resource Groups.
        /// </summary>
        public IList<ResourceGroups> ResourceGroups { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Availability.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.ClientId = input.GetStringValueOrDefault(nameof(this.ClientId), this.ClientId, this.Key);
            this.ClientSecret = input.GetStringValueOrDefault(nameof(this.ClientSecret), this.ClientSecret, this.Key);
            this.TenantId = input.GetStringValueOrDefault(nameof(this.TenantId), this.TenantId, this.Key);
            this.ResourceId = input.GetStringValueOrDefault(nameof(this.ResourceId), this.ResourceId, this.Key);
            this.ResourceDetailPath = input.GetStringValueOrDefault(nameof(this.ResourceDetailPath), this.ResourceDetailPath, this.Key);
            this.AvailabilityPath = input.GetStringValueOrDefault(nameof(this.AvailabilityPath), this.AvailabilityPath, this.Key);
            this.MetricPath = input.GetStringValueOrDefault(nameof(this.MetricPath), this.MetricPath, this.Key);

            if (input.ShouldIgnore(this.Key, nameof(this.ResourceGroups)))
            {
                return;
            }

            // Copy reports
            var rg = input.ExistingSetting.GetValue(nameof(this.ResourceGroups));

            if (this.ResourceGroups.Count == 0 && rg != null)
            {
                this.ResourceGroups = rg.ToObject<List<ResourceGroups>>();
            }
        }
    }
}