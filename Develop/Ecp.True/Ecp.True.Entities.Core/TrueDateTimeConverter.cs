// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueDateTimeConverter.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The TrueDateTimeConverter.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TrueDateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Gets or sets the date time formats.
        /// </summary>
        /// <value>
        /// The date time formats.
        /// </value>
        public IEnumerable<string> DateTimeFormats { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string dateString = Convert.ToString(reader?.Value, CultureInfo.InvariantCulture);
            if (DateTime.TryParseExact(dateString, this.DateTimeFormats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new JsonReaderException($"Date format of {this.Path} not date time type", this.Path, 100, 100, null);
            }
        }
    }
}
