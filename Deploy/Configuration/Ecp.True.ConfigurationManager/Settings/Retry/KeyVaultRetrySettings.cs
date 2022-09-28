// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultRetrySettings.cs" company="Microsoft">
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
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The Retry Settings.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Console.Settings.SettingsBase" />
    public class KeyVaultRetrySettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultRetrySettings"/> class.
        /// </summary>
        public KeyVaultRetrySettings()
        {
            this.RetryCount = 5;
            this.MinBackoff = 1.0;
            this.MaxBackoff = 16.0;
            this.DeltaBackoff = 2.0;
        }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the minimum backoff.
        /// </summary>
        /// <value>
        /// The minimum backoff.
        /// </value>
        public double MinBackoff { get; set; }

        /// <summary>
        /// Gets or sets the maximum backoff.
        /// </summary>
        /// <value>
        /// The maximum backoff.
        /// </value>
        public double MaxBackoff { get; set; }

        /// <summary>
        /// Gets or sets the delta backoff.
        /// </summary>
        /// <value>
        /// The delta backoff.
        /// </value>
        public double DeltaBackoff { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "KeyVault.RetrySettings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.RetryCount = input.GetIntValueOrDefault(nameof(this.RetryCount), this.RetryCount, this.Key);
            this.MinBackoff = input.GetDoubleValueOrDefault(nameof(this.MinBackoff), this.MinBackoff, this.Key);
            this.MaxBackoff = input.GetDoubleValueOrDefault(nameof(this.MaxBackoff), this.MaxBackoff, this.Key);
            this.DeltaBackoff = input.GetDoubleValueOrDefault(nameof(this.DeltaBackoff), this.DeltaBackoff, this.Key);
        }
    }
}
