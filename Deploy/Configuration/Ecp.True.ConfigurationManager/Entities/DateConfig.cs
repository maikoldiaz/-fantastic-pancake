// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateConfig.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.ConfigurationManager.Console.Entities
{
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The System Settings.
    /// </summary>
    public class DateConfig : ICopyable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateConfig"/> class.
        /// </summary>
        /// <param name="lastDays">The last days.</param>
        public DateConfig(int lastDays)
        {
            this.LastDays = lastDays;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateConfig"/> class.
        /// </summary>
        /// <param name="lastDays">The last days.</param>
        /// <param name="dateRange">The date range.</param>
        public DateConfig(int lastDays, int dateRange)
            : this(lastDays)
        {
            this.DateRange = dateRange;
        }

        /// <summary>
        /// Gets or sets the acceptable number of days.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public int? LastDays { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public int? DateRange { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="existing">The existing.</param>
        public void CopyFrom(JToken existing)
        {
            if (existing == null)
            {
                return;
            }

            this.LastDays = existing.GetIntValueOrDefault(nameof(this.LastDays), this.LastDays);
            this.DateRange = existing.GetIntValueOrDefault(nameof(this.DateRange), this.DateRange);
        }
    }
}
