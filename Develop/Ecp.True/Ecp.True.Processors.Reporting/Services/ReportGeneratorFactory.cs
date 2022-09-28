// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportGeneratorFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Reporting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Reporting.Interfaces;

    /// <summary>
    /// The Report Processing Factory.
    /// </summary>
    public class ReportGeneratorFactory : IReportGeneratorFactory
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IEnumerable<IReportGenerator> reportGenerators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGeneratorFactory"/> class.
        /// </summary>
        /// <param name="reportGenerators">The report generators.</param>
        public ReportGeneratorFactory(IEnumerable<IReportGenerator> reportGenerators)
        {
            this.reportGenerators = reportGenerators;
        }

        /// <summary>
        /// Gets the report processor.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>The Report Processor Object.</returns>
        public IReportGenerator GetReportGenerator(ReportType reportType)
        {
            return this.reportGenerators.Single(r => r.Type == reportType);
        }
    }
}
