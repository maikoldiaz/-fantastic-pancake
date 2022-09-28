// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ecp.True.Core;

    /// <summary>
    /// The DataGeneratorBase.
    /// </summary>
    public abstract class DataGeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorBase"/> class.
        /// </summary>
        protected DataGeneratorBase()
        {
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The integer value.</returns>
        protected static int GetInt(IDictionary<string, object> parameters, string key, int defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToInt32(value, CultureInfo.InvariantCulture) : defaultValue;
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The nullable integer value.</returns>
        protected static int? GetInt(IDictionary<string, object> parameters, string key, int? defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? (int?)value : defaultValue;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The string value.</returns>
        protected static string GetString(IDictionary<string, object> parameters, string key, string defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToString(value, CultureInfo.InvariantCulture) : defaultValue;
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The decimal value.</returns>
        protected static decimal GetDecimal(IDictionary<string, object> parameters, string key, decimal defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToDecimal(value, CultureInfo.InvariantCulture) : defaultValue;
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The nullable decimal value.</returns>
        protected static decimal? GetDecimal(IDictionary<string, object> parameters, string key, decimal? defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? (decimal?)value : defaultValue;
        }

        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>The boolean value.</returns>
        protected static bool GetBoolean(IDictionary<string, object> parameters, string key, bool defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToBoolean(value, CultureInfo.InvariantCulture) : defaultValue;
        }

        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>The boolean value.</returns>
        protected static bool? GetBoolean(IDictionary<string, object> parameters, string key, bool? defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToBoolean(value, CultureInfo.InvariantCulture) : defaultValue;
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Teh datetime value.</returns>
        protected static DateTime GetDate(IDictionary<string, object> parameters, string key, DateTime defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToDateTime(value, CultureInfo.InvariantCulture) : defaultValue;
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Teh datetime value.</returns>
        protected static DateTime? GetDate(IDictionary<string, object> parameters, string key, DateTime? defaultValue)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            return parameters.TryGetValue(key, out object value) ? Convert.ToDateTime(value, CultureInfo.InvariantCulture) : defaultValue;
        }
    }
}
