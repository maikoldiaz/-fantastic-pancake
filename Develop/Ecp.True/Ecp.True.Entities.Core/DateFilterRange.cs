// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateFilterRange.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Date Filter range.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DateFilterRange
    {
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets converts to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public DateTime ToDate { get; set; }
    }
}
