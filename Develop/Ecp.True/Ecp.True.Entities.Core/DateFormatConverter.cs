// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateFormatConverter.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The DateFormatConverter.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DateFormatConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateFormatConverter"/> class.
        /// </summary>
        public DateFormatConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateFormatConverter"/> class.
        /// </summary>
        /// <param name="format">The Format.</param>
        public DateFormatConverter(string format)
        {
            this.DateTimeFormat = format;
        }
    }
}
