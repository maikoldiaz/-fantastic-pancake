namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class OfficialDeltaCalculationProcessUISteps : EcpWebStepDefinitionBase
    {
        [Then(@"I navigate to ""(.*)"" tab")]
        public void ThenINavigateToTab(string menu)
        {
            this.Get<ElementPage>().Click(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[menu]);
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Navigation[menu]);
            this.Get<ElementPage>().Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[menu]);
        }

        [StepDefinition(@"I navigates to ""(.*)"" link under ""(.*)"" tab")]
        public void GivenINavigateToLinkUnderTab(string submenuItem, string menuitem)
        {
            this.Get<ElementPage>().Click(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[menuitem]);
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Navigation[menuitem]);
            this.Get<ElementPage>().Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[menuitem]);
            this.Get<ElementPage>().ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[submenuItem]);
            this.Get<ElementPage>().Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[submenuItem]);
        }

        [Then(@"Verify if Active segments are showing in the segment drop down")]
        public void ThenVerifyAllActiveSegmentsShouldBeDisplayedInDropdown()
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            this.IClickOn("initOfficialDeltaTicket\" \"segment", "dropdown");
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.SelectBoxOptionByValue), formatArgs: this.ScenarioContext["DeltaCategorySegment"].ToString()));
        }

        [Given(@"I have delta calculation process running for the segment Todos/All")]
        public async Task GivenIHaveDeltaCalculationProcessRunningForTheSegmentTodosAllAsync()
        {
            var count = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetDeltaCalculationRunning).ConfigureAwait(false);
            this.ScenarioContext["OfficialDeltasProcessingCount"] = count;
        }

        [Given(@"I choose active segment for which official information is not present")]
        public async Task GivenIChooseSegmentForWhichOwnershipIsNotGeneratedAsync()
        {
            var segmentRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentWithoutOfficailInfo).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = segmentRow["Name"].ToString();
        }

        [StepDefinition(@"I choose active segment for Official Delta calculation")]
        public async Task GivenIChooseActiveSegmentForOfficialDeltaCalculationAsync()
        {
            var categoryElement = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetActiveSegment).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = categoryElement["Name"].ToString();
            this.ScenarioContext["CategoryElementId"] = categoryElement["ElementId"].ToString();
        }

        [Given(@"I choose a segment which has no operationally unapproved nodes")]
        public async Task GivenIChooseASegmentWhichHasNoOperationallyUnapprovedNodesAsync()
        {
            var categoryElement = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentsNoUnapporvedNodes).ConfigureAwait(false);
        }

        [Given(@"I choose a segment which has operationally unapproved nodes")]
        public async Task GivenIChooseASegmentWhichHasOperationallyUnapprovedNodesAsync()
        {
            var categoryElement = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOperationallyUnapprovedNode).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = categoryElement["NAME"].ToString();
        }

        [Given(@"I choose segment which has delta calculation process running in the background")]
        public async Task GivenIChooseSegmentHasDeltaCalculationProcessRunningInTheBackgroundAsync()
        {
            var categoryElement = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentDeltaIsProcessing).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = categoryElement["NAME"].ToString();
        }

        [Given(@"I choose active segment with movement start date and end date for Official Delta calculation")]
        public async Task GivenIChooseActiveSegmentWithMovementStartDateAndEndDateForOfficialDeltaCalculationAsync()
        {
            var time = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentwithMovements, args: new { Year = "2020" }).ConfigureAwait(false);
            this.ScenarioContext["StartDate"] = time["StartTime"].ToString();
            this.ScenarioContext["EndDate"] = time["EndTime"].ToString();
            this.ScenarioContext["DeltaCategorySegment"] = time["Name"].ToString();
            this.ScenarioContext["SegmentId"] = time["SegmentId"].ToString();
        }

        [Given(@"I choose the active segment with nodes with deltas for a previous period and the official deltas have not been approved for all nodes")]
        public async Task GivenIChooseTheActiveSegmentWithNodesWithDeltasForAPreviousPeriodAndTheOfficialDeltasHaveNotBeenApprovedForAllNodesAsync()
        {
            var time = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentNodesWithDeltasPreviousPeriod, args: new { Year = "2020" }).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = time["Name"].ToString();
        }

        [Then(@"I verify year drop down should have last 5 years as values and defaulted to current year")]
        public void ThenIVerifyYearDropDownShouldHaveLastYearsAsValuesAndDefaultedToCurrentYear()
        {
            int year = DateTime.Today.Year;
            this.ScenarioContext["CurrentYear"] = year;
            List<int> expectedYearList = new List<int>();

            for (int i = 0; i <= 4; i++)
            {
                expectedYearList.Add(year - i);
            }

            var page = this.Get<ElementPage>();
            page.GetElement(nameof(Resources.SelectBox), formatArgs: "Año").Click();

            var actual = this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptions), formatArgs: "Año").Text;

            var actualList = actual.Replace("\n", string.Empty).Split('\r').ToList().ConvertAll(int.Parse);
            Assert.AreEqual(expectedYearList, actualList);
        }

        [Then(@"I verify default value of Año drop down is current year")]
        public void ThenIVerifyDefaultValueOfAnoDropDownIsCurrentYear()
        {
            var page = this.Get<ElementPage>();
            var expectedYear = this.ScenarioContext["CurrentYear"].ToString();

            var actualYear = this.Get<ElementPage>().GetElement(nameof(Resources.SelectBox), formatArgs: "Año").Text;
            Assert.AreEqual(expectedYear, actualYear);
        }

        [Then(@"I select segment (from "".*"" "".*"")")]
        public void ThenISelectSegmentFrom(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: this.ScenarioContext["DeltaCategorySegment"].ToString()).Click();
        }

        [Then(@"I select Todos segment (from "".*"" "".*"")")]
        public void ThenISelectTodosSegmentFrom(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            page.GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: "TOdos").Click();
        }

        [Then(@"Verify wizard has ""(.*)"" as ""(.*)""")]
        public void ThenVerifyWizardHasMessageAs(string title, string message)
        {
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ValidationMessage), formatArgs: title).Text);
        }

        [Then(@"I verify ""(.*)"" model pop up displayed with message ""(.*)""")]
        public void ThenIVerifyModelPopUpDisplayedWithMessage(string title, string message)
        {
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.ModelPopUpWIthTitle), formatArgs: title).Text);
        }

        [Then(@"I select ""(.*)"" period from drop down")]
        public void ThenISelectAPeriodFromDropDown(string month)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBox), formatArgs: "Período del procesamiento").Click();
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: "01-" + month).Click();
            this.ScenarioContext["Period"] = this.Get<ElementPage>().GetElement(nameof(Resources.SelectBox), formatArgs: "Período del procesamiento").Text;
            this.ScenarioContext["Mon"] = month;
        }

        [Then(@"I select ""(.*)"" year from drop down")]
        public void ThenISelectYearFromDropDown(string year)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBox), formatArgs: "Año").Click();
            this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), formatArgs: year).Click();
            this.ScenarioContext["Year"] = this.Get<ElementPage>().GetElement(nameof(Resources.SelectBox), formatArgs: "Año").Text;
        }

        [Then(@"I verify Chosen segment and period are displayed")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "lowercase")]
        public void ThenIVerifyChosenSegmentAndPeriodAreDisplayed()
        {
            string actualnameUI = this.Get<ElementPage>().GetElement(nameof(Resources.ValidationMessage), formatArgs: "Segmento").Text;
            string expectednameUI = this.ScenarioContext["DeltaCategorySegment"].ToString();
            string actualDateUI = this.Get<ElementPage>().GetElement(nameof(Resources.ValidationMessage), formatArgs: "Período").Text;
            string expectedDateUI = this.ScenarioContext["Period"].ToString();
            Assert.AreEqual(expectednameUI.ToLower(), this.Get<ElementPage>().GetElement(nameof(Resources.ValidationMessage), formatArgs: "Segmento").Text);
            Assert.AreEqual(expectedDateUI.Split('\r')[0], actualDateUI.Replace("-20", string.Empty));
        }

        [Then(@"Verify that wizard has ""(.*)"" error message")]
        public void ThenVerifyThatWizardHasErrorMessage(string expectedMessage)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var actualMessage = page.GetElement(nameof(Resources.DuplicatetransformationMessage)).Text;
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Then(@"I verify total number of unapproved nodes in segment should be displayed")]
        public async Task ThenIVerifyTotalNumberOfUnapprovedNodesInSegmentShouldBeDisplayedAsync()
        {
            int m = DateTime.ParseExact(this.ScenarioContext["Mon"].ToString(), "MMM", CultureInfo.CurrentCulture).Month;
            string actualCount = this.Get<ElementPage>().GetElement(nameof(Resources.ValidationMessage), formatArgs: "Total de registros:").Text;
            string name = this.ScenarioContext["DeltaCategorySegment"].ToString();
            string year = this.ScenarioContext["Year"].ToString();
            string startDate = year + '-' + m + '-' + "01";
            string endDate = year + '-' + m + '-' + "30";
            var expectedCount = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetUnapprovedNodesCount, args: new { Name = name, StartDate = startDate, EndDate = endDate }).ConfigureAwait(false);
            string expected = string.Empty;
            bool getCount = expectedCount.TryGetValue("Total", out expected);
            Assert.AreEqual(actualCount, expected);
        }

        [Then(@"I validate no paging is available for grid")]
        public void ThenIValidateNoPagingIsAvailableForGrid()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.Pagination)));
        }

        [Then(@"Verify wizard has below columns")]
        public void ThenVerifyWizardHasBelowColumns(Table table)
        {
            var dict = table?.Rows.ToDictionary(r => r[0]);

            foreach (string key in dict.Keys)
            {
                Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.WizardColumns), formatArgs: key));
            }
        }

        [Then(@"the results should be sorted based on ExecutionDate be descending in the Official Delta Grid")]
        public void ThenTheResultsShouldBeSortedBasedOnExecutionDateBeDescendingInTheOfficialDeltaGrid()
        {
            var valuesInApplication = this.Get<ElementPage>().GetElements(nameof(Resources.DeltaExecutionGrid)).ToList();
            var sortedList = valuesInApplication.OrderByDescending(x => x.Text).ToList();
            Assert.AreEqual(sortedList, valuesInApplication);
        }

        [When(@"I set the delta calculation process ""(.*)"" in the background for a segment")]
        public async Task GivenISetTheDeltaCalculationProcessRunningInTheBackgroundForASegmentAsync(string status)
        {
            int s;
            if (status == "running")
            {
                s = 1;
                await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.SetDeltaCalculationRunning, args: new { SegmentId = this.ScenarioContext["SegmentId"].ToString(), Status = s }).ConfigureAwait(false);
            }
            else if (status == "stopped")
            {
                s = 2;
                await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.SetDeltaCalculationRunning, args: new { SegmentId = this.ScenarioContext["SegmentId"].ToString(), Status = s }).ConfigureAwait(false);
            }
        }

        [StepDefinition(@"I set the Segment status to ""(.*)""")]
        public async Task GivenISetTheSegmentStatusToAsync(string status)
        {
            int s;
            if (status == "Active")
            {
                s = 1;
                await this.ReadAllSqlAsDictionaryAsync(SqlQueries.SetSegmentStatus, args: new { SegmentName = this.ScenarioContext["DeltaCategorySegment"].ToString(), Status = s }).ConfigureAwait(false);
            }
            else if (status == "Inctive")
            {
                s = 0;
                await this.ReadAllSqlAsDictionaryAsync(SqlQueries.SetSegmentStatus, args: new { SegmentName = this.ScenarioContext["DeltaCategorySegment"].ToString(), Status = s }).ConfigureAwait(false);
            }
        }

        [Then(@"I check the start date and end date of movements should be between start date and end date of the selected year")]
        public void ThenICheckTheStartDateAndEndDateOfMovementsShouldBeBetweenStartDateAndEndDateOfTheSelectedYear()
        {
            DateTime startdate = this.ScenarioContext["StartDate"].ToDateTime();
            DateTime endate = this.ScenarioContext["EndDate"].ToDateTime();
            DateTime yearStartDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime yearEndDate = new DateTime(DateTime.Now.Year, 12, 31);
            Assert.True(startdate >= yearStartDate);
            Assert.True(endate <= yearEndDate);
        }
    }
}
