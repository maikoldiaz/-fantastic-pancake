// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleAvailabilityTrueSapSettings.cs" company="Microsoft">
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
    /// Module Availability True SAP PO API Settings.
    /// </summary>
    public class ModuleAvailabilityTrueSapSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAvailabilityTrueSapSettings"/> class.
        /// </summary>
        public ModuleAvailabilityTrueSapSettings()
        {
            this.Name = "TrueSAP_POAPI";
            this.Resources = new List<string>();
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets resource Groups.
        /// </summary>
        public IList<string> Resources { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "ModuleAvailabilityTrueSapPoApi.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.Name = input.GetStringValueOrDefault(nameof(this.Name), this.Name, this.Key);

            if (input.ShouldIgnore(this.Key, nameof(this.Resources)))
            {
                return;
            }

            // Copy reports
            var rg = input.ExistingSetting.GetValue(nameof(this.Resources));

            if (this.Resources.Count == 0 && rg != null)
            {
                this.Resources = rg.ToObject<List<string>>();
            }
        }
    }
}
