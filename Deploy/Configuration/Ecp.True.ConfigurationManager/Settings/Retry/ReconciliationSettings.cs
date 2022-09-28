// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReconciliationSettings.cs" company="Microsoft">
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
    /// The reconciliation settings.
    /// </summary>
    public class ReconciliationSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationSettings"/> class.
        /// </summary>
        public ReconciliationSettings()
        {
            this.ReconcileIntervalInMinutes = 120;
            this.MaxRetryCount = 3;
            this.BatchSize = 1000;
        }

        /// <summary>
        /// Gets or sets the reconcile interval in minutes.
        /// </summary>
        /// <value>
        /// The reconcile interval in minutes.
        /// </value>
        public int? ReconcileIntervalInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the maximum retry count.
        /// </summary>
        /// <value>
        /// The maximum retry count.
        /// </value>
        public int? MaxRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the batch size.
        /// </summary>
        /// <value>
        /// The batch size.
        /// </value>
        public int? BatchSize { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Reconciliation.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.ReconcileIntervalInMinutes = input.GetIntValueOrDefault(nameof(this.ReconcileIntervalInMinutes), this.ReconcileIntervalInMinutes, this.Key);
            this.MaxRetryCount = input.GetIntValueOrDefault(nameof(this.MaxRetryCount), this.MaxRetryCount, this.Key);
            this.BatchSize = input.GetIntValueOrDefault(nameof(this.BatchSize), this.BatchSize, this.Key);
        }
    }
}
