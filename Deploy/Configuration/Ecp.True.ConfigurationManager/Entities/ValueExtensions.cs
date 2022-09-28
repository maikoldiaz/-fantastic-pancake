// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueExtensions.cs" company="Microsoft">
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
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The extensions.
    /// </summary>
    public static class ValueExtensions
    {
        /// <summary>
        /// Gets the string value or default.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The string value.</returns>
        public static string GetStringValueOrDefault(this JToken input, string key, string defaultValue)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var token = input.GetValue(key);
            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<string>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the int value or default.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The integer value.
        /// </returns>
        public static int GetIntValueOrDefault(this JToken input, string key, int defaultValue)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var token = input.GetValue(key);

            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<int>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the int value or default.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The integer value.
        /// </returns>
        public static int? GetIntValueOrDefault(this JToken input, string key, int? defaultValue)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var token = input.GetValue(key);

            if (token == null)
            {
                return defaultValue;
            }

            var value = token.Value<int?>();

            return value == default ? defaultValue : value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="key">The key.</param>
        /// <returns>The token.</returns>
        public static JToken GetValue(this JToken existing, string key)
        {
            ArgumentValidators.ThrowIfNull(existing, nameof(existing));
            var valueToken = existing.SelectToken("Value");

            if (valueToken == null)
            {
                return valueToken;
            }

            var valueObj = JObject.Parse(valueToken.ToString());
            return valueObj.SelectToken(key);
        }
    }
}
