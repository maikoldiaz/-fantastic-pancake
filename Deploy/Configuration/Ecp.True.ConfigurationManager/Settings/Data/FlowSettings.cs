// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowSettings.cs" company="Microsoft">
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
    /// The flow settings.
    /// </summary>
    public class FlowSettings : SettingsBase
    {
        /// <summary>
        /// Gets or sets the Environment identifier.
        /// </summary>
        /// <value>
        /// The Environment identifier.
        /// </value>
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Flow.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.EnvironmentId = input.GetStringValueOrDefault(nameof(this.EnvironmentId), this.EnvironmentId, this.Key);
        }
    }
}
