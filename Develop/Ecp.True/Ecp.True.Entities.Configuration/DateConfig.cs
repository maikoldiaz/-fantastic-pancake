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

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The system configuration.
    /// </summary>
    public class DateConfig
    {
        /// <summary>
        /// Gets or sets the acceptable number of days.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public int? LastDays { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public int? DateRange { get; set; }
    }
}
