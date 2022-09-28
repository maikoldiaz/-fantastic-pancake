// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Services.Excel.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel service.
    /// </summary>
    public class ExcelService : IExcelService
    {
        /// <summary>
        /// The excel transformer.
        /// </summary>
        private readonly string noDataTableError = "No data table in the data set to create excel worksheet.";

        /// <summary>
        /// The excel transformer.
        /// </summary>
        private readonly IExcelTransformer excelTransformer;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ExcelService> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelService"/> class.
        /// </summary>
        /// <param name="excelTransformer">The excel transformer.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public ExcelService(
            IExcelTransformer excelTransformer,
            ITrueLogger<ExcelService> logger,
            IAzureClientFactory azureClientFactory)
        {
            this.excelTransformer = excelTransformer;
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Processes the excel asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stream">The blob stream.</param>
        /// <param name="excelType">The excel type.</param>
        /// <param name="ticketId">The ticketId from file name.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the result of the asynchronous operation.
        /// </returns>
        public async Task<(JObject result, ICollection<ErrorInfo> errorInfo)> ProcessExcelAsync(TrueMessage message, Stream stream, OwnershipExcelType excelType, int ticketId)
        {
            JObject output;
            try
            {
                output = await this.excelTransformer.TransformExcelAsync(message, stream, excelType).ConfigureAwait(false);
                return (output, new List<ErrorInfo>());
            }
            catch (Exception exception) when (exception.Message != null)
            {
                this.logger.LogError(exception, $"Excel transformation failed with: {exception.Message}");
                var errors = new List<ErrorInfo>();
                errors.Add(new ErrorInfo($"Excel transformation failed with: {exception.Message}"));

                return (new JObject(), errors);
            }
        }

        /// <summary>
        /// Exports the and upload logistics excel asynchronous.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="pathName">Name of the path.</param>
        /// <param name="ticketNumber">The ticket number.</param>
        /// <param name="segmentName">Name of the segment.</param>
        /// <param name="ownerName">Name of the owner.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The task.</returns>
        public async Task ExportAndUploadLogisticsExcelAsync(DataSet dataSet, string containerName, string pathName, string ticketNumber, string segmentName, string ownerName, string fileName)
        {
            this.ValidateInputs(dataSet, containerName, ticketNumber, segmentName, ownerName);

            using (var stream = new MemoryStream())
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    CreateWorkbook(dataSet, wb);

                    wb.SaveAs(stream);
                }

                stream.Position = 0;
                await this.UploadAsync(containerName, pathName, ticketNumber, segmentName, ownerName, stream, fileName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates the workbook.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="wb">The wb.</param>
        private static void CreateWorkbook(DataSet dataSet, XLWorkbook wb)
        {
            //// cannot directly add data table to worksheet due to app service sandbox restriction preventing call to system.drawing dll,
            //// adding table to first cell instead.

            for (int i = 1; i <= dataSet.Tables.Count; i++)
            {
                wb.Worksheets.Add(dataSet.Tables[i - 1].TableName);
                wb.Worksheet(i).Cell(1, 1).InsertTable(dataSet.Tables[i - 1]);

                int j = 1;
                foreach (DataColumn column in dataSet.Tables[i - 1].Columns)
                {
                    if (column.DataType == typeof(decimal) || column.DataType == typeof(double) || column.DataType == typeof(int))
                    {
                        wb.Worksheet(i).Column(j).CellsUsed().Skip(1).ForEach(x =>
                        {
                            x.DataType = XLDataType.Number;
                            x.Style.NumberFormat.NumberFormatId = column.DataType == typeof(int) ? 1 : 2;
                        });
                    }

                    j++;
                }
            }

            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            wb.Style.Font.Bold = true;
            wb.Worksheets.ToList().ForEach(ws =>
            {
                ws.Tables.First().SetShowAutoFilter(false);
                //// cannot use auto resize functionality due to app service sandbox restriction preventing call to system.drawing dll.
                ////ws.Columns().AdjustToContents();
                ws.Rows().First().Style.Font.FontColor = XLColor.WarmBlack;
            });
        }

        /// <summary>
        /// Validates the inputs.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="ticketNumber">The ticket number.</param>
        /// <param name="segmentName">Name of the segment.</param>
        /// <param name="ownerName">Name of the owner.</param>
        private void ValidateInputs(DataSet dataSet, string containerName, string ticketNumber, string segmentName, string ownerName)
        {
            ArgumentValidators.ThrowIfNull(dataSet, nameof(dataSet));
            ArgumentValidators.ThrowIfNullOrEmpty(containerName, nameof(containerName));
            ArgumentValidators.ThrowIfNullOrEmpty(ticketNumber, nameof(ticketNumber));
            ArgumentValidators.ThrowIfNullOrEmpty(segmentName, nameof(segmentName));
            ArgumentValidators.ThrowIfNullOrEmpty(ownerName, nameof(ownerName));

            if (dataSet.Tables.Count == 0)
            {
                throw new ArgumentException(this.noDataTableError);
            }
        }

        /// <summary>
        /// Uploads the asynchronous.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="pathName">Name of the path.</param>
        /// <param name="ticketNumber">The ticket number.</param>
        /// <param name="segmentName">Name of the segment.</param>
        /// <param name="ownerName">Name of the owner.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns> The task. </returns>
        private Task UploadAsync(string containerName, string pathName, string ticketNumber, string segmentName, string ownerName, Stream stream, string fileName)
        {
            string blobName = $"{pathName}/{fileName}_{segmentName}_{ownerName}_{ticketNumber}.xlsx";
            return this.azureClientFactory.GetBlobStorageSaSClient(containerName, blobName).CreateBlobAsync(stream);
        }
    }
}