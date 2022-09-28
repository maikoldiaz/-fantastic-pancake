// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportProductByUnit.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Report
{
    /// <summary>
    /// The ReportProductByUnit.
    /// </summary>
    public class ReportProductByUnit
    {
        /// <summary>
        /// Gets or sets SourceProductId.
        /// </summary>
        /// <value>
        /// The SourceProductId identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets MeasurementUnit identifier.
        /// </summary>
        /// <value>
        /// The MeasurementUnit identifier.
        /// </value>
        public int? MeasurementUnit { get; set; }
    }
}