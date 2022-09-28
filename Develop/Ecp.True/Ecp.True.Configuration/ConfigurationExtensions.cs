// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// Extensions for configuration.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// The parse type.
        /// </summary>
        /// <param name="objValue">
        /// The string value.
        /// </param>
        /// <param name="destType">
        /// The destination type.
        /// </param>
        /// <returns>
        /// The configuration value and exception.
        /// </returns>
        public static Tuple<object, Exception> ParseType(this object objValue, Type destType)
        {
            object value = null;

            if (objValue == null || destType == null)
            {
                return new Tuple<object, Exception>(value, null);
            }

            try
            {
                if (destType == typeof(string))
                {
                    value = objValue.ToString();
                }
                else if (destType == typeof(TimeSpan))
                {
                    value = TimeSpan.Parse(objValue.ToString(), CultureInfo.InvariantCulture);
                }
                else if (destType == typeof(Guid))
                {
                    value = Guid.Parse(objValue.ToString());
                }
                else if (destType == typeof(DateTimeOffset))
                {
                    value = DateTimeOffset.Parse(objValue.ToString(), null, DateTimeStyles.AssumeUniversal);
                }
                else if (destType.IsEnum)
                {
                    value = Enum.Parse(destType, objValue.ToString());
                }
                else if (destType == typeof(byte[]))
                {
                    value = Convert.FromBase64String(objValue.ToString());
                }
                else if (destType == typeof(bool))
                {
                    value = Convert.ToBoolean(objValue.ToString(), CultureInfo.InvariantCulture);
                }
                else
                {
                    // JSonConvert handles all type conversions except the above
                    value = JsonConvert.DeserializeObject(objValue.ToString(), destType);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<object, Exception>(null, ex);
            }

            return new Tuple<object, Exception>(value, null);
        }
    }
}