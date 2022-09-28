// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadExcelFilesForPlanningAndProgrammingAndCollaborationAgreementsSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Executors.UI;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class UploadExcelFilesForPlanningAndProgrammingAndCollaborationAgreementsSteps : WebStepDefinitionBase
    {
        [Given(@"I have processed files information from the last (.*) days")]
        public async Task GivenIHaveProcessedFilesInformationFromTheLastDaysAsync(int days)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            ////this.When("I navigate to \"FileUploadForPlanningAndProgrammingAndCollaborationAgreements\" page");
            this.UiNavigation("FileUploadForPlanningAndProgrammingAndCollaborationAgreements");
            try
            {
                if (Assert.Equals(ConstantValues.SinRegistros, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text))
                {
                    ////this.When("I click on \"LoadNew\" \"button\"");
                    this.IClickOn("LoadNew", "button");
                    ////this.Then("I should see upload new file interface");
                    this.IShouldSeeUploadNewFileInterface();
                    ////this.When("I select \"Planning, Programming and Agreements\" from FileType dropdown");
                    this.ISelectFromFileTypeDropdown("Planning, Programming and Agreements");
                    ////this.When("I select \"Insert\" from FileUpload dropdown");
                    this.ISelectFileFromFileUploadDropdown("Insert");
                    ////this.When("I click on \"Browse\" to upload in planning, programming and collaboration agreements page");
                    this.IClickOnUploadButton("Browse");
                    ////this.When("I select \"ValidExcel\" file from planning, programming and collaboration agreements directory");
                    await this.ISelectFileFromExplorerAsync("ValidExcel").ConfigureAwait(false);
                    ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
                    this.IClickOn("uploadFile\" \"Submit", "button");
                    ////this.Then("I should see the uploaded file in the Grid");
                    Assert.AreEqual(ConstantValues.ValidExcel, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text);
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.IsNotNull(days);
            }
        }

        [Given(@"I have no information from the last (.*) days")]
        public void GivenIHaveNoInformationFromTheLastDays(int days2)
        {
            Assert.IsNotNull(days2);
        }

        [Then(@"I should see the ""(.*)"" on Events File Upload page")]
        public void ThenIShouldSeeTheOnFileUploadPage(string column)
        {
            IList<IWebElement> tableColumns = this.Get<ElementPage>().GetElements(nameof(Resources.FileUploadHeader));
            switch (column)
            {
                case "Fecha":
                    Assert.AreEqual(column, tableColumns[0].Text);
                    break;
                case "Archivo":
                    Assert.AreEqual(column, tableColumns[1].Text);
                    break;
                case "Acción":
                    Assert.AreEqual(column, tableColumns[2].Text);
                    break;
                case "Usuario":
                    Assert.AreEqual(column, tableColumns[3].Text);
                    break;
                case "Estado":
                    Assert.AreEqual(column, tableColumns[4].Text);
                    break;
                case "Tipo":
                    Assert.AreEqual(column, tableColumns[5].Text);
                    break;
                case "Registros procesados":
                    Assert.AreEqual(column, tableColumns[6].Text);
                    break;
                default:
                    break;
            }
        }

        [When(@"I select ""(.*)"" from FileType dropdown")]
        public void WhenISelectFromFileTypeDropdown(string fileType)
        {
            this.ISelectFromFileTypeDropdown(fileType);
        }

        [StepDefinition(@"I should see upload new file interface")]
        public void ThenIShouldSeeUploadNewFileInterface()
        {
            this.IShouldSeeUploadNewFileInterface();
        }

        [Then(@"I should see Search Interface")]
        public void ThenIShouldSeeSearchInterface()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.ElementByText), formatArgs: ConstantValues.SearchCriteria).Displayed);
        }

        [When(@"I select ""(.*)"" file from planning, programming and collaboration agreements directory")]
        public async Task WhenISelectFileFromDirectoryAsync(string fileName)
        {
            await this.ISelectFileFromExplorerAsync(fileName).ConfigureAwait(false);
        }

        [When(@"I click on ""(.*)"" to upload in planning, programming and collaboration agreements page")]
        public void WhenIClickOnToUpload(string locator)
        {
            this.IClickOnUploadButton(locator);
        }

        [When(@"I select start date on Search Interface")]
        public void WhenISelectStartDateOnSearchInterface()
        {
            this.ScenarioContext.Add("StartDate", DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().Click(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(this.ScenarioContext["StartDate"].ToString());
            this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [When(@"I select end date greater than start date on Search Interface")]
        public void WhenISelectEndDateGreaterThanStartDateOnSearchInterface()
        {
            this.ScenarioContext.Add("FinalDate", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().Click(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(this.ScenarioContext["FinalDate"].ToString());
            this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [When(@"I select start date on Search Interface less than (.*) months from current date")]
        public void WhenISelectStartDateOnSearchInterfaceLessThanMonthsFromCurrentDate(int months)
        {
            Assert.IsNotNull(months);
            this.ScenarioContext.Add("StartDate", DateTime.Now.AddDays(-200).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().Click(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(this.ScenarioContext["StartDate"].ToString());
            this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"I should see the results based on the selected filter on Search Interface")]
        public async Task ThenIShouldSeeTheResultsBasedOnTheSelectedFilterOnSearchInterfaceAsync()
        {
            if (this.GetValue("StartDate") == DateTime.Now.AddDays(-200).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
            {
                this.ScenarioContext["StartDate"] = DateTime.Now.AddDays(-200).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 00:00:00.000";
            }

            this.ScenarioContext["StartDate"] = DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 00:00:00.000";
            this.ScenarioContext["FinalDate"] = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " 00:00:00.000";
            var totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllUploadedRecordsInFileUploadGrid, args: new { startDate = this.ScenarioContext["StartDate"].ToString(), endDate = this.ScenarioContext["FinalDate"].ToString() }).ConfigureAwait(false);
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var count = paginationCount.Split(' ');
            Assert.AreEqual(totalRecords.Count().ToString(CultureInfo.InvariantCulture), count[4]);
        }

        [When(@"I do not enter values for Mandatory fields on Search Interface")]
        public void WhenIDoNotEnterValuesForMandatoryFieldsOnSearchInterface()
        {
            //// Not writing step definition as it is not required anything
        }

        [When(@"I enter values for Mandatory fields on Search Interface")]
        public void WhenIEnterValuesForMandatoryFieldsOnSearchInterface()
        {
            this.ScenarioContext.Add("StartDate", DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().Click(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(this.ScenarioContext["StartDate"].ToString());
            this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
            this.ScenarioContext.Add("FinalDate", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().Click(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid));
            this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(this.ScenarioContext["FinalDate"].ToString());
            this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnSearchInterfaceOfFileUploadGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [Then(@"I should see the uploaded file in the Grid")]
        public void ThenIShouldSeeTheUploadedFileInTheGrid()
        {
            Assert.AreEqual(ConstantValues.ValidExcel, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text);
        }

        [StepDefinition(@"I should see entered values in the fields are cleared")]
        public void ThenIShouldSeeEnteredValuesInTheFieldsAreCleared()
        {
            ////this.When("I click on \"UploadFileFilter\" \"Apply\" \"button\"");
            this.IClickOn("UploadFileFilter\" \"Apply", "button");
            ////this.Then("I should see the message on interface \"Requerido\"");
            this.IShouldSeeTheMessageRequired("Requerido");
        }

        [Then(@"I should be able to download file successfully")]
        public async Task ThenIShouldBeAbleToDownloadTheFilesSuccessfullyAsync()
        {
            var completedFileOnFileUploadGrid = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestUploadedEventRecordInFileUploadGrid).ConfigureAwait(false);
            Assert.IsTrue(this.Get<FilePage>().CheckDownloadedFileContent(completedFileOnFileUploadGrid[ConstantValues.UploadId] + ".xlsx", this.ScenarioContext, this.FeatureContext).IsCompleted);
        }
    }
}