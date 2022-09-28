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

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The report details.
    /// </summary>
    public class ReportDetails
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
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the is analysis.
        /// </summary>
        /// <value>
        /// The is analysis.
        /// </value>
        public bool IsAnalysis { get; set; }
    }
}
