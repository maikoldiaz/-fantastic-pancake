// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobRetrySettings.cs" company="Microsoft">
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
    /// The blob retry settings.
    /// </summary>
    public class BlobRetrySettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobRetrySettings"/> class.
        /// </summary>
        public BlobRetrySettings()
        {
            this.DeltaBackoff = 2;
            this.MaxAttempts = 5;
        }

        /// <summary>
        /// Gets or sets the delta backoff.
        /// </summary>
        /// <value>
        /// The delta backoff.
        /// </value>
        public double DeltaBackoff { get; set; }

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
        public override string Key => "Blob.RetrySettings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.DeltaBackoff = input.GetDoubleValueOrDefault(nameof(this.DeltaBackoff), this.DeltaBackoff, this.Key);
            this.MaxAttempts = input.GetIntValueOrDefault(nameof(this.MaxAttempts), this.MaxAttempts, this.Key);
        }
    }
}
