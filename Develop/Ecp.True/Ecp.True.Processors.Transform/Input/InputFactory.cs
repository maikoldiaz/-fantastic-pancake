// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Input
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using ClosedXML.Excel;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using ExcelDataReader;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The input factory.
    /// </summary>
    public class InputFactory : IInputFactory
    {
        /// <summary>
        /// The excel has formula.
        /// </summary>
        private readonly string excelHasFormula = "The excel sheet {0} has formula in cell {1}.";

        /// <summary>
        /// The BLOB client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<InputFactory> logger;

        /// <summary>
        /// The data service.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService transactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputFactory" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="dataService">The data service.</param>
        /// <param name="transactionService">The file registration transaction service.</param>
        public InputFactory(
            ITrueLogger<InputFactory> logger,
            IAzureClientFactory azureClientFactory,
            IDataService dataService,
            IFileRegistrationTransactionService transactionService)
        {
            ArgumentValidators.ThrowIfNull(azureClientFactory, nameof(azureClientFactory));

            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
            this.dataService = dataService;
            this.transactionService = transactionService;
        }

        /// <inheritdoc/>
        public Task<JToken> GetJsonInputAsync(string blobPath)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(blobPath, nameof(blobPath));
            return this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, blobPath).ParseAsync<JToken>();
        }

        /// <inheritdoc/>
        public async Task<ExcelInput> GetExcelInputAsync(TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            var blobStream = await this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, message.InputBlobPath).GetCloudBlobStreamAsync().ConfigureAwait(false);

            this.logger.LogInformation($"Excel downloaded from path : {message.InputBlobPath}");

            // Fail the stream if it has a formula
            var formulaCell = HasFormula(blobStream);
            if (formulaCell != null)
            {
                throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, this.excelHasFormula, formulaCell.Item1, formulaCell.Item2));
            }

            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(blobStream))
            {
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = x => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true,
                    },
                };

                return new ExcelInput(excelReader.AsDataSet(conf), message);
            }
        }

        /// <inheritdoc/>
        public async Task<FileRegistration> GetFileRegistrationAsync(string uploadId)
        {
            return await this.transactionService.GetFileRegistrationAsync(uploadId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<FileRegistrationTransaction> GetFileRegistrationTransactionAsync(int fileRegistrationTransactionId)
        {
            return this.transactionService.GetFileRegistrationTransactionAsync(fileRegistrationTransactionId);
        }

        /// <summary>
        /// Saves the sap json array asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        public Task<string> SaveSapJsonArrayAsync(object entity, TrueMessage message)
        {
            return this.dataService.SaveExternalSourceEntityArrayAsync(JArray.Parse(JsonConvert.SerializeObject(entity)), message);
        }

        /// <summary>
        /// Saves the sap json asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        public Task<string> SaveSapJsonAsync(object entity, TrueMessage message)
        {
            return this.dataService.SaveExternalSourceEntityAsync(JObject.Parse(JsonConvert.SerializeObject(entity)), message);
        }

        /// <summary>
        /// Saves the sap json asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        public Task<string> SaveSapLogisticJsonAsync(object entity, TrueMessage message)
        {
            return this.dataService.SaveLogisticEntityAsync(JObject.Parse(JsonConvert.SerializeObject(entity)), message);
        }

        /// <summary>
        /// Determines whether the specified BLOB stream has formula.
        /// </summary>
        /// <param name="blobStream">The BLOB stream.</param>
        /// <returns>
        ///   <c>true</c> if the specified BLOB stream has formula; otherwise, <c>false</c>.
        /// </returns>
        private static Tuple<string, string> HasFormula(Stream blobStream)
        {
            using (var wb = new XLWorkbook(blobStream))
            {
                foreach (var worksheet in wb.Worksheets)
                {
                    foreach (var cell in worksheet.CellsUsed())
                    {
                        if (cell.HasFormula)
                        {
                            return System.Tuple.Create(worksheet.Name, cell.Address.ToString());
                        }
                    }
                }
            }

            return null;
        }
    }
}
