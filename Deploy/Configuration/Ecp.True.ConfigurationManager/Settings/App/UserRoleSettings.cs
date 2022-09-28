// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRoleSettings.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.ConfigurationManager.Console.Settings;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The user role settings.
    /// </summary>
    public class UserRoleSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleSettings"/> class.
        /// </summary>
        public UserRoleSettings()
        {
            this.Mapping = new Dictionary<Role, string>();
        }

        /// <summary>
        /// Gets the mapping.
        /// </summary>
        public IDictionary<Role, string> Mapping { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        [JsonIgnore]
        public override string Key => "UserRole.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            if (input.ShouldIgnore(this.Key, nameof(this.Mapping)))
            {
                return;
            }

            var mapping = input.ExistingSetting.GetValue(nameof(this.Mapping));

            if (this.Mapping.Count == 0 && mapping != null)
            {
                this.Mapping = mapping.ToObject<IDictionary<Role, string>>();
            }

            var allKeys = this.Mapping.Keys.ToArray();
            foreach (var key in allKeys)
            {
                var keyStr = key.ToString();
                var existingMapping = mapping?.SelectToken(keyStr);
                if (existingMapping != null)
                {
                    this.Mapping[key] = existingMapping.GetStringValueOrDefault(keyStr, this.Mapping[key]);
                }
            }
        }
    }
}
