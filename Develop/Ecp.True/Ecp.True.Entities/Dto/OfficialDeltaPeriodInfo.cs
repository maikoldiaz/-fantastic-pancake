// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaPeriodInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// The OfficialDeltaPeriodInfo.
    /// </summary>
    public class OfficialDeltaPeriodInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaPeriodInfo"/> class.
        /// </summary>
        public OfficialDeltaPeriodInfo()
        {
            this.DefaultYear = DateTime.UtcNow.ToTrue().Year;
            this.OfficialPeriods = new Dictionary<string, IEnumerable<OfficialDeltaMovementPeriodInfo>>();
        }

        /// <summary>
        /// Gets or sets the default year.
        /// </summary>
        /// <value>
        /// The default year.
        /// </value>
        public int DefaultYear { get; set; }

        /// <summary>
        /// Gets the official periods.
        /// </summary>
        /// <value>
        /// The official periods.
        /// </value>
        public IDictionary<string, IEnumerable<OfficialDeltaMovementPeriodInfo>> OfficialPeriods { get; }
    }
}
