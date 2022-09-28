// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalValueFormatConverter.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Formatter
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// The Decimal Value Format Converter.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class DecimalValueFormatConverter : JsonConverter
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Newtonsoft.Json.JsonConverter" /> can read JSON.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanRead
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Newtonsoft.Json.JsonConverter" /> can write JSON.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                return;
            }

            writer.WriteValue(((decimal)value).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
