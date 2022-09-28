// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageSettings.cs" company="Microsoft">
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
    /// Storage Settings.
    /// </summary>
    public class StorageSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSettings"/> class.
        /// </summary>
        public StorageSettings()
        {
            this.DeltaBackOff = 2;
            this.MaxAttempts = 5;
            this.ConnectionString = "#StorageConnectionString#";
            this.StorageAppKey = "#StorageAccessKey#";
        }

        /// <summary>
        /// Gets or sets Storage App Key.
        /// </summary>
        public string StorageAppKey { get; set; }

        /// <summary>
        /// Gets or sets storage Account Name.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the delta back off.
        /// </summary>
        /// <value>
        /// The delta back off.
        /// </value>
        public double DeltaBackOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum attempts.
        /// </summary>
        /// <value>
        /// The maximum attempts.
        /// </value>
        public int MaxAttempts { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Storage.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.AccountName = input.GetStringValueOrDefault(nameof(this.AccountName), this.AccountName, this.Key);
            this.DeltaBackOff = input.GetDoubleValueOrDefault(nameof(this.DeltaBackOff), this.DeltaBackOff, this.Key);
            this.MaxAttempts = input.GetIntValueOrDefault(nameof(this.MaxAttempts), this.MaxAttempts, this.Key);
        }
    }
}