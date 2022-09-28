// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelFileGenerationWithLogisticInformationfeatureSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Ecp.True.Bdd.Tests.Entities;
    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Utils;
    using NUnit.Framework;
    using OfficeOpenXml;
    using TechTalk.SpecFlow;

    [Binding]
    public class ExcelFileGenerationWithLogisticInformationfeatureSteps : EcpWebStepDefinitionBase
    {
        [Then(@"it should contains columns and values as per the defined mapping")]
        public async System.Threading.Tasks.Task ThenItShouldContainsColumnsAndValuesAsPerTheDefinedMappingAsync()
        {
            var actualSIVInformation = new Dictionary<string, string>()
            {
                { "Column1", "MOVIMIENTO" },
                { "Column2", "ALMACEN-ORIGEN" },
                { "Column3", "PRODUCTO-ORIGEN" },
                { "Column4", "ALMACEN-DESTINO" },
                { "Column5", "PRODUCTO-DESTINO" },
                { "Column6", "ORDEN-COMPRA" },
                { "Column7", "POS-COMPRA" },
                { "Column8", "VALOR" },
                { "Column9", "UOM" },
                { "Column10", "HALLAZGO" },
                { "Column11", "DIAGNOSTICO" },
                { "Column12", "IMPACTO" },
                { "Column13", "SOLUCION" },
                { "Column14", "ESTADO" },
                { "Column15", "ORDEN" },
                { "Column16", "FECHA-OPERATIVA" },
            };

            actualSIVInformation.Add("row[2,1]", ConstantValues.SIVMovementType);
            actualSIVInformation.Add("row[2,2]", ConstantValues.StorageLocationSIV);
            actualSIVInformation.Add("row[2,3]", ConstantValues.StorageLocationProduct);
            actualSIVInformation.Add("row[2,4]", ConstantValues.StorageLocationSIV);
            actualSIVInformation.Add("row[2,5]", ConstantValues.StorageLocationProduct);
            actualSIVInformation.Add("row[2,6]", null);
            actualSIVInformation.Add("row[2,7]", null);
            actualSIVInformation.Add("row[2,8]", ConstantValues.Volume);
            actualSIVInformation.Add("row[2,9]", ConstantValues.MeasurementUnit);
            actualSIVInformation.Add("row[2,10]", ConstantValues.LogisticStaticMeesage);
            actualSIVInformation.Add("row[2,11]", ConstantValues.LogisticStaticMeesage);
            actualSIVInformation.Add("row[2,12]", ConstantValues.LogisticStaticMeesage);
            actualSIVInformation.Add("row[2,13]", ConstantValues.LogisticStaticMeesage);
            actualSIVInformation.Add("row[2,14]", null);
            actualSIVInformation.Add("row[2,15]", null);
            actualSIVInformation.Add("row[2,16]", DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

            BlobStorageDataSource blobStorageDataSource = new BlobStorageDataSource();
            var blobContainer = blobStorageDataSource.Read("true/logistics");
            var blob = await blobStorageDataSource.Read(blobContainer, "ReporteLogistico_" + this.GetValue("CategorySegment") + "_" + this.GetValue(ConstantValues.TicketId) + "_" + "25302" + ".xlsx").ConfigureAwait(false);
            await blobStorageDataSource.Download(blob, Path.Combine(FilePaths.EventsFilePath.GetFullPath(), "OperationalDataAndConfiguration.xlsx")).ConfigureAwait(false);
            ExcelPackage package = new ExcelPackage(new FileInfo(FilePaths.DownloadedOwnershipExcelPath.GetFullPath()));
            var expectedEventsInformation = new Dictionary<string, string>();
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 1; i <= worksheet.Dimension.Columns; i++)
            {
                expectedEventsInformation.Add("Column" + i, worksheet.Cells[1, i].Value.ToString());
                if (i != 6 && i != 7 && i != 14 && i != 15)
                {
                    expectedEventsInformation.Add("row[2," + i + "]", worksheet.Cells[2, i].Value.ToString());
                }
                else
                {
                    expectedEventsInformation.Add("row[2," + i + "]", null);
                }
            }

            package.Dispose();
            Assert.IsTrue(this.VerifyDiffs(expectedEventsInformation, actualSIVInformation));
        }
    }
}
