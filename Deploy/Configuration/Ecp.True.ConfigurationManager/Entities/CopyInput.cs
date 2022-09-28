// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyInput.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The input for copy operation.
    /// </summary>
    public class CopyInput
    {
        /// <summary>
        /// The ignore all.
        /// </summary>
        private readonly bool ignoreAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyInput" /> class.
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="ignoreAll">if set to <c>true</c> [ignore all].</param>
        public CopyInput(JToken existing, bool ignoreAll)
        {
            this.Ignorables = new Dictionary<string, string[]>();
            this.ExistingSetting = existing;
            this.ignoreAll = ignoreAll;
        }

        /// <summary>
        /// Gets the ignorables.
        /// </summary>
        /// <value>
        /// The ignorables.
        /// </value>
        public IDictionary<string, string[]> Ignorables { get; }

        /// <summary>
        /// Gets the existing setting.
        /// </summary>
        /// <value>
        /// The existing setting.
        /// </value>
        public JToken ExistingSetting { get; }

        /// <summary>
        /// Ignores all.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Should ignore all or not.</returns>
        public bool ShouldIgnoreAll(string key)
        {
            return this.ignoreAll || (this.Ignorables.ContainsKey(key) && this.Ignorables[key].Contains("*"));
        }

        /// <summary>
        /// Should ignore.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="property">The property.</param>
        /// <returns>True or false.</returns>
        public bool ShouldIgnore(string key, string property)
        {
            return this.Ignorables.ContainsKey(key) && this.Ignorables[key].Contains(property, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the string value or default.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The string value.
        /// </returns>
        public string GetStringValueOrDefault(string key, string defaultValue, string settingName)
        {
            if (this.ShouldIgnore(settingName, key))
            {
                return defaultValue;
            }

            var token = this.ExistingSetting.GetValue(key);
            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<string>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the double value or default.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The string value.
        /// </returns>
        public double GetDoubleValueOrDefault(string key, double defaultValue, string settingName)
        {
            if (this.ShouldIgnore(settingName, key))
            {
                return defaultValue;
            }

            var token = this.ExistingSetting.GetValue(key);
            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<double>();

            return value.Equals(default) ? defaultValue : value;
        }

        /// <summary>
        /// Gets the int value or default.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The string value.
        /// </returns>
        public int GetIntValueOrDefault(string key, int defaultValue, string settingName)
        {
            if (this.ShouldIgnore(settingName, key))
            {
                return defaultValue;
            }

            var token = this.ExistingSetting.GetValue(key);
            if (token == null || !int.TryParse(token.Value<string>(), out _))
            {
                return defaultValue;
            }

            var value = token.Value<int>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the int value or default.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The string value.
        /// </returns>
        public int? GetIntValueOrDefault(string key, int? defaultValue, string settingName)
        {
            if (this.ShouldIgnore(settingName, key))
            {
                return defaultValue;
            }

            var token = this.ExistingSetting.GetValue(key);
            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<int?>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the decimal value or default.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The string value.
        /// </returns>
        public decimal? GetDecimalValueOrDefault(string key, decimal? defaultValue, string settingName)
        {
            if (this.ShouldIgnore(settingName, key))
            {
                return defaultValue;
            }

            var token = this.ExistingSetting.GetValue(key);
            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<decimal?>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the boolean value or default.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The string value.
        /// </returns>
        public bool GetBoolValueOrDefault(string key, bool defaultValue, string settingName)
        {
            if (this.ShouldIgnore(settingName, key))
            {
                return defaultValue;
            }

            var token = this.ExistingSetting.GetValue(key);
            if (token == null)
            {
                return defaultValue;
            }

            return token.Value<bool>();
        }
    }
}
