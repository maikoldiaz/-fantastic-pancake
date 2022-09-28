// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Processors.Ownership.Services.Excel.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Excel Generator Tests.
    /// </summary>
    [TestClass]
    public class ExcelServiceTests
    {
        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IExcelTransformer> mockExcelTransformer;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<ITrueLogger<ExcelService>> mockLogger;

        /// <summary>
        /// The excel generator.
        /// </summary>
        private ExcelService excelService;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockExcelTransformer = new Mock<IExcelTransformer>();
            this.mockLogger = new Mock<ITrueLogger<ExcelService>>();
            this.excelService = new ExcelService(this.mockExcelTransformer.Object, this.mockLogger.Object, this.mockAzureclientFactory.Object);
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task ExportAndUploadLogisticsExcelAsync_TestAsync()
        {
            var ticketNumber = "1";
            var segmentName = "Segment";
            var ownerName = "Ecopetrol";
            string containerName = "testContainer";
            string pathName = "testPath";

            this.mockAzureclientFactory.Setup(s => s.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<Stream>())).Returns(Task.CompletedTask);
            using (var dataSet = new DataSet("TestDtaSet") { Locale = CultureInfo.InvariantCulture })
            {
                var dataTable = new DataTable("TestTable") { Locale = CultureInfo.InvariantCulture };
                dataTable.Columns.Add("TestColumn", typeof(string));
                dataTable.Columns.Add("Decimal Column", typeof(decimal));
                dataTable.Rows.Add("Test Row Value", 3.45M);
                dataSet.Tables.Add(dataTable);
                var fileName = "ReporteLogistico";

                await this.excelService.ExportAndUploadLogisticsExcelAsync(dataSet, containerName, pathName, ticketNumber, segmentName, ownerName, fileName).ConfigureAwait(false);
            }

            this.mockAzureclientFactory.Verify(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<Stream>()), Times.Once);
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous no data table throws exception test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ExportAndUploadLogisticsExcelAsync_NoDataTable_ThrowsException_TestAsync()
        {
            var ticketNumber = "1";
            var segmentName = "Segment";
            var ownerName = "Ecopetrol";
            string containerName = "testContainer";
            var fileName = "ReporteLogistico";
            string pathName = "testPath";

            using (var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture })
            {
                await this.excelService.ExportAndUploadLogisticsExcelAsync(dataSet, containerName, pathName, ticketNumber, segmentName, ownerName, fileName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous no data set throws exception test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ExportAndUploadLogisticsExcelAsync_NoDataSet_ThrowsException_TestAsync()
        {
            var ticketNumber = "1";
            var segmentName = "Segment";
            var ownerName = "Ecopetrol";
            string containerName = "testContainer";
            var fileName = "ReporteLogistico";
            string pathName = "testPath";

            await this.excelService.ExportAndUploadLogisticsExcelAsync(null, containerName, pathName, ticketNumber, segmentName, ownerName, fileName).ConfigureAwait(false);
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous no container name throws exception test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ExportAndUploadLogisticsExcelAsync_NoContainerName_ThrowsException_TestAsync()
        {
            var ticketNumber = "1";
            var segmentName = "Segment";
            var ownerName = "Ecopetrol";
            var fileName = "ReporteLogistico";
            string pathName = "testPath";

            using (var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture })
            {
                await this.excelService.ExportAndUploadLogisticsExcelAsync(dataSet, null, pathName, ticketNumber, segmentName, ownerName, fileName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous no ticket number throws exception test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ExportAndUploadLogisticsExcelAsync_NoTicketNumber_ThrowsException_TestAsync()
        {
            var ownerName = "Ecopetrol";
            var segmentName = "Segment";
            string containerName = "testContainer";
            var fileName = "ReporteLogistico";
            string pathName = "testPath";

            using (var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture })
            {
                await this.excelService.ExportAndUploadLogisticsExcelAsync(dataSet, containerName, pathName, null, segmentName, ownerName, fileName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous no segment identifier throws exception test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ExportAndUploadLogisticsExcelAsync_NoSegmentId_ThrowsException_TestAsync()
        {
            var ticketNumber = "1";
            var ownerName = "Ecopetrol";
            string containerName = "testContainer";
            var fileName = "ReporteLogistico";
            string pathName = "testPath";

            using (var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture })
            {
                await this.excelService.ExportAndUploadLogisticsExcelAsync(dataSet, containerName, pathName, ticketNumber, null, ownerName, fileName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous no owner identifier throws exception test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ExportAndUploadLogisticsExcelAsync_NoOwnerId_ThrowsException_TestAsync()
        {
            var ticketNumber = "1";
            var segmentName = "Segment";
            string containerName = "testContainer";
            var fileName = "ReporteLogistico";
            string pathName = "testPath";

            using (var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture })
            {
                await this.excelService.ExportAndUploadLogisticsExcelAsync(dataSet, containerName, pathName, ticketNumber, segmentName, null, fileName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Processes the excel asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task ProcessExcel_TestAsync()
        {
            var message = new TrueMessage();
            int ticketId = 23900;
            JArray array = new JArray();
            JValue text = new JValue("Test text");
            array.Add(text);
            string key = "Test key";
            var entities = new JObject();
            entities.Add(key, array);
            this.mockExcelTransformer.Setup(t => t.TransformExcelAsync(It.IsAny<TrueMessage>(), It.IsAny<Stream>(), OwnershipExcelType.ERROREXCEL)).Returns(Task.FromResult(entities));
            await this.excelService.ProcessExcelAsync(message, It.IsAny<Stream>(), OwnershipExcelType.ERROREXCEL, ticketId).ConfigureAwait(false);

            this.mockExcelTransformer.Verify(t => t.TransformExcelAsync(message, It.IsAny<Stream>(), OwnershipExcelType.ERROREXCEL), Times.Once);
        }
    }
}
