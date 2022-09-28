// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsBase.cs" company="Microsoft">
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
    using Ecp.True.ConfigurationManager.Console.Settings.Interface;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Settings Base.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Console.Settings.Interface.ISettings" />
    public abstract class SettingsBase : ISettings
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public abstract string Key { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonIgnore]
        public string Value => JsonConvert.SerializeObject(this);

        /// <inheritdoc/>
        public void CopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            if (IgnoreCopy(input, this.Key))
            {
                System.Console.WriteLine($"Copy ignored for {this.Key}");
                return;
            }

            this.DoCopyFrom(input);
        }

        /// <summary>
        /// Does the copy from.
        /// </summary>
        /// <param name="input">The input.</param>
        protected abstract void DoCopyFrom(CopyInput input);

        private static bool IgnoreCopy(CopyInput input, string key)
        {
            // If a setting with the same name doesn't already exist in azure, we don't need to copy
            // If ignorables has this setting name with * as one of the values, we don't need to copy
            return input.ExistingSetting == null || input.ShouldIgnoreAll(key);
        }
    }
}
