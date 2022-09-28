// <copyright file="CommonSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Entities;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using OfficeOpenXml;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    using TechTalk.SpecFlow;

    using Keys = OpenQA.Selenium.Keys;

    [Binding]
    public class CommonSteps : EcpWebStepDefinitionBase
    {
        [When(@"I click (on "".*"" "".*"")")]
        [When(@"I click (on "".*"" "".*"") of a combination having or not having value")]
        [When(@"I click (on "".*"" "".*"") of any record")]
        [When(@"I click (on "".*"" "".*"") of a combination not having value")]
        [When(@"I click (on "".*"" "".*"") of a combination having value")]
        [Then(@"I click (on "".*"" "".*"")")]
        public void WhenIClickOn(ElementLocator elementLocator)
        {
            this.IClickOn(elementLocator);
        }

        [StepDefinition(@"I should (see "".*"" "".*"")")]
        public void ThenIShouldSee(ElementLocator elementLocator)
        {
            this.IShouldSee(elementLocator);
        }

        [When(@"I change value from ""(.*)""")]
        [When(@"I select value from ""(.*)""")]
        public void WhenISelectValueFrom(string field)
        {
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            //// this.Get<ElementPage>().Click(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.SelectBoxMenu), 5, formatArgs: UIContent.Conversion[field]);
            var connectionGridRow = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOption), formatArgs: UIContent.Conversion[field]);
            var selectOptionElement = connectionGridRow[connectionGridRow.Count - 1];
            this.SetValue(Entities.Keys.SelectedValue, connectionGridRow[connectionGridRow.Count - 1].Text);
            Actions action = new Actions(this.DriverContext.Driver);
            action.MoveToElement(selectOptionElement).Perform();
            selectOptionElement.Click();
        }

        [When(@"I select ""(.*)"" from ""(.*)""")]
        public void WhenISelectFrom(string value, string field)
        {
            this.SelectValueFromDropDown(value, field);
        }

        [When(@"I select ""(.*)"" from ""(.*)"" dropdown")]
        public void WhenISelectFromDropdown(string value, string field)
        {
            this.SelectValueFromDropDown(UIContent.Conversion[value], field);
        }

        [When(@"I select ""(.*)"" from ""(.*)"" combo box in ""(.*)"" grid")]
        public async Task WhenISelectFromComboBoxInGridAsync(string value, string field, string gridName)
        {
            await this.SelectValueFromComboBoxAsync(value, field, gridName).ConfigureAwait(false);
        }

        [When(@"I provide value (for "".*"" "".*"")")]
        public void WhenIProvideValueFor(ElementLocator elementLocator)
        {
            this.IProvideValueFor(elementLocator);
        }

        [StepDefinition(@"I am (logged in as "".*"")")]
        [StepDefinition(@"I (login again as "".*"")")]
        public void GivenIAmLoggedInAsUser(Credentials user)
        {
            this.LoggedInAsUser(user);
        }

        [When(@"I don't provide value (for "".*"" "".*"")")]
        [When(@"I remove value (for "".*"" "".*"")")]
        public void WhenIDonTProvideValueFor(ElementLocator elementLocator)
        {
            this.SetValue(Entities.Keys.Field, elementLocator != null ? elementLocator.Value : string.Empty);
            this.Get<ElementPage>().ClearText(elementLocator);
        }

        [StepDefinition(@"I refresh the page")]
        public void WhenIClickOnButton()
        {
            this.Get<WindowPage>().Refresh();
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        [StepDefinition(@"I navigate to ""(.*)"" page")]
        public void WhenINaviagteToPage(string link)
        {
            this.NavigateToPage(link);
        }

        [When(@"I enter valid ""(.*)"" (into "".*"" "".*"")")]
        public void WhenIEnterValidInto(string data, ElementLocator elementLocator)
        {
            this.IEnterValidInto(data, elementLocator);
        }

        [When(@"I enter invalid value (into "".*"" "".*"")")]
        public void WhenIEnterInvalidValueInto(ElementLocator elementLocator)
        {
            string data = "aa";
            this.EnterValueIntoTextBox(elementLocator, data);
        }

        [When(@"I clear value (into "".*"" "".*"")")]
        public void WhenIClearValueInto(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().GetElement(elementLocator).Clear();
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(Keys.Tab);
        }

        [StepDefinition(@"I enter valid value (into "".*"" "".*"")")]
        public void WhenIEnterValidValueInto(ElementLocator elementLocator)
        {
            this.IEnterValidValueInto(elementLocator);
        }

        [StepDefinition(@"I enter ""(.*)"" (into "".*"" "".*"")")]
        public void WhenIEnterInto(string value, ElementLocator elementLocator)
        {
            this.IEnterValueInto(value, elementLocator);
        }

        [Then(@"the new value should be updated (in "".*"" "".*"")")]
        public void ThenTheNewValueShouldBeUpdatedIn(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsTrue(this.Get<ElementPage>().GetElement(elementLocator).Text.Contains(ConstantValues.ValidValue));
        }

        [StepDefinition(@"I should see error message ""(.*)""")]
        public void ThenIShouldSeeErrorMessage(string message)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            try
            {
                this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessage));
                Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text);
            }
            catch (WebDriverTimeoutException)
            {
                try
                {
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessageControl));
                    Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessageControl)).Text);
                }
                catch (WebDriverTimeoutException)
                {
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorMessageOnCreateLogisticInterface));
                    Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessageOnCreateLogisticInterface)).Text);
                }
            }
        }

        [Then(@"I should see message ""(.*)""")]
        public void ThenIShouldSeeMessage(string message)
        {
            this.IShouldSeeMessage(message);
        }

        [When(@"I click (on "".*"" "".*"") in filter")]
        public void WhenIClickOnInFilter(ElementLocator elementLocator)
        {
            this.WhenIClickOn(elementLocator);
            this.ScenarioContext["DropDownIndex"] = 1;
        }

        [When(@"I click (on "".*"" "".*"") condition in filter")]
        public void WhenIClickOnConditionInFilter(ElementLocator elementLocator)
        {
            this.WhenIClickOn(elementLocator);
            this.ScenarioContext["DropDownIndex"] = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture) + 1;
        }

        [Then(@"I navigate to ""(.*)""")]
        public void ThenINavigateTo(string link)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().Click(nameof(Resources.Module), formatArgs: UIContent.Conversion[link]);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        [When(@"I select ""(.*)"" from ""(.*)"" filter")]
        public void WhenISelectFromFilter(string value, string field)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var dd_value = value.ContainsIgnoreCase("element") ? this.ScenarioContext[UIContent.FilterValueConversion[value]].ToString() : value;
            this.SelectValueFromDropDown(dd_value, field);
        }

        [StepDefinition(@"I update the excel with ""(.*)"" data")]
        public void WhenIUpdateTheExcelWithDaywiseData(string fileName)
        {
            this.WhenIUpdateTheExcelFileWithDaywiseData(fileName);
        }

        [When(@"I update the excel with ""(.*)""")]
        public void WhenIUpdateTheExcel(string fileName)
        {
            this.IUpdateTheExcelFile(fileName);
        }

        [When(@"I update the excel with ""(.*)"" new data")]
        public void WhenIUpdateTheExcelWithNewData(string fileName)
        {
            this.IUpdateTheExcelWithNewData(fileName);
        }

        [When(@"I update the excel data for inventories in ""(.*)""")]
        public void WhenIUpdateTheExcelDataForInventories(string fileName)
        {
            this.IUpdateTheExcelDataForInventories(fileName);
        }

        [When(@"I update the excel ""(.*)"" to process it")]
        public async Task WhenIUpdateTheExcelToProcessItAsync(string fileName)
        {
            BlobStorageDataSource blobStorageDataSource = new BlobStorageDataSource();
            var blobContainer = blobStorageDataSource.Read("ownership");
            var blob = await blobStorageDataSource.Read(blobContainer, "DatosOperativosyConfiguraciones_" + this.GetValue(ConstantValues.TicketId) + ".xlsx").ConfigureAwait(false);
            await blobStorageDataSource.Download(blob, Path.Combine(FilePaths.EventsFilePath.GetFullPath(), "OperationalDataAndConfiguration.xlsx")).ConfigureAwait(false);

            ExcelPackage package = new ExcelPackage(new FileInfo(FilePaths.DownloadedOwnershipExcelPath.GetFullPath()));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                this.SetValue("InventoryRows", worksheet.Dimension.Rows.ToString(CultureInfo.InvariantCulture));
                this.SetValue("Inventory" + (i - 1), worksheet.Cells[i, 2].Value.ToString());
                this.SetValue("NetStandardVolumeInventories" + (i - 1), worksheet.Cells[i, 9].Value.ToString());
            }

            worksheet = package.Workbook.Worksheets[1];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                this.SetValue("MovementRows", worksheet.Dimension.Rows.ToString(CultureInfo.InvariantCulture));
                this.SetValue("Movement" + (i - 1), worksheet.Cells[i, 2].Value.ToString());
                this.SetValue("NetStandardVolumeMovements" + (i - 1), worksheet.Cells[i, 16].Value.ToString());
            }

            package.Dispose();

            var newFileName = fileName != null ? fileName + "_" + this.GetValue(ConstantValues.TicketId) : string.Empty;

            var oldFileNamePath = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            var newFileNamePath = @"TestData\Input\ExcelUpload\" + newFileName + ".xlsx";

            File.Delete(newFileNamePath.GetFullPath());
            File.Move(oldFileNamePath.GetFullPath(), newFileNamePath.GetFullPath());

            ExcelPackage package1 = new ExcelPackage(new FileInfo(newFileNamePath.GetFullPath()));

            // For Movements
            ExcelWorksheet worksheet1 = package1.Workbook.Worksheets[1];
            for (int i = 2; i <= this.GetValue("MovementRows").ToInt(); i++)
            {
                worksheet1.Cells[i, 1].Value = this.GetValue(ConstantValues.TicketId);
                worksheet1.Cells[i, 2].Value = this.GetValue("Movement" + (i - 1));
                worksheet1.Cells[i, 3].Value = ConstantValues.EcoPetrolId;
                worksheet1.Cells[i, 4].Value = ConstantValues.EcoPetrol;
                worksheet1.Cells[i, 5].Value = ConstantValues.EcoPetrolOwnershipPercentage;
                worksheet1.Cells[i, 6].Value = this.GetValue("NetStandardVolumeMovements" + (i - 1));
                worksheet1.Cells[i, 7].Value = ConstantValues.RuleId;
                worksheet1.Cells[i, 8].Value = ConstantValues.OwnershipVersion;
                worksheet1.Cells[i, 9].Value = DateTime.Now.ToShortDateString();
            }

            // For Inventories
            ExcelWorksheet worksheet2 = package1.Workbook.Worksheets[2];
            for (int i = 2; i <= this.GetValue("InventoryRows").ToInt(); i++)
            {
                worksheet2.Cells[i, 1].Value = this.GetValue(ConstantValues.TicketId);
                worksheet2.Cells[i, 2].Value = this.GetValue("Inventory" + (i - 1));
                worksheet2.Cells[i, 3].Value = ConstantValues.EcoPetrolId;
                worksheet2.Cells[i, 4].Value = ConstantValues.EcoPetrol;
                worksheet2.Cells[i, 5].Value = ConstantValues.EcoPetrolOwnershipPercentage;
                worksheet2.Cells[i, 6].Value = this.GetValue("NetStandardVolumeInventories" + (i - 1));
                worksheet2.Cells[i, 7].Value = ConstantValues.RuleId;
                worksheet2.Cells[i, 8].Value = ConstantValues.OwnershipVersion;
                worksheet2.Cells[i, 9].Value = DateTime.Now.ToShortDateString();
            }

            package1.Save();
            package1.Dispose();

            try
            {
                await BlobExtensions.UploadExcelFileAsync(ApiContent.ContainerNames[ConstantValues.Ownership], newFileName, newFileName).ConfigureAwait(false);
            }
            catch (Exception)
            {
                Assert.Fail();
                throw;
            }
        }

        [When(@"I update date range in ""(.*)"" of Events")]
        public void WhenIUpdateTheEventsExcel(string fileName)
        {
            this.IUpdateTheEventsExcel(fileName);
        }

        [Then(@"I should see actual events infromation in the Excel generated in the Blob")]
        public async Task ExcelWithEventsInformationAsync()
        {
            await this.ExcelInformationOfEventsAsync().ConfigureAwait(false);
        }

        [Then(@"I should see headers of events in the Excel generated in the Blob")]
        public void ExcelWithoutEventsInformation()
        {
            this.ExcelInformationWithoutEvents();
        }

        [StepDefinition(@"I wait till file upload to complete")]
        public async Task WaitForFileUploadToCompleteAsync()
        {
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [StepDefinition(@"I wait till cutoff ticket processing to complete")]
        public async Task WaitForTicketProcessingToCompleteAsync()
        {
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
        }

        [When(@"I wait till ownership ticket geneation to complete")]
        public async Task WaitForOwnershipTicketGeneationToCompleteAsync()
        {
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [When(@"I wait till ownership ticket processing to complete")]
        public async Task WaitForOwnershipTicketProcessingToCompleteAsync()
        {
            var ticketRow = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetLastTicketAndElement).ConfigureAwait(false);
            for (int i = 1; i <= 4; i++)
            {
                this.SetValue(ConstantValues.Status, ticketRow[ConstantValues.Status].ToString());
                if (this.GetValue(ConstantValues.Status) == "Finalizdo")
                {
                    Assert.IsNotNull("OwnerShip Calculation is Processed Successfully");
                }
                else
                {
                    await Task.Delay(60000).ConfigureAwait(true);
                }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
                Assert.Fail("OwnerShip Calculation is not Processed Successfully");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }
        }

        [Given(@"I have ""(.*)"" data in  the system")]
        public async Task GivenIHaveDataInTheSystemAsync(string entity)
        {
            if (entity.EqualsIgnoreCase("SalesAndPurchase"))
            {
                var temp = 0;
                var uploadFileName = @"TestData\Input\PurchaseAndSales\ValidExcel.xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                ////updating the worksheet
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    temp = new Faker().Random.Number(1000000, 9999999);
                    worksheet.Cells[i, 1].Value = temp;
                }

#pragma warning disable CA1305 // Specify IFormatProvider
                this.SetValue<string>(ConstantValues.Contract, temp.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider
                package.Save();
                package.Dispose();

                ////this.Given("I have \"SalesAndPurchase\" homologation data in the system");
                await this.IHaveHomologationDataInTheSystemAsync(systemType: "SalesAndPurchase").ConfigureAwait(false);
            }
        }

        [StepDefinition(@"I update data in ""(.*)""")]
        public void GivenIUpdateDataIn(string fileName)
        {
            this.IUpdateDataInExcel(fileName);
        }

        [StepDefinition(@"I have ""(.*)"" in the system through Excel")]
        public async Task GivenIHaveInTheSystemThroughExcelAsync(string entity)
        {
            ////this.Given("I have \"Excel\" homologation data in the system");
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "Excel").ConfigureAwait(false);
            ////this.Given("I am logged in as \"admin\"");
            this.LoggedInAsUser("admin");
            if (entity.EqualsIgnoreCase("InventoryWithBothTankNameBatchId"))
            {
                ////this.Given("I update the excel \"BatchIdTankNameValidation\" with both \"TankName\" and \"BatchId\"");
                this.GivenIUpdateTheExcelWithBothTankNameAndBatchId("BatchIdTankNameValidation", "TankName", "BatchId");
            }
            else if (entity.EqualsIgnoreCase("InventoryWithOnlyTankName"))
            {
                ////this.Given("I update the excel \"BatchIdTankNameValidation\" with only \"TankName\"");
                this.GivenIUpdateTheExcelWithOnlyTankName("BatchIdTankNameValidation", "TankName");
            }
            else if (entity.EqualsIgnoreCase("InventoryWithOnlyBatchId"))
            {
                ////this.Given("I update the excel \"BatchIdTankNameValidation\" with only \"BatchId\"");
                this.GivenIUpdateTheExcelWithOnlyTankName("BatchIdTankNameValidation", "BatchId");
            }
            else if (entity.EqualsIgnoreCase("InventoryWithTwoDifferentProductsButSameTankNameBatchId"))
            {
                ////this.Given("I update the excel \"BatchIdTankNameValidationWithTwoDifferentProducts\" but same 'TankName' and 'BatchId'");
                this.GivenIUpdateTheExcelButSameTwoFields("BatchIdTankNameValidationWithTwoDifferentProducts", "TankName", "BatchId");
            }
            else if (entity.EqualsIgnoreCase("InventoryWithoutTankNameBatchId"))
            {
                ////this.Given("I update the excel with \"BatchIdTankNameValidation\" data");
                this.WhenIUpdateTheExcelFileWithDaywiseData("BatchIdTankNameValidation");
            }
            else
            {
                ////this.Given($"I update data in \"{UIContent.FileName[entity]}\"");
                this.IUpdateDataInExcel(UIContent.FileName[entity]);
            }

            ////this.When("I navigate to \"FileUpload\" page");
            this.NavigateToPage("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When($"I select \"{UIContent.FileName[entity]}\" file from directory");
            await this.ISelectFileFromDirectoryAsync(UIContent.FileName[entity]).ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" datas in the system")]
        public async Task GivenIHaveDatasInTheSystemAsync(string entity)
        {
            Assert.IsNotNull(entity);
            ////this.Given("I have \"SalesAndPurchase\" homologation data in the system");
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "SalesAndPurchase").ConfigureAwait(false);
            ////this.And("I update data in \"InsertContractExcel\"");
            this.IUpdateDataInExcel("InsertContractExcel");
            ////this.When("I navigate to \"FileUploadForSalesAndPurchases\" page");
            this.NavigateToPage("FileUploadForSalesAndPurchases");
            ////this.When("I click on \"LoadNew\" \"button\"");
            this.IClickOn("LoadNew", "button");
            ////this.When("I select in \"Contracts\" from movement type dropdown");
            this.WhenISelectInContractsFromMovementTypeDropdown("Contracts");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload contracts");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"InsertContractExcel\" file from purchase sales");
            this.WhenISelectFileFromGrid("InsertContractExcel");
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.Then("I should see the \"Contract\" \"Inserted\" in the system");
            await this.IShouldSeeTheInTheSystemAsync("Contract", "Inserted").ConfigureAwait(false);
        }

        [When(@"I select the StartDate lessthan ""(.*)"" days from CurrentDate on ""(.*)"" DatePicker")]
        public async Task WhenISelectTheStartDateLessthanDaysFromCurrentDateOnDatePickerAsync(int days, string type)
        {
            await this.ISelectTheStartDateLessthanDaysFromCurrentDateOnDatePickerAsync(days, type).ConfigureAwait(false);
        }

        [When(@"I select the EndDate lessthan ""(.*)"" days from CurrentDate on ""(.*)"" DatePicker")]
        public async Task WhenISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(int days, string type)
        {
            await this.ISelectTheEndDateLessthanDaysFromCurrentDateOnDatePickerAsync(days, type).ConfigureAwait(false);
        }

        [StepDefinition(@"I wait for ""(.*)"" seconds for the process to end")]
        public async Task WhenIWaitForSecondsForTheProcessToEndAsync(int seconds)
        {
            await Task.Delay(seconds * 1000).ConfigureAwait(false);
        }

        [Then(@"I should see welcome page")]
        public void ThenIShouldSeeWelcomePage()
        {
            Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.WelcomePage)));
        }

        [Then(@"I should not see welcome page")]
        public void ThenIShouldNotSeeWelcomePage()
        {
            Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.Unauthorized)));
        }

        [When(@"I have official delta in the system")]
        public async Task WhenIHaveOfficialDeltaInTheSystemAsync()
        {
            this.SetValue(ConstantValues.TestdataForOfficialDeltaPerNodeReport, "Yes");
            ////And I have ownership calculation data generated in the system
            await this.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);

            //// Approving all nodes related segment
            await this.ReadAllSqlAsync(SqlQueries.UpdateOwnershipNodeStatusBasedOnSegmentName, args: new { ownershipStatusId = 9, segment = this.GetValue("SegmentName") }).ConfigureAwait(false);

            ////Official Delta Calculation
            await this.OfficialDeltaTicketGenerationAsync().ConfigureAwait(false);
        }
    }
}