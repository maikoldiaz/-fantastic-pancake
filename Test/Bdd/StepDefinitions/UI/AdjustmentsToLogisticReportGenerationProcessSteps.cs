// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustmentsToLogisticReportGenerationProcessSteps.cs" company="Microsoft">
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

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdjustmentsToLogisticReportGenerationProcessSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have a segment where ownership is already calculated")]
        public async Task GivenIHaveASegmentWhereOwnershipIsAlreadyCalculatedAsync()
        {
            try
            {
                var statusOfTopTicketSegment = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOwnershipCalculationSegmentTicket).ConfigureAwait(false);
                this.SetValue(ConstantValues.TicketId, statusOfTopTicketSegment[ConstantValues.TicketId]);
                this.SetValue(ConstantValues.Segment, statusOfTopTicketSegment[ConstantValues.OwnershipSegmentName]);
                this.SetValue(ConstantValues.StartDate, statusOfTopTicketSegment[ConstantValues.StartDate]);
                this.SetValue(ConstantValues.FinalDate, statusOfTopTicketSegment[ConstantValues.EndDate]);
            }
            catch (ArgumentNullException)
            {
                ////this.Given("I have ownershipcalculated segment");
                await this.IHaveOwnershipcalculatedSegmentAsync().ConfigureAwait(false);
            }
        }

        [Given(@"I have nodes in the segment with status ""(.*)""")]
        public async Task GivenIHaveNodesInTheSegmentWithStatusAsync(string status)
        {
            if (status == ConstantValues.NodeStatusIsNotApproved)
            {
                await this.ReadAllSqlAsync(input: SqlQueries.UpdateNodeApprovalStatus, args: new { status = 1, ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            }
            else if (status == ConstantValues.NodeStatusIsApproved)
            {
                await this.ReadAllSqlAsync(input: SqlQueries.UpdateNodeApprovalStatus, args: new { status = 9, ticketId = this.GetValue(ConstantValues.TicketId) }).ConfigureAwait(false);
            }
        }

        [When(@"I see label ""(.*)""")]
        [Then(@"""(.*)"" field should be displayed")]
        [Then(@"I should see ""(.*)"" label on the wizard")]
        [When(@"I see ""(.*)"" step on the wizard")]
        public void WhenISeeStepOnTheWizard(string step)
        {
            var page = this.Get<ElementPage>();
            page.CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: step);
        }

        [When(@"I select ownership calculated segment (from "".*"" "".*"" "".*"")")]
        public void WhenISelectOwnershipCalculatedSegmentFrom(ElementLocator elementLocator)
        {
            this.ISelectOwnershipCalculatedSegmentFrom(elementLocator);
        }

        [When(@"I have test data for second logistic report generation")]
        public void WhenIHaveTestDataForSecondLogisticReportGeneration()
        {
            this.SetValue(ConstantValues.SendToSAP, ConstantValues.True);
        }

        [When(@"I have provided all required validations on criteria step")]
        public async Task WhenIHaveProvidedAllRequiredValidationsOnCriteriaStepAsync()
        {
            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.SendToSAP)))
            {
                var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeBySegment, args: new { segment = this.GetValue(ConstantValues.Segment) }).ConfigureAwait(false);
                if (!bool.Parse(nodeDetails[ConstantValues.SendToSAP]))
                {
                    await this.ReadAllSqlAsync(input: SqlQueries.UpdateNodeSendToSAPStatus, args: new { status = 1, name = nodeDetails[ConstantValues.Name] }).ConfigureAwait(false);
                }

                this.SetValue(ConstantValues.Node, nodeDetails[ConstantValues.Name]);
                this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                ////this.When("I click on \"CreateLogistics\" \"button\"");
                this.IClickOn("CreateLogistics", "button");
            }

            ////this.When("I select ownership calculated segment from \"Logistics Criteria\" \"Segment\" \"combobox\"");
            this.ISelectOwnershipCalculatedSegmentFrom("Logistics Criteria\" \"Segment", "combobox");
            ////this.When("I select Owner on the Create file interface");
            this.ISelectOwnerOnTheCreateFileInterface();
            ////this.Then("I should be able to select nodes belong to that segment");
            await this.ShouldBeAbleToSelectNodesBelongToThatSegmentAsync().ConfigureAwait(false);
        }

        [When(@"I click on Node textbox on criteria step")]
        public void WhenIClickOnNodeTextboxOnCriteriaStep()
        {
            this.Get<ElementPage>().Click(nameof(Resources.NodeFieldOnCreateLogisticsInterface));
        }

        [When(@"I have selected start date and end date on period step")]
        public void WhenIHaveSelectedStartDateAndEndDateOnPeriodStep()
        {
            ////this.When("I select Start date and End Date on Create file Interface");
            this.ISelectStartDateAndEndDateOnCreateFileInterface();
        }

        [When(@"I have not selected segment, node and owner on the wizard")]
        [When(@"I have not selected start date and end date on the wizard")]
        public void WhenIHaveNotSelectedSegmentNodeAndOwnerOnTheWizard()
        {
            // Method intentionally left empty.
        }

        [When(@"I have selected start date greater than end date on the wizard")]
        public void WhenIHaveSelectedStartDateGreaterThanEndDateOnTheWizard()
        {
            var page = this.Get<ElementPage>();
            var arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.InitialDate.ToCamelCase() };
            page.Click(nameof(Resources.Date), formatArgs: arguments);
            ////Selecting Start date of ownership calculation so using directly Enter key
            page.GetElement(nameof(Resources.Date), formatArgs: arguments).SendKeys(OpenQA.Selenium.Keys.Enter);
            arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.FinalDate.ToCamelCase() };
            page.Click(nameof(Resources.Date), formatArgs: arguments);
            page.GetElement(nameof(Resources.Date), formatArgs: arguments).SendKeys(this.GetValue(ConstantValues.StartDate));
            page.GetElement(nameof(Resources.Date), formatArgs: arguments).SendKeys(OpenQA.Selenium.Keys.Enter);
            this.SetValue(ConstantValues.StartDate, page.GetElement(nameof(Resources.Date), formatArgs: arguments).GetAttribute(ConstantValues.Value));
            this.SetValue(ConstantValues.FinalDate, page.GetElement(nameof(Resources.Date), formatArgs: arguments).GetAttribute(ConstantValues.Value));
        }

        [When(@"I have provided other required validations on criteria step")]
        public void WhenIHaveProvidedOtherRequiredValidationsOnCriteriaStep()
        {
            ////this.When("I select Owner on the Create file interface");
            this.ISelectOwnerOnTheCreateFileInterface();
            ////this.When("I click on Node textbox on criteria step");
            this.Get<ElementPage>().Click(nameof(Resources.NodeFieldOnCreateLogisticsInterface));
            var page = this.Get<ElementPage>();
            var nodeSelection = page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface));
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.SelectItemAtIndex(nodeSelection, 0);
        }

        [Then(@"I should see ""(.*)"" ""(.*)"" ""(.*)"" on the wizard")]
        public void ThenIShouldSeeOnTheWizard(string criteria, string period, string validation)
        {
            var page = this.Get<ElementPage>();
            page.CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: criteria);
            page.CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: period);
            page.CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: validation);
        }

        [Then(@"startdate and enddate fields should not be displayed")]
        public void ThenStartdateAndEnddateFieldsShouldNotBeDisplayed()
        {
            var page = this.Get<ElementPage>();
            var arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.InitialDate.ToCamelCase() };
            Assert.IsFalse(page.CheckIfElementIsPresent(nameof(Resources.Date), formatArgs: arguments));
            arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.FinalDate.ToCamelCase() };
            Assert.IsFalse(page.CheckIfElementIsPresent(nameof(Resources.Date), formatArgs: arguments));
        }

        [Then(@"ecopetrol should be shown first in the owners")]
        public void ThenEcopetrolShouldBeShownFirstInTheOwners()
        {
            var owner = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerOnCreateLogisticsInterface), formatArgs: UIContent.GridPosition[ConstantValues.OwnerName]).Text;
            Assert.IsTrue(ConstantValues.EcoPetrol.EqualsIgnoreCase(owner));
        }

        [Then(@"it should display ""(.*)"" value for selection")]
        public void ThenItShouldDisplayValueForSelection(string todos)
        {
            var page = this.Get<ElementPage>();
            var nodeSelection = page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface));
            nodeSelection.SendKeys(todos);
            nodeSelection.SendKeys(OpenQA.Selenium.Keys.Enter);
            Assert.AreEqual(todos, page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface)).GetAttribute(ConstantValues.Value));

            // Removing selected value for next step
            nodeSelection.SendKeys(OpenQA.Selenium.Keys.Control + "a");
            nodeSelection.SendKeys(OpenQA.Selenium.Keys.Backspace);
        }

        [Then(@"I should be able to select nodes belong to that segment")]
        public async Task ThenIShouldBeAbleToSelectNodesBelongToThatSegmentAsync()
        {
            await this.ShouldBeAbleToSelectNodesBelongToThatSegmentAsync().ConfigureAwait(false);
        }

        [Then(@"sent to SAP property is enabled for those nodes")]
        public void ThenSentToSAPPropertyIsEnabledForThoseNodes()
        {
            Assert.IsTrue(bool.Parse(this.GetValue(ConstantValues.NodeWithSendToSAPTrue)));
        }

        [Then(@"startdate and enddate fields should be displayed")]
        public void ThenStartdateAndEnddateFieldsShouldBeDisplayed()
        {
            var page = this.Get<ElementPage>();
            var arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.InitialDate.ToCamelCase() };
            Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.Date), formatArgs: arguments));
            arguments = new object[] { ConstantValues.LogisticsPeriod.ToCamelCase(), ConstantValues.FinalDate.ToCamelCase() };
            Assert.IsTrue(page.CheckIfElementIsPresent(nameof(Resources.Date), formatArgs: arguments));
        }

        [When(@"I see selected segment, node, owner and period")]
        [Then(@"I should see selected segment, node, owner and period")]
        public void ThenIShouldSeeSelectedSegmentNodeOwnerAndPeriod()
        {
            var page = this.Get<ElementPage>();
            Assert.AreEqual(this.GetValue(ConstantValues.Segment), page.GetElementText(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Segment]));
            Assert.AreEqual(this.GetValue(ConstantValues.Node), page.GetElementText(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]));
            Assert.IsTrue(ConstantValues.EcoPetrol.EqualsIgnoreCase(page.GetElementText(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Owner])));
            var period = this.GetValue(ConstantValues.StartDate) + " al " + this.GetValue(ConstantValues.FinalDate);
            var actualPeriod = page.GetElements(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.LogisticsPeriod]);
            Assert.IsTrue(period.EqualsIgnoreCase(actualPeriod[0].Text + " " + actualPeriod[1].Text + " " + actualPeriod[2].Text));
        }

        [Then(@"I should see error message on confirmation popup as ""(.*)""")]
        public void ThenIShouldSeeErrorMessageOnConfirmationPopupAs(string errorMessage)
        {
            var actualMessage = this.Get<ElementPage>().GetElementText(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Error]);
            Assert.AreEqual(errorMessage, actualMessage);
        }

        [Then(@"I should see total number of failed records")]
        public void ThenIShouldSeeTotalNumberOfFailedRecords()
        {
            var page = this.Get<ElementPage>();
            page.CheckIfElementIsPresent(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.NumberOfErrorRecords]);
            this.SetValue(ConstantValues.NumberOfErrorRecords, page.GetElementText(nameof(Resources.LogisticReportConfirmationDetails), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.NumberOfErrorRecords]));
            var actualNumberOfErrors = page.GetElements(nameof(Resources.LogisticsValifationGrid));
            Assert.AreEqual(int.Parse(this.GetValue(ConstantValues.NumberOfErrorRecords), CultureInfo.InvariantCulture), actualNumberOfErrors.Count);
        }

        [Then(@"one row for each day that is not approved, sorted by node and date descendent")]
        public void ThenOneRowForEachDayThatIsNotApprovedSortedByNodeAndDateDescendent()
        {
            var page = this.Get<ElementPage>();

            // Verification of Date for Top record
            var arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.Segment] };
            var dateInDescendingOrder = page.GetElementText(nameof(Resources.FailedRecordsOnCreateInterface), formatArgs: arguments);
            Assert.IsTrue(this.GetValue(ConstantValues.FinalDate).EqualsIgnoreCase(dateInDescendingOrder));

            // Verification of Status for Top record
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.Node] };
            var status = page.GetElementText(nameof(Resources.FailedRecordsOnCreateInterface), formatArgs: arguments);
            Assert.IsTrue(ConstantValues.Enviado.EqualsIgnoreCase(status));

            // Verification of Date for Last record
            arguments = new object[] { this.GetValue(ConstantValues.NumberOfErrorRecords), UIContent.ReportLogisticGrid[ConstantValues.Segment] };
            dateInDescendingOrder = page.GetElementText(nameof(Resources.FailedRecordsOnCreateInterface), formatArgs: arguments);
            Assert.IsTrue(this.GetValue(ConstantValues.StartDate).EqualsIgnoreCase(dateInDescendingOrder));

            // Verification of Status for Last record
            arguments = new object[] { this.GetValue(ConstantValues.NumberOfErrorRecords), UIContent.ReportLogisticGrid[ConstantValues.Node] };
            status = page.GetElementText(nameof(Resources.FailedRecordsOnCreateInterface), formatArgs: arguments);
            Assert.IsTrue(ConstantValues.Enviado.EqualsIgnoreCase(status));
        }

        [Then(@"I should see name of button as ""(.*)""")]
        public void ThenIShouldSeeNameOfButtonAs(string buttonName)
        {
            if (buttonName == ConstantValues.Aceptar)
            {
                var actualButton = this.Get<ElementPage>().GetElementText(nameof(Resources.ButtonsOnCreateLogisticInterface), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Segment]);
                Assert.IsTrue(buttonName.EqualsIgnoreCase(actualButton));
            }
            else if (buttonName == ConstantValues.CreateLogisticReport)
            {
                var actualButton = this.Get<ElementPage>().GetElementText(nameof(Resources.ButtonsOnCreateLogisticInterface), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]);
                Assert.IsTrue(buttonName.EqualsIgnoreCase(actualButton));
            }
        }

        [Then(@"I should see Logistic Report for selected segment, node and period in the Logistic Report Generation grid")]
        public void ThenIShouldSeeLogisticReportForSelectedSegmentNodeAndPeriodInTheLogisticReportGenerationGrid()
        {
            var page = this.Get<ElementPage>();

            // Segment Verification
            var arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.Segment] };
            Assert.AreEqual(this.GetValue(ConstantValues.Segment), page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments));

            // Node Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.Node] };
            Assert.AreEqual(this.GetValue(ConstantValues.Node), page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments));

            // Owner Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.Owner] };
            Assert.IsTrue(ConstantValues.EcoPetrol.EqualsIgnoreCase(page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments)));

            // Start Date Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.StartDate] };
            Assert.IsTrue(this.GetValue(ConstantValues.StartDate).EqualsIgnoreCase(page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments)));

            // End Date Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Segment], UIContent.ReportLogisticGrid[ConstantValues.EndDate] };
            Assert.IsTrue(this.GetValue(ConstantValues.FinalDate).EqualsIgnoreCase(page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments)));
        }

        [Then(@"I should see total (.*) Logistic Reports for selected segment, node and period in the Logistic Report Generation grid")]
        public void ThenIShouldSeeTotalLogisticReportsForSelectedSegmentNodeAndPeriodInTheInTheGrid(int numberOfReports)
        {
            var actualNumberOfReports = 0;

            // I should see Logistic Report for selected segment, node and period in the Logistic Report Generation grid
            this.ThenIShouldSeeLogisticReportForSelectedSegmentNodeAndPeriodInTheLogisticReportGenerationGrid();
            actualNumberOfReports++;

            var page = this.Get<ElementPage>();

            // Segment Verification
            var arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Node], UIContent.ReportLogisticGrid[ConstantValues.Segment] };
            Assert.AreEqual(this.GetValue(ConstantValues.Segment), page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments));

            // Node Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Node], UIContent.ReportLogisticGrid[ConstantValues.Node] };
            Assert.AreEqual(this.GetValue(ConstantValues.Node), page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments));

            // Owner Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Node], UIContent.ReportLogisticGrid[ConstantValues.Owner] };
            Assert.IsTrue(ConstantValues.EcoPetrol.EqualsIgnoreCase(page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments)));

            // Start Date Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Node], UIContent.ReportLogisticGrid[ConstantValues.StartDate] };
            Assert.IsTrue(this.GetValue(ConstantValues.StartDate).EqualsIgnoreCase(page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments)));

            // End Date Verification
            arguments = new object[] { UIContent.ReportLogisticGrid[ConstantValues.Node], UIContent.ReportLogisticGrid[ConstantValues.EndDate] };
            Assert.IsTrue(this.GetValue(ConstantValues.FinalDate).EqualsIgnoreCase(page.GetElementText(nameof(Resources.DataOnLogisticsGenerationGrid), formatArgs: arguments)));
            actualNumberOfReports++;

            // Veriication for total number of Reports
            Assert.AreEqual(numberOfReports, actualNumberOfReports);
        }

        [When(@"I have selected range between start date and end date is more than (.*) days")]
        public async Task WhenIHaveSelectedRangeBetweenStartDateAndEndDateIsMoreThanDaysAsync(int days)
        {
            // Getting Test data that Range between start date and end date is more than 60 days
            var ownershipCalculatedSegemntDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.RangeIsMoreThan60DaysForOwnershipCalculatedSegment).ConfigureAwait(false);
            this.SetValue(ConstantValues.Segment, ownershipCalculatedSegemntDetails[ConstantValues.OwnershipSegmentName]);
            this.SetValue(ConstantValues.StartDate, ownershipCalculatedSegemntDetails[ConstantValues.StartDate]);
            this.SetValue(ConstantValues.FinalDate, ownershipCalculatedSegemntDetails[ConstantValues.EndDate]);
            ////this.When("I select Start date and End Date on Create file Interface");
            this.ISelectStartDateAndEndDateOnCreateFileInterface();
            Assert.IsNotNull(days);
        }
    }
}