// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterMovementsInventoriesMonthChangesSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using OfficeOpenXml;

    using TechTalk.SpecFlow;

    [Binding]
    public class RegisterMovementsInventoriesMonthChangesSteps : EcpApiStepDefinitionBase
    {
        public RegisterMovementsInventoriesMonthChangesSteps(FeatureContext featureContext)
          : base(featureContext)
        {
        }

        [Given(@"I have '(.*)' in the system to register '(.*)'")]
        public async Task GivenIHaveInTheSystemToRegisterAsync(string entity, string entity1)
        {
            Assert.IsNotNull(entity);
            Assert.IsNotNull(entity1);
            ////this.Given("I want to register a \"Homologation\" in the system");
            await this.IWantToRegisterAnExcelHomologationInTheSystemAsync("Homologation", this).ConfigureAwait(false);
            ////this.When("I am logged in as \"admin\"");
            this.LoggedInAsUser("admin");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
        }

        [When(@"I receive ""(.*)"" of the previous month not in the first days of the current month")]
        [When(@"I receive ""(.*)"" of the previous month in the first days of the current month")]
        public async Task WhenIReceiveOfThePreviousMonthInTheFirstDaysOfTheCurrentMonthAsync(string field)
        {
            Assert.IsNotNull(field);
            this.UpdateExcel("PreviousMonth");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"Testdata_20213\" file from directory");
            await this.ISelectFileFromDirectoryAsync("Testdata_20213").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [When(@"I receive ""(.*)"" of the current month")]
        public async Task WhenIReceiveOfTheCurrentMonthAsync(string field)
        {
            Assert.IsNotNull(field);
            this.UpdateExcel("CurrentMonth");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"Testdata_20213\" file from directory");
            await this.ISelectFileFromDirectoryAsync("Testdata_20213").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Then(@"""(.*)"" must be stored in a Pendingtransactions repository with error message ""(.*)""")]
        public async Task ThenMustBeStoredInAPendingtransactionsRepositoryWithErrorMessageAsync(string entity, string message)
        {
            Assert.IsNotNull(entity);
            var lastCreatedRows = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.PendingTransactions], args: new { messageCode = this.ScenarioContext[ConstantValues.UploadId].ToString() }).ConfigureAwait(false);
            foreach (var singleRow in lastCreatedRows)
            {
                this.SetValue(Keys.Error, singleRow[ConstantValues.ErrorJson]);
                Assert.IsTrue(this.GetValue(Keys.Error).Contains(message));
            }
        }

        [When(@"I receive ""(.*)"" of the todays date")]
        public async Task WhenIReceiveOfTheTodaysDateAsync(string field)
        {
            Assert.IsNotNull(field);
            this.UpdateExcel("TodaysDate");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"Testdata_20213\" file from directory");
            await this.ISelectFileFromDirectoryAsync("Testdata_20213").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [When(@"I receive ""(.*)"" of the invalid previous month in the first days of the current month")]
        public async Task WhenIReceiveOfTheInvalidPreviousMonthInTheFirstDaysOfTheCurrentMonthAsync(string field)
        {
            Assert.IsNotNull(field);
            this.UpdateExcel("InValidPreviousMonth");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"Testdata_20213\" file from directory");
            await this.ISelectFileFromDirectoryAsync("Testdata_20213").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public void UpdateExcel(string month)
        {
            var uploadFileName = @"TestData\Input\ExcelUpload\Testdata_20213.xlsx";

            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[i, 2].Value = "AutomationInventory " + new Faker().Random.Number(99999, 999999);
            }

            var now = DateTime.Now;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            if (month.EqualsIgnoreCase("InValidPreviousMonth"))
            {
                worksheet.Cells[2, 3].Value = firstDayCurrentMonth.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 3].Value = firstDayCurrentMonth.AddDays(-9).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                worksheet.Cells[4, 3].Value = firstDayCurrentMonth.AddDays(-9).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            }
            else if (month.EqualsIgnoreCase("PreviousMonth"))
            {
                worksheet.Cells[2, 3].Value = firstDayCurrentMonth.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 3].Value = firstDayCurrentMonth.AddDays(-8).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                worksheet.Cells[4, 3].Value = firstDayCurrentMonth.AddDays(-8).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            }
            else if (month.EqualsIgnoreCase("TodaysDate"))
            {
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddHours(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddHours(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                worksheet.Cells[4, 3].Value = DateTime.UtcNow.AddHours(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            }
            else
            {
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 3].Value = firstDayCurrentMonth.ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                worksheet.Cells[4, 3].Value = firstDayCurrentMonth.ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            }

            // For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[i, 2].Value = new Faker().Random.Number(11111, 99999);
                ////this.FeatureContext.Add("Movement" + (i - 1), worksheet.Cells[i, 2].Value);
            }

            // Modifying data in FechaHoraInicial(j=4) and FechaHoraFinal(j=5) columns
            for (int j = 4; j <= 5; j++)
            {
                if (month.EqualsIgnoreCase("InValidPreviousMonth"))
                {
                    worksheet.Cells[2, j].Value = firstDayCurrentMonth.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                    worksheet.Cells[3, j].Value = firstDayCurrentMonth.AddDays(-9).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                    worksheet.Cells[4, j].Value = firstDayCurrentMonth.AddDays(-9).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
                }
                else if (month.EqualsIgnoreCase("PreviousMonth"))
                {
                    worksheet.Cells[2, j].Value = firstDayCurrentMonth.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                    worksheet.Cells[3, j].Value = firstDayCurrentMonth.AddDays(-8).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                    worksheet.Cells[4, j].Value = firstDayCurrentMonth.AddDays(-8).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
                }
                else if (month.EqualsIgnoreCase("TodaysDate"))
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddHours(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddHours(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                    worksheet.Cells[4, j].Value = DateTime.UtcNow.AddHours(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
                }
                else
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                    worksheet.Cells[3, j].Value = firstDayCurrentMonth.ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                    worksheet.Cells[4, j].Value = firstDayCurrentMonth.ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
                }
            }

            package.Save();
            package.Dispose();
        }
    }
}
