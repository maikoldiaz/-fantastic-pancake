// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonDateConverter.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Attributes
{
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Defines the <see cref="JsonDateConverter" />.
    /// </summary>
    public class JsonDateConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Defines the DefaultDateTimeFormat.
        /// </summary>
        private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDateConverter"/> class.
        /// </summary>
        public JsonDateConverter()
            : this(DefaultDateTimeFormat)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDateConverter"/> class.
        /// </summary>
        /// <param name="formatString">The formatString<see cref="string"/>.</param>
        public JsonDateConverter(string formatString)
        {
            this.DateTimeFormat = formatString;
        }
    }
}
