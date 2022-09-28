// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Services
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.ConfigurationManager.Console.Settings.Interface;
    using Ecp.True.ConfigurationManager.Entities;
    using Ecp.True.ConfigurationManager.Services.Interface;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The system manager class.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Services.Interface.ISettingsManager" />
    public class SettingsManager : ISettingsManager
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly IDictionary<string, IDictionary<string, JToken>> settings = new Dictionary<string, IDictionary<string, JToken>>();

        /// <summary>
        /// Initializes the specified json.
        /// </summary>
        /// <param name="jsonData">The json.</param>
        public void Initialize(string[] jsonData)
        {
            ArgumentValidators.ThrowIfNull(jsonData, nameof(jsonData));
            foreach (var json in jsonData)
            {
                var array = JObject.Parse(json);
                foreach (var item in array)
                {
                    this.BuildSettings(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// Transforms the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="input">The input.</param>
        public void Transform(ISettings settings, CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));

            var type = settings.GetType();
            if (this.settings.TryGetValue(type.Name, out var typeSettings))
            {
                Console.WriteLine($"Overriding {settings.Key} with value {JsonConvert.SerializeObject(typeSettings)}");
                foreach (var property in typeSettings)
                {
                    type.GetProperty(property.Key).SetValue(settings, GetTokenValue(property.Key, property.Value, type));
                }
            }

            settings.CopyFrom(input);
        }

        /// <summary>
        /// Gets the token value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns>Return the value of specific type.</returns>
        private static object GetTokenValue(string key, JToken token, Type type)
        {
            if (token.Type == JTokenType.Null)
            {
                return null;
            }

            if (token.Type == JTokenType.String)
            {
                return token.Value<string>();
            }

            if (token.Type == JTokenType.Date)
            {
                return token.Value<DateTime>();
            }

            if (token.Type == JTokenType.Integer)
            {
                return GetNumber(token, type, key);
            }

            if (token.Type == JTokenType.Float)
            {
                return token.Value<decimal>();
            }

            if (token.Type == JTokenType.Boolean)
            {
                return token.Value<bool>();
            }

            if (token.Type == JTokenType.Object || token.Type == JTokenType.Array)
            {
                var value = JsonConvert.DeserializeObject(token.ToString(), type.GetProperty(key).PropertyType);
                return value;
            }

            return null;
        }

        /// <summary>
        /// Gets the number.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Return the number.</returns>
        private static object GetNumber(JToken token, Type type, string propertyName)
        {
            TypeCode typeCode;

            var nullableType = Nullable.GetUnderlyingType(type.GetProperty(propertyName).PropertyType);

            // check if the type is nullable type
            typeCode = Type.GetTypeCode(nullableType ?? type.GetProperty(propertyName).PropertyType);

            if (typeCode == TypeCode.Int32)
            {
                return token.Value<int>();
            }

            return token.Value<long>();
        }

        /// <summary>
        /// Processes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void BuildSettings(string key, JToken value)
        {
            var array = key.Split('.');
            if (this.settings != null)
            {
                if (this.settings.TryGetValue(array[0], out var type))
                {
                    if (!type.ContainsKey(array[1]))
                    {
                        type.Add(array[1], value);
                    }
                }
                else
                {
                    this.settings.Add(array[0], new Dictionary<string, JToken> { { array[1], value } });
                }
            }
        }
    }
}
