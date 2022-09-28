// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelProcessToRegisterPurchaseAndSalesContractsSteps.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OfficeOpenXml;

    using TechTalk.SpecFlow;

    [Binding]
    public class ExcelProcessToRegisterPurchaseAndSalesContractsSteps : EcpWebStepDefinitionBase
    {
        [When(@"I select in ""(.*)"" from movement type dropdown")]
        public void WhenISelectInFromMovementTypeDropdown(string type)
        {
            this.WhenISelectInContractsFromMovementTypeDropdown(type);
        }

        [When(@"I click on ""(.*)"" to upload contracts")]
        public void WhenIClickOnToUploadContracts(string locator)
        {
            this.IClickOnUploadButton(locator);
        }

        [When(@"I select ""(.*)"" file from purchase sales")]
        public void WhenISelectFileFromPurchaseSales(string operation)
        {
            this.WhenISelectFileFromGrid(operation);
        }

        [Then(@"I verify if i have registered the contracts")]
        public async Task ThenIVerifyIfIHaveRegisteredTheContractsAsync()
        {
            await this.IVerifyIfIHaveRegisteredTheContractsAsync().ConfigureAwait(true);
        }

        [Then(@"I verify i see the ""(.*)"" in audit log")]
        public async Task ThenIVerifyISeeTheInAuditLogAsync(string message)
        {
            await Task.Delay(60000).ConfigureAwait(true);
            var errorRow = await this.ReadSqlScalarAsync<string>(SqlQueries.GetLastTransactionError).ConfigureAwait(false);
            Assert.AreEqual(message, errorRow);
        }

        [When(@"I update invalid (.*) in PurchaseAndSales Excel")]
        public void WhenIUpdateInvalidInPurchaseAndSalesExcel(string column)
        {
            var temp = 0;
            var uploadFileName = @"TestData\Input\PurchaseAndSales\WithoutRecords.xlsx";
#pragma warning disable CA2000 // Dispose objects before losing scope
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
#pragma warning restore CA2000 // Dispose objects before losing scope
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            ////updating the worksheet
            temp = new Faker().Random.Number(1000000, 9999999);
            worksheet.Cells[2, 1].Value = temp;
            switch (column)
            {
                case "Source Node":
                    worksheet.Cells[2, 4].Value = string.Empty;
                    break;
                case "Destination Node":
                    worksheet.Cells[2, 5].Value = string.Empty;
                    break;
                case "Product":
                    worksheet.Cells[2, 6].Value = string.Empty;
                    break;
                case "Comercial":
                    worksheet.Cells[2, 9].Value = string.Empty;
                    break;
                case "unit":
                    worksheet.Cells[2, 11].Value = string.Empty;
                    break;
                case "Frequencia":
                    worksheet.Cells[2, 12].Value = string.Empty;
                    break;
            }

            package.Save();
            package.Dispose();
        }
    }
}