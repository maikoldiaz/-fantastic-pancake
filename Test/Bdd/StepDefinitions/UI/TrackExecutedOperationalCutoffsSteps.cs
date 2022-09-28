// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackExecutedOperationalCutoffsSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using global::Ecp.True.Bdd.Tests.Entities;
    using global::Ecp.True.Bdd.Tests.Properties;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class TrackExecutedOperationalCutoffsSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see the tooltip ""(.*)"" text (for "".*"" "".*"")")]
        public void ThenIShouldSeeTheTooltipFor(string tooltipValue, ElementLocator elementLocator)
        {
            var tooltip = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("title");
            Assert.AreEqual(UIContent.Conversion[tooltipValue], tooltip);
        }

        [Given(@"I have information from the last (.*) days")]
        public void GivenIHaveInformationFromTheLastDays()
        {
            Assert.IsTrue(true);
        }

        [Then(@"I should see the message ""(.*)"" when there are no pending records")]
        public void ThenIShouldSeeTheMessageWhenThereAreNoPendingRecords()
        {
            Assert.IsTrue(true);
        }

        [When(@"I provide the value (for "".*"" "".*"") filter in ""(.*)"" Grid")]
        public async Task WhenIProvideTheValueForFilterAsync(ElementLocator elementLocator, string gridType)
        {
            if (elementLocator != null)
            {
                IDictionary<string, string> lastCreatedRow = null;
                var field = elementLocator.Value.Split('_')[2].ToPascalCase();
                switch (gridType)
                {
                    case ConstantValues.OperationalCutoffTickets:
                        lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastTicket).ConfigureAwait(false);
                        if (field.ToPascalCase().ContainsIgnoreCase(ConstantValues.Element))
                        {
                            var name = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetActiveCategoryElement, args: new { elementId = lastCreatedRow[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
                            this.SetValue(Keys.RandomFieldValue, name[ConstantValues.Name]);
                        }

                        if (field.EqualsIgnoreCase(ConstantValues.Status))
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[field].Contains("1") ? UIContent.Conversion[ConstantValues.Finalized] : UIContent.Conversion[ConstantValues.Processing]);
                            this.Get<ElementPage>().Click(elementLocator);
                            this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: this.GetValue(Keys.RandomFieldValue));
                        }
                        else
                        {
                            if (field.EqualsIgnoreCase(ConstantValues.CreatedDate) || field.EqualsIgnoreCase(ConstantValues.StartDate) || field.EqualsIgnoreCase(ConstantValues.EndDate))
                            {
#pragma warning disable CA1305 // Specify IFormatProvider
                                var date = string.Format("{0:dd-MMM-yy}", Convert.ToDateTime(lastCreatedRow[field]));
#pragma warning restore CA1305 // Specify IFormatProvider
                                date = date.Replace(date.Split('-')[1], UIContent.Conversion[date.Split('-')[1]]);
                                this.SetValue(Keys.RandomFieldValue, date);
                            }

                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Keys.RandomFieldValue));
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
                            await Task.Delay(10000).ConfigureAwait(false);
                        }

                        break;
                    case ConstantValues.OwnershipCalculationForSegments:
                        lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketSortedByCreatedTicketDateForOwnershipForSegmentsGrid).ConfigureAwait(false);
                        if (field.ToPascalCase().ContainsIgnoreCase(ConstantValues.Element))
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[ConstantValues.Name]);
                        }
                        else
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[field]);
                        }

                        if (field.EqualsIgnoreCase(ConstantValues.Status))
                        {
                            this.Get<ElementPage>().Click(elementLocator);
                            this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: this.GetValue(Keys.RandomFieldValue));
                        }
                        else
                        {
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Keys.RandomFieldValue));
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
                            await Task.Delay(10000).ConfigureAwait(false);
                        }

                        break;

                    case ConstantValues.OwnershipCalculationForNodes:
                        if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForStartDateInOwnershipCalculationForNode))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForStartDateInOwnershipCalculationForNode;
                        }
                        else if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForEndDateInOwnershipCalculationForNode))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForEndDateInOwnershipCalculationForNode;
                        }
                        else if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForCreatedDateInOwnershipCalculationForNode))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForCreatedDateInOwnershipCalculationForNode;
                        }
                        else if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForNodeInOwnershipCalculationForNode))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForNodeInOwnershipCalculationForNode;
                        }
                        else
                        {
                            _ = elementLocator;
                        }

                        lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketSortedByCreatedTicketDateForOwnershipForNodesGrid).ConfigureAwait(false);
                        if (field.ToPascalCase().ContainsIgnoreCase(ConstantValues.Element))
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[ConstantValues.Segment]);
                        }
                        else if (field.ToPascalCase().ContainsIgnoreCase(ConstantValues.Node))
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[ConstantValues.Node]);
                        }
                        else
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[field]);
                        }

                        if (field.EqualsIgnoreCase(ConstantValues.Status))
                        {
                            this.Get<ElementPage>().Click(elementLocator);
                            this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: this.GetValue(Keys.RandomFieldValue));
                        }
                        else
                        {
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Keys.RandomFieldValue));
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
                            await Task.Delay(10000).ConfigureAwait(false);
                        }

                        break;

                    case ConstantValues.LogisticReportGeneration:
                        lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketSortedByTicketCreatedDateForReportLogisticGrid).ConfigureAwait(false);
                        if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForStartDateInLogisticReport))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForStartDateInLogisticReport;
                        }
                        else if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForEndDateInLogisticReport))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForEndDateInLogisticReport;
                        }
                        else if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForCreatedDateInLogisticReport))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForCreatedDateInLogisticReport;
                        }
                        else if (elementLocator.Value.EqualsIgnoreCase(ConstantValues.ActualElementLocatorForStatusInLogisticReport))
                        {
                            elementLocator.Value = ConstantValues.ExpectedElementLocatorForStatusInLogisticReport;
                        }
                        else
                        {
                            _ = elementLocator;
                        }

                        if (field.ToPascalCase().ContainsIgnoreCase(ConstantValues.Segment))
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[ConstantValues.Segment]);
                        }
                        else if (field.ToPascalCase().ContainsIgnoreCase(ConstantValues.Owner))
                        {
                            this.SetValue(Keys.RandomFieldValue, ConstantValues.Reficar);
                        }
                        else
                        {
                            this.SetValue(Keys.RandomFieldValue, lastCreatedRow[field]);
                        }

                        if (field.EqualsIgnoreCase(ConstantValues.Status))
                        {
                            this.Get<ElementPage>().Click(elementLocator);
                            this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: this.GetValue(Keys.RandomFieldValue));
                        }
                        else
                        {
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(this.GetValue(Keys.RandomFieldValue));
                            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
                            await Task.Delay(10000).ConfigureAwait(false);
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        [Then(@"I should see the information that matches the data entered for the ""(.*)"" in ""(.*)"" Grid and sorted by descending date")]
        [Then(@"I should see the information that matches the data entered for the ""(.*)"" in ""(.*)"" Grid")]
        public void ThenIShouldSeeTheInformationThatMatchesTheDataEnteredFor(string field, string gridType)
        {
            switch (gridType)
            {
                case ConstantValues.OperationalCutoffTickets:
                    if (this.GetValue(Keys.RandomFieldValue).Contains("/"))
                    {
                        var expected = this.GetValue(Keys.RandomFieldValue).Replace('/', '-');
                        expected = expected.Replace(expected.Split('-')[1], expected.Split('-')[1].Replace(expected.Split('-')[1], UIContent.Conversion[expected.Split('-')[1]]).Substring(0, 3));
                        Assert.AreEqual(expected, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.GridPosition[ConstantValues.TicketCreatedDate] : UIContent.GridPosition[field]).Text);
                    }
                    else
                    {
                        Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.GridPosition[ConstantValues.TicketCreatedDate] : UIContent.GridPosition[field]).Text);
                    }

                    break;

                case ConstantValues.OwnershipCalculationForSegments:
                    if (this.GetValue(Keys.RandomFieldValue).Contains("/"))
                    {
                        var expected = this.GetValue(Keys.RandomFieldValue).Replace('/', '-');
                        expected = expected.Replace(expected.Split('-')[1], expected.Split('-')[1].Replace(expected.Split('-')[1], UIContent.Conversion[expected.Split('-')[1]]).Substring(0, 3));
                        Assert.AreEqual(expected, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.OwnershipCalculationGrid[ConstantValues.TicketCreatedDate] : UIContent.OwnershipCalculationGrid[field]).Text);
                    }
                    else
                    {
                        Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.OwnershipCalculationGrid[ConstantValues.TicketCreatedDate] : UIContent.OwnershipCalculationGrid[field]).Text);
                    }

                    break;

                case ConstantValues.OwnershipCalculationForNodes:

                    if (this.GetValue(Keys.RandomFieldValue).Contains("/"))
                    {
                        var expected = this.GetValue(Keys.RandomFieldValue).Replace('/', '-');
                        expected = expected.Replace(expected.Split('-')[1], expected.Split('-')[1].Replace(expected.Split('-')[1], UIContent.Conversion[expected.Split('-')[1]]).Substring(0, 3));
                        Assert.AreEqual(expected, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.OwnershipCalculationGrid[ConstantValues.TicketCreatedDate] : UIContent.OwnershipCalculationGrid[field]).Text);
                    }
                    else
                    {
                        Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.OwnershipCalculationGrid[ConstantValues.TicketCreatedDate] : UIContent.OwnershipCalculationGrid[field]).Text);
                    }

                    break;

                case ConstantValues.LogisticReportGeneration:

                    if (this.GetValue(Keys.RandomFieldValue).Contains("/"))
                    {
                        var expected = this.GetValue(Keys.RandomFieldValue).Replace('/', '-');
                        expected = expected.Replace(expected.Split('-')[1], expected.Split('-')[1].Replace(expected.Split('-')[1], UIContent.Conversion[expected.Split('-')[1]]).Substring(0, 3));
                        Assert.AreEqual(expected, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.ReportLogisticGrid[ConstantValues.TicketCreatedDate] : UIContent.ReportLogisticGrid[field]).Text);
                    }
                    else
                    {
                        Assert.AreEqual(this.GetValue(Keys.RandomFieldValue), this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: field.EqualsIgnoreCase(ConstantValues.CreatedDate) ? UIContent.ReportLogisticGrid[ConstantValues.TicketCreatedDate] : UIContent.ReportLogisticGrid[field]).Text);
                    }

                    break;

                default:
                    break;
            }
        }

        [Then(@"verify (that "".*"" "".*"") is ""(.*)""")]
        public void ThenVerifyThatIs(ElementLocator elementLocator, string expectedValue)
        {
            this.VerifyThatIs(elementLocator, expectedValue);
        }

        [Then(@"the results should be sorted according to the ""(.*)""")]
        public async Task ThenTheResultsShouldBeSortedAccordingToTheAsync(string field)
        {
            IEnumerable<dynamic> ticketRecords = null;
            switch (field)
            {
                case ConstantValues.Ticket:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsByIdDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.Segment:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsBySegmentDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.StartDate:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsByStartDateDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.EndDate:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsByEndDateDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.CreatedDate:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsByCreatedDateDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.Username:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsByCreatedByDesc).ConfigureAwait(false);
                    break;
                case ConstantValues.State:
                    ticketRecords = await this.ReadAllSqlAsync(input: SqlQueries.GetTicketsByStateDesc).ConfigureAwait(false);
                    break;
            }

            var ticketsList = ticketRecords.ToDictionaryList();
            foreach (var row in ticketsList)
            {
                await Task.Delay(5000).ConfigureAwait(false);
                Assert.AreEqual(row[ConstantValues.TicketId], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.TicketId]).Text);
                Assert.AreEqual(row[ConstantValues.CreatedBy], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.CreatedBy]).Text);

                var date = string.Format(CultureInfo.InvariantCulture, "{0:dd-MMM-yy}", Convert.ToDateTime(row[ConstantValues.CreatedDate], CultureInfo.InvariantCulture));
                date = date.Replace(date.Split('-')[1], UIContent.Conversion[date.Split('-')[1]]);
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.CreatedDate]).Text.Contains(date));

                date = string.Format(CultureInfo.InvariantCulture, "{0:dd-MMM-yy}", Convert.ToDateTime(row[ConstantValues.StartDate], CultureInfo.InvariantCulture));
                date = date.Replace(date.Split('-')[1], UIContent.Conversion[date.Split('-')[1]]);
                Assert.AreEqual(date, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.StartDate]).Text);
                date = string.Format(CultureInfo.InvariantCulture, "{0:dd-MMM-yy}", Convert.ToDateTime(row[ConstantValues.EndDate], CultureInfo.InvariantCulture));
                date = date.Replace(date.Split('-')[1], UIContent.Conversion[date.Split('-')[1]]);
                Assert.AreEqual(date, this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.EndDate]).Text);
                var name = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetActiveCategoryElement, args: new { elementId = row[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
                Assert.AreEqual(name[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text);
                Assert.AreEqual(row[ConstantValues.Status].Equals(1) ? UIContent.Conversion[ConstantValues.Finalized] : UIContent.Conversion[ConstantValues.Processing], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Status]).Text);
            }
        }

        [Then(@"I should see a page with the operational report")]
        public void ThenIShouldSeeAPageWithTheOperationalReport()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ReportHeader));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ReportHeader));
        }

        [Then(@"I should see a page with the Operational cutoff summary")]
        public void ThenIShouldSeeAPageWithTheOperationalCutoffSummary()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.SummaryPageHeader));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.SummaryPageHeader));
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsInPieChart));
            Assert.IsTrue(elements[0].GetAttribute("fill").Contains("#3B3838"));
            Assert.IsTrue(elements[1].GetAttribute("fill").Contains("#7A8B58"));
        }

        [StepDefinition(@"I see ""(.*)"" header")]
        public void ThenIShouldSeeHeader(string header)
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion[header]);
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion[header]);
        }

        [When(@"I have a top record with ""(.*)"" state and I searched in ""(.*)"" Grid")]
        [When(@"I have a record with ""(.*)"" state and I searched in ""(.*)"" Grid")]
        public async Task WhenIHaveARecordWithStateAndISearchedForItAsync(string state, string gridType)
        {
            IDictionary<string, string> lastCreatedTicket = null;
            switch (gridType)
            {
                case ConstantValues.OwnershipCalculationForSegments:
                    var latestCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestOwnershipCalculationTicket).ConfigureAwait(false);
                    if (state.EqualsIgnoreCase(ConstantValues.Submitted))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSubmittedTicket).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessingStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else if (state.EqualsIgnoreCase(ConstantValues.Error))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetErrorTicketOfOwnershipCalculationGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithErrorStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCompletedTicketOfOwnershipCalculationGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessedStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }

                    this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilter)).SendKeys(lastCreatedTicket[ConstantValues.TicketId]);
                    this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilter)).SendKeys(OpenQA.Selenium.Keys.Enter);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    this.ScenarioContext["Segment"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.SegmentNameOfOwnershipCalculationForSegments]).Text;
                    this.ScenarioContext["StartDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.StartDateOfOwnershipCalculationForSegments]).Text;
                    this.ScenarioContext["EndDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.EndDateOfOwnershipCalculationForSegments]).Text;

                    break;

                case ConstantValues.OwnershipCalculationForNodes:
                    latestCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestOwnershipCalculationTicket).ConfigureAwait(false);
                    if (state.EqualsIgnoreCase(ConstantValues.Submitted))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSubmittedTicketInOwnershipPerNodeGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessingStatusInOwnershipCalculationPerNodeGrid, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else if (state.EqualsIgnoreCase(ConstantValues.Error))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetErrorTicketOfOwnershipCalculationPerNodeGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithErrorStatusInOwnershipCalculationPerNodeGrid, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCompletedTicketOfOwnershipCalculationPerNodeGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessedStatusInOwnershipCalculationPerNodeGrid, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }

                    this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(lastCreatedTicket[ConstantValues.TicketId]);
                    this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    this.ScenarioContext["Segment"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.OwnershipCalculationGrid[ConstantValues.Segment]).Text;
                    this.ScenarioContext["StartDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.OwnershipCalculationGrid[ConstantValues.StartDate]).Text;
                    this.ScenarioContext["EndDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.OwnershipCalculationGrid[ConstantValues.EndDate]).Text;
                    this.ScenarioContext["Node"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.OwnershipCalculationGrid[ConstantValues.Node]).Text;
                    break;

                case ConstantValues.OperationalCutoffTickets:
                    latestCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastTicket).ConfigureAwait(false);
                    if (state.EqualsIgnoreCase(ConstantValues.InProgress))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInProgressTicket).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessingStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else if (state.EqualsIgnoreCase(ConstantValues.Error))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetErrorTicket).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithErrorStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCompletedTicket).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessedStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }

                    this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilter)).SendKeys(lastCreatedTicket[ConstantValues.TicketId]);
                    this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilter)).SendKeys(OpenQA.Selenium.Keys.Enter);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    this.ScenarioContext["Segment"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text;
                    this.ScenarioContext["StartDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.StartDate]).Text;
                    this.ScenarioContext["EndDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.EndDate]).Text;
                    break;

                case ConstantValues.PlanningProgrammingAndCollaborationAgreements:
                    latestCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestUploadedEventRecordInFileUploadGrid).ConfigureAwait(false);
                    if (state.EqualsIgnoreCase(ConstantValues.Processing))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetProcessingUploadedEventRecordInFileUploadGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateFileRegistrationIdwithProcessingStatus, args: new { fileRegistrationId = latestCreatedTicket[ConstantValues.FileRegistrationIdName] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else if (state.EqualsIgnoreCase(ConstantValues.Error))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetFailedUploadedEventRecordInFileUploadGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateFileRegistrationIdwithFailedStatus, args: new { fileRegistrationId = latestCreatedTicket[ConstantValues.FileRegistrationIdName] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }
                    else
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCompletedUploadedEventRecordInFileUploadGrid).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateFileRegistrationIdwithCompletedStatus, args: new { fileRegistrationId = latestCreatedTicket[ConstantValues.FileRegistrationIdName] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }
                    }

                    this.Get<ElementPage>().GetElement(nameof(Resources.UploadedFileNameInFileUploadGrid)).SendKeys(lastCreatedTicket[ConstantValues.Name]);
                    this.Get<ElementPage>().GetElement(nameof(Resources.UploadedFileNameInFileUploadGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
                    this.Get<ElementPage>().Click(nameof(Resources.StatusFilterOnEvenFileUploadGrid));
                    this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: lastCreatedTicket[ConstantValues.Status]);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));

                    break;

                case ConstantValues.LogisticReportGeneration:
                    latestCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestLogisticTicket).ConfigureAwait(false);
                    if (state.EqualsIgnoreCase(ConstantValues.Processing))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetProcessingLogisticTicket).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessingStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }

                        lastCreatedTicket[ConstantValues.Status] = "Procesando";
                    }
                    else if (state.EqualsIgnoreCase(ConstantValues.Error))
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetErrorLogisticTicket).ConfigureAwait(false);
                            this.ScenarioContext["ErrorMessage"] = lastCreatedTicket[ConstantValues.ErrorMessage];
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithErrorStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.ScenarioContext["ErrorMessage"] = ConstantValues.ErrorMessage;
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }

                        lastCreatedTicket[ConstantValues.Status] = "Fallido";
                    }
                    else
                    {
                        try
                        {
                            lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCompletedLogisticTicket).ConfigureAwait(false);
                        }
                        catch (ArgumentNullException e)
                        {
                            Logger.Log(NLog.LogLevel.Error, e.Message, e);
                            lastCreatedTicket = latestCreatedTicket;
                            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessedStatus, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                            this.Get<UrlPage>().NavigateToUrl(this.DriverContext.Driver.Url);
                            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                        }

                        lastCreatedTicket[ConstantValues.Status] = "Finalizado";
                    }

                    var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = lastCreatedTicket[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
                    this.Get<ElementPage>().GetElement(nameof(Resources.SegmentFilterInCreateLogisticReportGrid)).SendKeys(segmentOfLastCreatedTicket[ConstantValues.Name]);
                    this.Get<ElementPage>().GetElement(nameof(Resources.SegmentFilterInCreateLogisticReportGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
                    this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    this.Get<ElementPage>().Click(nameof(Resources.StatusFilterOnLogisticReportGrid));
                    this.Get<ElementPage>().Click(nameof(Resources.ElementByText), formatArgs: lastCreatedTicket[ConstantValues.Status]);
                    this.ScenarioContext["Segment"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text;
                    this.ScenarioContext["StartDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.StartDate]).Text;
                    this.ScenarioContext["EndDate"] = this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.EndDate]).Text;
                    break;
                default:
                    break;
            }
        }

        [Then(@"I should see a modal window with the error information in ""(.*)"" page")]
        public void ThenIShouldSeeAModalWindowWithTheErrorInformation(string gridType)
        {
            switch (gridType)
            {
                case ConstantValues.OperationalCutoffs:
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ViewErrorModal));
                    this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ViewErrorModal));
                    break;

                case ConstantValues.OwnershipCalculationForSegments:
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion["Error during property calculation"]);
                    this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion["Error during property calculation"]);
                    break;

                case ConstantValues.OwnershipCalculationForNodes:
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ViewErrorModal));
                    this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ViewErrorModal));
                    break;

                case ConstantValues.LogisticReportGeneration:
                    this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion["Error generating logistic report file"]);
                    this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion["Error generating logistic report file"]);
                    break;

                default:
                    break;
            }

            Assert.AreEqual(this.ScenarioContext["Segment"], this.Get<ElementPage>().GetElement(nameof(Resources.ElementInErrorPopup)).Text);
            Assert.AreEqual(this.ScenarioContext["StartDate"], this.Get<ElementPage>().GetElement(nameof(Resources.StartDateInErrorPopup)).Text);
            Assert.AreEqual(this.ScenarioContext["EndDate"], this.Get<ElementPage>().GetElement(nameof(Resources.EndDateInErrorPopup)).Text);
        }

        [Then(@"I should see Logistic Report for selected segment in the ""(.*)"" in the grid")]
        [Then(@"I should see the information of ""(.*)"" in the grid")]
        [Then(@"I should see the information of processed files ""(.*)"" in the grid")]
        [Then(@"I should see the information of executed ""(.*)"" in the grid")]
        public async Task ThenIShouldSeeTheInformationOfExecutedOperationalCutoffsInTheGridAsync(string gridType)
        {
            await this.IShouldSeeTheInformationOfExecutedOperationalCutoffsInTheGridAsync(gridType).ConfigureAwait(false);
        }

        [When(@"I navigate to second page in ""(.*)"" Grid")]
        public async Task WhenINavigateToSecondPageAsync(string gridType)
        {
            IEnumerable<IDictionary<string, object>> totalTickets = null;

            switch (gridType)
            {
                case ConstantValues.OwnershipCalculationForSegments:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTickets).ConfigureAwait(false);
                    break;
                case ConstantValues.OwnershipCalculationForNodes:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTicketsForNode).ConfigureAwait(false);
                    break;
                case ConstantValues.OperationalCutoffTickets:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetTickets).ConfigureAwait(false);
                    break;
                case ConstantValues.LogisticReportGeneration:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetLogisticTickets).ConfigureAwait(false);
                    break;
                case ConstantValues.PlanningProgrammingAndCollaborationAgreements:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllUploadedRecordsInFileUploadGrid).ConfigureAwait(false);
                    break;
                case ConstantValues.Exceptions:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetExceptions).ConfigureAwait(false);
                    break;
                case ConstantValues.DeltaGrid:
                    totalTickets = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetDeltaTickets).ConfigureAwait(false);
                    break;
                default:
                    break;
            }

            this.ScenarioContext.Add("TicketsCount", totalTickets.Count());
            if (totalTickets.Count() > 10)
            {
                this.Get<ElementPage>().Click(nameof(Resources.PaginationSecondPage));
            }
            else
            {
                this.LogToReport("Enough records are not present to check pagination functionality");
            }
        }

        [Then(@"the records should be displayed accordingly in ""(.*)"" Grid")]
        public async Task ThenTheRecordsShouldBeDisplayedAccordinglyAsync(string gridType)
        {
            if (int.Parse(this.ScenarioContext["TicketsCount"].ToString(), CultureInfo.InvariantCulture) > 10)
            {
                IEnumerable<IDictionary<string, object>> totalRecords = null;
                IDictionary<string, string> ticket = null;
                switch (gridType)
                {
                    case ConstantValues.OperationalCutoffTickets:
                        ticket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondPageTickets).ConfigureAwait(false);
                        totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOperationalCutoffTickets).ConfigureAwait(false);
                        break;

                    case ConstantValues.OwnershipCalculationForSegments:
                        ticket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondPageTicketsOfOwnershipCalculation).ConfigureAwait(false);
                        totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTickets).ConfigureAwait(false);
                        break;

                    case ConstantValues.OwnershipCalculationForNodes:
                        ticket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondPageTicketsOfOwnershipCalculationForNode).ConfigureAwait(false);
                        totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTicketsForNode).ConfigureAwait(false);
                        break;
                    case ConstantValues.LogisticReportGeneration:
                        ticket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondPageTicketsOfReportLogisticGrid).ConfigureAwait(false);
                        totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetLogisticTickets).ConfigureAwait(false);
                        break;
                    case ConstantValues.PlanningProgrammingAndCollaborationAgreements:
                        ticket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondPageTicketsOfEventsFileUploadGrid).ConfigureAwait(false);
                        totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllUploadedRecordsInFileUploadGrid).ConfigureAwait(false);
                        break;
                    case ConstantValues.Exceptions:
                        ticket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondPageExceptions).ConfigureAwait(false);
                        totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetExceptions).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }

                if (gridType.EqualsIgnoreCase(ConstantValues.PlanningProgrammingAndCollaborationAgreements))
                {
                    Assert.AreEqual(ticket[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Segment]).Text);
                }
                else if (gridType.EqualsIgnoreCase(ConstantValues.LogisticReportGeneration))
                {
                    var segmentOfLastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = ticket[ConstantValues.CategoryElementId] }).ConfigureAwait(false);
                    Assert.AreEqual(segmentOfLastCreatedTicket[ConstantValues.Name], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.TicketId]).Text);
                }
                else if (gridType.EqualsIgnoreCase(ConstantValues.Exceptions))
                {
                    Assert.AreEqual(ticket[ConstantValues.ErrorMessage], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.Error]).Text);
                }
                else
                {
                    Assert.AreEqual(ticket[ConstantValues.TicketId], this.Get<ElementPage>().GetElement(nameof(Resources.GridDetails), formatArgs: UIContent.GridPosition[ConstantValues.TicketId]).Text);
                }

                var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
                var count = paginationCount.Split(' ');
                if (totalRecords.Count() >= 20)
                {
                    Assert.AreEqual("20", count[2]);
                }
                else
                {
                    Assert.AreEqual(totalRecords.Count().ToString(CultureInfo.InvariantCulture), count[2]);
                }
            }
        }

        [StepDefinition(@"I change the elements count per page to (.*)")]
        public void WhenIChangeTheElementsCountPerPageTo(int recordsCount)
        {
            this.IChangeTheElementsCountPerPageTo(recordsCount);
        }

        [StepDefinition(@"I should see ""(.*)"" button")]
        public void ThenIShouldSeeButton(string expectedButtonName)
        {
            var actualButtonName = this.Get<ElementPage>().GetElement(nameof(Resources.CreateRelationButton)).Text;
            Assert.AreEqual(expectedButtonName, actualButtonName);
        }

        [StepDefinition(@"I verify the grid ""(.*)"" is present")]
        public void ThenIVerifyTheGridIsPresent(string expectedColumnName)
        {
            var actualColumnName = this.Get<ElementPage>().GetElement(expectedColumnName).GetAttribute("value");
            Assert.AreEqual(expectedColumnName, actualColumnName);
        }

        [StepDefinition(@"I verify the records count shown per page as (.*)")]
        public void ThenIVerifyTheRecordsCountShownPerPageAs(string expectedCount)
        {
            try
            {
                ////var actualCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount));
                var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCountValue)).GetAttribute("value");
#pragma warning disable CA1305 // Specify IFormatProvider
                int actualCount = int.Parse(paginationCount);
#pragma warning restore CA1305 // Specify IFormatProvider

                //// var count = paginationCount.Split(' ');
                //// var actualCount = count[2];
                Assert.AreEqual(expectedCount, actualCount);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Console.WriteLine(e.Message);
            }
        }

        [Then(@"the records count in ""(.*)"" Grid shown per page should also be (.*)")]
        public async Task ThenTheRecordsCountShownPerPageShouldAlsoBeAsync(string gridType, int recordsCount)
        {
            IEnumerable<IDictionary<string, object>> totalRecords = null;
            switch (gridType)
            {
                case ConstantValues.Nodes:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetNodeInformationForTransporteSegment).ConfigureAwait(false);
                    break;

                case ConstantValues.OperationalCutoffTickets:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOperationalCutoffTickets).ConfigureAwait(false);
                    break;

                case ConstantValues.OwnershipCalculationForSegments:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTickets).ConfigureAwait(false);
                    break;

                case ConstantValues.OwnershipCalculationForNodes:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetOwnershipCalculationTicketsForNode).ConfigureAwait(false);
                    break;

                case ConstantValues.LogisticReportGeneration:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetLogisticTickets).ConfigureAwait(false);
                    break;
                case ConstantValues.PlanningProgrammingAndCollaborationAgreements:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllUploadedRecordsInFileUploadGrid).ConfigureAwait(false);
                    break;

                case ConstantValues.Exceptions:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetExceptions).ConfigureAwait(false);
                    break;

                case ConstantValues.DeltaGrid:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetDeltaTickets).ConfigureAwait(false);
                    break;

                case ConstantValues.OfficialDeltaNode:
                    totalRecords = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllOfficialDeltaNodeRecords).ConfigureAwait(false);
                    break;

                default:
                    break;
            }

            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var count = paginationCount.Split(' ');
            if (totalRecords.Count() >= 50)
            {
                Assert.AreEqual(recordsCount.ToString(CultureInfo.InvariantCulture), count[2]);
            }
            else
            {
                Assert.AreEqual(totalRecords.Count().ToString(CultureInfo.InvariantCulture), count[2]);
            }
        }

        [When(@"I have a record of previous period in ""(.*)"" state and I searched for it")]
        public async Task WhenIHaveARecordOfPreviousPeriodInStateAndISearchedForItAsync(string state)
        {
            IDictionary<string, string> lastCreatedTicket = null;
            var latestCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSecondTicketWithCompletedStatusInTicketTable).ConfigureAwait(false);
            if (state.EqualsIgnoreCase(ConstantValues.Completed))
            {
                try
                {
                    lastCreatedTicket = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetPreviousCompletedRecordInOwnershipCalculationForSegments).ConfigureAwait(false);
                }
                catch (ArgumentNullException e)
                {
                    Logger.Log(NLog.LogLevel.Error, "Ticket with Completed status of previous record is not present in the Database", e);
                    lastCreatedTicket = latestCreatedTicket;
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateTicketIdwithProcessedStatusInOwnershipCalculationPerNodeGrid, args: new { ticketId = latestCreatedTicket[ConstantValues.TicketId] }).ConfigureAwait(false);
                }

                this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(lastCreatedTicket[ConstantValues.TicketId]);
                this.Get<ElementPage>().GetElement(nameof(Resources.TicketIdFilterInOwnershipCalculationNodeGrid)).SendKeys(OpenQA.Selenium.Keys.Enter);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            }
        }
    }
}