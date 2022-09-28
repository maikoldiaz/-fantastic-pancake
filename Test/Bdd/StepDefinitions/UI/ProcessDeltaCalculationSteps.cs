// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessDeltaCalculationSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ProcessDeltaCalculationSteps : EcpWebStepDefinitionBase
    {
        [When(@"I provided value for ""(.*)"" in the grid")]
        public async Task WhenIProvidedValueForInTheGridAsync(string fieldName)
        {
            var gridResult = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetTopTicketBySegmentForDeltaGrid).ConfigureAwait(false);
            var page = this.Get<ElementPage>();

            switch (fieldName)
            {
                case ConstantValues.Segment:
                    page.GetElement(nameof(Resources.DeltaColumnHeader), formatArgs: "segment").SendKeys(gridResult["Name"] + OpenQA.Selenium.Keys.Enter);
                    var actualSegmentResult = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaSegment"]).Text;
                    Assert.AreEqual(gridResult["Name"], actualSegmentResult);
                    break;
                case ConstantValues.StartDate:
                    page.GetElement(nameof(Resources.DeltaGridDt), formatArgs: "ticketStartDate").SendKeys(gridResult["StartDate"] + OpenQA.Selenium.Keys.Enter);
                    var actualStartDateResult = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaStartDate"]).Text;
                    Assert.AreEqual(gridResult["StartDate"], actualStartDateResult);
                    break;
                case ConstantValues.EndDate:
                    page.GetElement(nameof(Resources.DeltaGridDt), formatArgs: "ticketFinalDate").SendKeys(gridResult["EndDate"] + OpenQA.Selenium.Keys.Enter);
                    var actualEndDate = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaEndDate"]).Text;
                    Assert.AreEqual(gridResult["EndDate"], actualEndDate);
                    break;
                case ConstantValues.ExecutionDate:
                    page.GetElement(nameof(Resources.DeltaGridDt), "cutoffExecutionDate").SendKeys(gridResult["CreatedDate"] + OpenQA.Selenium.Keys.Enter);
                    var actualExecutionDate = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaExecutionDate"]).Text;
                    Assert.IsTrue(actualExecutionDate.ContainsIgnoreCase(gridResult["CreatedDate"]));
                    break;
                case ConstantValues.Status:
                    page.GetElement(nameof(Resources.DeltaGridDropdown), "state").SendKeys(gridResult["Status"] + OpenQA.Selenium.Keys.Enter);
                    var actualStatus = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaStatus"]).Text;
                    Assert.AreEqual(gridResult["Status"], actualStatus);
                    break;
                case ConstantValues.Ticket:
                    page.GetElement(nameof(Resources.DeltaColumnHeader), formatArgs: "ticketId").SendKeys(gridResult["TicketID"] + OpenQA.Selenium.Keys.Enter);
                    var actualTicket = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaTicket"]).Text;
                    Assert.AreEqual(gridResult["TicketID"], actualTicket);
                    break;
                case ConstantValues.User:
                    page.GetElement(nameof(Resources.DeltaColumnHeader), formatArgs: "createdBy").SendKeys(gridResult["CreatedBy"] + OpenQA.Selenium.Keys.Enter);
                    var actualUser = page.GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaUser"]).Text;
                    Assert.AreEqual(gridResult["CreatedBy"], actualUser);
                    break;
                default:
                    break;
            }
        }

        [Then(@"I should see the records filtered as per the searched criteria")]
        public void ThenIShouldSeeTheRecordsFilteredAsPerTheSearchedCriteria()
        {
            ////Method Left Intentionally blank and validation convers in previous steps
        }

        [Then(@"the results should be sorted based on ExecutionDate be descending in the Grid")]
        public async Task ThenTheResultsShouldBeSortedBasedOnExecutionDateBeDescendingInTheGridAsync()
        {
            var allDeltaTickets = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllDeltaTickets).ConfigureAwait(false);
            var allDeltaTicketsList = allDeltaTickets.ToDictionaryList();
            var valuesInApplication = this.Get<ElementPage>().GetElements(nameof(Resources.DeltaExecutionGrid));
            for (int i = 0; i < valuesInApplication.Count; i++)
            {
                var actualResult = valuesInApplication.ElementAt(i).Text;
                Assert.IsTrue(actualResult.Contains(allDeltaTicketsList[i]["ExecutionDate"].ToString()));
            }
        }

        [When(@"I don't have any Delta Calculated Tickets in the system")]
        public void WhenIDonTHaveAnyDeltaCalculatedTicketsInTheSystem()
        {
            this.Get<ElementPage>().GetElement(nameof(Resources.DeltaColumnHeader), formatArgs: "segment").SendKeys(new Faker().Random.AlphaNumeric(5) + OpenQA.Selenium.Keys.Enter);
        }

        [Given(@"I configure the N days from the Configuration in storage explorer")]
        [Given(@"I have Segments information for Delta Calculation from the last Fourty days")]
        public void GivenIHaveSegmentsInformationForDeltaCalculationFromTheLastFourtyDays()
        {
            ////Method Left Intentionally blank and validation convers in previous steps
        }

        [Then(@"the records from deltas executed in the last N days should be displayed in the Grid")]
        public async Task ThenTheRecordsFromDeltasExecutedInTheLastNDaysShouldBeDisplayedInTheGridAsync()
        {
            var allDeltaTickets = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllDeltaTickets).ConfigureAwait(false);
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var count = paginationCount.Split(' ');
            Assert.AreEqual(allDeltaTickets.Count().ToString(CultureInfo.InvariantCulture), count[4]);
        }

        [Then(@"I should see ""(.*)"" subtitle")]
        public void ThenIShouldSeeSubtitle(string expectedHeader)
        {
            var actualHeader = this.Get<ElementPage>().GetElement(nameof(Resources.DeltaInitialHeader)).Text;
            Assert.AreEqual(expectedHeader, actualHeader);
        }

        [Then(@"I should see ""(.*)"" on initial wizard")]
        public void ThenIShouldSeeOnInitialWizard(string expectedMessage)
        {
            var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.DeltaInitialMessage)).Text;
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Then(@"Active Segments which is ""(.*)"" configured as SON shouldn't be displayed in the dropdown")]
        [Then(@"Active Segments ""(.*)"" as SON should be displayed in the dropdown")]
        public async Task ThenActiveSegmentsAsSONShouldBeDisplayedInTheDropdownAsync(string state)
        {
            if (state.EqualsIgnoreCase("configured"))
            {
                var activeSONSegments = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSONSegments).ConfigureAwait(false);
                var activeSONSegmentsList = activeSONSegments.ToDictionaryList();
                ////this.IClickOn("initTicket\" \"segment", "dropdown");
                var activeSONSegmentsinUI = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion["Segment"]);
                await Task.Delay(3000).ConfigureAwait(false);
                Assert.AreEqual(activeSONSegmentsinUI.Count, activeSONSegmentsList.Count);
            }
            else if (state.EqualsIgnoreCase("not configured"))
            {
                var activeNonSONSegments = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNonSONSegments).ConfigureAwait(false);
                var activeNonSONSegmentsList = activeNonSONSegments.ToDictionaryList();
                ////this.IClickOn("initTicket\" \"segment", "dropdown");
                var sonSegmentsinUI = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion["Segment"]);
                await Task.Delay(3000).ConfigureAwait(false);
                Assert.IsFalse(sonSegmentsinUI.Count.Equals(activeNonSONSegmentsList.Count));
            }
        }

        [Given(@"If the current day is greater than the value set in the parameter \(ValidDaysCurrentMonth\) in Configuration setting")]
        [Given(@"If the current day is less than or equal to the value set in the parameter \(ValidDaysCurrentMonth\) in Configuration setting")]
        public void GivenIfTheCurrentDayIsLessThanOrEqualToTheValueSetInTheParameterValidDaysCurrentMonthInConfigurationSetting()
        {
            if (DateTime.UtcNow.Day - 4 > 0)
            {
                this.SetValue("CurrentMonthDate", "1");
            }
            else
            {
                this.SetValue("PreviousMonthDate", "1");
            }
        }

        [Then(@"Verify that value (from "".*"" "".*"") should be the first day of the previous month")]
        public void ThenVerifyThatValueInShouldBeTheFirstDayOfThePreviousMonth(ElementLocator elementLocator)
        {
            var dateinInitialField = this.Get<ElementPage>().GetElement(elementLocator).Text;
            if (this.GetValue("PreviousMonthDate") == "1")
            {
                var expectedDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).AddMonths(-1).GetDateTimeFormats(CultureInfo.InvariantCulture)[67];
                Assert.IsTrue(expectedDate.ContainsIgnoreCase(dateinInitialField));
            }
        }

        [Then(@"Verify that value (from "".*"" "".*"") the start date must be the first day of the current month")]
        public void ThenVerifyThatValueFromTheStartDateMustBeTheFirstDayOfTheCurrentMonth(ElementLocator elementLocator)
        {
            var dateinInitialField = this.Get<ElementPage>().GetElement(elementLocator).Text;
            if (this.GetValue("CurrentMonthDate") == "1")
            {
                var expectedDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).GetDateTimeFormats(CultureInfo.InvariantCulture)[67];
                Assert.IsTrue(expectedDate.ContainsIgnoreCase(dateinInitialField));
            }
        }

        [Then(@"I should see ""(.*)"" page based on user ""(.*)""")]
        public void ThenIShouldSeePageBasedOnUser(string pageName, string user)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);

            switch (user)
            {
                case "admin":
                case "profesional":
                    page.Click(nameof(Resources.NavigationBar));

                    page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                    page.WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Navigation[pageName]);
                    page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                    page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[pageName]);

                    Assert.AreEqual(true, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]));
                    break;
            }
        }

        [Then(@"I should not see ""(.*)"" page based on user ""(.*)""")]
        public void ThenIShouldNotSeePageBasedOnUser(string pageName, string user)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);

            switch (user)
            {
                case "consulta":
                    page.Click(nameof(Resources.NavigationBar));
                    Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]));
                    break;
                case "aprobador":
                case "auditor":
                    page.Click(nameof(Resources.NavigationBar));
                    page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[pageName]);
                    Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[pageName]));
                    break;
                case "programador":
                    Assert.AreEqual(false, page.CheckIfElementIsPresent(nameof(Resources.NavigationBar)));
                    break;
            }
        }

        [StepDefinition(@"I select delta segment (from "".*"" "".*"")")]
        public void WhenISelectDeltaSegmentFrom(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            page.Click(elementLocator);
            var option = page.GetElement(nameof(Resources.SelectBoxOption), formatArgs: this.ScenarioContext["DeltaCategorySegment"]);
            Actions action = new Actions(this.DriverContext.Driver);
            action.MoveToElement(option).Perform();
            option.Click();
        }

        [Given(@"I have Segment for which ownership is not generated")]
        public async Task GivenIHaveSegmentForWhichOwnershipIsNotGeneratedAsync()
        {
            var segmentRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentWithoutOwnerhsip).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = segmentRow["Name"].ToString();
        }

        [Then(@"Verify that ""(.*)"" error message should be displayed\.")]
        public async Task ThenVerifyThatErrorMessageShouldBeDisplayedAsync(string expectedMessage)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var actualMessage = page.GetElement(nameof(Resources.DuplicatetransformationMessage)).Text;
            Assert.AreEqual(expectedMessage, actualMessage);
            if (this.GetValue("UpdateProcessingTicket") == "True")
            {
                await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketForSuccessCutoff, args: new { Ticket = this.ScenarioContext["TicketId"].ToInt() }).ConfigureAwait(false);
            }
        }

        [StepDefinition(@"I have Segment for which Cutoff Process is running in system")]
        public async Task GivenIHaveSegmentForWhichCutoffProcessIsRunningInSystemAsync()
        {
            var ticketRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentWithRunningCutoff).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketForProcessingCutoff, args: new { Ticket = ticketRow["TicketId"].ToInt() }).ConfigureAwait(false);
            this.ScenarioContext["TicketId"] = ticketRow["TicketId"];
            this.SetValue("UpdateProcessingTicket", "True");
            var segmentDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentDetails, args: new { Element = ticketRow["CategoryElementId"].ToInt() }).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = segmentDetails["Name"].ToString();
        }

        [Given(@"I have valid segment for Delta Calculation Process")]
        [Given(@"I have Segment for which there is neither pending movements or pending inventories present in process of Calculation of Delta")]
        [Given(@"I have Segment for which there is no pending movements present in process of Calculation of Delta")]
        [Given(@"I have Segment for which there is no pending inventories present in process of Calculation of Delta")]
        public async Task GivenIHaveSegmentForWhichThereIsNoPendingInventoriesPresentInProcessOfCalculationOfDeltaAsync()
        {
            var ticketDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentWithoutDeltaCalculation).ConfigureAwait(false);
            this.ScenarioContext["ElementId"] = ticketDetails["CategoryElementId"].ToInt();
            var segmentDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentDetails, args: new { Element = ticketDetails["CategoryElementId"].ToInt() }).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = segmentDetails["Name"].ToString();
        }

        [Then(@"Verify that value (from "".*"" "".*"") should be the last date of ownership calculation of the chosen segment")]
        public async Task ThenVerifyThatValueInShouldBeTheLastDateOfOwnershipCalculationOfTheChosenSegmentAsync(ElementLocator elementLocator)
        {
            var segmentDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastDateForDeltaTicket, args: new { Element = this.ScenarioContext["ElementId"].ToInt() }).ConfigureAwait(false);
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);
            var actualDate = page.GetElement(elementLocator).GetAttribute("Value");
            Assert.IsTrue(actualDate.ContainsIgnoreCase(segmentDetails["Date"]));
        }

        [Given(@"I have delta calculation process running for the segment")]
        public async Task GivenIHaveDeltasCalculationProcessRunningForTheSegmentAsync()
        {
            var ticketRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentWithRunningDeltaTicket).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketForProcessingCutoff, args: new { Ticket = ticketRow["TicketId"].ToInt() }).ConfigureAwait(false);
            this.ScenarioContext["TicketId"] = ticketRow["TicketId"];
            this.SetValue("UpdateProcessingTicket", "True");
            var segmentDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentDetails, args: new { Element = ticketRow["CategoryElementId"].ToInt() }).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = segmentDetails["Name"].ToString();
        }

        [Given(@"I uploaded the movements and inventories for the same segment in system")]
        public async Task GivenIUploadedTheMovementsAndInventoriesForTheSameSegmentInSystemAsync()
        {
            var ticketRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentDeltaForValidMovements).ConfigureAwait(false);
            this.ScenarioContext["TicketId"] = ticketRow["TicketId"];
            var segmentDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSegmentDetails, args: new { Element = ticketRow["CategoryElementId"].ToInt() }).ConfigureAwait(false);
            this.ScenarioContext["DeltaCategorySegment"] = segmentDetails["Name"].ToString();
        }

        [Then(@"verify that user must return to the calculation of deltas by operational adjust page without performing any action")]
        public void ThenVerifyThatUserMustReturnToTheCalculationOfDeltasByOperationalAdjustPageWithoutPerformingAnyAction()
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);
            Assert.IsTrue(page.GetElement(nameof(Resources.DeltaWizard)).Displayed);
        }

        [Then(@"I should see the ""(.*)"" on pending movement wizard")]
        [Then(@"I should see the ""(.*)"" on pending inventory wizard")]
        public void ThenIShouldSeeTheOnPendingInventoryWizard(string field, Table table)
        {
            var i = 0;
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                var actualValue = this.Get<ElementPage>().GetElements(nameof(Resources.ColumnHeaders)).ElementAt(i).Text;
                Assert.IsTrue(row.Default.EqualsIgnoreCase(actualValue));
                i++;
            }
        }

        [Then(@"verify that user must return to the Calculation of deltas by operational adjust page without performing any action")]
        public void ThenVerifyUserMustReturnCalculationOfDeltasWithoutPerformingAnyAction()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DeltaWizard)).Displayed);
        }

        [Then(@"the last record of each inventory should be displayed")]
        [Then(@"All the records should not have an ownership calculation ticket")]
        public async Task ThenAllTheRecordsShouldNotHaveAnOwnershipCalculationTicketAsync()
        {
            var inventoryIds = this.Get<ElementPage>().GetElements(nameof(Resources.UploadId));
            foreach (var inventoryId in inventoryIds)
            {
                var inventoryDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryDetailsWithId, args: new { InventoryId = inventoryId.Text }).ConfigureAwait(false);
                Assert.IsNull(inventoryDetails["OwnershipTicketId"]);
            }
        }

        [Then(@"I verify that list must be sorted by Inventory date, Inventory Id for the oldest to the recent")]
        [Then(@"the nodes should belongs to the selected segment")]
        public void ThenTheNodesShouldBelongsToTheSelectedSegment()
        {
            //// Step left intentionally blanks
        }

        [Then(@"the operating date is between the selected period including the start date or the end date")]
        public void ThenTheOperatingDateIsBetweenTheSelectedPeriodIncludingTheStartDateOrTheEndDate()
        {
            var ticketStatus = this.Get<ElementPage>().GetElements(nameof(Resources.DatesinGrid));
            foreach (var ticketdetails in ticketStatus)
            {
                Assert.IsTrue(ticketdetails.Text.ToDateTime().DayOfYear > DateTime.Today.AddDays(-25).GetDateTimeFormats(CultureInfo.InvariantCulture)[6].ToDateTime().DayOfYear);
                Assert.IsTrue(ticketdetails.Text.ToDateTime().DayOfYear < DateTime.Today.GetDateTimeFormats(CultureInfo.InvariantCulture)[6].ToDateTime().DayOfYear);
            }
        }

        [Then(@"I verify filter the content of the list based on ""(.*)""")]
        public void ThenIVerifyFilterTheContentOfTheListBasedOn(string field, Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                var page = this.Get<ElementPage>();

                switch (row.Default)
                {
                    case "Inventory Id":
                        var gridText = page.GetElement(nameof(Resources.PendingGridFilters), formatArgs: 1).Text;
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "inventoryId").SendKeys(gridText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "inventoryId").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "inventoryId").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Node":
                        var nodeText = page.GetElement(nameof(Resources.PendingGridFilters), formatArgs: 2).Text;
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "node").SendKeys(nodeText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "node").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "node").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Product":
                        var productText = page.GetElement(nameof(Resources.PendingGridFilters), formatArgs: 3).Text;
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "product").SendKeys(productText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "product").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "product").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Amount":
                        var amountText = page.GetElement(nameof(Resources.PendingGridFilters), formatArgs: 4).Text;
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "amount").SendKeys(amountText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "amount").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "amount").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Unit":
                        var unitText = page.GetElement(nameof(Resources.PendingGridFilters), formatArgs: 5).Text;
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "unit").SendKeys(unitText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "unit").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "unit").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Action":
                        var actionText = page.GetElement(nameof(Resources.PendingGridFilters), formatArgs: 7).Text;
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "action").SendKeys(actionText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "action").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingInventories), formatArgs: "action").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    default:
                        break;
                }
            }
        }

        [Then(@"All the records should not have an globalID")]
        public async Task ThenAllTheRecordsShouldNotHaveAnGlobalIDAsync()
        {
            var activeDeltaPendingMovements = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetSegmentDeltaPendingMovementDetails, args: new { Element = this.ScenarioContext["DeltaCategorySegment"] }).ConfigureAwait(false);
            var activeDeltaPendingMovementsList = activeDeltaPendingMovements.ToDictionaryList();
            Assert.IsTrue(activeDeltaPendingMovementsList.Count == 0);
        }

        [Then(@"I verify that list must be sorted by Operational date, Movement Id for the oldest to the recent")]
        [Then(@"the last record of each movement should be displayed")]
        public void ThenTheLastRecordOfEachMovementShouldBeDisplayed()
        {
            ////Method Left Blank
        }

        [Then(@"All the movements records should not have an ownership calculation ticket")]
        public async Task ThenAllTheMovementsRecordsShouldNotHaveAnOwnershipCalculationTicketAsync()
        {
            var movementIds = this.Get<ElementPage>().GetElements(nameof(Resources.UploadId));
            foreach (var movementid in movementIds)
            {
                var movementDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDetailsByMovementId, args: new { MovementId = movementid.Text }).ConfigureAwait(false);
                Assert.IsNull(movementDetails["OwnershipTicketId"]);
            }
        }

        [Then(@"the Movement operating date is between the selected period including the start date or the end date")]
        public void ThenTheMovementOperatingDateIsBetweenTheSelectedPeriodIncludingTheStartDateOrTheEndDate()
        {
            var ticketStatus = this.Get<ElementPage>().GetElements(nameof(Resources.MovementsDatesinGrid));
            foreach (var ticketdetails in ticketStatus)
            {
                Assert.IsTrue(ticketdetails.Text.ToDateTime().DayOfYear > DateTime.Today.AddDays(-25).GetDateTimeFormats(CultureInfo.InvariantCulture)[6].ToDateTime().DayOfYear);
                Assert.IsTrue(ticketdetails.Text.ToDateTime().DayOfYear < DateTime.Today.GetDateTimeFormats(CultureInfo.InvariantCulture)[6].ToDateTime().DayOfYear);
            }
        }

        [Then(@"I verify filter the content of the Pending Movements list based on ""(.*)""")]
        public void ThenIVerifyFilterTheContentOfThePendingMovementsListBasedOn(string field, Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                var page = this.Get<ElementPage>();

                switch (row.Default)
                {
                    case "Movement Id":
                        var movementText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 1).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "movementId").SendKeys(movementText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "movementId").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "movementId").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Movement type":
                        var typeText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 2).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "movementType").SendKeys(typeText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "movementType").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "movementType").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Source Node":
                        var srcNodeText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 3).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "sourceNode").SendKeys(srcNodeText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "sourceNode").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "sourceNode").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Destination Node":
                        var destNodeText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 4).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "destinationNode").SendKeys(destNodeText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "destinationNode").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "destinationNode").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Source Product":
                        var srcProdText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 5).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "sourceProduct").SendKeys(srcProdText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "sourceProduct").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "sourceProduct").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Destination Product":
                        var destProdText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 6).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "destinationProduct").SendKeys(destProdText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "destinationProduct").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "destinationProduct").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Amount":
                        var amountText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 7).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "amount").SendKeys(amountText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "amount").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "amount").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    case "Unit":
                        var unitText = page.GetElement(nameof(Resources.PendingMovementsGrid), formatArgs: 8).Text;
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "unit").SendKeys(unitText + OpenQA.Selenium.Keys.Enter);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "unit").SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Backspace);
                        page.GetElement(nameof(Resources.PendingMovementsTextBox), formatArgs: "unit").SendKeys(OpenQA.Selenium.Keys.Enter);
                        break;
                    default:
                        break;
                }
            }
        }

        [StepDefinition(@"I create the Annulation Movement for these updated movements")]
        public async Task WhenICreateTheAnnulationMovementForTheseUpdatedMovementsAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = this.GetValue("MovementTypeId"), AnnulationMovementTypeId = this.GetValue("AnnulationMovementTypeId"), isActive = 1 }).ConfigureAwait(false);
        }

        [Then(@"Verify that Delta Calculation should be successful")]
        public async Task ThenVerifyThatDeltaCalculationShouldBeSuccessfulAsync()
        {
            await this.VerifyThatDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }
    }
}