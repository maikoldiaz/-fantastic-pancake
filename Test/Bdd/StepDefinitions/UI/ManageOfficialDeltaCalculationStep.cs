// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageOfficialDeltaCalculationStep.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ManageOfficialDeltaCalculationStep : EcpWebStepDefinitionBase
    {
        [Then(@"I should see the Official deltas by node list sorted by node in ascending order")]
        public async System.Threading.Tasks.Task ThenIShouldSeeTheOfficialDeltasByNodeListSortedByNodeInAscendingOrderAsync()
        {
            var expectedLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLatestOfficialDeltaNode).ConfigureAwait(false);

            var ticketID = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 1).Text;
            var starDate = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 2).Text;
            var finalDate = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 3).Text;
            var executionDate = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 4).Text;
            var userName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 5).Text;
            var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 6).Text;
            var segment = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 7).Text;
            var status = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 8).Text;

            Assert.AreEqual(expectedLatestRow["TicketId"], ticketID);
            Assert.AreEqual(expectedLatestRow["StartDate"], starDate);
            Assert.AreEqual(expectedLatestRow["EndDate"], finalDate);
            Assert.IsTrue(expectedLatestRow["CreatedBy"].EqualsIgnoreCase(userName));
            Assert.IsTrue(expectedLatestRow["NodeName"].EqualsIgnoreCase(nodeName));
            Assert.IsTrue(expectedLatestRow["Segment"].EqualsIgnoreCase(segment));
            Assert.IsTrue(expectedLatestRow["Status"].EqualsIgnoreCase(status));
        }

        [Then(@"I should see the records on the grid for last (.*) days")]
        public async System.Threading.Tasks.Task ThenIShouldSeeTheRecordsOnTheGridForLastDaysAsync(int days)
        {
            Assert.IsNotNull(days);
            var expectedLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetOfficialDeltaNodeRecordCount).ConfigureAwait(false);
            var page = this.Get<ElementPage>();
            this.Get<ElementPage>().WaitUntilElementExists(nameof(Resources.PaginationCount));
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            Assert.AreEqual(expectedLatestRow["DeltaRecords"], paginationCount.Split(' ')[4]);
        }

        [When(@"I select ""(.*)"" (for "".*"" "".*"") filter")]
        public async System.Threading.Tasks.Task WhenISelectForFilterAsync(string field, ElementLocator elementLocator)
        {
            var expectedLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeltaStatusRecordfromOfficialAdjustment).ConfigureAwait(false);
            field = expectedLatestRow["TicketId"];
            this.EnterValueIntoTextBox(elementLocator, field);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
            await Task.Delay(3000).ConfigureAwait(false);
        }

        [When(@"I click on ""(.*)"" ""(.*)"" link in the grid")]
        public async System.Threading.Tasks.Task WhenIClickOnLinkInTheGridAsync(string field, string element)
        {
            Assert.IsNotNull(field);
            Assert.IsNotNull(element);
            var expectedLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeltaStatusRecordfromOfficialAdjustment).ConfigureAwait(false);
            string ticketId = expectedLatestRow["TicketId"];
            var officialDeltaPage = this.Get<ElementPage>();
            officialDeltaPage.GetElement(nameof(Resources.TicketViewSummaryButton), formatArgs: ticketId).Click();
        }

        [Then(@"I should see the Official deltas by node list for filtered segment ticket and sorted by node in ascending order")]
#pragma warning disable S4144 // Methods should not have identical implementations
        public async System.Threading.Tasks.Task ThenIShouldSeeTheOfficialDeltasByNodeListForFilteredSegmentTicketAndSortedByNodeInAscendingOrderAsync()
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            var officialDeltaLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetDeltaStatusRecordfromOfficialAdjustment).ConfigureAwait(false);
            string selectedTicketID = officialDeltaLatestRow["TicketId"];

            var expectedLatestRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetOfficialDeltaNodeByTicketId, args: new { TicketId = selectedTicketID }).ConfigureAwait(false);

            var ticketID = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 1).Text;
            var starDate = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 2).Text;
            var finalDate = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 3).Text;
            var executionDate = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 4).Text;
            var userName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 5).Text;
            var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 6).Text;
            var segment = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 7).Text;
            var status = this.Get<ElementPage>().GetElement(nameof(Resources.GetNameFromGrid), 8).Text;

            Assert.AreEqual(expectedLatestRow["TicketId"], ticketID);
            Assert.AreEqual(expectedLatestRow["StartDate"], starDate);
            Assert.AreEqual(expectedLatestRow["EndDate"], finalDate);
            Assert.IsTrue(expectedLatestRow["CreatedBy"].EqualsIgnoreCase(userName));
            Assert.IsTrue(expectedLatestRow["NodeName"].EqualsIgnoreCase(nodeName));
            Assert.IsTrue(expectedLatestRow["Segment"].EqualsIgnoreCase(segment));
            Assert.IsTrue(expectedLatestRow["Status"].EqualsIgnoreCase(status));
        }

        [Then(@"I should see ""(.*)"" ""(.*)"" is hide")]
        public void ThenIShouldSeeIsHide(string field, string field1)
        {
            Assert.IsNotNull(field);
            Assert.IsNotNull(field1);
        }

        [When(@"I select ""(.*)"" (from the "".*"" "".*"") filter")]
        public void WhenISelectFromTheFilter(string value, string field)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            if (field.Contains("officialDeltaNodesGrid"))
#pragma warning restore CA1062 // Validate arguments of public methods
            {
                    var officialDeltaPage = this.Get<ElementPage>();
                    officialDeltaPage.GetElement(nameof(Resources.OfficialDeltaNodeGridStatus)).Click();

                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.OfficialDeltaStatusMenu), 5);
                    var connectionGridRow = this.Get<ElementPage>().GetElements(nameof(Resources.OfficialDeltaStatusOption), formatArgs: value);
                    var selectOptionElement = connectionGridRow[connectionGridRow.Count - 1];
                    this.SetValue(Entities.Keys.SelectedValue, connectionGridRow[connectionGridRow.Count - 1].Text);
                    Actions action = new Actions(this.DriverContext.Driver);
                    action.MoveToElement(selectOptionElement).Perform();
                    selectOptionElement.Click();
                }
                else if (field.Contains("tickets"))
                {
                    var officialAdjustmentPage = this.Get<ElementPage>();
                    officialAdjustmentPage.GetElement(nameof(Resources.OfficialAdjustmentGridStatus)).Click();

                    this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.OfficialAdjustmentStatusMenu), 5);
                    var officialAdjustmentGridRow = this.Get<ElementPage>().GetElements(nameof(Resources.OfficialAdjustmentStatusOption), formatArgs: value);
                    var selectOptionElement1 = officialAdjustmentGridRow[officialAdjustmentGridRow.Count - 1];
                    this.SetValue(Entities.Keys.SelectedValue, officialAdjustmentGridRow[officialAdjustmentGridRow.Count - 1].Text);
                    Actions action1 = new Actions(this.DriverContext.Driver);
                    action1.MoveToElement(selectOptionElement1).Perform();
                    selectOptionElement1.Click();
                }
        }
    }
}
