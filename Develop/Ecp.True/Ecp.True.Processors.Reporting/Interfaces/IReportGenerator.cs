// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Reporting.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// Report Generator.
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Gets the report type.
        /// </summary>
        /// <value>
        /// The report type.
        /// </value>
        ReportType Type { get; }

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task GenerateAsync(int executionId);

        /// <summary>
        /// Purges the report history.
        /// </summary>
        /// <returns>The task.</returns>
        Task PurgeReportHistoryAsync();
    }
}
