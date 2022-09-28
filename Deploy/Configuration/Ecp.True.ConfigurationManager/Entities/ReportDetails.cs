// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportDetails.cs" company="Microsoft">
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
    /// The report details.
    /// </summary>
    public class ReportDetails : ICopyable
    {
        /// <summary>
        /// Gets or sets the report identifier.
        /// </summary>
        /// <value>
        /// The report identifier.
        /// </value>
        public string ReportId { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the data set identifier.
        /// </summary>
        /// <value>
        /// The data set identifier.
        /// </value>
        public string DataSetId { get; set; }

        /// <summary>
        /// Gets or sets the is analysis.
        /// </summary>
        /// <value>
        /// The is analysis.
        /// </value>
        public int IsAnalysis { get; set; }

        /// <inheritdoc/>
        public void CopyFrom(JToken existing)
        {
            if (existing == null)
            {
                return;
            }

            this.ReportId = existing.GetStringValueOrDefault(nameof(this.ReportId), this.ReportId);
            this.GroupId = existing.GetStringValueOrDefault(nameof(this.GroupId), this.GroupId);
            this.DataSetId = existing.GetStringValueOrDefault(nameof(this.DataSetId), this.DataSetId);
            this.IsAnalysis = existing.GetIntValueOrDefault(nameof(this.IsAnalysis), this.IsAnalysis);
        }
    }
}
