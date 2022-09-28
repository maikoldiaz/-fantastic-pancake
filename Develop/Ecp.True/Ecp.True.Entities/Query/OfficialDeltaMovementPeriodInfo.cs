// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaMovementPeriodInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using Newtonsoft.Json;

    /// <summary>
    /// The OfficialDeltaMovementPeriodInfo.
    /// </summary>
    public class OfficialDeltaMovementPeriodInfo : QueryEntity
    {
        /// <summary>
        /// Gets or sets the month information.
        /// </summary>
        /// <value>
        /// The month information.
        /// </value>
        [JsonProperty("month")]
        public int MonthInfo { get; set; }

        /// <summary>
        /// Gets or sets the year information.
        /// </summary>
        /// <value>
        /// The year information.
        /// </value>
        [JsonProperty("year")]
        public int YearInfo { get; set; }
    }
}
