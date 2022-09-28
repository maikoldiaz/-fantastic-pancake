// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportGeneratorFactory.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The  Report Processing Factory Interface.
    /// </summary>
    public interface IReportGeneratorFactory
    {
        /// <summary>
        /// Gets the report processor.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>The Report Processor Object.</returns>
        IReportGenerator GetReportGenerator(ReportType reportType);
    }
}
