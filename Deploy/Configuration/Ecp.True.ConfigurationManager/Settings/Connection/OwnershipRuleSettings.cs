// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleSettings.cs" company="Microsoft">
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
    /// The OwnershipRuleSettings.
    /// </summary>
    public class OwnershipRuleSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipRuleSettings"/> class.
        /// </summary>
        public OwnershipRuleSettings()
        {
            this.ClientSecret = "#OwnershipRuleClientSecret#";
            this.RefreshIntervalInHours = 24;
            this.ShouldStoreResponse = true;
            this.IsCompressed = true;
            this.TimeoutInMinutes = 10;
        }

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
        /// Gets or sets the base path.
        /// </summary>
        /// <value>
        /// The base path.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the registration path.
        /// </summary>
        /// <value>
        /// The registration path.
        /// </value>
        public string RegistrationPath { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule path.
        /// </summary>
        /// <value>
        /// The ownership rule path.
        /// </value>
        public string OwnershipRulePath { get; set; }

        /// <summary>
        /// Gets or sets the refresh interval in hours.
        /// </summary>
        /// <value>
        /// The refresh interval.
        /// </value>
        public int RefreshIntervalInHours { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ownership response should be stored to blob or not.
        /// </summary>
        /// <value>
        /// The value indicating whether ownership response should be stored to blob or not.
        /// </value>
        public bool ShouldStoreResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is compressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompressed { get; set; }

        /// <summary>
        /// Gets or sets the delta base path.
        /// </summary>
        /// <value>
        /// The delta base path.
        /// </value>
        public string DeltaBasePath { get; set; }

        /// <summary>
        /// Gets or sets the delta API path.
        /// </summary>
        /// <value>
        /// The delta API path.
        /// </value>
        public string DeltaApiPath { get; set; }

        /// <summary>
        /// Gets or sets the official delta base path.
        /// </summary>
        /// <value>
        /// The official delta base path.
        /// </value>
        public string OfficialDeltaBasePath { get; set; }

        /// <summary>
        /// Gets or sets the official delta API path.
        /// </summary>
        /// <value>
        /// The delta API path.
        /// </value>
        public string OfficialDeltaApiPath { get; set; }

        /// <summary>
        /// Gets or sets the timeout in minutes.
        /// </summary>
        /// <value>
        /// The timeout in minutes.
        /// </value>
        public int TimeoutInMinutes { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "OwnershipRule.Settings";

        /// <summary>
        /// Does the copy from.
        /// </summary>
        /// <param name="input">The input.</param>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.ClientId = input.GetStringValueOrDefault(nameof(this.ClientId), this.ClientId, this.Key);
            this.ClientSecret = input.GetStringValueOrDefault(nameof(this.ClientSecret), this.ClientSecret, this.Key);
            this.BasePath = input.GetStringValueOrDefault(nameof(this.BasePath), this.BasePath, this.Key);
            this.RegistrationPath = input.GetStringValueOrDefault(nameof(this.RegistrationPath), this.RegistrationPath, this.Key);
            this.OwnershipRulePath = input.GetStringValueOrDefault(nameof(this.OwnershipRulePath), this.OwnershipRulePath, this.Key);
            this.BasePath = input.GetStringValueOrDefault(nameof(this.BasePath), this.BasePath, this.Key);
            this.RefreshIntervalInHours = input.GetIntValueOrDefault(nameof(this.RefreshIntervalInHours), this.RefreshIntervalInHours, this.Key);
            this.ShouldStoreResponse = input.GetBoolValueOrDefault(nameof(this.ShouldStoreResponse), this.ShouldStoreResponse, this.Key);
            this.IsCompressed = input.GetBoolValueOrDefault(nameof(this.IsCompressed), this.IsCompressed, this.Key);
            this.DeltaBasePath = input.GetStringValueOrDefault(nameof(this.DeltaBasePath), this.DeltaBasePath, this.Key);
            this.DeltaApiPath = input.GetStringValueOrDefault(nameof(this.DeltaApiPath), this.DeltaApiPath, this.Key);
            this.OfficialDeltaBasePath = input.GetStringValueOrDefault(nameof(this.OfficialDeltaBasePath), this.OfficialDeltaBasePath, this.Key);
            this.OfficialDeltaApiPath = input.GetStringValueOrDefault(nameof(this.OfficialDeltaApiPath), this.OfficialDeltaApiPath, this.Key);
        }
    }
}
