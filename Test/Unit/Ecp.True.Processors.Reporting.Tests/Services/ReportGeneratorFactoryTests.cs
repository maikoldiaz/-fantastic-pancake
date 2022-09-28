// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportGeneratorFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Reporting.Tests.Services
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Reporting.Interfaces;
    using Ecp.True.Processors.Reporting.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The reversal processor tests.
    /// </summary>
    [TestClass]
    public class ReportGeneratorFactoryTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private ReportGeneratorFactory reportProcessingFactory;

        /// <summary>
        /// The report generators.
        /// </summary>
        private List<IReportGenerator> reportGenerators;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var mockCutOff = new Mock<IReportGenerator>();
            mockCutOff.Setup(m => m.Type).Returns(ReportType.BeforeCutOff);

            var mockOfficialInitial = new Mock<IReportGenerator>();
            mockOfficialInitial.Setup(m => m.Type).Returns(ReportType.OfficialInitialBalance);

            this.reportGenerators = new List<IReportGenerator> { mockCutOff.Object, mockOfficialInitial.Object };
            this.reportProcessingFactory = new ReportGeneratorFactory(this.reportGenerators);
        }

        /// <summary>
        /// Gets the report processor should get report processor when invoked.
        /// </summary>
        [TestMethod]
        public void GetReportProcessor_ShouldGetReportProcessorForOperationalDataWithoutCutoffReportType_WhenInvoked()
        {
            Assert.IsNotNull(this.reportProcessingFactory.GetReportGenerator(ReportType.BeforeCutOff));
            Assert.IsNotNull(this.reportProcessingFactory.GetReportGenerator(ReportType.OfficialInitialBalance));
        }
    }
}
