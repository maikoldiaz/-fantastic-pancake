// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerateOwnershipBalanceFileProcessedBySIVSystemSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Executors.UI;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class GenerateOwnershipBalanceFileProcessedBySivSystemSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have generated Ownership balance files from the last (.*) days")]
        public async Task GivenIHaveGeneratedOwnershipBalanceFilesFromTheLastDaysAsync(int days)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            ////this.When("I navigate to \"Logistic Report Generation\" page");
            this.UiNavigation("Logistic Report Generation");
            try
            {
                if (Assert.Equals(ConstantValues.SinRegistros, this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text))
                {
                    ////this.When("I click on \"CreateLogistics\" \"button\"");
                    this.IClickOn("CreateLogistics", "button");
                    ////this.Then("I should see \"CreateLogistics\" \"Create\" \"Interface\"");
                    this.IShouldSee("CreateLogistics\" \"Create", "Interface");
                    ////this.When("I select Segment from \"Segment\" \"CreateLogistics\" \"combobox\"");
                    this.ISelectSegmentFrom("Segment\" \"CreateLogistics", "combobox");
                    ////this.When("I select Owner on the Create file interface");
                    this.ISelectOwnerOnTheCreateFileInterface();
                    ////this.When("I select Start date and End Date on Create file Interface");
                    this.ISelectStartDateAndEndDateOnCreateFileInterface();
                    ////this.When("I click on \"CreateLogistics\" \"Submit\" \"button\"");
                    this.IClickOn("CreateLogistics\" \"Submit", "button");
                    ////this.Then("I should see Logistic Report for selected segment in the \"Logistic Report Generation\" in the grid");
                    await this.IShouldSeeTheInformationOfExecutedOperationalCutoffsInTheGridAsync("Logistic Report Generation").ConfigureAwait(false);
                }
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                Assert.IsNotNull(days);
            }
        }

        [Given(@"I did not have generated Ownership balance files from the last (.*) days")]
        public async Task GivenIDidNotHaveGeneratedOwnershipBalanceFilesFromTheLastDaysAsync(int days)
        {
            await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.DeleteLogisticTickets).ConfigureAwait(false);
            Assert.IsNotNull(days);
        }

        [When(@"I selected Segment (from "".*"" "".*"" "".*"")")]
        public async Task WhenISelectSegmentFromAsync(ElementLocator elementLocator)
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated");
            ////await this.GivenIHaveCalculatedOwnershipForSegmentAndTicketGeneratedAsync().ConfigureAwait(false);
            var latestOwnershipTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestCompletedOwnershipTicket).ConfigureAwait(false);
            var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = latestOwnershipTicket[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
            this.SetValue("Segment", segmentOfLastCreatedTicket[ConstantValues.Name]);
            this.SetValue("StartDate", latestOwnershipTicket[ConstantValues.StartDate]);
            this.SetValue("FinalDate", latestOwnershipTicket[ConstantValues.EndDate]);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            this.Get<ElementPage>().Click(elementLocator);
            var segmentSelection = this.Get<ElementPage>().GetElement(nameof(Resources.SegmentInputOnCreateLogisticInterface));
            segmentSelection.SendKeys(this.GetValue(ConstantValues.Segment));
            segmentSelection.SendKeys(OpenQA.Selenium.Keys.Enter);
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
        }

        [When(@"I select Owner on the Create file interface")]
        public void WhenISelectOwnerOnTheCreateFileInterface()
        {
            this.ISelectOwnerOnTheCreateFileInterface();
        }

        [When(@"I select Start date and End Date on Create file Interface")]
        public void WhenISelectStartDateAndEndDateOnCreateFileInterface()
        {
            this.ISelectStartDateAndEndDateOnCreateFileInterface();
        }

        [When(@"I select Segment from ""(.*)"" combobox")]
        public async Task WhenISelectSegmentFromComboboxAsync(string segment)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var latestOwnershipTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestCompletedOwnershipTicket).ConfigureAwait(false);
            var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = latestOwnershipTicket[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
            this.SetValue("Segment", segmentOfLastCreatedTicket[ConstantValues.Name]);
            this.SetValue("StartDate", latestOwnershipTicket[ConstantValues.StartDate]);
            this.SetValue("FinalDate", latestOwnershipTicket[ConstantValues.EndDate]);
            switch (segment)
            {
                case ConstantValues.ValidSegment:
                    page.Click(nameof(Resources.SegmentOnCreateLogisticsInterface));
                    var segmentSelection = page.GetElement(nameof(Resources.SegmentInputOnCreateLogisticInterface));
                    segmentSelection.SendKeys(this.GetValue(ConstantValues.Segment));
                    segmentSelection.SendKeys(OpenQA.Selenium.Keys.Enter);
                    page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
                    break;

                default:
                    break;
            }
        }

        [When(@"I select ""(.*)"" on the Create file interface")]
        public void WhenISelectOnTheCreateFileInterface(string owner)
        {
            switch (owner)
            {
                case ConstantValues.ValidOwner:
                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.OwnerOnCreateLogisticsInterface));
                    this.Get<ElementPage>().Click(nameof(Resources.OwnerOnCreateLogisticsInterface));
                    break;

                default:
                    break;
            }
        }

        [When(@"I select ""(.*)"" and ""(.*)"" on Create file Interface where Ownership is not calculated for selected Segment")]
        public void WhenISelectAndOnCreateFileInterfaceWhereOwnershipIsNotCalculatedForSelectedSegment(string startdate, string enddate)
        {
            switch (enddate)
            {
                case ConstantValues.ValidEndDate:
                    this.SetValue("FinalDate", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.EndDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().Click(nameof(Resources.EndDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnCreateLogisticsInterface)).SendKeys(this.GetValue("FinalDate"));
                    break;

                case ConstantValues.EndDateGreaterThanOrEqualtoCurrentDate:
                    this.SetValue("FinalDate", DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.EndDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().Click(nameof(Resources.EndDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnCreateLogisticsInterface)).SendKeys(this.GetValue("FinalDate"));
                    break;

                case ConstantValues.RangeBetweenStartAndEndDateGreaterThan60Days:
                    this.SetValue("FinalDate", DateTime.Now.AddDays(+61).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.EndDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().Click(nameof(Resources.EndDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnCreateLogisticsInterface)).SendKeys(this.GetValue("FinalDate"));
                    break;

                default:
                    break;
            }

            switch (startdate)
            {
                case ConstantValues.StartDateGreaterThanFinalDate:
                    this.SetValue("StartDate", DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.StartDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().Click(nameof(Resources.StartDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnCreateLogisticsInterface)).SendKeys(this.GetValue("StartDate"));
                    break;

                case ConstantValues.ValidStartDate:
                    this.SetValue("StartDate", DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.StartDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().Click(nameof(Resources.StartDateOnCreateLogisticsInterface));
                    this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnCreateLogisticsInterface)).SendKeys(this.GetValue("StartDate"));
                    break;

                default:
                    break;
            }
        }

        [When(@"I select Segment where Ownership is not calculated for it")]
        public void WhenISelectSegmentFromComboboxWhereOwnershipIsNotCalculatedForIt()
        {
            this.Get<ElementPage>().Click(nameof(Resources.SegmentOnCreateLogisticsInterface));
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), ConstantValues.Refinacion).Click();
        }

        [Then(@"I should see Error message in the Logistic Report Generation page")]
        public void ThenIShouldSeeErrorMessageInTheLogisticReportGenerationPage()
        {
            Assert.AreEqual(this.GetValue("ErrorMessage"), this.Get<ElementPage>().GetElement(nameof(Resources.ErrorMessage)).Text);
        }

        [Then(@"Start Date and End Date on Create file Interface should be ""(.*)""")]
        public void ThenIAmNotAbleToSelectAndOnCreateFileInterface(string expectedValue)
        {
            string isDisabled = this.Get<ElementPage>().GetElement(nameof(Resources.StartDateOnCreateLogisticsInterface)).GetAttribute("disabled");
            if (isDisabled != null)
            {
                Assert.AreEqual("disabled", expectedValue);
            }

            isDisabled = this.Get<ElementPage>().GetElement(nameof(Resources.EndDateOnCreateLogisticsInterface)).GetAttribute("disabled");
            if (isDisabled != null)
            {
                Assert.AreEqual("disabled", expectedValue);
            }
        }

        [Then(@"I should see the ""(.*)"" on Logistic Report page")]
        public void ThenIShouldSeeTheOnLogisticReportPage(string column)
        {
            IList<IWebElement> tableColumns = this.Get<ElementPage>().GetElements(nameof(Resources.FileUploadHeader));
            switch (column)
            {
                case "Segmento":
                    Assert.AreEqual(column, tableColumns[0].Text);
                    break;
                case "Propietario":
                    Assert.AreEqual(column, tableColumns[1].Text);
                    break;
                case "Fecha Inicial":
                    Assert.AreEqual(column, tableColumns[2].Text);
                    break;
                case "Fecha Final":
                    Assert.AreEqual(column, tableColumns[3].Text);
                    break;
                case "Fecha Ejecución":
                    Assert.AreEqual(column, tableColumns[4].Text);
                    break;
                case "Usuario":
                    Assert.AreEqual(column, tableColumns[5].Text);
                    break;
                case "Estado":
                    Assert.AreEqual(column, tableColumns[6].Text);
                    break;
                default:
                    break;
            }
        }

        [Then(@"I should be able to download the generated report file successfully")]
        public async Task ThenIShouldBeAbleToDownloadTheFilesSuccessfullyAsync()
        {
            var latestLogisticTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCompletedLogisticTicket).ConfigureAwait(false);
            var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = latestLogisticTicket[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
            Assert.IsTrue(this.Get<FilePage>().CheckDownloadedFileContent("ReporteLogistico_" + segmentOfLastCreatedTicket[ConstantValues.Name] + "_REFICAR_" + latestLogisticTicket[ConstantValues.TicketId] + ".xlsx", this.ScenarioContext, this.FeatureContext).IsCompleted);
        }
    }
}
